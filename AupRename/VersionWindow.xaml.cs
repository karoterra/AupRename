using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AupRename
{
    /// <summary>
    /// VersionInfo.xaml の相互作用ロジック
    /// </summary>
    public partial class VersionWindow : Window
    {
        public VersionWindow()
        {
            InitializeComponent();

            Assembly assem = Assembly.GetExecutingAssembly();
            var assemName = assem.GetName();
            nameLabel.Content = assemName.Name;
            var version = assemName.Version;
            versionLabel.Content = $"Version {version.Major}.{version.Minor}.{version.Build}";
            copyrightLabel.Content = assem.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
