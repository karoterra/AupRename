using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace AupRename
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private const string Url = "https://github.com/karoterra/AupRename";

        public event PropertyChangedEventHandler? PropertyChanged;
        public readonly Renamer Renamer;

        public DelegateCommand OpenFileCommand { get; init; }
        public DelegateCommand ReferEditorCommand { get; init; }
        public DelegateCommand NewEditCommand { get; init; }
        public DelegateCommand ReEditCommand { get; init; }
        public DelegateCommand ApplyCommand { get; init; }
        public DelegateCommand RevertCommand { get; init; }
        public DelegateCommand OpenUrlCommand { get; init; }
        public DelegateCommand ShowVersionCommand { get; init; }
        public DelegateCommand ShutdownCommand { get; init; }
        public DelegateCommand<string> ChangeLanguageCommand { get; init; }

        public string Filename
        {
            get => Renamer.Filename;
            set
            {
                Renamer.Filename = value;
                RaisePropertyChanged();
            }
        }

        public string Editor
        {
            get => Renamer.Editor;
            set
            {
                Renamer.Editor = value;
                RaisePropertyChanged();
            }
        }

        public bool EnableVideo
        {
            get => Renamer.EnableVideo;
            set
            {
                Renamer.EnableVideo = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableImage
        {
            get => Renamer.EnableImage;
            set
            {
                Renamer.EnableImage = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableAudio
        {
            get => Renamer.EnableAudio;
            set
            {
                Renamer.EnableAudio = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableWaveform
        {
            get => Renamer.EnableWaveform;
            set
            {
                Renamer.EnableWaveform = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableShadow
        {
            get => Renamer.EnableShadow;
            set
            {
                Renamer.EnableShadow = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableBorder
        {
            get => Renamer.EnableBorder;
            set
            {
                Renamer.EnableBorder = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableVideoComposition
        {
            get => Renamer.EnableVideoComposition;
            set
            {
                Renamer.EnableVideoComposition = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableImageComposition
        {
            get => Renamer.EnableImageComposition;
            set
            {
                Renamer.EnableImageComposition = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableFigure
        {
            get => Renamer.EnableFigure;
            set
            {
                Renamer.EnableFigure = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableMask
        {
            get => Renamer.EnableMask;
            set
            {
                Renamer.EnableMask = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableDisplacement
        {
            get => Renamer.EnableDisplacement;
            set
            {
                Renamer.EnableDisplacement = value;
                RaisePropertyChanged();
            }
        }
        public bool EnablePartialFilter
        {
            get => Renamer.EnablePartialFilter;
            set
            {
                Renamer.EnablePartialFilter = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableScript
        {
            get => Renamer.EnableScript;
            set
            {
                Renamer.EnableScript = value;
                RaisePropertyChanged();
            }
        }

        public bool EnablePsdToolKit
        {
            get => Renamer.EnablePsdToolKit;
            set
            {
                Renamer.EnablePsdToolKit = value;
                RaisePropertyChanged();
            }
        }

        public string Status
        {
            get => Renamer.Status;
            set
            {
                Renamer.Status = value;
                RaisePropertyChanged();
            }
        }

        public string Language
        {
            get => Properties.Settings.Default.Language;
            set
            {
                Properties.Settings.Default.Language = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            Renamer = new Renamer();
            OpenFileCommand = new DelegateCommand(OpenFile);
            ReferEditorCommand = new DelegateCommand(ReferEditor);
            ReEditCommand = new DelegateCommand(
                () => { Renamer.ReEdit(); RaisePropertyChanged(nameof(Status)); },
                () => Renamer.IsEditing);
            ApplyCommand = new DelegateCommand(
                () => { Renamer.Apply(); RaisePropertyChanged(nameof(Status)); },
                () => Renamer.IsEditing);
            RevertCommand = new DelegateCommand(
                () => { Renamer.Revert(); RaisePropertyChanged(nameof(Status)); },
                () => Renamer.IsEditing);
            NewEditCommand = new DelegateCommand(() =>
            {
                Renamer.NewEdit();
                RaisePropertyChanged(nameof(Status));
                ReEditCommand.RaiseCanExecuteChanged();
                ApplyCommand.RaiseCanExecuteChanged();
                RevertCommand.RaiseCanExecuteChanged();
            });
            OpenUrlCommand = new DelegateCommand(OpenUrl);
            ShowVersionCommand = new DelegateCommand(ShowVersion);
            ShutdownCommand = new DelegateCommand(Shutdown);
            ChangeLanguageCommand = new DelegateCommand<string>(ChangeLanguage);

            LoadSetting();
        }

        private void OpenFile()
        {
            OpenFileDialog dialog = new()
            {
                Title = Properties.Resources.Dialog_AupTitle,
                Filter = Properties.Resources.Dialog_AupFilter,
            };
            if (dialog.ShowDialog() == true)
            {
                Filename = dialog.FileName;
            }
        }

        private void ReferEditor()
        {
            OpenFileDialog dialog = new()
            {
                Title = Properties.Resources.Dialog_EditorTitle,
                Filter = Properties.Resources.Dialog_EditorFilter,
            };
            if (dialog.ShowDialog() == true)
            {
                Editor = dialog.FileName;
            }
        }

        private void ShowVersion()
        {
            new VersionWindow().ShowDialog();
        }

        private void OpenUrl()
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {Url}") { CreateNoWindow = true });
        }

        private void Shutdown()
        {
            Application.Current.Shutdown();
        }

        private void ChangeLanguage(string? culture)
        {
            Language = culture ?? "";
            MessageBox.Show(Properties.Resources.Message_ChangeLanguage, "AupRename", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void LoadSetting()
        {
            if (!Properties.Settings.Default.IsUpgrade)
            {
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.IsUpgrade = true;
            }

            Editor = Properties.Settings.Default.Editor;
            EnableVideo = Properties.Settings.Default.EnableVideo;
            EnableImage = Properties.Settings.Default.EnableImage;
            EnableAudio = Properties.Settings.Default.EnableAudio;
            EnableWaveform = Properties.Settings.Default.EnableWaveform;
            EnableShadow = Properties.Settings.Default.EnableShadow;
            EnableBorder = Properties.Settings.Default.EnableBorder;
            EnableVideoComposition = Properties.Settings.Default.EnableVideoComposition;
            EnableImageComposition = Properties.Settings.Default.EnableImageComposition;
            EnableFigure = Properties.Settings.Default.EnableFigure;
            EnableMask = Properties.Settings.Default.EnableMask;
            EnableDisplacement = Properties.Settings.Default.EnableDisplacement;
            EnablePartialFilter = Properties.Settings.Default.EnablePartialFilter;
            EnableScript = Properties.Settings.Default.EnableScript;
            EnablePsdToolKit = Properties.Settings.Default.EnablePsdToolKit;
        }

        public void SaveSetting()
        {
            Properties.Settings.Default.Editor = Editor;
            Properties.Settings.Default.EnableVideo = EnableVideo;
            Properties.Settings.Default.EnableImage = EnableImage;
            Properties.Settings.Default.EnableAudio = EnableAudio;
            Properties.Settings.Default.EnableWaveform = EnableWaveform;
            Properties.Settings.Default.EnableShadow = EnableShadow;
            Properties.Settings.Default.EnableBorder = EnableBorder;
            Properties.Settings.Default.EnableVideoComposition = EnableVideoComposition;
            Properties.Settings.Default.EnableImageComposition = EnableImageComposition;
            Properties.Settings.Default.EnableFigure = EnableFigure;
            Properties.Settings.Default.EnableMask = EnableMask;
            Properties.Settings.Default.EnableDisplacement = EnableDisplacement;
            Properties.Settings.Default.EnablePartialFilter = EnablePartialFilter;
            Properties.Settings.Default.EnableScript = EnableScript;
            Properties.Settings.Default.EnablePsdToolKit = EnablePsdToolKit;
            Properties.Settings.Default.Save();
        }

        private void RaisePropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
