using System;
using System.IO;
using System.Collections.Generic;

namespace diffAW
{
    class WindowsManager
    {
        List<FileInfo> files;
        public List<String> getLocalFiles()
        {
            files = getFilesInPath();
            List<int> toRemove = new List<int>();
            List<String> stringFiles = new List<String>();
            for(int i=0; i<files.Count; i++)
            {
                FileInfo file = files[i];
                if (!skipFile(file.Name)){
                    stringFiles.Add(file.Name);
                }
                else
                {
                    toRemove.Add(i);
                }
            }
            for(int i=toRemove.Count-1; i>=0 ; i--)
            {
                files.RemoveAt(toRemove[i]);
            }
            return stringFiles;
        }

        bool skipFile(string fileName)
        {
            //I have some albumarts on the folder, but i don't need them
            return fileName.Contains("AlbumArt");
        }
        List<FileInfo> getFilesInPath()
        {
            string match = "*.*";
            string[] files = Directory.GetFiles(Program.pathToLocal, match, SearchOption.AllDirectories);
            List<FileInfo> toReturn=new List<FileInfo>();
            foreach(String file in files)
            {
                toReturn.Add(new FileInfo(file));
            }
            return toReturn;
        }
    }
}
