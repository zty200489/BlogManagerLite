using System;
using System.Windows.Data;
using System.Globalization;
using BlogMarnagerLite.MVVM.Model;

namespace BlogMarnagerLite.Converter {
    internal class PageFileDisplayConverterStatusText : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            switch((FileStatus)value) {
                case FileStatus.Edited:
                    return "Edited";
                case FileStatus.Committed:
                    return "Committed";
                case FileStatus.Uploaded:
                    return "Uploaded";
            }
            throw new NotImplementedException();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
