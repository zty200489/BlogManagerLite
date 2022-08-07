using BlogMarnagerLite.Core;
using BlogMarnagerLite.MVVM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using VTemplate.Engine;

namespace BlogMarnagerLite.MVVM.ViewModel {
    class PageFileViewModel {
        public ObservableCollection<PageFile> FileDisplayList { get; set; }
        public RelayCommand RefreshAllCommand { get; set; }
        public RelayCommand CommitAllCommand { get; set; }
        public RelayCommand UploadAllCommand { get; set; }
        public RelayCommand ForcePushCommand { get; set; }

        public void UpdateList() {
            List<PageFile> DisplayList = new List<PageFile>();
            foreach(var File in BlogContent.Pages.Values) {
                DisplayList.Add(File);
            }
            DisplayList.Sort(delegate(PageFile x, PageFile y) {
                if(x.DateYear == y.DateYear) {
                    if(x.DateMonth == y.DateMonth) {
                        return Comparer<int>.Default.Compare(y.DateDay, x.DateDay);
                    } else return Comparer<int>.Default.Compare(y.DateMonth, x.DateMonth);
                } else return Comparer<int>.Default.Compare(y.DateYear, x.DateYear);
            });
            FileDisplayList.Clear();
            for(int i = 0; i < DisplayList.Count; i++) {
                FileDisplayList.Add(DisplayList[i]);
            }
        }
        public PageFileViewModel() {
            FileDisplayList = new ObservableCollection<PageFile>();
            UpdateList();

            RefreshAllCommand = new RelayCommand(o => { UpdateList(); });
            CommitAllCommand = new RelayCommand(o => {
                for(int i = 0; i < FileDisplayList.Count; i++) {
                    var File = FileDisplayList[i];
                    if(BlogContent.Pages[File.Name].Status == FileStatus.Edited) {
                        BlogContent.Pages[File.Name].Content = Tools.ConvertMarkdown(BlogContent.Pages[File.Name].FileContent);
                        BlogContent.Pages[File.Name].Abstract = Tools.GetAbstract(BlogContent.Pages[File.Name].Content, 150);
                        BlogContent.Pages[File.Name].Status = FileStatus.Committed;
                        FileDisplayList.Remove(File);
                        File.Status = FileStatus.Committed;
                        FileDisplayList.Insert(i, File);
                    }
                }
            });
            UploadAllCommand = new RelayCommand(o => {
                DirectoryInfo Folder = new DirectoryInfo(Tools.GetDirectory() + "\\Posts\\pages");
                Folder.Delete(true);

                for(int i = 0; i < FileDisplayList.Count; i++) {
                    var File = FileDisplayList[i];
                    //if(BlogContent.Pages[File.Name].Status == FileStatus.Edited) {
                        BlogContent.Pages[File.Name].Content = Tools.ConvertMarkdown(BlogContent.Pages[File.Name].FileContent);
                        BlogContent.Pages[File.Name].Abstract = Tools.GetAbstract(BlogContent.Pages[File.Name].Content, 150);
                        BlogContent.Pages[File.Name].Status = FileStatus.Committed;
                        FileDisplayList.Remove(File);
                        File.Status = FileStatus.Committed;
                        FileDisplayList.Insert(i, File);
                    //}
                    TemplateDocument PostTemplate = new TemplateDocument(Tools.GetDirectory() + "\\Templates\\post.cshtml", Encoding.UTF8);
                    //if(BlogContent.Pages[File.Name].Status == FileStatus.Committed) {
                        PostTemplate.SetValue("Model", BlogContent.Pages[File.Name]);
                        if(!Directory.Exists(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory)) {
                            Directory.CreateDirectory(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory);
                        }
                        PostTemplate.RenderTo(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory + "\\" + BlogContent.Pages[File.Name].Name + ".html", Encoding.UTF8);
                        BlogContent.Pages[File.Name].Status = FileStatus.Uploaded;
                        FileDisplayList.Remove(File);
                        File.Status = FileStatus.Uploaded;
                        FileDisplayList.Insert(i, File);
                    //}
                }

                TemplateDocument IndexTemplate = new TemplateDocument(Tools.GetDirectory() + "\\Templates\\index.cshtml", Encoding.UTF8);
                IndexTemplate.SetValue("Model", BlogContent.Pages);
                IndexTemplate.RenderTo(Tools.GetDirectory() + "\\Posts\\index.html", Encoding.UTF8);
                BlogContent.OutputMottoJson(Tools.GetDirectory() + "\\Posts\\assets\\json");

                string RemoteServer = File.ReadAllText(Tools.GetDirectory() + "\\.token");
                var git = new CommandRunner("git", Tools.GetDirectory() + "\\Posts");
                git.Run(@"add .");
                git.Run(@"commit -m ""auto commit""");
                git.Run(@"push " + RemoteServer + @" master");
            });
            /*
            ForcePushCommand = new RelayCommand(o => {
                for(int i = 0; i < FileDisplayList.Count; i++) {
                    var File = FileDisplayList[i];
                    //if(BlogContent.Pages[File.Name].Status == FileStatus.Edited) {
                        BlogContent.Pages[File.Name].Content = Tools.ConvertMarkdown(BlogContent.Pages[File.Name].FileContent);
                        BlogContent.Pages[File.Name].Abstract = Tools.GetAbstract(BlogContent.Pages[File.Name].Content, 150);
                        BlogContent.Pages[File.Name].Status = FileStatus.Committed;
                        FileDisplayList.Remove(File);
                        File.Status = FileStatus.Committed;
                        FileDisplayList.Insert(i, File);
                    //}
                    TemplateDocument PostTemplate = new TemplateDocument(Tools.GetDirectory() + "\\Templates\\post.cshtml", Encoding.UTF8);
                    //if(BlogContent.Pages[File.Name].Status == FileStatus.Committed) {
                        PostTemplate.SetValue("Model", BlogContent.Pages[File.Name]);
                        if(!Directory.Exists(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory)) {
                            Directory.CreateDirectory(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory);
                        }
                        PostTemplate.RenderTo(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory + "\\" + BlogContent.Pages[File.Name].Name + ".html", Encoding.UTF8);
                        BlogContent.Pages[File.Name].Status = FileStatus.Uploaded;
                        FileDisplayList.Remove(File);
                        File.Status = FileStatus.Uploaded;
                        FileDisplayList.Insert(i, File);
                    //}
                }

                TemplateDocument IndexTemplate = new TemplateDocument(Tools.GetDirectory() + "\\Templates\\index.cshtml", Encoding.UTF8);
                IndexTemplate.SetValue("Model", BlogContent.Pages);
                IndexTemplate.RenderTo(Tools.GetDirectory() + "\\Posts\\index.html", Encoding.UTF8);
                string RemoteServer = File.ReadAllText(Tools.GetDirectory() + ".token");
                var git = new CommandRunner("git", Tools.GetDirectory() + "\\Posts");
                git.Run(@"add .");
                git.Run(@"commit -m ""auto commit""");
                git.Run(@"push " + RemoteServer + @" master");
            });
            */
        }
    }
}
