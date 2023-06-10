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
using System.Threading;

namespace IpExporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource cancellation;
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

        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            var foderIsExists = Directory.Exists(txtBoxPath.Text);
            if (!foderIsExists)
            {
                MessageBox.Show("Такого директория не существует",
                    "Неверный путь",MessageBoxButton.OK,MessageBoxImage.Error);
                txtBoxPath.Focus();
                return;
            }

            btnStart.Content = "Отменить операцию";
            btnStart.Click -= Button_Click;
            btnStart.Click += CancelTask;

            var exporterDocs = new ExporterStationsFromDoc(txtBoxPath.Text);
            var exporterExcels = new ExporterStationsFromExcel(txtBoxPath.Text);

            int countFiles = exporterDocs.FileNames.Count() + exporterExcels.FileNames.Count();
            progBar.Maximum = countFiles;

            var logger = new Logger( exporterDocs, exporterExcels);
            var progress = new Progress<int>(p => progBar.Value = p);

            this.cancellation = new CancellationTokenSource();
            try
            {
                await logger.InitilizeAsync(progress, cancellation);
            }
            catch (Exception)
            {
                        MessageBox.Show("Операция отменена");
                return;
            }
            finally
            {
                MessageBox.Show("Операция ОТМЕНЕНА");
                cancellation.Dispose();

                btnStart.Content = "Поехали!";
                btnStart.Click -= CancelTask;
                btnStart.Click += Button_Click;
                progBar.Value = 0;
            }
            

            txtBlCountStations.Text = logger.Stations.Count.ToString();
            txtBlCountProblemStations.Text = logger.Stations.
                Where(net=>net.AllSubnets.Count!=net.CorrectSubnets.Count).
                Count().ToString();
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

        private void CancelTask(object sender, RoutedEventArgs e)
        {
            cancellation.Cancel();
        }
    }
}
