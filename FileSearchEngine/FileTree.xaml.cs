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

namespace FileSearchEngine
{
    /// <summary>
    /// Логика взаимодействия для FileTree.xaml
    /// </summary>
    public partial class FileTree : UserControl
    {
        public event Action onFilterBegin;
        public event Action onFilterEnd;

        private DirectoryInfo directoryInfo;

        public FileTree()
        {
            InitializeComponent();
        }

        public void Update(DirectoryInfo dir = null, Func<FileInfo, bool> predicate = null)
        {
            onFilterBegin?.Invoke();

            if (dir != null)
                directoryInfo = dir;

            FileTreeView.Items.Clear();

            UpdateTreeView(dir, null, predicate);

            onFilterEnd?.Invoke();
        }

        private bool UpdateTreeView(DirectoryInfo directory = null, TreeViewItem root = null, Func<FileInfo, bool> predicate = null)
        {
            if (directory == null)
            {
                directory = directoryInfo;
            }

            if (root == null)
            {
                root = new TreeViewItem()
                {
                    Header = directoryInfo.FullName
                };

                FileTreeView.Items.Add(root);
            }

            if(predicate == null)
            {
                predicate = e => true;
            }

            bool hasResult = false;

            foreach (var dirinfo in directory.GetDirectories())
            {
                TreeViewItem folder = new TreeViewItem()
                {
                    Header = dirinfo.Name
                };

                hasResult = UpdateTreeView(dirinfo, folder, predicate);

                if (hasResult)
                    root.Items.Add(folder);

            }
            
            hasResult = false;

            foreach (var fileInfo in directory.GetFiles())
            {
                if (predicate(fileInfo))
                {
                    hasResult = true;

                    root.Items.Add(fileInfo.Name);
                }

            }

            return hasResult;
        }
    }
}
