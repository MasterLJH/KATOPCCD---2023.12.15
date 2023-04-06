using DXH.ViewModel;
using KATOPCCD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KATOPCCD.ViewModel
{
     public class JobViewModel : DXH.ViewModel.ViewModelBase
    {
        MainWindow mMainWindow;
        public JobViewModel(MainWindow _MainWindow)
        {
            mMainWindow = _MainWindow;
            JobManager.SelectedJobChanged += JobManager_SelectedJobChanged;
        }
        private void JobManager_SelectedJobChanged(object sender, EventArgs e)
        {
            this.OnPropertyChanged("SelectedJob");
        }
        #region 作业配置

        public bool mModified;
        public bool Modified
        {
            get { return mModified; }
            set
            {
                mModified = value;
                this.OnPropertyChanged("Modified");
            }
        }
        RelayCommand _JobModified;
        public ICommand JobModified
        {
            get
            {
                if (_JobModified == null)
                    _JobModified = new RelayCommand(param => this.Task_JobModified((string)param));
                return _JobModified;
            }
        }
        public void Task_JobModified(string _param)
        {
            Modified = true;
            if (_param == "Direct")
            {
                JobManager.OnSelectedJobChanged();
            }
        }
        public ObservableCollection<Job> AllJobs
        {
            get { return JobManager.AllJobs; }
            set { JobManager.AllJobs = value; }
        }
        public Job SelectedJob
        {
            get { return JobManager.SelectedJob; }
            set
            {
                bool temp = Modified;
                //if (JobManager.SelectedJob != null)
                //    JobManager.SelectedJob.Reset();
                JobManager.SelectedJob = value;
                this.OnPropertyChanged("SelectedJob");
                Modified = temp;
            }
        }
        RelayCommand _JobCommand;
        public ICommand JobViewModelCommand
        {
            get
            {
                if (_JobCommand == null)
                    _JobCommand = new RelayCommand(param => this.Task_JobCommand((string)param));
                return _JobCommand;
            }
        }
        public void Task_JobCommand(string param)
        {
            Console.WriteLine(param);
            if (param == "Create")
            {
                Task_Create();
            }
            else if (param == "Duplicate")
            {
                Task_Duplicate();
            }
            else if (param == "Save")
            {
                Task_Save();
            }
            else if (param == "Delete")
            {
                Task_Delete();
            }
            else if (param == "UnderlyingConstants_Add")
            {
                UnderlyingConstants_Add();
            }
            else if (param == "UnderlyingConstants_Delete")
            {
                UnderlyingConstants_Delete();
            }
                  
        }

      
      
        public void Task_Create()
        {
            Global.SaveLog("作业配置|新建作业");
            JobManager.CreateJob();
            this.OnPropertyChanged("SelectedJob");
            Modified = true;
        }
        public void Task_Duplicate()
        {
            Global.SaveLog("作业配置|复制作业");
            JobManager.DuplicateJob();
            Modified = true;
        }
        public void Task_Save()
        {
            Global.SaveLog("作业配置|保存作业");
            try
            {
                JobManager.SaveJobs();
                Modified = false;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message, "Save");
            }
        }
        public void Task_Delete()
        {
            if (JobManager.DeleteJob())
            {
                Global.SaveLog("作业配置|删除作业");
                this.OnPropertyChanged("SelectedJob");
                Modified = true;
            }
        }

        /// <summary>
        /// 添加基础变量函数
        /// </summary>
        public void UnderlyingConstants_Add()
        {
            if (SelectedJob == null)
                return;
          
            UnderlyingConstants _UnderlyingConstants = new UnderlyingConstants();

            _UnderlyingConstants.ID = (SelectedJob.UnderlyingConstantsCollection.Count+1).ToString();

            _UnderlyingConstants.Value = "0";
            _UnderlyingConstants.Info = "0";

            SelectedJob.UnderlyingConstantsCollection.Add(_UnderlyingConstants);

            Modified = true;
        }


        /// <summary>
        /// 删除基础变量函数
        /// </summary>
        public void UnderlyingConstants_Delete()
        {
            if (SelectedJob == null)
                return;

            SelectedJob.UnderlyingConstantsCollection.RemoveAt(UnderlyingConstants_Index);
            int count = SelectedJob.UnderlyingConstantsCollection.Count;
            ObservableCollection<UnderlyingConstants> result = new ObservableCollection<UnderlyingConstants>();
            //每次删除后，动态改变所有ID排序
           
            for (int i = 0; i < count; i++)
            {
                UnderlyingConstants New = new UnderlyingConstants();
                New.ID = (i + 1).ToString();
                New.Info = SelectedJob.UnderlyingConstantsCollection[i].Info;
                New.Value = SelectedJob.UnderlyingConstantsCollection[i].Value;
                result.Add(New);                                   
            }

            int count2= SelectedJob.UnderlyingConstantsCollection.Count;
            for (int i2 = count2-1; i2 >=0; i2--)
            {
                SelectedJob.UnderlyingConstantsCollection.RemoveAt(i2);
            }

            foreach (var item in result)
            {
                SelectedJob.UnderlyingConstantsCollection.Add(item);
            }

        
            Modified = true;
        }

        int mUnderlyingConstants_Index = -1;
        /// <summary>
        /// 基础变量当前选中
        /// </summary>
        public int UnderlyingConstants_Index
        {
            get { return mUnderlyingConstants_Index; }
            set
            {             
                mUnderlyingConstants_Index = value;
                OnPropertyChanged("UnderlyingConstants_Index");         
            }
        }
        #endregion
    }
}
