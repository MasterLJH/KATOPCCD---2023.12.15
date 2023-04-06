using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KATOPCCD.Model
{
    //全局静态类
     public static class Global
    {
        #region 全局静态常量
        //最高权限用户，密码
        public static string TopUser = "KATOP";
        public static string TopPassword = "KATOP123";
        #endregion

        #region 通用方法
        public static string FilePath = Directory.GetCurrentDirectory() + "/logs/";
        static object LogLock = new object();
        public static async void SaveLog(string message)
        {
            Task Task_SaveLog = Task.Run(() =>
            {
                try
                {
                    lock (LogLock)
                    {
                        if (!Directory.Exists(FilePath + DateTime.Now.ToString("yyyy-MM")))
                            Directory.CreateDirectory(FilePath + DateTime.Now.ToString("yyyy-MM"));
                        File.AppendAllText(FilePath + DateTime.Now.ToString("yyyy-MM") + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss    ") + ":" + DateTime.Now.Millisecond.ToString() + " " + message + Environment.NewLine);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SaveDebugLog:" + ex.Message);
                }
            });
            await Task_SaveLog;
        }

        #endregion

    }
}
