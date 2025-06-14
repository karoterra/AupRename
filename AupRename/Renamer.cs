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
        public bool EnablePsdToolKit { get; set; }

        public string Status { get; set; } = "";

        public bool IsEditing => _aup != null;

        private string CurrentFilename = "";
        private AviUtlProject? _aup;
        private ExEditProject? _exedit;
        private PsdToolKitProject? _psdToolKit;
        private readonly List<IRenameItem> _renameItems = [];

        private void OpenEditor()
        {
            try
            {
                Process.Start(Editor, ListFilename);
            }
            catch (InvalidOperationException)
            {
                ShowError(Properties.Resources.Error_EditorNotSelected);
                return;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                ShowError(Properties.Resources.Error_EditorNotFound);
                return;
            }
            Status = string.Format(Properties.Resources.Status_OpenEditor, Path.GetFileName(CurrentFilename));
        }

        public void NewEdit()
        {
            CurrentFilename = Filename;
            _aup = null;
            _exedit = null;
            _psdToolKit = null;
            _renameItems.Clear();
            Status = "";

            if (!File.Exists(Filename))
            {
                ShowError(Properties.Resources.Error_FileNotFound);
                return;
            }

            try
            {
                _aup = new AviUtlProject(CurrentFilename);
            }
            catch (FileFormatException)
            {
                ShowError(Properties.Resources.Error_NotAviUtlProjectFile);
                return;
            }
            catch (EndOfStreamException)
            {
                ShowError(Properties.Resources.Error_NotAviUtlProjectFile);
                return;
            }
            catch (Exception ex)
            {
                ShowError(Properties.Resources.Error_CorruptedAviUtlProjectFile);
                LogException(ex);
                return;
            }

            _exedit = null;
            _psdToolKit = null;
            try
            {
                for (int i = 0; i < _aup.FilterProjects.Count; i++)
                {
                    if (_aup.FilterProjects[i] is RawFilterProject filter)
                    {
                        if (filter.Name == "拡張編集")
                        {
                            _exedit = new ExEditProject(filter);
                            _aup.FilterProjects[i] = _exedit;
                        }
                        else if (filter.Name == "Advanced Editing")
                        {
                            _exedit = new EnglishExEditProject(filter);
                            _aup.FilterProjects[i] = _exedit;
                        }
                    }
                    if (_aup.FilterProjects[i].Name == "PSDToolKit")
                    {
                        _psdToolKit = new PsdToolKitProject(_aup.FilterProjects[i]);
                        _aup.FilterProjects[i] = _psdToolKit;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(Properties.Resources.Error_CorruptedAviUtlProjectFile);
                LogException(ex);
                _exedit = null;
                _aup = null;
                return;
            }

            if (_exedit == null)
            {
                ShowError(Properties.Resources.Error_ExEditNotFound);
                _aup = null;
                return;
            }

            _renameItems.Clear();
            // 拡張編集
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
            // PSDToolKit
            if (_psdToolKit != null && EnablePsdToolKit)
            {
                foreach (var psdImage in _psdToolKit.Images)
                {
                    _renameItems.Add(new PsdRenameItem(psdImage, _exedit.Objects));
                }
            }

            if (_renameItems.Count == 0)
            {
                ShowInfo(Properties.Resources.Message_NoFilesToEdit);
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
                ShowError(Properties.Resources.Error_CannotWriteListTxt);
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
                ShowError(Properties.Resources.Error_NoFileOpen);
                return;
            }
            OpenEditor();
        }

        private void Rename(List<string> newNames)
        {
            if (_aup == null || _exedit == null)
            {
                ShowError(Properties.Resources.Error_NoFileOpen);
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
                    ShowError(string.Format(Properties.Resources.Error_FileNameTooLong, i + 1));
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
                ShowError(Properties.Resources.Error_WriteFailed);
                return;
            }
            catch (UnauthorizedAccessException)
            {
                ShowError(Properties.Resources.Error_WriteFailed);
                return;
            }

            ShowInfo(Properties.Resources.Message_RenameCompleted);
            Status = string.Format(Properties.Resources.Status_RenameCompleted, Path.GetFileName(CurrentFilename));
        }

        public void Apply()
        {
            if (_renameItems.Count == 0)
            {
                ShowError(Properties.Resources.Error_NoFileOpen);
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
                ShowError(Properties.Resources.Error_ListTxtNotFound);
                return;
            }

            if (newNames.Count > _renameItems.Count)
            {
                ShowError(Properties.Resources.Error_TooManyFilenames);
                return;
            }
            else if (newNames.Count < _renameItems.Count)
            {
                ShowError(Properties.Resources.Error_TooFewFilenames);
                return;
            }

            Rename(newNames);
        }

        public void Revert()
        {
            if (_aup == null || _renameItems.Count == 0)
            {
                ShowError(Properties.Resources.Error_NoFileOpen);
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
                ShowError(Properties.Resources.Error_WriteFailed);
                return;
            }
            catch (UnauthorizedAccessException)
            {
                ShowError(Properties.Resources.Error_WriteFailed);
                return;
            }

            ShowInfo(Properties.Resources.Message_RevertCompleted);
            Status = string.Format(Properties.Resources.Status_RevertCompleted, Path.GetFileName(CurrentFilename));
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
                ShowError(Properties.Resources.Error_LogWriteFailed + logEx.Message);
            }
        }
    }
}
