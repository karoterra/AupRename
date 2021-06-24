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
using Karoterra.AupDotNet;
using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ListFilename = @".\list.txt";

        private string path;
        private AviUtlProject aup;
        private ExEditProject exedit;
        private readonly List<RenameItem> renameItems = new();

        public MainWindow()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            VersionWindow versionWindow = new();
            versionWindow.ShowDialog();
        }

        private void Window_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(DataFormats.FileDrop) is string[] dropFiles && !File.GetAttributes(dropFiles[0]).HasFlag(FileAttributes.Directory))
            {
                filePathTextBox.Text = dropFiles[0];
            }
        }

        private void OpenFile()
        {
            OpenFileDialog dialog = new()
            {
                Filter = "AviUtlプロジェクトファイル (*.aup)|*.aup|全てのファイル (*.*)|*.*",
            };
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
            OpenFileDialog dialog = new()
            {
                Filter = "プログラム (*.exe)|*.exe|全てのファイル (*.*)|*.*",
            };
            if (dialog.ShowDialog() == true)
            {
                editorTextBox.Text = dialog.FileName;
            }
        }

        private void OpenEditor()
        {
            System.Diagnostics.Process.Start(editorTextBox.Text, ListFilename);
        }

        private void newEditButton_Click(object sender, RoutedEventArgs e)
        {
            path = filePathTextBox.Text;
            aup = null;
            exedit = null;
            renameItems.Clear();
            if (!File.Exists(path))
            {
                ShowError("ファイルが見つかりません。");
                return;
            }

            try
            {
                aup = new AviUtlProject(path);
            }
            catch (FileFormatException)
            {
                ShowError("AviUtlプロジェクトファイルではありません。");
                return;
            }
            catch (EndOfStreamException)
            {
                ShowError("AviUtlプロジェクトファイルではありません。");
                return;
            }

            for (int i = 0; i < aup.FilterProjects.Count; i++)
            {
                if (aup.FilterProjects[i] is RawFilterProject filter && filter.Name == "拡張編集")
                {
                    exedit = new ExEditProject(filter);
                    aup.FilterProjects[i] = exedit;
                    break;
                }
            }
            if (exedit == null)
            {
                ShowError("拡張編集のデータが見つかりません。");
                aup = null;
                return;
            }

            for (int objIdx = 0; objIdx < exedit.Objects.Count; objIdx++)
            {
                var obj = exedit.Objects[objIdx];
                for (int effectIdx = 0; effectIdx < obj.Effects.Count; effectIdx++)
                {
                    var effect = obj.Effects[effectIdx];
                    if (videoChk.IsChecked == true && effect is VideoFileEffect video && video.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = video.Filename });
                    }
                    else if (imageChk.IsChecked == true && effect is ImageFileEffect image && image.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = image.Filename });
                    }
                    else if (audioChk.IsChecked == true && effect is AudioFileEffect audio && audio.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = audio.Filename });
                    }
                    else if (waveformChk.IsChecked == true && effect is WaveformEffect waveform && waveform.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = waveform.Filename });
                    }
                    else if (shadowChk.IsChecked == true && effect is ShadowEffect shadow && shadow.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = shadow.Filename });
                    }
                    else if (borderChk.IsChecked == true && effect is BorderEffect border && border.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = border.Filename });
                    }
                    else if (videoCompositionChk.IsChecked == true && effect is VideoCompositionEffect videoComp && videoComp.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = videoComp.Filename });
                    }
                    else if (imageCompositionChk.IsChecked == true && effect is ImageCompositionEffect imageComp && imageComp.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = imageComp.Filename });
                    }
                    else if (figureChk.IsChecked == true && effect is FigureEffect figure && figure.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = figure.Filename });
                    }
                    else if (maskChk.IsChecked == true && effect is MaskEffect mask && mask.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = mask.Filename });
                    }
                    else if (displacementChk.IsChecked == true && effect is DisplacementEffect displacement && displacement.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = displacement.Filename });
                    }
                    else if (partialFilterChk.IsChecked == true && effect is PartialFilterEffect pf && pf.Filename != "")
                    {
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = pf.Filename });
                    }
                    else if (scriptChk.IsChecked == true && effect is ScriptFileEffect script && script.Params.ContainsKey("file") && script.Params["file"] != "")
                    {
                        var filename = script.Params["file"];
                        filename = filename[1..^1].Replace(@"\\", @"\");
                        renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = filename });
                    }
                }
            }

            try
            {
                using (StreamWriter sw = new(ListFilename, false, Encoding.UTF8))
                {
                    foreach (var item in renameItems)
                    {
                        sw.WriteLine(item.Filename);
                    }
                }
            }
            catch (IOException)
            {
                ShowError("list.txt に書き込めません。\n既にテキストエディタで開いている場合は閉じてください。");
                aup = null;
                exedit = null;
                renameItems.Clear();
                return;
            }

            OpenEditor();
        }

        private void reEditButton_Click(object sender, RoutedEventArgs e)
        {
            OpenEditor();
        }

        private void Rename(List<string> newNames)
        {
            for (int i = 0; i < renameItems.Count; i++)
            {
                var effect = exedit.Objects[renameItems[i].ObjectIndex].Effects[renameItems[i].EffectIndex];
                switch (effect)
                {
                    case VideoFileEffect video:
                        video.Filename = newNames[i];
                        break;
                    case ImageFileEffect image:
                        image.Filename = newNames[i];
                        break;
                    case AudioFileEffect audio:
                        audio.Filename = newNames[i];
                        break;
                    case WaveformEffect waveform:
                        waveform.Filename = newNames[i];
                        break;
                    case ShadowEffect shadow:
                        shadow.Filename = newNames[i];
                        break;
                    case BorderEffect border:
                        border.Filename = newNames[i];
                        break;
                    case VideoCompositionEffect video:
                        video.Filename = newNames[i];
                        break;
                    case ImageCompositionEffect image:
                        image.Filename = newNames[i];
                        break;
                    case FigureEffect figure:
                        figure.Filename = newNames[i];
                        break;
                    case MaskEffect mask:
                        mask.Filename = newNames[i];
                        break;
                    case DisplacementEffect d:
                        d.Filename = newNames[i];
                        break;
                    case PartialFilterEffect pf:
                        pf.Filename = newNames[i];
                        break;
                    case ScriptFileEffect script:
                        script.Params["file"] = '"' + newNames[i].Replace(@"\", @"\\") + '"';
                        break;
                }
            }

            try
            {
                using (BinaryWriter writer = new(File.Create(path)))
                {
                    aup.Write(writer);
                }
            }
            catch (UnauthorizedAccessException)
            {
                ShowError("書き込みに失敗しました。");
            }

            ShowInfo("リネームが完了しました。");
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            if (renameItems.Count == 0)
            {
                ShowError("ファイルを指定して新規編集してください。");
                return;
            }

            List<string> newNames = new();
            try
            {
                using (StreamReader sr = new(ListFilename, Encoding.UTF8))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            newNames.Add(line);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                ShowError("list.txt が見つかりません。");
                return;
            }

            if (newNames.Count > renameItems.Count)
            {
                ShowError("list.txt: ファイル名が多すぎます。");
                return;
            }
            else if (newNames.Count < renameItems.Count)
            {
                ShowError("list.txt: ファイル名が少なすぎます。");
                return;
            }

            Rename(newNames);
        }

        private void revertButton_Click(object sender, RoutedEventArgs e)
        {
            if (renameItems.Count == 0)
            {
                ShowError("ファイルを指定して新規編集してください。");
                return;
            }

            var newNames = renameItems.Select(item => item.Filename).ToList();
            Rename(newNames);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "AupRename", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "AupRename", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
