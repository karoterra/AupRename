using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
using Microsoft.Win32;

namespace AupRename
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            VersionWindow versionWindow = new VersionWindow();
            versionWindow.ShowDialog();
        }

        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            var dropFiles = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (dropFiles != null && !File.GetAttributes(dropFiles[0]).HasFlag(FileAttributes.Directory))
            {
                filePathTextBox.Text = dropFiles[0];
            }
        }

        private void OpenFile()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "AviUtlプロジェクトファイル (*.aup)|*.aup|全てのファイル (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                filePathTextBox.Text = dialog.FileName;
            }
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void referEditorButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "プログラム (*.exe)|*.exe|全てのファイル (*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                editorTextBox.Text = dialog.FileName;
            }
        }
    }
}
