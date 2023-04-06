using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KATOPCCD.ViewModel
{
  
    public class MainWindowViewModel:DXH.ViewModel.ViewModelBase
    {
        MainWindow mMainWindow;
        public MonitorViewModel MonitorViewModel { get; set; }
        public JobViewModel JobViewModel { get; set; }
        public MainWindowViewModel(MainWindow _MainWindow)
        {
            mMainWindow = _MainWindow;
            MonitorViewModel = new MonitorViewModel(mMainWindow);
            JobViewModel = new JobViewModel(mMainWindow);
        }

        //TabControl控件选择属性
        int mTabIndex = 0;
        public int TabIndex
        {
            get { return mTabIndex; }
            set
            {
                mTabIndex = value;
            }
        }


    }
}
