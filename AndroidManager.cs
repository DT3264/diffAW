using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AndroidCtrl.ADB.Binary;
using AndroidCtrl.ADB.Device.IO;
using AndroidCtrl.Tools;
using AndroidCtrl;


namespace diffAW
{
    class AndroidManager
    {
        List<String> files;
        IDeviceInfo androidDevice;
        Dictionary<String, bool> recursiveFolders = new Dictionary<string, bool>();
        public bool prepareADB()
        {
            Console.WriteLine("Preparing ADB");
            if (!ADB.IntegrityCheck())
            {
                Deploy.ADB();
            }
            Console.WriteLine("Starting ADB");
            ADB.Start();
            if (ADB.Devices().ToArray().Length > 0)
            {
                //Grabs the first device in the list
                androidDevice = ADB.Devices().ToArray()[0];
            }
            return ADB.Devices().ToArray().Length > 0;
        }
        public List<String> getAndroidFiles()
        {
            files = new List<string>();
            Directories _ADBD = ADB.Instance(androidDevice).Device.IO.Directories(Program.pathToDevice);
            _ADBD.Parse(DirectoryParserEventHandler);
            return files;
        }
        public List<String> getAndroidFiles(string path)
        {
            if (files == null)
            {
                files = new List<string>();
            }
            Directories _ADBD = ADB.Instance(androidDevice).Device.IO.Directories(path);
            Console.WriteLine("Looking in " + path);
            _ADBD.Parse(DirectoryParserEventHandler);
            return files;
        }
        public void DirectoryParserEventHandler(object sender, DirectoryParserEventArgs e)
        {
            if (e.Element.ID == FileType.File)
            {
                files.Add(niceFileName(e.Element.Path));
            }
            else if(e.Element.ID==FileType.Directory && Program.recursiveAndroid)
            {
                getAndroidFiles(e.Element.Path);
            }
        }

        public string niceFileName(string fileName)
        {
            string[] arr = fileName.Split('/');
            return arr[arr.Length - 1];
        }
    }
}