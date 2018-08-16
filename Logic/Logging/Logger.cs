using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Logging {
    public static class Logger {
        private static StringBuilder log = new StringBuilder();

        public static void AddToLog(string message) {
            log.Insert(0, $"{message}{Environment.NewLine}");
        }

        public static string Log { get => log.ToString(); }
    }
}
