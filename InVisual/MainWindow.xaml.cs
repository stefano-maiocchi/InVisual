using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using Microsoft.Win32;
using System.IO;

namespace InVisual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate void OnWriteMessageHandler(string message, int mode);
        string m_currentFileName = "";

        public MainWindow()
        {
            InitializeComponent();
            this.Title += " - Ver. " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        #region toolbar

        private void m_OnToolbarClicked(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.Tag.ToString())
            {
                case "OPEN":
                    m_openFile();
                    break;

                case "CONFIGURE":
                    m_configure();
                    break;                

                case "EXIT":
                    m_exit();
                    break;
            }
            
        }

        void m_openFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = "Selezionare la FE";
            ofd.Filter = "xml Files (*.xml)|*.xml|p7m Files (*.p7m)|*.p7m";
            ofd.InitialDirectory = InVisualLib.configuration.clsXml.getValue(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InVisual.xml"), "configuration", "lastXmlPath");
      
            if (ofd.ShowDialog() == true)
            {
                m_currentFileName = ofd.FileName;
                if (System.IO.Path.GetExtension(m_currentFileName).Equals(".P7M",StringComparison.InvariantCultureIgnoreCase))
                {
                    // esegue la decodifica del file
                    m_currentFileName = m_decodeAndSave();
                    if (m_currentFileName == string.Empty) return;
                }
                // recupera e visualizza il file
                m_showInBrowser(m_currentFileName);
                InVisualLib.configuration.clsXml.setValue(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InVisual.xml"), "configuration", "lastXmlPath",System.IO.Path.GetDirectoryName(ofd.FileName));
            }

        }


        string m_decodeAndSave()
        {
            try
            {                
                string xml = InVisualLib.p7m.clsDecrypt.ReadXmlSigned(m_currentFileName);
                Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
                string filename = $"{System.IO.Path.GetFileNameWithoutExtension(m_currentFileName)}.xml";
                filename = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(m_currentFileName), filename);
                filename = $"{filename.ToUpper().Replace(".XML", "")}.XML";
                // verifica se il file esiste nella destinazione e chiede se sovrascrivere                
                if (File.Exists(filename))
                {
                    if (MessageBoxResult.No == MessageBox.Show("Il file decrittato è già presente nella cartella di destinazione, sovrascrivere?", "Informazione", MessageBoxButton.YesNo, MessageBoxImage.Information))
                        return string.Empty;
                }               
                FileStream fs = new FileStream(filename, FileMode.Create);
                stream.CopyTo(fs);
                fs.Close();
                fs = null;

                return filename;
            }
            catch (Exception ex)
            {                
                MessageBox.Show($"{ex.Message}-{ex.StackTrace}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }

        }

        void m_configure()
        {
            pages.wndConfigure wnd = new pages.wndConfigure();
            wnd.ShowDialog();
        }                     

        void m_exit()
        {
            // verifica eventuali modifiche

            if (MessageBox.Show("Uscire dall'applicazione?", "Informazione", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No) return;
            this.Close();
        }

        #endregion

        #region messages

        private void m_OnShowSystemMessages(object sender, RoutedEventArgs e)
        {
            Expander exp = sender as Expander;
            exp.Header = "Selezionare per nascondere i messaggi di sistema".ToUpper();
        }

        private void m_OnHideSystemMessages(object sender, RoutedEventArgs e)
        {
            Expander exp = sender as Expander;            
            exp.Header = "Selezionare per visualizzare i messaggi di sistema".ToUpper();
        }

        void m_writeMessage(string message, int mode)
        {
            //todo: visualizzare icona
            message = $"{DateTime.Now}\t{message}";
            lstMessages.Items.Add(message);
            lstMessages.SelectedIndex = lstMessages.Items.Count - 1;
        }
        
        #endregion

        #region preview        

        void m_showInBrowser(string xmlfile)
        {
            lblTitle.Content = $"ANTEPRIMA FATTURA : {System.IO.Path.GetFileName(xmlfile)}";
            string xsltFullPath = InVisualLib.configuration.clsXml.getValue(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InVisual.xml"), "configuration", "xslt");
            string html = InVisualLib.xml.clsXmlTransform.getHtml(xmlfile, xsltFullPath);
            
            wb.NavigateToString(html);
        }

        #endregion



    }
}
