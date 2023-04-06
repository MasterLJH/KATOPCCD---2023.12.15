using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace KATOPCCD.Model
{
    public static class JobManager
    {
        public static Job mCurJob;
        /// <summary>
        /// 当前作业配置
        /// </summary>
        public static Job CurJob
        {
            get { return mCurJob; }
            set
            {
                mCurJob = value;
                SaveDefaultJob(mCurJob);
            }
        }
        public static Job mSelectedJob;
        public static Job SelectedJob//作业配置时操作的作业
        {
            get { return mSelectedJob; }
            set
            {
                mSelectedJob = value;
                OnSelectedJobChanged();
            }
        }
        public static ObservableCollection<Job> AllJobs = new ObservableCollection<Job>();
        public static event EventHandler SelectedJobChanged;
        public static void OnSelectedJobChanged()
        {
            if (SelectedJobChanged != null)
                SelectedJobChanged(null, null);
        }
        public static void ini()
        {
            LoadJobs();
            CurJob = LoadDefaultJob();
        }
        //2022-7-1:针对本地不会生成相应文件夹，进行优化
        #region
        public static List<string> CZList = new List<string>() { };//操作集合，用来记录创建，复制，删除操作
        public static void CreateJob()
        {
            Job Empty = new Job();
            Empty.Name = "New Job" + "_" + (AllJobs.Count + 1).ToString();
            AllJobs.Add(Empty);
            SelectedJob = Empty;
            CZList.Add("Create+" + Empty.Name);
        }

        public static void DuplicateJob()
        {
            if (SelectedJob != null)
            {
                CZList.Add("Duplicate+" + SelectedJob.Name);
                Job New = SelectedJob.Clone();
                New.Name = SelectedJob.Name + "_Clone";
                AllJobs.Add(New);
            }
        }


        public static bool DeleteJob()
        {
            try
            {
                if (SelectedJob != null)
                {
                    CZList.Add("Delete+" + SelectedJob.Name);
                    AllJobs.Remove(SelectedJob);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static void SaveJobs()
        {
            FileStream fileStream = new FileStream(Path, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, AllJobs.Count);
            for (int mTick = 0; mTick < AllJobs.Count; mTick++)
            {
                b.Serialize(fileStream, AllJobs[mTick]);
            }
            fileStream.Close();
            //2022-7-1
            CZListWork();
        }

        public static void CZListWork()
        {
            //处理CZlist,长度为0说明没有进行操作，检测是否修改了名字
            if (CZList.Count == 0)
            {
                //检测是否需要修改文件夹名字
                DirectoryInfo d = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "//Vision//");
                DirectoryInfo[] directs = d.GetDirectories();//文件夹
                if (directs.Length != AllJobs.Count)
                {
                    Console.WriteLine("文件夹数量与作业数量不匹配");
                    return;
                }
                //找出需要修改的文件夹是哪个
                string NeedFolder = "";
                for (int i = 0; i < directs.Length; i++)
                {
                    for (int j = 0; j < AllJobs.Count; j++)
                    {

                        if (directs[i].Name == AllJobs[j].Name)
                            break;
                        if (j == AllJobs.Count - 1)
                            NeedFolder = directs[i].Name;

                    }
                }

                //没有改名，直接退出
                if (NeedFolder == "")
                    return;

                //找出需要修改的作业是哪个
                string NeedJob = "";
                for (int i1 = 0; i1 < AllJobs.Count; i1++)
                {
                    for (int j1 = 0; j1 < directs.Length; j1++)
                    {
                        if (AllJobs[i1].Name == directs[j1].Name)
                            break;
                        if (j1 == AllJobs.Count - 1)
                            NeedJob = AllJobs[i1].Name;
                    }
                }

                if (NeedJob == "")
                    return;
                //将原NeedFolder文件夹修改为NeedJob，重命名
                Directory.Move(AppDomain.CurrentDomain.BaseDirectory + "//Vision//" + NeedFolder, AppDomain.CurrentDomain.BaseDirectory + "//Vision//" + NeedJob);
            }
            else
            {
                //czlist长度不为零
                for (int m = 0; m < CZList.Count; m++)
                {
                    if (CZList[m].Contains("Create"))
                    {
                        //创建,temp[1]为文件夹名字
                        string[] temp = CZList[m].Split('+');

                        string sPath = AppDomain.CurrentDomain.BaseDirectory + "//Vision//" + temp[1];
                        if (!Directory.Exists(sPath))
                        {
                            Directory.CreateDirectory(sPath);
                        }
                    }
                    else if (CZList[m].Contains("Duplicate"))
                    {
                        //复制,temp1[1]为需要复制的文件夹名字
                        string[] temp1 = CZList[m].Split('+');
                        CopyFiles(AppDomain.CurrentDomain.BaseDirectory + "//" + "Vision" + "//" + temp1[1], AppDomain.CurrentDomain.BaseDirectory + "//" + "Vision" + "//" + temp1[1] + "_Clone");
                    }
                    else
                    {
                        //删除    
                        string[] temp2 = CZList[m].Split('+');
                        if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "//Vision//" + temp2[1]))
                        {
                            Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "//Vision//" + temp2[1], true);
                        }
                    }

                }
                CZList = new List<string>() { };
                CZListWork();
            }
        }


        //2022-7-1复制文件夹及其子文件
        public static void CopyFiles(string strSouPath, string strDesPath)
        {
            Directory.CreateDirectory(strDesPath);

            if (!Directory.Exists(strSouPath)) return;

            string[] directories = Directory.GetDirectories(strSouPath);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFiles(d, strDesPath + d.Substring(d.LastIndexOf("\\")));
                }
            }

            string[] files = Directory.GetFiles(strSouPath);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, strDesPath + s.Substring(s.LastIndexOf("\\")));
                }
            }
        }
        #endregion
        public static void CreateFilmPosition_1()
        {
            if (SelectedJob != null)
            {
                Coor Empty = new Coor();
                Empty.ID = (Int16)(SelectedJob.Target_1_Positions.Count + 1);
                SelectedJob.Target_1_Positions.Add(Empty);
            }
        }
        public static void CreateFilmPosition_1(double x, double y, double r)
        {
            if (SelectedJob != null)
            {
                Coor Empty = new Coor();
                Empty.Real_X = x;
                Empty.Real_Y = y;
                Empty.Real_Angle = r;
                Empty.ID = (Int16)(SelectedJob.Target_1_Positions.Count + 1);
                SelectedJob.Target_1_Positions.Add(Empty);
            }
        }
        public static void DuplicateFilmPosition_1(Coor Selected)
        {
            if (SelectedJob != null)
            {
                if (Selected != null)
                {
                    Coor New = Selected.Clone();
                    New.ID = (Int16)(SelectedJob.Target_1_Positions.Count + 1);
                    SelectedJob.Target_1_Positions.Add(New);
                }
            }
        }
        public static bool DeleteFilmPosition_1(Coor Selected)
        {
            if (SelectedJob != null)
            {
                if (Selected != null)
                {
                    SelectedJob.Target_1_Positions.Remove(Selected);
                    return true;
                }
            }
            return false;
        }

        //
        public static void CreateFilmPosition_2()
        {
            if (SelectedJob != null)
            {
                Coor Empty = new Coor();
                Empty.ID = (Int16)(SelectedJob.Target_2_Positions.Count + 1);
                SelectedJob.Target_2_Positions.Add(Empty);
            }
        }
        public static void CreateFilmPosition_2(double x, double y, double r)
        {
            if (SelectedJob != null)
            {
                Coor Empty = new Coor();
                Empty.Real_X = x;
                Empty.Real_Y = y;
                Empty.Real_Angle = r;
                Empty.ID = (Int16)(SelectedJob.Target_2_Positions.Count + 1);
                SelectedJob.Target_2_Positions.Add(Empty);
            }
        }
        public static void DuplicateFilmPosition_2(Coor Selected)
        {
            if (SelectedJob != null)
            {
                if (Selected != null)
                {
                    Coor New = Selected.Clone();
                    New.ID = (Int16)(SelectedJob.Target_2_Positions.Count + 1);
                    SelectedJob.Target_2_Positions.Add(New);
                }
            }
        }
        public static bool DeleteFilmPosition_2(Coor Selected)
        {
            if (SelectedJob != null)
            {
                if (Selected != null)
                {
                    SelectedJob.Target_2_Positions.Remove(Selected);
                    return true;
                }
            }
            return false;
        }

        public static void Backup()
        {
            try
            {
                File.Copy(Path, BackupPath, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void Restore()
        {
            try
            {
                File.Copy(BackupPath, Path, true);
                LoadJobs();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static string BackupPath = AppDomain.CurrentDomain.BaseDirectory + "Jobs_Backup.config";

        public static string Path = AppDomain.CurrentDomain.BaseDirectory + "Jobs.config";
        //默认作业文件
        public static string DefaultJobFile = AppDomain.CurrentDomain.BaseDirectory + "Default.job";
        public static void LoadJobs()
        {
            try
            {
                AllJobs.Clear();
                FileStream fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter mBinFmat = new BinaryFormatter();
                int count = (int)mBinFmat.Deserialize(fileStream);
                for (int mTick = 0; mTick < count; mTick++)
                {
                    Job mTemp = new Job();
                    mTemp = mBinFmat.Deserialize(fileStream) as Job;
                    AllJobs.Add(mTemp);
                }
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void LoadJobs(ObservableCollection<Job> JobList)
        {
            try
            {
                if (JobList == null)
                    return;
                JobList.Clear();
                FileStream fileStream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
                BinaryFormatter mBinFmat = new BinaryFormatter();
                int count = (int)mBinFmat.Deserialize(fileStream);
                for (int mTick = 0; mTick < count; mTick++)
                {
                    Job mTemp = new Job();
                    mTemp = mBinFmat.Deserialize(fileStream) as Job;
                    JobList.Add(mTemp);
                }
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        public static Job LoadDefaultJob()
        {
            Job _DefaultJob = null;
            if (File.Exists(DefaultJobFile))
            {
                try
                {
                    FileStream fileStream = new FileStream(DefaultJobFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                    BinaryFormatter mBinFmat = new BinaryFormatter();
                    _DefaultJob = mBinFmat.Deserialize(fileStream) as Job;
                    fileStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return _DefaultJob;
        }
        public static void SaveDefaultJob(Job _DefaultJob)
        {
            try
            {
                FileStream fileStream = new FileStream(DefaultJobFile, FileMode.Create);
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(fileStream, _DefaultJob);
                fileStream.Close();
            }
            catch { }
        }
    }
}
