using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICustomDataHandler
{
    interface ICustomDataHandler
    {
        string Run();
    }
    public class Logger
    {
        public static void LogWriter(string logMessage)
        {
            string m_exePath = Directory.GetCurrentDirectory() + "\\logging";
            try
            {
                System.IO.Directory.CreateDirectory(m_exePath);
                string filename = "log_" + DateTime.Now.ToString("yyyy.MM.dd_HH.mm") + ".txt";
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + filename))
                {
                    try
                    {
                        w.Write("\r\nLog Entry : ");
                        w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                            DateTime.Now.ToLongDateString());
                        w.WriteLine("  :");
                        w.WriteLine("  :{0}", logMessage);
                        w.WriteLine("-------------------------------");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("File write error");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("File handling error.");
            }
        }
    }
}
