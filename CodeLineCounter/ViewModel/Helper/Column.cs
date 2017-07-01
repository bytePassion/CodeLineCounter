namespace FileOperator.ViewModel.Helper
{
    public class Column
    {
        public Column (string file, int lines)
        {
            CurrentFile  = file;
            CountedLines = lines;
        }

        public string CurrentFile  { get; set; }
        public int    CountedLines { get; set; }
    }
}
