using BlogMarnagerLite.Core;
using BlogMarnagerLite.MVVM.Model;
using BlogMarnagerLite.MVVM.View;
using System.Diagnostics;
using System.Windows;

namespace BlogMarnagerLite.MVVM.ViewModel {
    internal class MainWindowViewModel : ObservableObject{
        private object _View;
        public object View {
            get { return _View; }
            set {
                _View = value;
                OnPropertyChanged();
            }
        }
        public RelayCommand DragWindowCommand { get; set; }
        public RelayCommand MinimizeWindowCommand { get; set; }
        public RelayCommand MaximizeWindowComamnd { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand ChangeViewPages { get; set; }
        public RelayCommand ChangeViewMottos { get; set; }

        private PageFileView _PageFileView { get; set; }
        private MottoView _MottoView { get; set; }

        public MainWindowViewModel() {
            BlogContent.Load();
            BlogContent.UpdateContent();

            _PageFileView = new PageFileView();
            _MottoView = new MottoView();

            View = _PageFileView;

            Application.Current.MainWindow.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MinimizeWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.WindowState = WindowState.Minimized; });
            MaximizeWindowComamnd = new RelayCommand(o => { Application.Current.MainWindow.WindowState = (Application.Current.MainWindow.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized); });
            CloseWindowCommand = new RelayCommand(o => { Application.Current.Shutdown(); });
            DragWindowCommand = new RelayCommand(o => { Application.Current.MainWindow.DragMove(); });
            ChangeViewPages = new RelayCommand(o => { View = _PageFileView; });
            ChangeViewMottos = new RelayCommand(o => { View = _MottoView; });
        }
    }
}
