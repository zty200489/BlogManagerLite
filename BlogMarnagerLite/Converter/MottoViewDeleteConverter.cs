using BlogMarnagerLite.Core;
using BlogMarnagerLite.MVVM.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace BlogMarnagerLite.Converter {
    internal class MottoViewDeleteConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return new RelayCommand(o => {
                BlogContent.DeleteMotto(value as string);
            });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
