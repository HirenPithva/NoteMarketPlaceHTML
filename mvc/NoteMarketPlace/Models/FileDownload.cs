using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using System.IO;
namespace NoteMarketPlace.Models
{
    public class FileDownload
    {
        public List<fileInfo> GetFile(string path)
        {
            List<fileInfo> listFiles = new List<fileInfo>();
            string Direcotorypath = System.Web.Hosting.HostingEnvironment.MapPath(path);
            DirectoryInfo DirInfo = new DirectoryInfo(Direcotorypath);
            int i = 0;
            foreach(var item in DirInfo.GetFiles())
            {
                listFiles.Add(new fileInfo() { 
                    FileID=i+1,
                    FileName=item.Name,
                    FilePath=item.FullName
                });
                i = i + 1;
            }
            return listFiles;
        }
    }
}