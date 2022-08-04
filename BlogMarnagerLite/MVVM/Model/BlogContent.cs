using System.IO;
using BlogMarnagerLite.Core;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace BlogMarnagerLite.MVVM.Model {
    internal static class BlogContent {
        public static Dictionary<string, PageFile> Pages = new Dictionary<string, PageFile>();

        public static void UpdateContent() {
            if(!Directory.Exists(Tools.GetDirectory() + "\\Pages")) {
                Directory.CreateDirectory(Tools.GetDirectory() + "\\Pages");
                return;
            }
            var Folder = new DirectoryInfo(Tools.GetDirectory() + "\\Pages");
            Dictionary<string, PageFile> Match = new Dictionary<string, PageFile>();
            foreach(var File in Folder.GetFiles()) {
                if(File.Extension != ".md") continue;
                PageFile FileInfo = new PageFile();
                string Name = Path.GetFileNameWithoutExtension(File.Name);
                FileInfo.Name = Name;
                FileInfo.Title = Name;
                FileInfo.DateDay = File.CreationTime.Day;
                FileInfo.DateMonth = File.CreationTime.Month;
                FileInfo.DateYear = File.CreationTime.Year;
                using(var DataStream = File.OpenText()) {
                    FileInfo.FileContent = DataStream.ReadToEnd();
                    FileInfo.SHA256 = Tools.ComputeSHA256(FileInfo.FileContent);
                }
                FileInfo.Status = FileStatus.Edited;

                if(Pages.ContainsKey(Name) &&
                   Pages[Name].Status != FileStatus.Edited &&
                   Pages[Name].SHA256 == FileInfo.SHA256) {
                    FileInfo.Status = Pages[Name].Status;
                    FileInfo.Content = Pages[Name].Content;
                    FileInfo.Abstract = Pages[Name].Abstract;
                }

                Match.Add(Name, FileInfo);
            }
            Pages = Match;
        }
        public static void Load() {
            if(!File.Exists(Tools.GetDirectory() + "\\pages.json")) return;
            using(var Reader = new StreamReader(Tools.GetDirectory() + "\\pages.json")) {
                string SerializedData = Reader.ReadToEnd();
                Pages = JsonConvert.DeserializeObject<Dictionary<string, PageFile> >(SerializedData);
            }
        }
        public static void Save() {
            string SerialzedData = JsonConvert.SerializeObject(Pages);
            using(var Writer = new StreamWriter(Tools.GetDirectory() + "\\pages.json", false, Encoding.UTF8)) {
                Writer.Write(SerialzedData);
            }
        }
    }
}
