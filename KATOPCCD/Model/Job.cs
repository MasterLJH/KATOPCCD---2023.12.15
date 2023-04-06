using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using ViewROI;

namespace KATOPCCD.Model
{
    [Serializable]
    public class Job
    {
        public Job()
        {
           
            Camera_1_Calibration = new VisionCoor();
            Camera_2_Calibration = new VisionCoor();
            Camera_1_Positions = new ObservableCollection<VisionCoor>();
            Camera_2_Positions = new ObservableCollection<VisionCoor>();
            Target_1_Positions = new ObservableCollection<Coor>();
            Target_2_Positions = new ObservableCollection<Coor>();
            ROIList = new ObservableCollection<ROI>();
            UnderlyingConstantsCollection = new ObservableCollection<UnderlyingConstants>();

        }
        /// <summary>
        /// 作业名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 预留信息
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 扫码用ROI
        /// </summary>
        public ObservableCollection<ROI> ROIList { get; set; }

        public Job Clone()
        {
            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (Job)b.Deserialize(stream);
            }
        }





    
        /// <summary>
        /// 相机1标定坐标
        /// </summary>
        public VisionCoor Camera_1_Calibration { get; set; }
        /// <summary>
        /// 相机2标定坐标
        /// </summary>
        public VisionCoor Camera_2_Calibration { get; set; }
        /// <summary>
        /// 相机1拍照坐标集合
        /// </summary>
        public ObservableCollection<VisionCoor> Camera_1_Positions { get; set; }
        /// <summary>
        /// 相机2拍照坐标集合
        /// </summary>
        public ObservableCollection<VisionCoor> Camera_2_Positions { get; set; }
        /// <summary>
        /// 目标坐标
        /// </summary>
        public ObservableCollection<Coor> Target_1_Positions { get; set; }
        public ObservableCollection<Coor> Target_2_Positions { get; set; }





        /// <summary>
        /// 基础配置信息集合
        /// </summary>
        public ObservableCollection<UnderlyingConstants> UnderlyingConstantsCollection { get; set; }

        /// <summary>
        /// 根据ID或者Info获取Value
        /// </summary>
        /// <param name="IDorInfo"></param>
        /// <returns></returns>
        public string GetValue(string IDorInfo)
        {
            try
            {
                foreach (var item in UnderlyingConstantsCollection)
                {
                    if (item.ID == IDorInfo||item.Info== IDorInfo)
                        return item.Value;
                }
                return "null";
            }
            catch (Exception ex)
            {
                return ex.Message;    
            }
        }


    }

    [Serializable]
    public class VisionCoor
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int16 ID { get; set; }
        public string IDName
        {
            get
            {
                if (ID == 1)
                {
                    return "2-1";
                }
                else if (ID == 2)
                {
                    return "2-2";
                }
                else if (ID == 3)
                {
                    return "2-3";
                }
                else if (ID == 4)
                {
                    return "2-4";
                }
                else if (ID == 5)
                {
                    return "2-5";
                }
                else if (ID == 6)
                {
                    return "2-6";
                }
                else if (ID == 7)
                {
                    return "2-7";
                }
                else if (ID == 8)
                {
                    return "2-8";
                }
                else if (ID == 9)
                {
                    return "1-1";
                }
                else if (ID == 10)
                {
                    return "1-2";
                }
                else if (ID == 11)
                {
                    return "1-3";
                }
                else if (ID == 12)
                {
                    return "1-4";
                }
                else if (ID == 13)
                {
                    return "1-5";
                }
                else if (ID == 14)
                {
                    return "1-6";
                }
                else if (ID == 15)
                {
                    return "1-7";
                }
                else if (ID == 16)
                {
                    return "1-8";
                }
                else
                {
                    return ID.ToString();
                }
            }
            set { }
        }
        /// <summary>
        /// 模式
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 拍照坐标X
        /// </summary>
        public double Cam_X { get; set; }
        /// <summary>
        /// 拍照坐标Y
        /// </summary>
        public double Cam_Y { get; set; }
        /// <summary>
        /// 拍照坐标R
        /// </summary>
        public double Cam_R { get; set; }

        //GW137每次拍照识别4个模板
        /// <summary>
        /// 相机识别出的实际坐标X的集合
        /// </summary>
        public double[] Real_X { get; set; }
        /// <summary>
        /// 相机识别出的实际坐标Y的集合
        /// </summary>
        public double[] Real_Y { get; set; }
        /// <summary>
        /// 相机识别出的实际坐标R的集合
        /// </summary>
        public double[] Real_R { get; set; }
    
        /// <summary>
        /// 预留信息
        /// </summary>
        public string Info { get; set; }

        public VisionCoor Clone()
        {
            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (VisionCoor)b.Deserialize(stream);
            }
        }
    }

    [Serializable]
    public class Coor
    {
        /// <summary>
        /// ID
        /// </summary>
        public Int16 ID { get; set; }
        public string IDName
        {
            get
            {
                if (ID == 1)
                {
                    return "2-1";
                }
                else if (ID == 2)
                {
                    return "2-2";
                }
                else if (ID == 3)
                {
                    return "2-3";
                }
                else if (ID == 4)
                {
                    return "2-4";
                }
                else if (ID == 5)
                {
                    return "2-5";
                }
                else if (ID == 6)
                {
                    return "2-6";
                }
                else if (ID == 7)
                {
                    return "2-7";
                }
                else if (ID == 8)
                {
                    return "2-8";
                }
                else if (ID == 9)
                {
                    return "1-1";
                }
                else if (ID == 10)
                {
                    return "1-2";
                }
                else if (ID == 11)
                {
                    return "1-3";
                }
                else if (ID == 12)
                {
                    return "1-4";
                }
                else if (ID == 13)
                {
                    return "1-5";
                }
                else if (ID == 14)
                {
                    return "1-6";
                }
                else if (ID == 15)
                {
                    return "1-7";
                }
                else if (ID == 16)
                {
                    return "1-8";
                }
                else
                {
                    return ID.ToString();
                }
            }
            set { }
        }
     
        public double CAD_Angle { get; set; }
        /// <summary>
        /// 通过Mark点计算出的实际坐标X
        /// </summary>
        public double Real_X { get; set; }
        /// <summary>
        /// 通过Mark点计算出的实际坐标Y
        /// </summary>
        public double Real_Y { get; set; }
        /// <summary>
        /// 通过Mark点计算出的实际坐标角度
        /// </summary>
        public double Real_Angle { get; set; }
        /// <summary>
        /// 预留信息
        /// </summary>
        public double Info { get; set; }

        public Coor Clone()
        {
            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (Coor)b.Deserialize(stream);
            }
        }
    }


    /// <summary>
    /// 基础常量
    /// </summary>
    [Serializable]
    public class UnderlyingConstants
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 注释信息
        /// </summary>
        public string Info { get; set; }

        public UnderlyingConstants Clone()
        {
            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (UnderlyingConstants)b.Deserialize(stream);
            }
        }
    }

}
