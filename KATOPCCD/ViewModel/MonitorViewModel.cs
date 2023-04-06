using DXH.ViewModel;
using KATOPCCD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KATOPCCD.ViewModel
{
    public class MonitorViewModel : ViewModelBase
    {
        MainWindow mMainWindow;
        public MonitorViewModel(MainWindow _MainWindow)
        {
            mMainWindow = _MainWindow;            
            LoadPersons();
            JobManager.ini();
            Jobs = new ObservableCollection<Job>();         
        }

        #region 日志属性
        public string LogLive { get; set; }
        string LogHeader = " -> ";
        object LogLock = new object();
        int LogLine = 0;
        public async void KATOPLog(string strtoappend)
        {
            string logstr = CurTime() + LogHeader + strtoappend + Environment.NewLine;
            //Global.SaveLog(strtoappend);
            Task task_log = Task.Run(() =>
            {
                lock (LogLock)
                {
                    LogLine++;
                    if (LogLine > 100)
                    {
                        LogLive = "";
                        LogLine = 1;
                    }
                    LogLive = LogLive + logstr;
                }
                this.OnPropertyChanged("LogLive");
            });
            await task_log;
        }
        public string CurTime()
        {
            return DateTime.Now.ToString() + ":" + DateTime.Now.Millisecond.ToString() + " ";
        }
        #endregion

        #region 报警属性
        public string Warning { get; set; }
        public void AddWarning(string StrWar, bool PLCWarning = false)
        {
            if (string.IsNullOrEmpty(Warning))
                Warning = StrWar + Environment.NewLine;
            else
            {
                if (Warning.Contains(StrWar))
                    return;
                Warning += StrWar + Environment.NewLine;
            }
            this.OnPropertyChanged("Warning");
        }
        public void RemoveWarning(string StrWar)
        {
            if (string.IsNullOrEmpty(Warning))
                return;
            if (!Warning.Contains(StrWar))
            {
                return;
            }
            else
            {
                if (Warning.Contains(StrWar + Environment.NewLine))
                    Warning = Warning.Replace(StrWar + Environment.NewLine, "");

                //Warning = Warning.Replace(StrWar, "");
            }
            this.OnPropertyChanged("Warning");
        }
        #endregion

        #region 加载作业
        public void LoadJob()
        {
            JobManager.LoadJobs(Jobs);
            SelectedJob = null;
            ShowLoadJobWindow = !ShowLoadJobWindow;
        }
     
        public ObservableCollection<Job> Jobs { get; set; }
      

        public Job mCurJob;
        public Job CurJob
        {
            get { return JobManager.CurJob; }
            set
            {
                JobManager.CurJob = value;
                this.OnPropertyChanged("CurJob");
            }
        }
        public Job mSelectedJob;
        public Job SelectedJob
        {
            get { return mSelectedJob; }
            set
            {
                mSelectedJob = value;
                this.OnPropertyChanged("SelectedJob");
            }
        }
   
        RelayCommand _DialogCancel;
        public ICommand MonitorViewModelDialogCancel
        {
            get
            {
                if (_DialogCancel == null)
                    _DialogCancel = new RelayCommand(param => this.Task_DialogCancel());
                return _DialogCancel;
            }
        }
        public void Task_DialogCancel()
        {
            ShowLoadJobWindow = !ShowLoadJobWindow;
        }
        RelayCommand _DialogConfirm;
        public ICommand MonitorViewModelDialogConfirm
        {
            get
            {
                if (_DialogConfirm == null)
                    _DialogConfirm = new RelayCommand(param => this.Task_DialogConfirm());
                return _DialogConfirm;
            }
        }
        public void Task_DialogConfirm()
        {
            if (SelectedJob != null)
            {
                CurJob = SelectedJob;
                KATOPLog("加载作业");
            }
            ShowLoadJobWindow = !ShowLoadJobWindow;

        }
        #endregion

        #region 命令

        RelayCommand _ActionCommand;
        public ICommand MonitorViewModelCommand
        {
            get
            {
                if (_ActionCommand == null)
                    _ActionCommand = new RelayCommand(param => this.Action(param as string));
                return _ActionCommand;
            }
        }
        public void Action(string param)
        {
            if (param == "Login")
            {
                if (!HasLogin)
                { ShowLoginWindow = !ShowLoginWindow; }
                else
                {                 
                    ClearLoginMemory();
                }
            }
            else if (param == "LoadJob")
            {
                LoadJob();                
            }


            #region LoginWindow 命令
            if (param == "LoginWindow_Login")
            {
                LoginWindow_Do();
            }
            else if (param == "LoginWindow_Cancel")
            {
                ShowLoginWindow = !ShowLoginWindow;
            }
            else if (param == "LoginWindow_Register")
            {
                LoginWindow_Do();
                if (HasLogin)
                {
                    for (int i = 0; i < CurrentPerson.Level; i++)
                    { Level.Add(i+1); }
                    ShowRegisterWindow = !ShowRegisterWindow;                  
                }                
            }
            #endregion


            #region  RegisterWindow 命令
            if (param == "RegisterWindow_Add")
            {
                AddPerson();
            }
            else if (param == "RegisterWindow_Delete")
            {
                DeletePerson();
            }
            #endregion

        }
        #endregion

        #region MonitorView 属性
        private bool show;

        /// <summary>
        /// 显示登录窗口 True为显示，false为关闭
        /// </summary>
        public bool ShowLoginWindow
        {
            get { return show; }
            set
            {
                show = value;
                OnPropertyChanged("ShowLoginWindow");
            }
        }



        private bool registershow;

        /// <summary>
        /// 显示注册窗口 True为显示，false为关闭
        /// </summary>
        public bool ShowRegisterWindow
        {
            get { return registershow; }
            set
            {
                registershow = value;
                OnPropertyChanged("ShowRegisterWindow");
            }
        }

        private bool loadjobshow;

        /// <summary>
        /// 显示加载作业窗口 True为显示，false为关闭
        /// </summary>
        public bool ShowLoadJobWindow
        {
            get { return loadjobshow; }
            set
            {
                loadjobshow = value;
                OnPropertyChanged("ShowLoadJobWindow");
            }
        }

        
        bool mHasLogin = false;     
        /// <summary>
        /// 用于MonitorView上权限控制，只要登录就能使用
        /// </summary>
        public bool HasLogin
        {
            get { return mHasLogin; }
            set
            {
                mHasLogin = value;
                if (HasLogin)
                {
                    LoginState = "注销";
                    KATOPLog("登录成功"+"|"+User);
                }
                else
                {
                    LoginState = "登录";
                    KATOPLog("注销");                    
                }
                this.OnPropertyChanged("HasLogin");
            }
        }


        bool mHasLoginOtherWindow = false;
        /// <summary>
        /// 用于其他页面上权限控制，受制与Level
        /// </summary>
        public bool HasLoginOtherWindow
        {
            get { return mHasLoginOtherWindow; }
            set
            {
                mHasLoginOtherWindow = value;
                this.OnPropertyChanged("HasLoginOtherWindow");
            }
        }



        private string loginstate="登录";
        /// <summary>
        /// LoginWindow的登录状态
        /// </summary>

        public string LoginState
        {
            get { return loginstate; }
            set
            {
                loginstate = value;
                OnPropertyChanged("LoginState");
            }
        }
        #endregion

        #region MonitorView 方法
        /// <summary>
        /// 清除登录参数
        /// </summary>
        public void ClearLoginMemory()
        {
            try
            {
                User = null;
                PassWord = null;
                CurrentPerson = null;

                RegisterLevel = null;
                RegisterPassword = null;
                RegisterUser = null;

                ShowRegisterWindow = false;
                HasLoginOtherWindow = false;
                HasLogin = false;
            }
            catch (Exception ex)
            {
                KATOPLog("ClearLoginMemory()"+ex.Message);
            }
        }
        #endregion


        #region LoginWindow 属性

        private string password;
        /// <summary>
        /// LoginWindow的登录密码
        /// </summary>

        public string PassWord
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("PassWord");
            }
        }


        private string user;
        /// <summary>
        /// LoginWindow的登录用户
        /// </summary>

        public string User
        {
            get { return user; }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }
        #endregion

        #region LoginWindow 方法
        /// <summary>
        /// 查询是否存在当前用户名和密码
        /// </summary>
        /// <returns></returns>
        public Person CheckUser(Person p)
        {
            foreach (var item in RegisterUsers)
            {
                if (p.IsSame(item))
                    return item;
            }
            return null;
        }

        public Person CheckUserName(Person p)
        {
            foreach (var item in RegisterUsers)
            {
                if (p.IsSameUserName(item))
                    return item;
            }
            return null;
        }


        public void LoginWindow_Do()
        {
            //判断用户名密码是否为空，不为空就去判断是否注册过，注册过就登录成功，否则失败
            if (!string.IsNullOrEmpty(User) && !string.IsNullOrEmpty(PassWord))
            {
                Person temp = new Person(User, PassWord);
                Person result = CheckUser(temp);
                if (result!=null)
                {
                    //如果能查到，就允许登录
                    HasLogin = true;
                    if (temp.UserName == "KATOP")
                        PassWordVisibility = 200;
                    
                    ShowLoginWindow = !ShowLoginWindow;
                    CurrentPerson = result;
                }
                else
                {
                    KATOPLog("用户名或密码错误!");
                }
            }
            else
            {
                KATOPLog("用户名或密码为空，请重新输入!");
            }
        }
        #endregion

        #region RegisterWindow 属性

        public ObservableCollection<Person> registerusers = new ObservableCollection<Person>();
        /// <summary>
        /// RegisterWindow的用户列表
        /// </summary>
        public ObservableCollection<Person> RegisterUsers
        {
            get
            {
                return registerusers;
            }
            set
            {
                registerusers = value;
                OnPropertyChanged("RegisterUsers");
            }
        }

        public int passwordvisibility = 0;
        /// <summary>
        /// 注册列表显示密码
        /// </summary>
        public int PassWordVisibility
        {
            get
            {
                return passwordvisibility;
            }
            set
            {
                passwordvisibility = value;
                OnPropertyChanged("PassWordVisibility");
            }
        }


        public ObservableCollection<int> level = new ObservableCollection<int>();
        /// <summary>
        /// RegisterWindow的级别
        /// </summary>
        public ObservableCollection<int> Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                OnPropertyChanged("Level");
            }
        }

        private Person currentPerson;
        /// <summary>
        /// 当前用户
        /// </summary>
        public Person CurrentPerson
        {
            get
            {         
                return currentPerson;
            }
            set
            {
                if (value != null)
                {
                    if (value.Level >= 2)
                        HasLoginOtherWindow = true;
                }         
                currentPerson = value;
                OnPropertyChanged("CurrentPerson");
            }
        }


        private Person selectPerson;
        public Person SelectPerson
        {
            get { return selectPerson; }
            set
            {
                selectPerson = value;
                OnPropertyChanged("SelectPerson");
            }
        }


        public string registerlevel;
        public string RegisterLevel
        {
            get { return registerlevel; }
            set
            {
                registerlevel = value;
                OnPropertyChanged("RegisterLevel");
            }
        }


        public string registeruser;
        public string RegisterUser
        {
            get { return registeruser; }
            set
            {
                registeruser = value;
                OnPropertyChanged("RegisterUser");
            }
        }

        public string registerpassword;
        public string RegisterPassword  
        {
            get { return registerpassword; }
            set
            {
                registerpassword = value;
                OnPropertyChanged("RegisterPassword");
            }
        }
        #endregion

        #region RegisterWindow 方法
        public  string UserPath = AppDomain.CurrentDomain.BaseDirectory + "Users.config";
        public  void LoadPersons()
        {
            try
            {
                if (File.Exists(UserPath))
                {
                    FileStream fileStream = new FileStream(UserPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    BinaryFormatter mBinFmat = new BinaryFormatter();
                    int count = (int)mBinFmat.Deserialize(fileStream);
                    for (int mTick = 0; mTick < count; mTick++)
                    {
                        Person mTemp = mBinFmat.Deserialize(fileStream) as Person;
                        RegisterUsers.Add(mTemp);
                    }
                    fileStream.Close();
                }
                else
                {
                    RegisterUsers.Add(new Person(Global.TopUser, Global.TopPassword, 3));
                }         
            }
            catch (Exception ex)
            {
                KATOPLog("LoadPersons()"+ex.Message);
            }
        }
        public  void SavePersons()
        {
            FileStream fileStream = new FileStream(UserPath, FileMode.Create);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, RegisterUsers.Count);
            for (int mTick = 0; mTick < RegisterUsers.Count; mTick++)
            {
                b.Serialize(fileStream, RegisterUsers[mTick]);
            }
            fileStream.Close();
        }

        public void AddPerson()
        {
            if (!string.IsNullOrEmpty(RegisterUser) && !string.IsNullOrEmpty(RegisterPassword) && !string.IsNullOrEmpty(RegisterLevel))
            {
                Person p = CheckUserName(new Person(RegisterUser, RegisterPassword));
                if (p == null)
                {
                    RegisterUsers.Add(new Person(RegisterUser, RegisterPassword, Convert.ToInt16(RegisterLevel)));
                    SavePersons();
                }
                else
                {
                    KATOPLog(RegisterUser+"已经存在!");
                }
               
            }
            else
            {
                KATOPLog("注册参数不能为空！");
            }
          
        }

        public void DeletePerson()
        {
            //当前用户的权限必须高于等于所删除的账号，最高账户除外
            if (CurrentPerson.Level >= SelectPerson.Level && SelectPerson.UserName != Global.TopUser)
            {
                RegisterUsers.Remove(SelectPerson);
                SavePersons();
            }
            else
            {
                KATOPLog("无权删除！");
            }
        }
        #endregion
    }

}

