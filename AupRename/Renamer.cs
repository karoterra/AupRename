using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using AupRename.RenameItems;
using Karoterra.AupDotNet;
using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;
using Karoterra.AupDotNet.Extensions;

namespace AupRename
{
    public class Renamer
    {
        private const string ListFilename = @".\list.txt";

        public string Filename { get; set; } = "";
        public string Editor { get; set; } = "";

        public bool EnableVideo { get; set; }
        public bool EnableImage { get; set; }
        public bool EnableAudio { get; set; }
        public bool EnableWaveform { get; set; }
        public bool EnableShadow { get; set; }
        public bool EnableBorder { get; set; }
        public bool EnableVideoComposition { get; set; }
        public bool EnableImageComposition { get; set; }
        public bool EnableFigure { get; set; }
        public bool EnableMask { get; set; }
        public bool EnableDisplacement { get; set; }
        public bool EnablePartialFilter { get; set; }
        public bool EnableScript { get; set; }

        public string Status { get; set; } = "";

        public bool IsEditing => _aup != null;

        private string CurrentFilename = "";
        private AviUtlProject? _aup;
        private ExEditProject? _exedit;
        private readonly List<IRenameItem> _renameItems = [];

        private void OpenEditor()
        {
            try
            {
                Process.Start(Editor, ListFilename);
            }
            catch (InvalidOperationException)
            {
                ShowError("テキストエディタを指定してください。");
                return;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                ShowError("テキストエディタが見つかりません。");
                return;
            }
            Status = $"{Path.GetFileName(CurrentFilename)} を編集中";
        }

        public void NewEdit()
        {
            CurrentFilename = Filename;
            _aup = null;
            _exedit = null;
            _renameItems.Clear();
            Status = "";

            if (!File.Exists(Filename))
            {
                ShowError("ファイルが見つかりません");
                return;
            }

            try
            {
                _aup = new AviUtlProject(CurrentFilename);
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
            catch (Exception ex)
            {
                ShowError("AviUtlプロジェクトファイル読み込み中にエラーが発生しました。\nファイルが破損している可能性があります。");
                LogException(ex);
                return;
            }

            _exedit = null;
            try
            {
                for (int i = 0; i < _aup.FilterProjects.Count; i++)
                {
                    if (_aup.FilterProjects[i] is RawFilterProject filter && filter.Name == "拡張編集")
                    {
                        _exedit = new ExEditProject(filter);
                        _aup.FilterProjects[i] = _exedit;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("AviUtlプロジェクトファイル読み込み中にエラーが発生しました。\nファイルが破損している可能性があります。");
                LogException(ex);
                _exedit = null;
                _aup = null;
                return;
            }

            if (_exedit == null)
            {
                ShowError("拡張編集のデータが見つかりません。");
                _aup = null;
                return;
            }

            _renameItems.Clear();
            for (int objIdx = 0; objIdx < _exedit.Objects.Count; objIdx++)
            {
                var obj = _exedit.Objects[objIdx];
                if (obj.Chain) continue;

                for (int effectIdx = 0; effectIdx < obj.Effects.Count; effectIdx++)
                {
                    var effect = obj.Effects[effectIdx];
                    IRenameItem? renameItem = null;
                    if(EnableVideo && (renameItem = VideoFileRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableImage && (renameItem = ImageFileRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableAudio && (renameItem = AudioFileRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableWaveform && (renameItem = WaveformRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableShadow && (renameItem = ShadowRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableBorder && (renameItem = BorderRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableVideoComposition && (renameItem = VideoCompositionRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableImageComposition && (renameItem = ImageCompositionRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableFigure && (renameItem = FigureRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableMask && (renameItem = MaskRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableDisplacement && (renameItem = DisplacementRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnablePartialFilter && (renameItem = PartialFilterRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                    else if (EnableScript && (renameItem = ScriptFileRenameItem.CreateIfTarget(effect)) != null)
                    {
                        _renameItems.Add(renameItem);
                    }
                }
            }

            if (_renameItems.Count == 0)
            {
                ShowInfo("編集するファイルがありません。");
                _aup = null;
                _exedit = null;
                return;
            }

            try
            {
                using StreamWriter sw = new(ListFilename, false, Encoding.UTF8);
                foreach (var item in _renameItems)
                {
                    sw.WriteLine(item.OldName);
                }
            }
            catch (IOException)
            {
                ShowError("list.txt に書き込めません。\n既にテキストエディタで開いている場合は閉じてください。");
                _aup = null;
                _exedit = null;
                _renameItems.Clear();
                return;
            }

            OpenEditor();
        }

        public void ReEdit()
        {
            if (_aup == null)
            {
                ShowError("ファイルを開いていません。\n新規編集してください。");
                return;
            }
            OpenEditor();
        }

        private void Rename(List<string> newNames)
        {
            if (_aup == null || _exedit == null)
            {
                ShowError("ファイルを指定して新規編集してください。");
                return;
            }

            for (int i = 0; i < _renameItems.Count; i++)
            {
                try
                {
                    _renameItems[i].Rename(newNames[i]);
                }
                catch (MaxByteCountOfStringException)
                {
                    ShowError($"{i + 1}行目のファイル名が長すぎます。");
                    return;
                }
            }

            try
            {
                using BinaryWriter writer = new(File.Create(CurrentFilename));
                _aup.Write(writer);
            }
            catch (IOException)
            {
                ShowError("書き込みに失敗しました。");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                ShowError("書き込みに失敗しました。");
                return;
            }

            ShowInfo("リネームが完了しました。");
            Status = $"{Path.GetFileName(CurrentFilename)} に変更を適用";
        }

        public void Apply()
        {
            if (_renameItems.Count == 0)
            {
                ShowError("ファイルを指定して新規編集してください。");
                return;
            }

            List<string> newNames = [];
            try
            {
                using StreamReader sr = new(ListFilename, Encoding.UTF8);
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        newNames.Add(line);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                ShowError("list.txt が見つかりません。");
                return;
            }

            if (newNames.Count > _renameItems.Count)
            {
                ShowError("list.txt: ファイル名が多すぎます。");
                return;
            }
            else if (newNames.Count < _renameItems.Count)
            {
                ShowError("list.txt: ファイル名が少なすぎます。");
                return;
            }

            Rename(newNames);
        }

        public void Revert()
        {
            if (_aup == null || _renameItems.Count == 0)
            {
                ShowError("ファイルを指定して新規編集してください。");
                return;
            }

            foreach (var item in _renameItems)
            {
                item.Revert();
            }

            try
            {
                using BinaryWriter writer = new(File.Create(CurrentFilename));
                _aup.Write(writer);
            }
            catch (IOException)
            {
                ShowError("書き込みに失敗しました。");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                ShowError("書き込みに失敗しました。");
                return;
            }

            ShowInfo("変更を元に戻しました。");
            Status = $"{Path.GetFileName(CurrentFilename)} の変更をリセット";
        }

        private static void ShowError(string message)
        {
            MessageBox.Show(message, "AupRename", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void ShowInfo(string message)
        {
            MessageBox.Show(message, "AupRename", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static void LogException(Exception ex)
        {
            string logFilePath = "log.txt";

            try
            {
                File.AppendAllText(logFilePath,
                    $"[{DateTime.Now}] {ex.GetType()}: {ex.Message}\n{ex.StackTrace}\n\n"
                );
            }
            catch (Exception logEx)
            {
                ShowError($"エラーログの書き込みに失敗しました。: {logEx.Message}");
            }
        }
    }
}
