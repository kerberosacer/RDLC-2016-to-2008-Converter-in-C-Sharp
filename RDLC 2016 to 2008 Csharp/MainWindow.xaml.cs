using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace RDLC_2016_to_2008_Csharp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBoxButton_Click(object sender, RoutedEventArgs e)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(TextBoxFile.Text);
            var root = xmlDoc.DocumentElement;

            if (root.Attributes["xmlns"].Value != "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")
                root.Attributes["xmlns"].Value = "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition";

            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("bk", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");

            var autoRefreshElements = root.GetElementsByTagName("AutoRefresh");
            while (autoRefreshElements.Count > 0)
                root.RemoveChild(autoRefreshElements.Item(0));

            var ReportParametersLayout = root.GetElementsByTagName("ReportParametersLayout");
            while (ReportParametersLayout.Count > 0)
                root.RemoveChild(ReportParametersLayout.Item(0));

            var ReportSections = root.GetElementsByTagName("ReportSections");

            if (ReportSections.Count > 0)
            {
                // Move content of ReportSections just below the block.
                var ReportSection = ReportSections.Item(0).ChildNodes;

                // First, copy the elements after
                var precedent = ReportSections.Item(0);
                foreach(XmlNode child in ReportSection.Item(0).ChildNodes)
                {
                    var clone = child.Clone();
                    root.InsertAfter(clone, precedent);
                    precedent = clone;
                }
    
                //After deleting the existing block
                while(ReportSections.Count > 0)
                    root.RemoveChild(ReportSections.Item(0));
                
            }

            xmlDoc.Save(@"C:\Users\jandrews\Desktop\xml.xml");

            System.Windows.MessageBox.Show("Ok,\n"+ @"File at C:\Users\jandrews\Desktop\xml.xml");
        }
    }
}
