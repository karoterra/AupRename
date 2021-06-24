using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Karoterra.AupDotNet;
using Karoterra.AupDotNet.ExEdit;
using Karoterra.AupDotNet.ExEdit.Effects;

namespace AupRename
{
    public class Renamer
    {
        private const string ListFilename = @".\list.txt";

        public string Filename { get; set; }
        public string Editor { get; set; }

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

        public string Status { get; set; }

        public bool IsEditing => _aup != null;

        private string CurrentFilename;
        private AviUtlProject _aup;
        private ExEditProject _exedit;
        private List<RenameItem> _renameItems = new();

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

            _exedit = null;
            for (int i = 0; i < _aup.FilterProjects.Count; i++)
            {
                if (_aup.FilterProjects[i] is RawFilterProject filter && filter.Name == "拡張編集")
                {
                    _exedit = new ExEditProject(filter);
                    _aup.FilterProjects[i] = _exedit;
                    break;
                }
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
                for (int effectIdx = 0; effectIdx < obj.Effects.Count; effectIdx++)
                {
                    var effect = obj.Effects[effectIdx];
                    if (EnableVideo && effect is VideoFileEffect video && video.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = video.Filename });
                    }
                    else if (EnableImage && effect is ImageFileEffect image && image.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = image.Filename });
                    }
                    else if (EnableAudio && effect is AudioFileEffect audio && audio.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = audio.Filename });
                    }
                    else if (EnableWaveform && effect is WaveformEffect waveform && waveform.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = waveform.Filename });
                    }
                    else if (EnableShadow && effect is ShadowEffect shadow && shadow.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = shadow.Filename });
                    }
                    else if (EnableBorder && effect is BorderEffect border && border.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = border.Filename });
                    }
                    else if (EnableVideoComposition && effect is VideoCompositionEffect videoComp && videoComp.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = videoComp.Filename });
                    }
                    else if (EnableImageComposition && effect is ImageCompositionEffect imageComp && imageComp.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = imageComp.Filename });
                    }
                    else if (EnableFigure && effect is FigureEffect figure && figure.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = figure.Filename });
                    }
                    else if (EnableMask && effect is MaskEffect mask && mask.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = mask.Filename });
                    }
                    else if (EnableDisplacement && effect is DisplacementEffect displacement && displacement.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = displacement.Filename });
                    }
                    else if (EnablePartialFilter && effect is PartialFilterEffect pf && pf.Filename != "")
                    {
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = pf.Filename });
                    }
                    else if (EnableScript && effect is ScriptFileEffect script && script.Params.ContainsKey("file") && script.Params["file"] != "")
                    {
                        var filename = script.Params["file"];
                        filename = filename[1..^1].Replace(@"\\", @"\");
                        _renameItems.Add(new RenameItem() { ObjectIndex = objIdx, EffectIndex = effectIdx, Filename = filename });
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
                    sw.WriteLine(item.Filename);
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
            for (int i = 0; i < _renameItems.Count; i++)
            {
                var effect = _exedit.Objects[_renameItems[i].ObjectIndex].Effects[_renameItems[i].EffectIndex];
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

            List<string> newNames = new();
            try
            {
                using StreamReader sr = new(ListFilename, Encoding.UTF8);
                string line;
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
            if (_renameItems.Count == 0)
            {
                ShowError("ファイルを指定して新規編集してください。");
                return;
            }

            var newNames = _renameItems.Select(item => item.Filename).ToList();
            Rename(newNames);
        }

        private static void ShowError(string message)
        {
            MessageBox.Show(message, "AupRename", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static void ShowInfo(string message)
        {
            MessageBox.Show(message, "AupRename", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
