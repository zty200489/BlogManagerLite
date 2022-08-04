using BlogMarnagerLite.Core;
using BlogMarnagerLite.MVVM.Model;
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
            BlogContent.UpdateContent();
            FileDisplayList.Clear();
            foreach(var File in BlogContent.Pages.Values) {
                FileDisplayList.Add(File);
            }
        }
        public PageFileViewModel() {
            FileDisplayList = new ObservableCollection<PageFile>();
            UpdateList();

            RefreshAllCommand = new RelayCommand(o => { UpdateList(); });
            CommitAllCommand = new RelayCommand(o => {
                foreach(var File in FileDisplayList) {
                    if(BlogContent.Pages[File.Name].Status == FileStatus.Edited) {
                        BlogContent.Pages[File.Name].Content = Tools.ConvertMarkdown(BlogContent.Pages[File.Name].FileContent);
                        BlogContent.Pages[File.Name].Abstract = Tools.GetAbstract(BlogContent.Pages[File.Name].Content, 150);
                        BlogContent.Pages[File.Name].Status = FileStatus.Committed;
                    }
                }
                UpdateList();
            });
            UploadAllCommand = new RelayCommand(o => {
                foreach(var File in FileDisplayList) {
                    if(BlogContent.Pages[File.Name].Status == FileStatus.Edited) {
                        BlogContent.Pages[File.Name].Content = Tools.ConvertMarkdown(BlogContent.Pages[File.Name].FileContent);
                        BlogContent.Pages[File.Name].Abstract = Tools.GetAbstract(BlogContent.Pages[File.Name].Content, 150);
                        BlogContent.Pages[File.Name].Status = FileStatus.Committed;
                    }
                    TemplateDocument PostTemplate = new TemplateDocument(Tools.GetDirectory() + "\\Templates\\post.cshtml", Encoding.UTF8);
                    if(BlogContent.Pages[File.Name].Status == FileStatus.Committed) {
                        PostTemplate.SetValue("Model", BlogContent.Pages[File.Name]);
                        if(!Directory.Exists(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory)) {
                            Directory.CreateDirectory(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory);
                        }
                        PostTemplate.RenderTo(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory + "\\" + BlogContent.Pages[File.Name].Name + ".html", Encoding.UTF8);
                        BlogContent.Pages[File.Name].Status = FileStatus.Uploaded;
                    }
                }

                TemplateDocument IndexTemplate = new TemplateDocument(Tools.GetDirectory() + "\\Templates\\index.cshtml", Encoding.UTF8);
                IndexTemplate.SetValue("Model", BlogContent.Pages);
                IndexTemplate.RenderTo(Tools.GetDirectory() + "\\Posts\\index.html", Encoding.UTF8);
                var git = new CommandRunner("git", Tools.GetDirectory() + "\\Posts");
                git.Run(@"add .");
                git.Run(@"commit -m ""auto commit""");
                git.Run(@"push hub master");
                UpdateList();
            });
            ForcePushCommand = new RelayCommand(o => {
                foreach(var File in FileDisplayList) {
                    //if(BlogContent.Pages[File.Name].Status == FileStatus.Edited) {
                        BlogContent.Pages[File.Name].Content = Tools.ConvertMarkdown(BlogContent.Pages[File.Name].FileContent);
                        BlogContent.Pages[File.Name].Abstract = Tools.GetAbstract(BlogContent.Pages[File.Name].Content, 150);
                        BlogContent.Pages[File.Name].Status = FileStatus.Committed;
                    //
                    TemplateDocument PostTemplate = new TemplateDocument(Tools.GetDirectory() + "\\Templates\\post.cshtml", Encoding.UTF8);
                    //if(BlogContent.Pages[File.Name].Status == FileStatus.Committed) {
                        PostTemplate.SetValue("Model", BlogContent.Pages[File.Name]);
                        if(!Directory.Exists(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory)) {
                            Directory.CreateDirectory(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory);
                        }
                        PostTemplate.RenderTo(Tools.GetDirectory() + "\\Posts\\pages" + BlogContent.Pages[File.Name].Directory + "\\" + BlogContent.Pages[File.Name].Name + ".html", Encoding.UTF8);
                        BlogContent.Pages[File.Name].Status = FileStatus.Uploaded;
                    //}
                }

                TemplateDocument IndexTemplate = new TemplateDocument(Tools.GetDirectory() + "\\Templates\\index.cshtml", Encoding.UTF8);
                IndexTemplate.SetValue("Model", BlogContent.Pages);
                IndexTemplate.RenderTo(Tools.GetDirectory() + "\\Posts\\index.html", Encoding.UTF8);
                var git = new CommandRunner("git", Tools.GetDirectory() + "\\Posts");
                git.Run(@"add .");
                git.Run(@"commit -m ""auto commit""");
                git.Run(@"push hub master");
                UpdateList();
            });
        }
    }
}
