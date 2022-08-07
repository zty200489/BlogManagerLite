using BlogMarnagerLite.Core;
using BlogMarnagerLite.MVVM.Model;
using BlogMarnagerLite.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogMarnagerLite.MVVM.ViewModel {
    internal class MottoViewModel {
        public RelayCommand AddNewMottoCommand { get; set; }
        public RelayCommand UpdateMottoCommand { get; set; }
        public ObservableCollection<Motto> DisplayList { get; set; }

        public void AddNewMotto(string Line) {
            Motto motto = new Motto();
            motto.Content = Line;
            BlogContent.Mottos.Add(motto);
            UpdateList();
        }
        private void UpdateList() {
            DisplayList.Clear();
            BlogContent.Mottos.Sort(delegate(Motto x, Motto y) { return Comparer<string>.Default.Compare(x.Content, y.Content); });
            for(int i = 0; i < BlogContent.Mottos.Count; i++) {
                DisplayList.Add(BlogContent.Mottos[i]);
            }
        }

        public MottoViewModel() {
            DisplayList = new ObservableCollection<Motto>();
            UpdateList();
            BlogContent.Recaller += UpdateList;

            AddNewMottoCommand = new RelayCommand(o => {
                var Display = new MottoUserInputView();
                var DisplayViewModel = new MottoUserInputViewModel();
                DisplayViewModel.Recaller += AddNewMotto;
                Display.DataContext = DisplayViewModel;
                Display.Show();
            });

            UpdateMottoCommand = new RelayCommand(o => {
                BlogContent.OutputMottoJson(Tools.GetDirectory() + "\\Posts\\assets\\json");
                string RemoteServer = File.ReadAllText(Tools.GetDirectory() + "\\.token");
                var git = new CommandRunner("git", Tools.GetDirectory() + "\\Posts");
                git.Run(@"add .");
                git.Run(@"commit -m ""auto commit""");
                git.Run(@"push " + RemoteServer + @" master");
            });
        }
    }
}
