using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace InVisualLib.logger
{
    public class clsLog
    {
        // enum dei livelli possibili
        public enum levels { NONE, INFORMATION = 2, WARNING = 4, ERROR = 8 };
        // valore di soglia
        int m_threshold = 0;
        // percorso completo del file
        string m_fileFullName = "";
        // stringa di connessione
        string m_connectionString = "";
        // tabella ove effettuare il logging
        string m_tableName = "";
        // oggetto per il lock
        object sync = new object();
        // massima dimensione del log
        int m_maxLength = 5;

        public clsLog(int threshold)
        {
            // recupera i parametri
            m_threshold = threshold;
            // recupera il percorso dell'eseguibile
            m_fileFullName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // compone il percorso completo
            string fileName = m_fileFullName.Substring(m_fileFullName.LastIndexOf(@"\"),
                m_fileFullName.Length - m_fileFullName.LastIndexOf(@"\")).Split('.')[0] + ".log";
            m_fileFullName = m_fileFullName.Substring(0, m_fileFullName.LastIndexOf(@"\")) + fileName;
        }

        public clsLog(int threshold, string path)
        {
            // recupera i parametri
            m_threshold = threshold;
            // recupera il percorso dell'eseguibile
            m_fileFullName = path;
        }

        public clsLog(int threshold, string connectionString, string tableName)
        {
            // recupera i parametri
            m_threshold = threshold;
            m_connectionString = connectionString;
            m_tableName = tableName;
        }

        public int maxLenght
        {
            set { m_maxLength = value; }
        }

        public void log2(levels level, string applicationName, string message)
        {

            try
            {
                lock (sync)
                {
                    int currentLevel = (int)level;
                    // traccia solamente se abilitato
                    if ((m_threshold & currentLevel) != currentLevel) return;
                    // verifica se splittare il file
                    FileInfo f = new FileInfo(m_fileFullName);
                    if (f.Exists && f.Length > m_maxLength * 1024000)
                    {
                        string destination = m_fileFullName.Replace(".log", "_old.log");
                        if (File.Exists(destination)) File.Delete(destination);
                        File.Move(m_fileFullName, destination);
                    }
                    // traccia un file
                    StreamWriter sw = new StreamWriter(m_fileFullName, true);
                    string fullMessage = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString()
                        + "|" + currentLevel.ToString() + "|"
                        + applicationName + "|"
                        + message;
                    sw.WriteLine(fullMessage);
                    sw.Close();
                }
            }
            catch (Exception e)
            {

            }
        }

        public void log(levels level, string applicationName, string message)
        {

            try
            {
                lock (sync)
                {
                    int currentLevel = (int)level;
                    // traccia solamente se abilitato
                    if ((m_threshold & currentLevel) != currentLevel) return;
                    // verifica se splittare il file
                    FileInfo f = new FileInfo(m_fileFullName);
                    if (f.Exists && f.Length > m_maxLength * 1024000)
                    {
                        string destination = m_fileFullName.Replace(".log", "_old.log");
                        if (File.Exists(destination)) File.Delete(destination);
                        File.Move(m_fileFullName, destination);
                    }
                    // traccia un file
                    StreamWriter sw = new StreamWriter(m_fileFullName, true);
                    string fullMessage = DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString()
                        + "|" + currentLevel.ToString() + "|"
                        + applicationName + "|"
                        + message;
                    sw.WriteLine(fullMessage);
                    sw.Close();
                }
            }
            catch (Exception e)
            {

            }
        }


        /// <summary>
        /// traccia su database
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void logOnDb(int level, string message)
        {

        }
    }
}

