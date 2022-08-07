using System;
using System.Collections.Generic;
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

namespace BlogMarnagerLite.MVVM.View {
    /// <summary>
    /// MottoView.xaml 的交互逻辑
    /// </summary>
    public partial class MottoView : UserControl {
        public MottoView() {
            InitializeComponent();
        }
        private void OnWidthChanged(object sender, SizeChangedEventArgs e) {
            this.ContentColumn.Width = this.DisplayContainer.ActualWidth - 123;
        }
    }
}
