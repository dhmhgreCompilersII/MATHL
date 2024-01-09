using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MATHL {

    public abstract class LogRecord {
        public abstract string LogResult(int verbocity);
    }

    /// <summary>
    /// Centralize the logging class to handle interpreter reporting
    /// Apart from the messages the class will optionally report the class,
    /// method that generates the reporting
    /// </summary>
    public class Logging {
        private Dictionary<object, List<LogRecord>> m_logHistory;
        private int m_verbocity = 0;

        public Logging() {
            m_logHistory = new Dictionary<object, List<LogRecord>>();
        }

        public void LogStdOut(object sender, LogRecord record) {
            Console.WriteLine(record.LogResult(m_verbocity));
            if (!m_logHistory.ContainsKey(sender)) {
                m_logHistory[sender] = new List<LogRecord>();
                m_logHistory[sender].Add(record);
            }
        }

    }
}
