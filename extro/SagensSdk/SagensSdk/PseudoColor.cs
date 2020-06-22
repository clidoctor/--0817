using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SagensSdk
{
    public class PseudoColor
    {

        public static string AutoSavePath { set; get; }

        public static void markColor(PictureBox pictureBox1, PictureBox pictureBox2, byte[] grayArr, double z_byte_resolution,double zStart)
        {
            int w = pictureBox1.Width;
            int h = pictureBox1.Height;
            int mstart = 5;
            int mheight = 13;
            int mWidth = 20;
            int mspace = 15;
            double[] heightArr = new double[5];
            Color[] colorArr = new Color[5];
            for (int i = 0; i < grayArr.Length; i++)
            {
                heightArr[i] = Math.Round(grayArr[i] / z_byte_resolution + zStart , 3) ;
                colorArr[i] = Color.FromArgb(rainTable[grayArr[i]/2, 0], rainTable[grayArr[i]/2, 1], rainTable[grayArr[i]/2, 2]);
            }

            Bitmap bt = new Bitmap(w, h);
            for (int i = 0; i < w; i++)
            {
                if (i < mWidth)
                {
                    for (int j = 0; j < h; j++)
                    {
                        if (j > mstart && j < mstart + mheight)
                        {
                            bt.SetPixel(i, j, colorArr[0]);
                        }
                        else if (j > mstart + mheight + mspace && j < mstart + mheight + mspace + mheight)
                        {
                            bt.SetPixel(i, j, colorArr[1]);
                        }
                        else if (j > mstart + (mheight * 2) + (mspace * 2) && j < mstart + (mheight * 3) + (mspace * 2))
                        {
                            bt.SetPixel(i, j, colorArr[2]);
                        }
                        else if (j > mstart + (mheight * 3) + (mspace * 3) && j < mstart + (mheight * 4) + (mspace * 3))
                        {
                            bt.SetPixel(i, j, colorArr[3]);
                        }
                        else if (j > mstart + (mheight * 4) + (mspace * 4) && j < mstart + (mheight * 5) + (mspace * 4))
                        {
                            bt.SetPixel(i, j, colorArr[4]);
                        }
                        else
                        {
                            bt.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                        }

                    }
                }
            }

            pictureBox1.Image = bt;
            PointF pf = new PointF(5, mstart);
            PointF pf1 = new PointF(5, mstart + mheight + mspace);
            PointF pf2 = new PointF(5, mstart + (mheight * 2) + (mspace * 2));
            PointF pf3 = new PointF(5, mstart + (mheight * 3) + (mspace * 3));
            PointF pf4 = new PointF(5, mstart + (mheight * 4) + (mspace * 4));
            using (Graphics g = pictureBox2.CreateGraphics())
            {
                Font f = new Font("Arial", 8);
                g.DrawString(heightArr[0].ToString(), f, Brushes.White, pf);
                g.DrawString(heightArr[1].ToString(), f, Brushes.White, pf1);
                g.DrawString(heightArr[2].ToString(), f, Brushes.White, pf2);
                g.DrawString(heightArr[3].ToString(), f, Brushes.White, pf3);
                g.DrawString(heightArr[4].ToString(), f, Brushes.White, pf4);
            } 
        }
        public static void HeightAreaToPseudoColor(HObject heightImg,out HObject rgbImg, HTuple validThreMin, HTuple validThreMax, HTuple heightMin, HTuple heightMax)
        {
            HObject threReg;
            HOperatorSet.Threshold(heightImg, out threReg, validThreMin, validThreMax);
            HTuple grayMin, grayMax, grayRange;
            HOperatorSet.MinMaxGray(threReg, heightImg,0, out grayMin, out grayMax, out grayRange);
            threReg.Dispose();
            double bs = (255 / grayRange);

            HObject imgRange, imgMax;
            scale_image_range(heightImg, out imgRange, grayMin, grayMax);
            HOperatorSet.ScaleImageMax(imgRange, out imgMax);
            imgRange.Dispose();

            Bitmap bp;
            HObject2Bpp8(imgMax, out bp);
            HTuple w, h;
            HOperatorSet.GetImageSize(imgMax, out w, out h);
            Rectangle rect = new Rectangle(0, 0, w, h);
            imgMax.Dispose();

            int heightTograyMin = (int)Math.Ceiling(((double)heightMin - (double)grayMin) * bs);
            int heightTograyMax = (int)Math.Ceiling(((double)heightMax - (double)grayMin) * bs);

            Bitmap a = PGrayToPseudoColor2(bp, rect, 2, heightTograyMin, heightTograyMax);
            bp.Dispose();

            Bitmap b = (Bitmap)a.Clone();
            Bitmap2HImageBpp24(b, out rgbImg);
         
           
           
           
            a.Dispose();
            b.Dispose();
        }


        public static void GrayToPseudoColor(HObject grayImg, out HObject rgbImg)
        {
            try
            {
                Bitmap bp;
                HObject2Bpp8(grayImg, out bp);
                HTuple w, h;
                HOperatorSet.GetImageSize(grayImg, out w, out h);

                Rectangle rect = new Rectangle(0, 0, w, h);
                Bitmap a = PGrayToPseudoColor2(bp, rect, 2);

                Bitmap b = (Bitmap)a.Clone();
                Bitmap2HImageBpp24(b, out rgbImg);
                if (!string.IsNullOrEmpty(AutoSavePath))
                {
                    HOperatorSet.WriteImage(rgbImg, Path.GetExtension(AutoSavePath).Remove(0, 1), 0, AutoSavePath);
                }
                bp.Dispose();
                a.Dispose();
                b.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void GrayToPseudoColor(HObject grayImg, out HObject rgbImg, HTuple validThreMin, HTuple validThreMax)
        {
            try
            {
                HObject imgMax;
              
                GetImgMax(grayImg, out imgMax, validThreMin, validThreMax);
                
                Bitmap bp;
                HObject2Bpp8(imgMax, out bp);
                HTuple w, h;
                HOperatorSet.GetImageSize(imgMax, out w,out h);

                Rectangle rect = new Rectangle(0, 0, w, h);
                Bitmap a = PGrayToPseudoColor2(bp, rect, 2);

                Bitmap b = (Bitmap)a.Clone();
                Bitmap2HImageBpp24(b, out rgbImg);
                if (!string.IsNullOrEmpty(AutoSavePath))
                {
                    HOperatorSet.WriteImage(rgbImg, Path.GetExtension(AutoSavePath).Remove(0, 1), 0, AutoSavePath);
                }
                imgMax.Dispose();
                bp.Dispose();
                a.Dispose();
                b.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
         }



        [DllImport("kernel32.dll")]
        private static extern void CopyMemory(int Destination, int add, int Length);
        [DllImport("kernel32.dll")]
        private static extern void CopyMemory(long Destination, long add, int Length);

        private static void HObject2Bpp8(HObject image, out Bitmap res)
        {
            try
            {
                HTuple hpoint, type, width, height;

                const int Alpha = 255;
                long[] ptr = new long[2];
                HOperatorSet.GetImagePointer1(image, out hpoint, out type, out width, out height);

                res = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                ColorPalette pal = res.Palette;
                for (int i = 0; i <= 255; i++)
                {
                    pal.Entries[i] = Color.FromArgb(Alpha, i, i, i);
                }
                res.Palette = pal;
                Rectangle rect = new Rectangle(0, 0, width, height);
                BitmapData bitmapData = res.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                int PixelSize = Bitmap.GetPixelFormatSize(bitmapData.PixelFormat) / 8;
                ptr[0] = bitmapData.Scan0.ToInt64();
                ptr[1] = hpoint.L;
                if (width % 4 == 0)
                    CopyMemory(ptr[0], ptr[1], width * height * PixelSize);
                else
                {
                    for (int i = 0; i < height - 1; i++)
                    {
                        ptr[1] += width;
                        CopyMemory(ptr[0], ptr[1], width * PixelSize);
                        ptr[0] += bitmapData.Stride;
                    }
                }
                res.UnlockBits(bitmapData);
                hpoint = null;
            }
            catch (Exception ex)
            {
                res = null;
                throw ex;
            }
        }

        private static void Bitmap2HImageBpp24(Bitmap bmp, out HObject image) //转换500ms
        {
            try
            {
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);

                BitmapData bmp_data = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                byte[] arrayR = new byte[bmp_data.Width * bmp_data.Height];//红色数组 
                byte[] arrayG = new byte[bmp_data.Width * bmp_data.Height];//绿色数组 
                byte[] arrayB = new byte[bmp_data.Width * bmp_data.Height];//蓝色数组 
                unsafe
                {
                    byte* pBmp = (byte*)bmp_data.Scan0;//BitMap的头指针 
                                                       //下面的循环分别提取出红绿蓝三色放入三个数组 
                    for (int R = 0; R < bmp_data.Height; R++)
                    {
                        for (int C = 0; C < bmp_data.Width; C++)
                        {
                            //因为内存BitMap的储存方式，行宽用Stride算，C*3是因为这是三通道，另外BitMap是按BGR储存的 
                            byte* pBase = pBmp + bmp_data.Stride * R + C * 3;
                            arrayR[R * bmp_data.Width + C] = *(pBase + 2);
                            arrayG[R * bmp_data.Width + C] = *(pBase + 1);
                            arrayB[R * bmp_data.Width + C] = *(pBase);
                            pBase = null;
                        }
                    }
                    fixed (byte* pR = arrayR, pG = arrayG, pB = arrayB)
                    {
                        HOperatorSet.GenImage3(out image, "byte", bmp_data.Width, bmp_data.Height,
                                                                   new IntPtr(pR), new IntPtr(pG), new IntPtr(pB));
                        //如果这里报错，仔细看看前面有没有写错 
                    }
                    bmp.UnlockBits(bmp_data);
                }
                bmp.Dispose();
                
                GC.Collect();

            }
            catch (Exception ex)
            {
                image = null;
            }
        }


        private static Bitmap PGrayToPseudoColor2(Bitmap src, Rectangle rect, int type)
        {
            try
            {
                if (type == 1)
                {
                    Bitmap a = new Bitmap(src);
                    System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    int stride = bmpData.Stride;
                    unsafe
                    {
                        byte* pIn = (byte*)bmpData.Scan0.ToPointer();
                        int temp;
                        byte R, G, B;

                        for (int y = 0; y < rect.Height; y++)
                        {
                            for (int x = 0; x < rect.Width; x++)
                            {
                                temp = pIn[0] / 2;

                                R = ironTable[temp, 0];
                                G = ironTable[temp, 1];
                                B = ironTable[temp, 2];

                                pIn[0] = B;
                                pIn[1] = G;
                                pIn[2] = R;

                                pIn += 3;
                            }
                            pIn += stride - rect.Width * 3;
                        }
                        
                    }
                    a.UnlockBits(bmpData);
                   
                    return a;
                }
                else if (type == 2)
                {
                    Bitmap a = new Bitmap(src);
                    System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    int stride = bmpData.Stride;
                    unsafe
                    {
                        byte* pIn = (byte*)bmpData.Scan0.ToPointer();
                        int temp;
                        byte R, G, B;

                        for (int y = 0; y < rect.Height; y++)
                        {
                            for (int x = 0; x < rect.Width; x++)
                            {
                                temp = pIn[0] / 2;

                                R = rainTable[temp, 0];
                                G = rainTable[temp, 1];
                                B = rainTable[temp, 2];

                                pIn[0] = B;
                                pIn[1] = G;
                                pIn[2] = R;

                                pIn += 3;
                            }
                            pIn += stride - rect.Width * 3;
                        }
                        
                    }
                    a.UnlockBits(bmpData);
                    return a;
                }
                else
                {
                    throw new Exception("type 参数不合法！");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return null;
            }
        }

        private static Bitmap PGrayToPseudoColor2(Bitmap src, Rectangle rect, int type,int grayMin,int grayMax)
        {
            try
            {
                if (type == 1)
                {
                    Bitmap a = new Bitmap(src);
                    System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    int stride = bmpData.Stride;
                    unsafe
                    {
                        byte* pIn = (byte*)bmpData.Scan0.ToPointer();
                        int temp;
                        byte R, G, B;

                        for (int y = 0; y < rect.Height; y++)
                        {
                            for (int x = 0; x < rect.Width; x++)
                            {
                                temp = pIn[0] / 2;
                                if (temp!=0 && pIn[0] < grayMin)
                                {
                                    temp = 10;
                                }
                                else if (pIn[0] > grayMax)
                                {
                                    temp = 110;
                                }

                                R = ironTable[temp, 0];
                                G = ironTable[temp, 1];
                                B = ironTable[temp, 2];

                                pIn[0] = B;
                                pIn[1] = G;
                                pIn[2] = R;

                                pIn += 3;
                            }
                            pIn += stride - rect.Width * 3;
                        }

                    }
                    a.UnlockBits(bmpData);

                    return a;
                }
                else if (type == 2)
                {
                    Bitmap a = new Bitmap(src);
                    System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    int stride = bmpData.Stride;
                    unsafe
                    {
                        byte* pIn = (byte*)bmpData.Scan0.ToPointer();
                        int temp;
                        byte R, G, B;

                        for (int y = 0; y < rect.Height; y++)
                        {
                            for (int x = 0; x < rect.Width; x++)
                            {
                                temp = pIn[0] / 2;
                                if (temp != 0 && pIn[0] < grayMin)
                                {
                                    temp = grayMin/2;
                                }
                                else if (pIn[0] > grayMax)
                                {
                                    temp = grayMax/2;
                                }


                                R = rainTable[temp, 0];
                                G = rainTable[temp, 1];
                                B = rainTable[temp, 2];

                                pIn[0] = B;
                                pIn[1] = G;
                                pIn[2] = R;

                                pIn += 3;
                            }
                            pIn += stride - rect.Width * 3;
                        }

                    }
                    a.UnlockBits(bmpData);
                    return a;
                }
                else
                {
                    throw new Exception("type 参数不合法！");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
                return null;
            }
        }

        /// <summary>
        /// 灰度图转伪彩色图像函数（通过查表的方法）
        /// </summary>
        /// <param name="src"></param>
        /// <param name="type">转换类型（1.使用铁红  2.使用彩虹）</param>
        /// <returns></returns>
        private static Bitmap PGrayToPseudoColor2(Bitmap src, int type)
        {
            try
            {
                if (type == 1)
                {
                    Bitmap a = new Bitmap(src);
                    Rectangle rect = new Rectangle(0, 0, a.Width, a.Height);
                    System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    int stride = bmpData.Stride;
                    unsafe
                    {
                        byte* pIn = (byte*)bmpData.Scan0.ToPointer();
                        int temp;
                        byte R, G, B;

                        for (int y = 0; y < a.Height; y++)
                        {
                            for (int x = 0; x < a.Width; x++)
                            {
                                temp = pIn[0] / 2;

                                R = ironTable[temp, 0];
                                G = ironTable[temp, 1];
                                B = ironTable[temp, 2];

                                pIn[0] = B;
                                pIn[1] = G;
                                pIn[2] = R;

                                pIn += 3;
                            }
                            pIn += stride - a.Width * 3;
                        }
                    }
                    a.UnlockBits(bmpData);
                    return a;
                }
                else if (type == 2)
                {
                    Bitmap a = new Bitmap(src);
                    Rectangle rect = new Rectangle(0, 0, a.Width, a.Height);
                    System.Drawing.Imaging.BitmapData bmpData = a.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    int stride = bmpData.Stride;
                    unsafe
                    {
                        byte* pIn = (byte*)bmpData.Scan0.ToPointer();
                        int temp;
                        byte R, G, B;

                        for (int y = 0; y < a.Height; y++)
                        {
                            for (int x = 0; x < a.Width; x++)
                            {
                                temp = pIn[0] / 2;

                                R = rainTable[temp, 0];
                                G = rainTable[temp, 1];
                                B = rainTable[temp, 2];

                                pIn[0] = B;
                                pIn[1] = G;
                                pIn[2] = R;

                                pIn += 3;
                            }
                            pIn += stride - a.Width * 3;
                        }
                    }
                    a.UnlockBits(bmpData);
                    return a;
                }
                else
                {
                    throw new Exception("type 参数不合法！");
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 铁红色带映射表
        /// 每一行代表一个彩色分类，存放顺序是RGB
        /// </summary>
        private static byte[,] ironTable = new byte[128, 3] {
                {0,   0,  0},
                {0,   0,  0},
                {0,   0,  36},
                {0,   0,  51},
                {0,   0,  66},
                {0,   0,  81},
                {2,   0,  90},
                {4,   0,  99},
                {7,   0, 106},
                {11,   0, 115},
                {14,   0, 119},
                {20,   0, 123},
                {27,   0, 128},
                {33,   0, 133},
                {41,   0, 137},
                {48,   0, 140},
                {55,   0, 143},
                {61,   0, 146},
                {66,   0, 149},
                {72,   0, 150},
                {78,   0, 151},
                {84,   0, 152},
                {91,   0, 153},
                {97,   0, 155},
                {104,   0, 155},
                {110,   0, 156},
                {115,   0, 157},
                {122,   0, 157},
                {128,   0, 157},
                {134,   0, 157},
                {139,   0, 157},
                {146,   0, 156},
                {152,   0, 155},
                {157,   0, 155},
                {162,   0, 155},
                {167,   0, 154},
                {171,   0, 153},
                {175,   1, 152},
                {178,   1, 151},
                {182,   2, 149},
                {185,   4, 149},
                {188,   5, 147},
                {191,   6, 146},
                {193,   8, 144},
                {195,  11, 142},
                {198,  13, 139},
                {201,  17, 135},
                {203,  20, 132},
                {206,  23, 127},
                {208,  26, 121},
                {210,  29, 116},
                {212,  33, 111},
                {214,  37, 103},
                {217,  41,  97},
                {219,  46,  89},
                {221,  49,  78},
                {223,  53,  66},
                {224,  56,  54},
                {226,  60,  42},
                {228,  64,  30},
                {229,  68,  25},
                {231,  72,  20},
                {232,  76,  16},
                {234,  78,  12},
                {235,  82,  10},
                {236,  86,   8},
                {237,  90,   7},
                {238,  93,   5},
                {239,  96,   4},
                {240, 100,   3},
                {241, 103,   3},
                {241, 106,   2},
                {242, 109,   1},
                {243, 113,   1},
                {244, 116,   0},
                {244, 120,   0},
                {245, 125,   0},
                {246, 129,   0},
                {247, 133,   0},
                {248, 136,   0},
                {248, 139,   0},
                {249, 142,   0},
                {249, 145,   0},
                {250, 149,   0},
                {251, 154,   0},
                {252, 159,   0},
                {253, 163,   0},
                {253, 168,   0},
                {253, 172,   0},
                {254, 176,   0},
                {254, 179,   0},
                {254, 184,   0},
                {254, 187,   0},
                {254, 191,   0},
                {254, 195,   0},
                {254, 199,   0},
                {254, 202,   1},
                {254, 205,   2},
                {254, 208,   5},
                {254, 212,   9},
                {254, 216,  12},
                {255, 219,  15},
                {255, 221,  23},
                {255, 224,  32},
                {255, 227,  39},
                {255, 229,  50},
                {255, 232,  63},
                {255, 235,  75},
                {255, 238,  88},
                {255, 239, 102},
                {255, 241, 116},
                {255, 242, 134},
                {255, 244, 149},
                {255, 245, 164},
                {255, 247, 179},
                {255, 248, 192},
                {255, 249, 203},
                {255, 251, 216},
                {255, 253, 228},
                {255, 254, 239},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249},
                {255, 255, 249} };

        /// <summary>
        /// 彩虹色带映射表
        /// </summary>
        private static byte[,] rainTable = new byte[128, 3] {
            {5,   0,   5},
            {5,   0,   5},
            {15,   0,  15},
            {31,   0,  31},
            {47,   0,  47},
            {63,   0,  63},
            {79,   0,  79},
            {95,   0,  95},
            {111,   0, 111},
            {127,   0, 127},
            {143,   0, 143},
            {159,   0, 159},
            {175,   0, 175},
            {191,   0, 191},
            {207,   0, 207},
            {223,   0, 223},
            {239,   0, 239},
            {255,   0, 255},
            {239,   0, 250},
            {223,   0, 245},
            {207,   0, 240},
            {191,   0, 236},
            {175,   0, 231},
            {159,   0, 226},
            {143,   0, 222},
            {127,   0, 217},
            {111,   0, 212},
            {95,   0, 208},
            {79,   0, 203},
            {63,   0, 198},
            {47,   0, 194},
            {31,   0, 189},
            {15,   0, 184},
            {0,   0, 180},
            {0,  15, 184},
            {0,  31, 189},
            {0,  47, 194},
            {0,  63, 198},
            {0,  79, 203},
            {0,  95, 208},
            {0, 111, 212},
            {0, 127, 217},
            {0, 143, 222},
            {0, 159, 226},
            {0, 175, 231},
            {0, 191, 236},
            {0, 207, 240},
            {0, 223, 245},
            {0, 239, 250},
            {0, 255, 255},
            {0, 245, 239},
            {0, 236, 223},
            {0, 227, 207},
            {0, 218, 191},
            {0, 209, 175},
            {0, 200, 159},
            {0, 191, 143},
            {0, 182, 127},
            {0, 173, 111},
            {0, 164,  95},
            {0, 155,  79},
            {0, 146,  63},
            {0, 137,  47},
            {0, 128,  31},
            {0, 119,  15},
            {0, 110,   0},
            {15, 118,   0},
            {30, 127,   0},
            {45, 135,   0},
            {60, 144,   0},
            {75, 152,   0},
            {90, 161,   0},
            {105, 169,  0},
            {120, 178,  0},
            {135, 186,  0},
            {150, 195,  0},
            {165, 203,  0},
            {180, 212,  0},
            {195, 220,  0},
            {210, 229,  0},
            {225, 237,  0},
            {240, 246,  0},
            {255, 255,  0},
            {251, 240,  0},
            {248, 225,  0},
            {245, 210,  0},
            {242, 195,  0},
            {238, 180,  0},
            {235, 165,  0},
            {232, 150,  0},
            {229, 135,  0},
            {225, 120,  0},
            {222, 105,  0},
            {219,  90,  0},
            {216,  75,  0},
            {212,  60,  0},
            {209,  45,  0},
            {206,  30,  0},
            {203,  15,  0},
            {200,   0,  0},
            {202,  11,  11},
            {205,  23,  23},
            {207,  34,  34},
            {210,  46,  46},
            {212,  57,  57},
            {215,  69,  69},
            {217,  81,  81},
            {220,  92,  92},
            {222, 104, 104},
            {225, 115, 115},
            {227, 127, 127},
            {230, 139, 139},
            {232, 150, 150},
            {235, 162, 162},
            {237, 173, 173},
            {240, 185, 185},
            {242, 197, 197},
            {245, 208, 208},
            {247, 220, 220},
            {250, 231, 231},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243},
            {252, 243, 243}
        };

        #region 转换成0-255
        private static void GetImgMax(HObject ho_Img, out HObject ho_ImageScaleMax, HTuple hv_ThreMin,
      HTuple hv_ThreMax)
        {




            // Local iconic variables 

            HObject ho_Regions, ho_ImageScaled;

            // Local control variables 

            HTuple hv_Min = null, hv_Max = null, hv_Range = null;
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaleMax);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            ho_Regions.Dispose();
            HOperatorSet.Threshold(ho_Img, out ho_Regions, hv_ThreMin, hv_ThreMax);
            HOperatorSet.MinMaxGray(ho_Regions, ho_Img, 0, out hv_Min, out hv_Max, out hv_Range);
            ho_ImageScaled.Dispose();
            scale_image_range(ho_Img, out ho_ImageScaled, hv_Min, hv_Max);
            ho_ImageScaleMax.Dispose();
            HOperatorSet.ScaleImageMax(ho_ImageScaled, out ho_ImageScaleMax);
            ho_Regions.Dispose();
            ho_ImageScaled.Dispose();

            return;
        }
        private static void scale_image_range(HObject ho_Image, out HObject ho_ImageScaled, HTuple hv_Min,
        HTuple hv_Max)
        {




            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_SelectedChannel = null, ho_LowerRegion = null;
            HObject ho_UpperRegion = null;

            // Local copy input parameter variables 
            HObject ho_Image_COPY_INP_TMP;
            ho_Image_COPY_INP_TMP = ho_Image.CopyObj(1, -1);



            // Local control variables 

            HTuple hv_LowerLimit = new HTuple(), hv_UpperLimit = new HTuple();
            HTuple hv_Mult = null, hv_Add = null, hv_Channels = null;
            HTuple hv_Index = null, hv_MinGray = new HTuple(), hv_MaxGray = new HTuple();
            HTuple hv_Range = new HTuple();
            HTuple hv_Max_COPY_INP_TMP = hv_Max.Clone();
            HTuple hv_Min_COPY_INP_TMP = hv_Min.Clone();

            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_ImageScaled);
            HOperatorSet.GenEmptyObj(out ho_SelectedChannel);
            HOperatorSet.GenEmptyObj(out ho_LowerRegion);
            HOperatorSet.GenEmptyObj(out ho_UpperRegion);
            //Convenience procedure to scale the gray values of the
            //input image Image from the interval [Min,Max]
            //to the interval [0,255] (default).
            //Gray values < 0 or > 255 (after scaling) are clipped.
            //
            //If the image shall be scaled to an interval different from [0,255],
            //this can be achieved by passing tuples with 2 values [From, To]
            //as Min and Max.
            //Example:
            //scale_image_range(Image:ImageScaled:[100,50],[200,250])
            //maps the gray values of Image from the interval [100,200] to [50,250].
            //All other gray values will be clipped.
            //
            //input parameters:
            //Image: the input image
            //Min: the minimum gray value which will be mapped to 0
            //     If a tuple with two values is given, the first value will
            //     be mapped to the second value.
            //Max: The maximum gray value which will be mapped to 255
            //     If a tuple with two values is given, the first value will
            //     be mapped to the second value.
            //
            //output parameter:
            //ImageScale: the resulting scaled image
            //
            if ((int)(new HTuple((new HTuple(hv_Min_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_LowerLimit = hv_Min_COPY_INP_TMP[1];
                hv_Min_COPY_INP_TMP = hv_Min_COPY_INP_TMP[0];
            }
            else
            {
                hv_LowerLimit = 0.0;
            }
            if ((int)(new HTuple((new HTuple(hv_Max_COPY_INP_TMP.TupleLength())).TupleEqual(
                2))) != 0)
            {
                hv_UpperLimit = hv_Max_COPY_INP_TMP[1];
                hv_Max_COPY_INP_TMP = hv_Max_COPY_INP_TMP[0];
            }
            else
            {
                hv_UpperLimit = 255.0;
            }
            //
            //Calculate scaling parameters
            hv_Mult = (((hv_UpperLimit - hv_LowerLimit)).TupleReal()) / (hv_Max_COPY_INP_TMP - hv_Min_COPY_INP_TMP);
            hv_Add = ((-hv_Mult) * hv_Min_COPY_INP_TMP) + hv_LowerLimit;
            //
            //Scale image
            {
                HObject ExpTmpOutVar_0;
                HOperatorSet.ScaleImage(ho_Image_COPY_INP_TMP, out ExpTmpOutVar_0, hv_Mult, hv_Add);
                ho_Image_COPY_INP_TMP.Dispose();
                ho_Image_COPY_INP_TMP = ExpTmpOutVar_0;
            }
            //
            //Clip gray values if necessary
            //This must be done for each channel separately
            HOperatorSet.CountChannels(ho_Image_COPY_INP_TMP, out hv_Channels);
            HTuple end_val48 = hv_Channels;
            HTuple step_val48 = 1;
            for (hv_Index = 1; hv_Index.Continue(end_val48, step_val48); hv_Index = hv_Index.TupleAdd(step_val48))
            {
                ho_SelectedChannel.Dispose();
                HOperatorSet.AccessChannel(ho_Image_COPY_INP_TMP, out ho_SelectedChannel, hv_Index);
                HOperatorSet.MinMaxGray(ho_SelectedChannel, ho_SelectedChannel, 0, out hv_MinGray,
                    out hv_MaxGray, out hv_Range);
                ho_LowerRegion.Dispose();
                HOperatorSet.Threshold(ho_SelectedChannel, out ho_LowerRegion, ((hv_MinGray.TupleConcat(
                    hv_LowerLimit))).TupleMin(), hv_LowerLimit);
                ho_UpperRegion.Dispose();
                HOperatorSet.Threshold(ho_SelectedChannel, out ho_UpperRegion, hv_UpperLimit,
                    ((hv_UpperLimit.TupleConcat(hv_MaxGray))).TupleMax());
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.PaintRegion(ho_LowerRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                        hv_LowerLimit, "fill");
                    ho_SelectedChannel.Dispose();
                    ho_SelectedChannel = ExpTmpOutVar_0;
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.PaintRegion(ho_UpperRegion, ho_SelectedChannel, out ExpTmpOutVar_0,
                        hv_UpperLimit, "fill");
                    ho_SelectedChannel.Dispose();
                    ho_SelectedChannel = ExpTmpOutVar_0;
                }
                if ((int)(new HTuple(hv_Index.TupleEqual(1))) != 0)
                {
                    ho_ImageScaled.Dispose();
                    HOperatorSet.CopyObj(ho_SelectedChannel, out ho_ImageScaled, 1, 1);
                }
                else
                {
                    {
                        HObject ExpTmpOutVar_0;
                        HOperatorSet.AppendChannel(ho_ImageScaled, ho_SelectedChannel, out ExpTmpOutVar_0
                            );
                        ho_ImageScaled.Dispose();
                        ho_ImageScaled = ExpTmpOutVar_0;
                    }
                }
            }
            ho_Image_COPY_INP_TMP.Dispose();
            ho_SelectedChannel.Dispose();
            ho_LowerRegion.Dispose();
            ho_UpperRegion.Dispose();

            return;
        }
        #endregion
    }
}
