using UnityEngine;
using System.Collections;

namespace Kpable.InGameConsole
{
    public class Log
    {
        public enum LogType
        {
            Info, Warning, Error, None
        }

        LogType logLevel = LogType.Warning;


        public void SetLogLevel(LogType level = LogType.Info)
        {

        }

        public void Info(string message)
        {
            if (logLevel <= LogType.Info)
            {
                // blue
                Console.Instance.WriteLine(message);
            }
        }

        public void Warn(string message)
        {
            if (logLevel <= LogType.Warning)
            {
                // yellow
                Console.Instance.WriteLine(message);
            }
        }

        public void Error(string message)
        {
            if (logLevel <= LogType.Error)
            {
                // red
                Console.Instance.WriteLine(message);
            }
        }
    }
}