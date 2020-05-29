using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ViewWindow.Model;

namespace Calibration
{

    public class XmlSeriazlize
    {
        public static bool SaveToXML(String path, object sourceObj, Type type, string xmlRootName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(path) && sourceObj != null)
                {
                    using (StreamWriter write = new StreamWriter(path))
                    {
                        XmlSerializer xmlSerializer = !string.IsNullOrWhiteSpace(xmlRootName) ?
                            new XmlSerializer(type) : new XmlSerializer(type, new XmlRootAttribute(xmlRootName));
                        xmlSerializer.Serialize(write, sourceObj);
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static Object LoadFormXml(String filePath, Type type)
        {
            try
            {
                Object result = null;
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(type);
                        result = xmlSerializer.Deserialize(reader);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }

  
    
    [Serializable]
    public class RoiParam
    {
        public bool enable;
        public double roi_row_start;
        public double roi_col_start;
        public double roi_row_end;
        public double roi_col_end;
        public string roi_name;
        public string edge_select;
        public string edge_transition;
        public int meterbox_height;
        public int meterbox_width;
        public int meterbox_num;
        public double edge_sigma;
        public double edge_threshold;
        public double roi_score;
        public int circle_startAng;
        public int circle_endAng;
        public string circle_oriente;

        public double xOffset;
        public double yOffset;

        public int PointCount = 0;

        public List<IntersetionCoord> intersectCoord = new List<IntersetionCoord>();     

        public RoiParam() { }
        public void Set(bool enable, double roi_row_start,double roi_col_start,double roi_row_end,double roi_col_end
            , string roi_name,string edge_select, string edge_transition,
            int meterbox_height, int meterbox_width, int meterbox_num, double edge_sigma, double edge_threshold,
            double roi_score, int circle_startAng, int circle_endAng, string circle_oriente)
        {
            this.enable = enable;
            this.roi_row_start = roi_row_start;
            this.roi_col_start = roi_col_start;
            this.roi_row_end = roi_row_end;
            this.roi_col_end = roi_col_end;
            switch (edge_select)
            {
                case "所有":
                    this.edge_select = "all";
                    break;
                case "第一点":
                    this.edge_select = "first";
                    break;
                case "最后一点":
                    this.edge_select = "last";
                    break;
            }

            this.roi_name = roi_name;
            switch (edge_transition)
            {
                case "所有":
                    this.edge_transition = "all";
                    break;
                case "由明到暗":
                    this.edge_transition = "negative";
                    break;
                case "由暗到明":
                    this.edge_transition = "positive";
                    break;
            }
                 
            this.meterbox_height = meterbox_height;
            this.meterbox_width = meterbox_width;
            this.meterbox_num = meterbox_num;
            this.edge_sigma = edge_sigma;
            this.edge_threshold = edge_threshold;
            this.roi_score = roi_score;
            this.circle_startAng = circle_startAng;
            this.circle_endAng = circle_endAng;
            this.circle_oriente = circle_oriente;
        }

        
       
    }
    [Serializable]
    public struct IntersetionCoord
    {
        public double Row;
        public double Col;
        public double Zpoint;
        public string LorC;
    }

}
