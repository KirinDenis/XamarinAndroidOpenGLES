using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace OBJParser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pathFile = string.Empty;
        private string pathFolder = string.Empty;
        private string nameFile = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            logTextBlock.Text += "Please, chose obj file.\n";
        }

        private void doParse_Click(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(pathFile) && pathFile.Contains(".obj"))
            {
                try
                {

                    List<byte[]> objectData = ParseObj.ParsedObject(pathFile);
                    File.WriteAllBytes(pathFolder+"\\"+nameFile+"_objvertex", objectData[0]);
                    File.WriteAllBytes(pathFolder + "\\"+nameFile+"_objtexture", objectData[1]);
                    File.WriteAllBytes(pathFolder + "\\"+nameFile+"_objnormal", objectData[2]);

                    logTextBlock.Text += "Creating files in folder " + pathFolder + " is complited.\n";
                }
                catch (Exception ex)
                {
                    logTextBlock.Text += "Error: \"" + ex.Message + "\".\n";
                }

            }

            else
            {
                logTextBlock.Text += "Please, chose obj file.\n";
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "3d files (*.obj)|*.obj|All files (*.*)|*.*";
            dialog.FilterIndex = 2;

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                // Open document
                pathFile = dialog.FileName;
                pathFolder = Path.GetDirectoryName(pathFile);
                nameFile = System.IO.Path.GetFileNameWithoutExtension(pathFile);
                logTextBlock.Text += "File with path \"" + pathFile + "\" has chosen.\n";
                doParse_Click(null, null);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                OpenFile_Click(null, null);
            }
            else
            {
                Close();
            }
        }

        private void linkTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://en.wikipedia.org/wiki/Wavefront_.obj_file");
        }
    }
}
