using System;
using System.Windows.Data;
using System.Globalization;
using BlogMarnagerLite.MVVM.Model;

namespace BlogMarnagerLite.Converter {
    internal class PageFileDisplayConverterStatusColor : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            switch((FileStatus)value) {
                case FileStatus.Edited:
                    return "#f55762";
                case FileStatus.Committed:
                    return "#FFE4A0";
                case FileStatus.Uploaded:
                    return "#92E492";
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
