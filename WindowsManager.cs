using System;
using System.IO;
using System.Collections.Generic;

namespace diffAW
{
    class WindowsManager
    {
        string path;
        string pathToMove;
        List<FileInfo> files;
        bool shouldMove;
        public WindowsManager(string path)
        {
            shouldMove = true;
            this.path = path;
            pathToMove = path + @"\temp\";
            if (!Directory.Exists(pathToMove))
            {
                Directory.CreateDirectory(pathToMove);
            }
            else
            {
                Directory.Delete(pathToMove, true);
                Directory.CreateDirectory(pathToMove);
            }
        }
        public List<String> getLocalFiles()
        {
            files = getFilesInPath(path);
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

        public void moveSong(int index)
        {
            FileInfo actualFile = files[index];
            if (shouldMove)
            {
                actualFile.MoveTo(pathToMove + actualFile.Name);
            }
            else
            {
                actualFile.CopyTo(pathToMove + actualFile.Name, true);
            }
        }

        bool skipFile(string fileName)
        {
            //I have some albumarts on the folder, but i don't need them
            return fileName.Contains("AlbumArt");
        }
        List<FileInfo> getFilesInPath(string _path)
        {
            string match = "*.*";
            string[] files = Directory.GetFiles(_path, match, SearchOption.AllDirectories);
            List<FileInfo> toReturn=new List<FileInfo>();
            foreach(String file in files)
            {
                toReturn.Add(new FileInfo(file));
            }
            return toReturn;
        }
    }
}
