using GoVision3D.SaveLoad;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SagensSdk
{
    public class StaticTool
    {
        public static void WriteSerializable(string filepath, object data)
        {
            BinaryFormatter binaryFomat = new BinaryFormatter();
            using (FileStream fs = new FileStream(filepath, FileMode.Create))
            {
                binaryFomat.Serialize(fs, data);
                fs.Dispose();
            }
        }

        public static object ReadSerializable(string filepath, Type type)
        {
            BinaryFormatter binaryFomat = new BinaryFormatter();
            object obj;
            using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate))
            {
                obj = binaryFomat.Deserialize(fs);
                fs.Dispose();
            }
            return obj;
        }

        public static void ConvertDataToKdata(SurfaceZSaveDat ssd, string SaveKdatDirectoy, string Side)
        {
            PointMsg pmoffset = ssd.offset;
            PointMsg pmscale = ssd.resolution; ;
            Point3d64f poffset = new Point3d64f() { x = pmoffset.x, y = pmoffset.y, z = pmoffset.z };
            Point3d64f pscale = new Point3d64f() { x = pmscale.x, y = pmscale.y, z = pmscale.z };
            int size = Marshal.SizeOf(typeof(short));
            IntPtr surfacePtr = Marshal.AllocHGlobal(size * ssd.points.Length);
            Marshal.Copy(ssd.points, 0, surfacePtr, ssd.points.Length);
            GoSurface insurface = new GoSurface() { data = surfacePtr, width = (int)ssd.width, height = (int)ssd.height, offset = poffset, scale = pscale };
            GoSaveSurfaceGraphic.SaveSufaceToFile($"{SaveKdatDirectoy}{Side}.kdat", ref insurface, false);
            Marshal.FreeHGlobal(surfacePtr);
        }

        public static void GetUnlineRunImg(SurfaceZSaveDat ssd, SurfaceIntensitySaveDat sid, SurfaceZSaveDat szd, double zStart, double z_byte_resolution, out HObject zoomHeightImg, out HObject zoomIntensityImg, out HObject zoomRgbImg, out HObject zoomPlaneImg)
        {
            HObject heightImg, intensityImg, byteImg, rgbImg, planeImg ;
            zoomPlaneImg = null;
            GenHeightImg(ssd, zStart, z_byte_resolution, out heightImg, out byteImg);
            GenIntensityImg(sid, out intensityImg);
            if (szd!=null)
            {
                GenHeightImg(szd, out planeImg);
                HOperatorSet.ZoomImageFactor(planeImg, out zoomPlaneImg, 2, 2, "constant");
            }
            

            PseudoColor.GrayToPseudoColor(byteImg, out rgbImg);
            byteImg.Dispose();
            HOperatorSet.ZoomImageFactor(rgbImg, out zoomRgbImg, 1, 4, "constant");
            rgbImg.Dispose();
            HOperatorSet.ZoomImageFactor(heightImg, out zoomHeightImg, 1, 4, "constant");
            heightImg.Dispose();
            HOperatorSet.ZoomImageFactor(intensityImg, out zoomIntensityImg, 1, 4, "constant");
            intensityImg.Dispose();
            GC.Collect();
        }

        public static void GenHeightImg(SurfaceZSaveDat ssd, double zStart, double z_byte_resolution, out HObject heightImg, out HObject byteImg)
        {
            int width = ssd.width;
            int height = ssd.height;
            float[] surfaceData = new float[height * width];
            byte[] surfaceDataZByte = new byte[height * width];
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    surfaceData[j * width + k] = ssd.points[j * width + k] == -32768 ? -12 : (float)(ssd.offset.z + ssd.resolution.z * ssd.points[j * width + k]);
                    if (ssd.points[j * width + k] != -32768)
                    {
                        surfaceDataZByte[j * width + k] = (byte)Math.Ceiling(((ssd.offset.z + ssd.resolution.z * ssd.points[j * width + k]) - zStart) * z_byte_resolution);
                    }
                    else
                    {
                        surfaceDataZByte[j * width + k] = 0;
                    }

                }
            }

            GenHalconImage(surfaceData, width, height, out heightImg);
            GenHalconImage(surfaceDataZByte, width, height, out byteImg);
            GC.Collect();

        }
        public static void GenHeightImg(SurfaceZSaveDat ssd, out HObject heightImg)
        {
            int width = ssd.width;
            int height = ssd.height;
            float[] surfaceData = new float[height * width];
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    surfaceData[j * width + k] = ssd.points[j * width + k] == -32768 ? -12 : (float)(ssd.offset.z + ssd.resolution.z * ssd.points[j * width + k]);
                }
            }
            heightImg = GenHalconImage(surfaceData, width, height);
            GC.Collect();
        }

        public static void GenIntensityImg(SurfaceIntensitySaveDat sid, out HObject intensityImg)
        {
            intensityImg = GenHalconImage(sid.points, sid.width, sid.height);
            GC.Collect();
        }

        private static HObject GenHalconImage(object pointsArr, long width, long height)
        {
            HObject image = null;
            try
            {
                string typeName = pointsArr.GetType().Name;
                string type = "";
                switch (typeName)
                {
                    case "Double[]":
                        type = "real";
                        break;
                    case "Byte[]":
                        type = "byte";
                        break;
                    case "Single[]":
                        type = "real";

                        break;
                    default:
                        //image = null;
                        return image;
                }
                GCHandle hObject = GCHandle.Alloc(pointsArr, GCHandleType.Pinned);
                IntPtr pObject = hObject.AddrOfPinnedObject();
                if (hObject.IsAllocated)
                    hObject.Free();
                image = new HObject();
                HOperatorSet.GenImage1(out image, type, width, height, pObject);
                return image;
            }
            catch
            {
                return image;
            }
        }
        private static void GenHalconImage(object pointsArr, long width, long height, out HObject image)
        {
            string typeName = pointsArr.GetType().Name;
            string type = "";
            int size = -1;
            switch (typeName)
            {
                case "Double[]":
                    type = "real";
                    size = Marshal.SizeOf(typeof(double));
                    double[] potArr1 = (double[])pointsArr;

                    IntPtr ptImage = Marshal.AllocHGlobal(size * potArr1.Length);
                    Marshal.Copy(potArr1, 0, ptImage, potArr1.Length);
                    HOperatorSet.GenImage1(out image, type, width, height, ptImage);
                    Marshal.FreeHGlobal(ptImage);
                    break;
                case "Byte[]":
                    type = "byte";
                    size = Marshal.SizeOf(typeof(byte));
                    byte[] potArr2 = (byte[])pointsArr;

                    IntPtr ptImage1 = Marshal.AllocHGlobal(size * potArr2.Length);
                    Marshal.Copy(potArr2, 0, ptImage1, potArr2.Length);
                    HOperatorSet.GenImage1(out image, type, width, height, ptImage1);
                    Marshal.FreeHGlobal(ptImage1);
                    break;
                case "Single[]":
                    type = "real";
                    size = Marshal.SizeOf(typeof(float));
                    float[] potArr3 = (float[])pointsArr;

                    IntPtr ptImage3 = Marshal.AllocHGlobal(size * potArr3.Length);
                    Marshal.Copy(potArr3, 0, ptImage3, potArr3.Length);
                    HOperatorSet.GenImage1(out image, type, width, height, ptImage3);
                    Marshal.FreeHGlobal(ptImage3);
                    break;
                default:
                    image = null;
                    break;
            }
        }

     
    }
}
