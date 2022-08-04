using BlogMarnagerLite.MVVM.Model;
using System.Windows;

namespace BlogMarnagerLite.MVVM.View {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            this.SizeChanged += WindowSizeChangedHandler;
        }
        public void WindowSizeChangedHandler(object sender, SizeChangedEventArgs e) {
            switch(this.WindowState) {
                case WindowState.Normal:
                    this.BorderThickness = new Thickness(0);
                    this.MaximizeWindow.Visibility = Visibility.Visible;
                    this.RestoreWindow.Visibility = Visibility.Hidden;
                    break;
                case WindowState.Maximized:
                    this.BorderThickness = new Thickness(7);
                    this.MaximizeWindow.Visibility = Visibility.Hidden;
                    this.RestoreWindow.Visibility = Visibility.Visible;
                    break;
            }
        }
        public void OnWindowClosed(object sender, System.EventArgs e) {
            BlogContent.Save();
        }
    }
}
