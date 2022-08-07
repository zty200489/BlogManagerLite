using BlogMarnagerLite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlogMarnagerLite.MVVM.ViewModel {
    internal delegate void RecallMethods(string Line);
    internal class MottoUserInputViewModel : ObservableObject {
        public string InputText { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand DragWindowCommand { get; set; }
        public RelayCommand ConfirmCommand { get; set; }

        public RecallMethods Recaller;

        public MottoUserInputViewModel() {
            CloseWindowCommand = new RelayCommand(o => { (o as Window).Close(); });
            DragWindowCommand = new RelayCommand(o => { (o as Window).DragMove(); });
            ConfirmCommand = new RelayCommand(o => { Recaller?.Invoke(InputText); (o as Window).Close(); });
        }
    }
}
