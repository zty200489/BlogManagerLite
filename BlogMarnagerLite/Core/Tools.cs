using CommonMark;
using System.Security.Cryptography;
using System.Text;

namespace BlogMarnagerLite.Core {
    internal class Tools {
        public static string GetDirectory() {
            return System.IO.Directory.GetCurrentDirectory();
        }

        public static string ComputeSHA256(string Content) {
            byte[] ResultBytes;
            StringBuilder ResultString;
            using(var Converter = SHA256.Create()) {
                ResultBytes = Converter.ComputeHash(Encoding.UTF8.GetBytes(Content));
            }
            ResultString = new StringBuilder();
            foreach(byte b in ResultBytes) {
                ResultString.Append(b.ToString("x2"));
            }
            return ResultString.ToString();
        }
        public static string ConvertMarkdown(string Content) {
            return CommonMarkConverter.Convert(Content);
        }
        public static string GetAbstract(string Content, int Length) {
            string Result = "";
            bool flag = false, space = false;
            for(int i = 0; i < Content.Length; i++) {
                if(flag) flag = (Content[i] != '>');
                else {
                    if(Content[i] == '<') {
                        flag = true;
                    } else if(Content[i] == ' ') {
                        if(!space) {
                            Result += Content[i];
                            if(--Length == 0) {
                                return Result;
                            }
                            space = true;
                        }
                    } else if(Content[i] == '\r' || Content[i] == '\n') {
                        space = false;
                        Result += ' ';
                        if(--Length == 0) {
                            return Result;
                        }
                    } else {
                        space = false;
                        Result += Content[i];
                        if(--Length == 0) {
                            return Result;
                        }
                    }
                }
            }
            return Result;
        }
    }
}
