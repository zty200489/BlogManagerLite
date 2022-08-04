namespace BlogMarnagerLite.MVVM.Model {
    public enum FileStatus {
        Edited,
        Committed,
        Uploaded
    }
    internal class PageFile {
        private int _DateDay;
        private int _DateMonth;
        private int _DateYear;
        private string _SHA256;

        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FileContent { get; set; }
        public string Abstract { get; set; }
        public FileStatus Status { get; set; }
        public string SHA256 { get { return _SHA256; } set { _SHA256 = value; } }
        public string FileID { get { return Title + "@" + _SHA256; } }
        public int DateDay { get { return _DateDay; } set { _DateDay = value; } }
        public int DateMonth { get { return _DateMonth; } set { _DateMonth = value; } }
        public int DateYear { get { return _DateYear; } set { _DateYear = value; } }
        public string Directory { get { return "\\" + _DateYear.ToString() + "\\" + _DateMonth.ToString() + "\\" + DateDay.ToString(); } }
    }
}
