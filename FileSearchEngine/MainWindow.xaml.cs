using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using WinForms = System.Windows.Forms;

namespace FileSearchEngine
{

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            FileTree.onFilterBegin += OnFilterBegin;
            FileTree.onFilterEnd += OnFilterEnd;

        }


        private void OnFilterEnd()
        {
            lable.Content = "Поиск Завершен";
        }

        int x = 0;
        private void OnFilterBegin()
        {
            if (x == 0)
            {
                lable.Content = "Поиск.";
                x++;
            }
            if (x == 1)
            {
                lable.Content = "Поиск..";
                x++;
            }
            if (x == 2)
            {
                lable.Content = "Поиск...";
                x = 0;
            }
        }

        private void SelectStartingDirectory_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == WinForms.DialogResult.OK)
            {
                File.WriteAllText("StartingDirectory.txt", fbd.SelectedPath);
            }
        }

        private void AddFileType_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();

            window1.Show();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(File.ReadAllText("StartingDirectory.txt"));

            Regex regex = new Regex(File.ReadAllText("FileName.txt"));

            FileTree.Update(directoryInfo, fileInfo => regex.Match(fileInfo.Name).Value != null);
        }

        private FileInfo[] InspectDirectory(DirectoryInfo dir)
        {
            List<FileInfo> allFiles = new List<FileInfo>();

            allFiles.AddRange(dir.GetFiles());

            foreach (var dirinfo in dir.GetDirectories())
            {
                var files = InspectDirectory(dirinfo);

                allFiles.AddRange(files);
            }

            return allFiles.ToArray();
        }
    }
}
