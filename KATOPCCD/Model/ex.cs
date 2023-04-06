using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KATOPCCD.Model
{
    public static class ex
    {
        public static void Ini()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        //新建加密Excel,Path文件路径,name文件名，sheet添加的表名，默认sheet1，创建的excel的密码
        public static void CreateExcel(string Path, string name, string password, string sheet = "sheet1")
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Path + "//" + name);
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheet);
                    package.Save(password);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //新建无需密码的Excel
        public static void CreateExcel(string Path, string name, string sheet = "sheet1")
        {
            try
            {
                FileInfo fileInfo = new FileInfo(Path + "//" + name);
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheet);
                    package.Save();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //向Excel中单个写入数据
        public static void WriteExcelData(string Path, int row, int col, string data, string password = "")
        {
            try
            {
                if (!File.Exists(Path))
                {
                    Console.WriteLine("文件不存在");
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(Path);
                    if (password == "")
                    {
                        using (ExcelPackage package = new ExcelPackage(fileInfo))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                            worksheet.Cells[row, col].Value = data;
                            package.Save();
                        }
                    }
                    else
                    {
                        using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                            worksheet.Cells[row, col].Value = data;
                            package.Save();

                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        //向Excel中单行写入数据,IsCover是覆盖写，还是接着前面的值写
        public static void WriteExcelRow(string Path, int row, string[] data, string password = "", bool IsCover = true)
        {
            try
            {
                if (!File.Exists(Path))
                {
                    Console.WriteLine("文件不存在");
                }
                else
                {
                    if (IsCover)
                    {
                        #region
                        FileInfo fileInfo = new FileInfo(Path);
                        if (password == "")
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                for (int i = 1; i <= data.Length; i++)
                                { worksheet.Cells[row, i].Value = data[i - 1]; }

                                package.Save();

                            }
                        }
                        else
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                for (int i = 1; i <= data.Length; i++)
                                { worksheet.Cells[row, i].Value = data[i - 1]; }

                                package.Save();

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        FileInfo fileInfo = new FileInfo(Path);
                        if (password == "")
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet

                                int index = 1;
                                while (worksheet.Cells[row, index].Value != null)
                                {
                                    index++;
                                }

                                for (int i = index; i <= index + data.Length - 1; i++)
                                { worksheet.Cells[row, index].Value = data[i - index]; }

                                package.Save();
                            }
                        }
                        else
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                int index = 1;
                                while (worksheet.Cells[row, index].Value != null)
                                {
                                    index++;
                                }

                                for (int i = index; i <= index + data.Length - 1; i++)
                                { worksheet.Cells[row, index].Value = data[i - index]; }

                                package.Save();

                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        //向Excel中单列写入数据
        public static void WriteExcelCol(string Path, int col, string[] data, string password = "", bool IsCover = true)
        {
            try
            {
                if (!File.Exists(Path))
                {
                    Console.WriteLine("文件不存在");
                }
                else
                {
                    if (IsCover)
                    {
                        #region
                        FileInfo fileInfo = new FileInfo(Path);
                        if (password == "")
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                for (int i = 1; i <= data.Length; i++)
                                { worksheet.Cells[i, col].Value = data[i - 1]; }

                                package.Save();

                            }
                        }
                        else
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                for (int i = 1; i <= data.Length; i++)
                                { worksheet.Cells[i, col].Value = data[i - 1]; }

                                package.Save();

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        FileInfo fileInfo = new FileInfo(Path);
                        if (password == "")
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet

                                int index = 1;
                                while (worksheet.Cells[index, col].Value != null)
                                {
                                    index++;
                                }

                                for (int i = index; i <= index + data.Length - 1; i++)
                                { worksheet.Cells[i, col].Value = data[i - index]; }

                                package.Save();

                            }
                        }
                        else
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet

                                int index = 1;
                                while (worksheet.Cells[index, col].Value != null)
                                {
                                    index++;
                                }

                                for (int i = index; i <= index + data.Length - 1; i++)
                                { worksheet.Cells[i, col].Value = data[i - index]; }

                                package.Save();

                            }
                        }

                        #endregion
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        //读取Excel中单个数据
        public static string ReadExcelData(string Path, int row, int col, string password = "")
        {
            try
            {
                if (!File.Exists(Path))
                {
                    Console.WriteLine("文件不存在");
                    return "文件不存在";
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(Path);
                    if (password == "")
                    {
                        using (ExcelPackage package = new ExcelPackage(fileInfo))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                            if (worksheet.Cells[row, col].Value == null)
                                return "";
                            return worksheet.Cells[row, col].Value.ToString();


                        }
                    }
                    else
                    {
                        using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                            if (worksheet.Cells[row, col].Value == null)
                                return "";
                            return worksheet.Cells[row, col].Value.ToString();


                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        //读取Excel中单行数据,如果不填写count数量，则默认一行知道读到空值为止
        public static string[] ReadExcelRow(string Path, int row, int count = 0, string password = "")
        {
            try
            {
                if (!File.Exists(Path))
                {
                    Console.WriteLine("文件不存在");
                    return new string[] { "文件不存在" };
                }
                else
                {
                    if (count == 0)
                    {
                        #region
                        FileInfo fileInfo = new FileInfo(Path);
                        if (password == "")
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                List<string> temp = new List<string>();

                                int index = 1;
                                while (worksheet.Cells[row, index].Value != null)
                                {
                                    temp.Add(worksheet.Cells[row, index].Value.ToString());
                                    index++;
                                }

                                return temp.ToArray();

                            }
                        }
                        else
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                List<string> temp = new List<string>();

                                int index = 1;
                                while (worksheet.Cells[row, index].Value != null)
                                {
                                    temp.Add(worksheet.Cells[row, index].Value.ToString());
                                    index++;
                                }

                                return temp.ToArray();
                            }

                        }

                        #endregion
                    }
                    else
                    {
                        #region
                        FileInfo fileInfo = new FileInfo(Path);
                        if (password == "")
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                List<string> temp = new List<string>();
                                for (int i = 1; i <= count; i++)
                                {
                                    if (worksheet.Cells[row, i].Value != null)
                                        temp.Add(worksheet.Cells[row, i].Value.ToString());
                                    //else
                                    //    temp.Add("NULL");
                                }


                                return temp.ToArray();
                            }
                        }
                        else
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                List<string> temp = new List<string>();
                                for (int i = 1; i <= count; i++)
                                {
                                    if (worksheet.Cells[row, i].Value != null)
                                        temp.Add(worksheet.Cells[row, i].Value.ToString());
                                    //else
                                    //    temp.Add("NULL");
                                }


                                return temp.ToArray();
                            }

                        }

                        #endregion
                    }
                }

            }
            catch (Exception)
            {
                return new string[] { "" };
                throw;
            }

        }


        //读取Excel中单列数据
        public static string[] ReadExcelCol(string Path, int col, int count = 0, string password = "")
        {
            try
            {
                if (!File.Exists(Path))
                {
                    Console.WriteLine("文件不存在");
                    return new string[] { "文件不存在" };
                }
                else
                {
                    if (count == 0)
                    {
                        #region
                        FileInfo fileInfo = new FileInfo(Path);
                        if (password == "")
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                List<string> temp = new List<string>();

                                int index = 1;
                                while (worksheet.Cells[index, col].Value != null)
                                {
                                    temp.Add(worksheet.Cells[index, col].Value.ToString());
                                    index++;
                                }

                                return temp.ToArray();
                            }
                        }
                        else
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                List<string> temp = new List<string>();
                                int index = 1;
                                while (worksheet.Cells[index, col].Value != null)
                                {
                                    temp.Add(worksheet.Cells[index, col].Value.ToString());
                                    index++;
                                }

                                return temp.ToArray();

                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        FileInfo fileInfo = new FileInfo(Path);
                        if (password == "")
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                List<string> temp = new List<string>();
                                for (int i = 1; i <= count; i++)
                                {
                                    if (worksheet.Cells[i, col].Value != null)
                                        temp.Add(worksheet.Cells[i, col].Value.ToString());
                                    //else
                                    //    temp.Add("NULL");
                                }


                                return temp.ToArray();
                            }
                        }
                        else
                        {
                            using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                            {
                                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                                List<string> temp = new List<string>();
                                for (int i = 1; i <= count; i++)
                                {
                                    if (worksheet.Cells[i, col].Value != null)
                                        temp.Add(worksheet.Cells[i, col].Value.ToString());
                                    //else
                                    //    temp.Add("NULL");
                                }

                                return temp.ToArray();

                            }
                        }
                        #endregion
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new string[] { "" };
            }

        }

        //读取Excel里的全部信息,按行存
        public static List<List<string>> ReadExcel(string Path, string password = "")
        {
            try
            {
                if (!File.Exists(Path))
                {
                    Console.WriteLine("文件不存在");
                    return new List<List<string>>() { new List<string>() { "文件不存在" } };
                }
                else
                {
                    if (password == "")
                    {
                        #region 
                        FileInfo fileInfo = new FileInfo(Path);
                        List<List<string>> Result = new List<List<string>>();
                        using (ExcelPackage package = new ExcelPackage(fileInfo))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                            List<string> temp = new List<string>();

                            //要先知道有多少行
                            int RowCount = 0;
                            while (worksheet.Cells[RowCount + 1, 1].Value != null)
                            {
                                RowCount++;
                            }




                            for (int row = 1; row <= RowCount; row++)
                            {
                                int index = 1;
                                while (worksheet.Cells[row, index].Value != null)
                                {
                                    temp.Add(worksheet.Cells[row, index].Value.ToString());
                                    index++;
                                }
                                Result.Add(temp);
                                temp = new List<string>();
                            }

                            return Result;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 
                        FileInfo fileInfo = new FileInfo(Path);
                        List<List<string>> Result = new List<List<string>>();
                        using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                            List<string> temp = new List<string>();

                            //要先知道有多少行
                            int RowCount = 0;
                            while (worksheet.Cells[RowCount + 1, 1].Value != null)
                            {
                                RowCount++;
                            }




                            for (int row = 1; row <= RowCount; row++)
                            {
                                int index = 1;
                                while (worksheet.Cells[row, index].Value != null)
                                {
                                    temp.Add(worksheet.Cells[row, index].Value.ToString());
                                    index++;
                                }
                                Result.Add(temp);
                                temp = new List<string>();
                            }

                            return Result;
                        }
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<List<string>>() { new List<string>() { ex.Message } };

            }
        }

        //读取Excel里的全部信息,按列存
        public static List<List<string>> ReadExcelByCol(string Path, string password = "")
        {
            try
            {
                if (!File.Exists(Path))
                {
                    Console.WriteLine("文件不存在");
                    return new List<List<string>>() { new List<string>() { "文件不存在" } };
                }
                else
                {
                    if (password == "")
                    {
                        #region 
                        FileInfo fileInfo = new FileInfo(Path);
                        List<List<string>> Result = new List<List<string>>();
                        using (ExcelPackage package = new ExcelPackage(fileInfo))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                            List<string> temp = new List<string>();

                            //要先知道有多少列
                            int ColCount = 0;
                            while (worksheet.Cells[1, ColCount + 1].Value != null)
                            {
                                ColCount++;
                            }




                            for (int col = 1; col <= ColCount; col++)
                            {
                                int index = 1;
                                while (worksheet.Cells[index, col].Value != null)
                                {
                                    temp.Add(worksheet.Cells[index, col].Value.ToString());
                                    index++;
                                }
                                Result.Add(temp);
                                temp = new List<string>();
                            }

                            return Result;
                        }
                        #endregion
                    }
                    else
                    {
                        #region 
                        FileInfo fileInfo = new FileInfo(Path);
                        List<List<string>> Result = new List<List<string>>();
                        using (ExcelPackage package = new ExcelPackage(fileInfo, password))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];//获取第一个sheet
                            List<string> temp = new List<string>();

                            //要先知道有多少列
                            int ColCount = 0;
                            while (worksheet.Cells[1, ColCount + 1].Value != null)
                            {
                                ColCount++;
                            }
                            for (int col = 1; col <= ColCount; col++)
                            {
                                int index = 1;
                                while (worksheet.Cells[index, col].Value != null)
                                {
                                    temp.Add(worksheet.Cells[index, col].Value.ToString());
                                    index++;
                                }
                                Result.Add(temp);
                                temp = new List<string>();
                            }

                            return Result;
                        }
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<List<string>>() { new List<string>() { ex.Message } };

            }
        }

    }
}
