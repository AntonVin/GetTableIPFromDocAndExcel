using word = Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using PlanIP;
using IpLibrary;
using forms = System.Windows.Forms;

namespace IpExporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtBoxPath.Text = InitPathDoc();
        }

        private string InitPathDoc()
        {
            var curDir = Environment.CurrentDirectory;
            string path = Directory.GetParent(curDir).Parent.Parent.FullName + "\\doc";
            return path;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var foderIsExists = Directory.Exists(txtBoxPath.Text);
            if (!foderIsExists)
            {
                MessageBox.Show("Такого директория не существует",
                    "Неверный путь",MessageBoxButton.OK,MessageBoxImage.Error);
                txtBoxPath.Focus();
                return;
            }
            var exporterDocs = new ExporterStationsFromDoc(txtBoxPath.Text);
            var exporterExcels = new ExporterStationsFromExcel(txtBoxPath.Text);
            var logger = new Logger(exporterDocs, exporterExcels);
            //txtBoxLog.Text = new Logger(expDocs).GetInformation();
            logger.GetInformation(richTxtLog);
        }

        private void btnFolderDialog_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new forms.FolderBrowserDialog();
            var result = folderDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
                txtBoxPath.Text = folderDialog.SelectedPath;
                
        }
    }
}
