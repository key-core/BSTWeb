using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MedinetLibrary
{
    public static class Excepciones
    {
        public static void LogException(Exception exc, string className, string methodName)
        {
            try
            {
                string appsDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) +
                    Path.DirectorySeparatorChar;

                string logFile = appsDirectory + "log.txt";
                if (!File.Exists(logFile))
                {
                    FileStream fs = File.Create(logFile);
                    fs.Flush();
                    fs.Close();
                }

                // Open the log file for append and write the log
                StreamWriter sw = new StreamWriter(logFile, true);
                sw.WriteLine("********** {0} **********", DateTime.Now);
                sw.Write("Exception Type: ");
                sw.WriteLine(exc.GetType().ToString());
                sw.WriteLine("Exception: " + exc.Message);
                sw.WriteLine("Class Name: " + className);
                sw.WriteLine("Method Name: " + methodName);
                sw.WriteLine("Stack Trace: ");
                if (exc.StackTrace != null)
                {
                    sw.WriteLine(exc.StackTrace);
                    sw.WriteLine();
                }
                sw.Close();
            }
            catch (Exception ex)
            {
                Excepciones.LogException(ex, "Excepciones", "LogException");
            }
        }
    }
}
