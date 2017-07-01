using System.Collections.ObjectModel;
using System.Windows.Input;
using bytePassion.Lib.WpfLib.ViewModelBase;
using FileOperator.ViewModel.Helper;

namespace FileOperator.ViewModel
{
    internal interface IMainWindowViewModel : IViewModel
    {
        ICommand Start { get; }

        string Folder { get; set; }
        
        int FileCount       { get; }
        int SummedLineCount { get; }

        bool CountEmptyLines               { get; set; }
        bool CountUsings                   { get; set; }
        bool CountLinesWithOnlyOneCharater { get; set; }

        ObservableCollection<Column> ItemList { get; }
    }
}