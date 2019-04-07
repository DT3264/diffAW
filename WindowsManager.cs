using System;
using System.IO;
using System.Collections.Generic;

namespace diffAW
{
    class WindowsManager
    {
        List<FileInfo> files;
        public WindowsManager()
        {
            if (!Directory.Exists(Program.pathToMove))
            {
                Console.WriteLine("To: " + Program.pathToMove);
                Directory.CreateDirectory(Program.pathToMove);
            }
        }
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

        public void moveFile(int index)
        {
            FileInfo actualFile = files[index];
            try
            {
                if (Program.move)
                {
                    actualFile.MoveTo(Program.pathToMove + actualFile.Name);
                }
                else
                {
                    actualFile.CopyTo(Program.pathToMove + actualFile.Name, true);
                }
            }
            catch(IOException e)
            {
                Console.WriteLine("Cannot move " + actualFile.Name);
            }
        }

        bool skipFile(string fileName)
        {
            //I have some albumarts on the folder, but i don't need them
            return fileName.Contains("AlbumArt");
        }
        List<FileInfo> getFilesInPath()
        {
            string match = "*.*";
            string[] files = Directory.GetFiles(Program.pathToLocal, match, (Program.recursiveWindows ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly));
            List<FileInfo> toReturn=new List<FileInfo>();
            foreach(String file in files)
            {
                toReturn.Add(new FileInfo(file));
            }
            return toReturn;
        }
    }
}
