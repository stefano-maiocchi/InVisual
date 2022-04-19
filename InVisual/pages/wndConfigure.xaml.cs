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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace InVisual.pages
{
    /// <summary>
    /// Interaction logic for wndBrowse.xaml
    /// </summary>
    public partial class wndConfigure : Window
    {
        public wndConfigure()
        {
            InitializeComponent();

            m_loadParms();

        }

        void m_loadParms()
        {
            utbXslt.text = InVisualLib.configuration.clsXml.getValue("InVisual.xml", "configuration", "xslt", 
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xsl", "fatturaordinaria_v1.2.xsl"));
            chkLog.IsChecked = InVisualLib.configuration.clsXml.getValue("InVisual.xml", "configuration", "log").Equals("Y", StringComparison.InvariantCultureIgnoreCase);
        }

        

        private void m_OnExecuteAction(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.Tag.ToString())
            {
                case "SAVE":
                    m_save();
                    break;

                case "EXIT":
                    m_exit();
                    break;
            }
        }

        void m_save()
        {
            InVisualLib.configuration.clsXml.setValue("InVisual.xml", "configuration", "xslt", utbXslt.text);
            InVisualLib.configuration.clsXml.setValue("InVisual.xml", "configuration", "log", chkLog.IsChecked.Value ? "Y" : "N");

            MessageBox.Show("Memorizzato!", "Informazione", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        void m_exit()
        {
            //todo: verificare eventuali modifiche e chiedere se salvare
            this.Close();
        }

        private void m_OnBrowseButtonClick(object sender)
        {
            controls.uTextButton utb = sender as controls.uTextButton;

            switch (utb.Tag.ToString())
            {
                case "XSLT":
                    m_browse(utbXslt, "Selezionare il file xslt", "Xsl files (*.xsl)|*.xsl|Xslt files(*.xslt)|*.xslt"); 
                    break;
            }
            
        }

        void m_browse(controls.uTextButton utb, string caption, string filter)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Title = caption;
            ofd.Filter = filter;
            ofd.InitialDirectory = InVisualLib.configuration.clsXml.getValue(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "InVisual.xml"), "configuration", "lastXmlPath");
            if (!ofd.ShowDialog().Value) return;
            utb.text = ofd.FileName;
        }

       
    }
}
