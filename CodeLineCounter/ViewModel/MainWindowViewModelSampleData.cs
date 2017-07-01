using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using FileOperator.ViewModel.Helper;

#pragma warning disable 0067

namespace FileOperator.ViewModel
{
    internal class MainWindowViewModelSampleData : IMainWindowViewModel
    {
        public MainWindowViewModelSampleData()
        {
            Folder = @"C:\Test";
            FileCount = 234;
            SummedLineCount = 123456;
            CountEmptyLines = true;
            CountLinesWithOnlyOneCharater = true;
            CountUsings = true;

            ItemList = new ObservableCollection<Column>
            {
                new Column("blubb1.cs", 100),
                new Column("blubb2.cs", 200),
                new Column("blubb3.cs", 300),
                new Column("blubb4.cs", 400),
                new Column("blubb5.cs", 500),
            };
        }

        public ICommand Start => null;

        public string Folder { get; set; }
        public int FileCount { get; }
        public int SummedLineCount { get; }
        public bool CountEmptyLines { get; set; }
        public bool CountUsings { get; set; }
        public bool CountLinesWithOnlyOneCharater { get; set; }

        public ObservableCollection<Column> ItemList { get; }

        public void Dispose () { }
        public event PropertyChangedEventHandler PropertyChanged;                
    }
}