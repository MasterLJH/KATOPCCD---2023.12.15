using KATOPCCD.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KATOPCCD.View
{
    /// <summary>
    /// MonitorView.xaml 的交互逻辑
    /// </summary>
    public partial class MonitorView : UserControl
    {
        MainWindow mainWindow;
       // MonitorViewModel MonitorViewModel;

        public MonitorView()
        {
            InitializeComponent();
            mainWindow = (MainWindow)App.Current.MainWindow;
            this.SetBinding(showProperty, "ShowLoginWindow");//绑定登录窗口
            this.SetBinding(showRegisterProperty, "ShowRegisterWindow");//绑定注册窗口
            this.SetBinding(showLoadJobProperty, "ShowLoadJobWindow");//绑定加载作业窗口
            //MonitorViewModel = new MonitorViewModel(mainWindow);
            //this.DataContext = MonitorViewModel;

        }

        #region 登录窗口
        //public bool Show
        //{
        //    get { return (bool)GetValue(showProperty); }
        //    set { SetValue(showProperty, value); }
        //}

        private static LoginWindow Login = null;

        public static readonly DependencyProperty showProperty =
         DependencyProperty.Register("ShowLoginWindow", typeof(bool), typeof(MonitorView), new PropertyMetadata(new PropertyChangedCallback(
             (d, e) =>
             {
                 if ((bool)e.NewValue)
                 {
                     var mMianWindow = d as MonitorView;
                     Login = new LoginWindow
                     {
                         Owner = Application.Current.MainWindow,
                         DataContext = mMianWindow.DataContext,
                     };
                     Login.Show();
                 }
                 else
                 {
                     Login.Close();
                 }
             })));




        #endregion

        #region 注册窗口
  
        private static RegisterWindow Register = null;

        public static readonly DependencyProperty showRegisterProperty =
         DependencyProperty.Register("ShowRegisterWindow", typeof(bool), typeof(MonitorView), new PropertyMetadata(new PropertyChangedCallback(
             (d, e) =>
             {
                 if ((bool)e.NewValue)
                 {
                     var mMianWindow = d as MonitorView;
                     Register = new RegisterWindow
                     {
                         Owner = Application.Current.MainWindow,
                         DataContext = mMianWindow.DataContext,
                     };
                     Register.Show();
                 }
                 else
                 {
                     Register.Close();
                 }
             })));




        #endregion

        #region 加载作业窗口
        private static LoadJobWindow LoadJob = null;

        public static readonly DependencyProperty showLoadJobProperty =
         DependencyProperty.Register("ShowLoadJobWindow", typeof(bool), typeof(MonitorView), new PropertyMetadata(new PropertyChangedCallback(
             (d, e) =>
             {
                 if ((bool)e.NewValue)
                 {
                     var mMianWindow = d as MonitorView;
                     LoadJob = new LoadJobWindow
                     {
                         Owner = Application.Current.MainWindow,
                         DataContext = mMianWindow.DataContext,
                     };
                     LoadJob.Show();
                 }
                 else
                 {
                     LoadJob.Close();
                 }
             })));
        #endregion

        private void TextBoxWarning_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!TextBoxWarning.IsFocused)
                ScrollWarning.ScrollToEnd();
        }

        double ScrollOffset = 0;
        int SelectionStart = 0;
        int SelectionLength = 0;
        private void TextBox_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ScrollOffset = Scroll.VerticalOffset;
            if (!Scroll.IsMouseOver && SelectionLength == 0)
            {
                Scroll.ScrollToEnd();
                SelectionStart = 0;
                SelectionLength = 0;
            }
            else
            {
                if (SelectionLength == 0 && SelectionStart == 0)
                { }
                else
                    TextBox.Select(SelectionStart, SelectionLength);
                Scroll.ScrollToVerticalOffset(ScrollOffset);
            }
        }

    
    }
}
