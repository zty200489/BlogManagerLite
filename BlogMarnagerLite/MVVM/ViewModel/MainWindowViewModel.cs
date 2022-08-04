using BlogMarnagerLite.Core;
using BlogMarnagerLite.MVVM.Model;
using BlogMarnagerLite.MVVM.View;
using System.Windows;

namespace BlogMarnagerLite.MVVM.ViewModel {
    internal class MainWindowViewModel {
        public object View { get; set; }
        public RelayCommand DragWindowCommand { get; set; }
        public RelayCommand MinimizeWindowCommand { get; set; }
        public RelayCommand MaximizeWindowComamnd { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }

        private PageFileView _PageFileView { get; set; }

        public MainWindowViewModel() {
            BlogContent.Load();

            _PageFileView = new PageFileView();

            View = _PageFileView;

            Application.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MinimizeWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });
            MaximizeWindowComamnd = new RelayCommand(o => { Application.Current.MainWindow.WindowState = (Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized); });
            CloseWindowCommand = new RelayCommand(o => { Application.Current.Shutdown(); });
            DragWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.DragMove(); });
        }
    }
}
