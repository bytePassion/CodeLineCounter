using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using bytePassion.Lib.FrameworkExtensions;
using bytePassion.Lib.WpfLib.Commands;
using bytePassion.Lib.WpfLib.Commands.Updater;
using FileOperator.ViewModel.Helper;

namespace FileOperator.ViewModel
{
    internal class MainWindowViewModel : bytePassion.Lib.WpfLib.ViewModelBase.ViewModel, IMainWindowViewModel
    {
        private string folder;
        private bool isOperationOngoing;
        private int fileCount;
        private int summedLineCount;

        public MainWindowViewModel()
        {
            Folder = string.Empty;
            IsOperationOngoing = false;
            FileCount = 0;
            SummedLineCount = 0;

            Start = new Command(DoStart,
                                () => !IsOperationOngoing && !string.IsNullOrWhiteSpace(Folder) && Directory.Exists(Folder),
                                new PropertyChangedCommandUpdater(this, nameof(Folder), nameof(IsOperationOngoing)));

            ItemList = new ObservableCollection<Column>();
        }

        private bool IsOperationOngoing
        {
            get { return isOperationOngoing; }
            set { PropertyChanged.ChangeAndNotify(this, ref isOperationOngoing, value); }
        }

        private void DoStart()
        {
            ItemList.Clear();
            IsOperationOngoing = true;
            FileCount = 0;
            SummedLineCount = 0;

            new Thread(() =>
            {
                cancel = false;   
                HandleDirectory(new DirectoryInfo(Folder));

                Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Normal,
                    new Action(() => 
                    {
                        MessageBox.Show("done");
                        IsOperationOngoing = false;
                    })
                );
            }).Start();            
        }

        private bool cancel;

        private void HandleDirectory (DirectoryInfo directory)
        {
            try
            {
                foreach (FileInfo currentFile in directory.GetFiles())
                {

                    if ((currentFile.Extension == ".cs" || currentFile.Extension == ".xaml") &&
                        !directory.FullName.Contains("\\Debug") &&
                        !directory.FullName.Contains("\\packages") &&
                        !directory.FullName.Contains("\\Release"))
                    {

                        int lineCount = 0;

                        var lines = File.ReadAllLines(currentFile.FullName);

                        foreach (string s in lines)
                        {
                            if (!string.IsNullOrWhiteSpace(s))
                                lineCount++;
                        }

                        if (lineCount > 0)
                        {
                            Application.Current.Dispatcher.BeginInvoke(
                                DispatcherPriority.Normal, 
                                new Action(() => 
                                {
                                    ItemList.Add(new Column(currentFile.FullName, lineCount));
                                    FileCount++;
                                    SummedLineCount += lineCount;
                                }))
                            ;
                        }
                    }
                }

                foreach (var currentDirectory in directory.GetDirectories())
                {
                    if (cancel)
                        return;

                    HandleDirectory(currentDirectory);
                }
            }
            catch (Exception except)
            {
                cancel = true;
                Application.Current.Dispatcher.Invoke(() => {
                    MessageBox.Show("error: " + except.Message);
                });
            }
        }

        public ICommand Start { get; }

        public string Folder
        {
            get { return folder; }
            set { PropertyChanged.ChangeAndNotify(this, ref folder, value); }
        }

        public int FileCount
        {
            get { return fileCount; }
            private set { PropertyChanged.ChangeAndNotify(this, ref fileCount, value); }
        }

        public int SummedLineCount
        {
            get { return summedLineCount; }
            private set { PropertyChanged.ChangeAndNotify(this, ref summedLineCount, value); }
        }

        public ObservableCollection<Column> ItemList { get; }

        protected override void CleanUp ()
        {
            throw new System.NotImplementedException();
        }

        public override event PropertyChangedEventHandler PropertyChanged;
    }
}
