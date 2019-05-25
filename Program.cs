using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidCtrl;

namespace diffAW
{
    class Program
    {
        public static string pathToDevice = null;
        public static string pathToLocal = null;
        static void Main(string[] args)
        {
            List<String> androidFiles;
            List<String> windowsFiles;
            Dictionary<String, bool> androidDict = new Dictionary<String, bool>();
            Dictionary<String, bool> windowsDict = new Dictionary<String, bool>();
            if (!handleArgs(args))
            {
                return;
            }
            AndroidManager deviceManager = new AndroidManager();
            if (!deviceManager.prepareADB())
            {
                Console.WriteLine("There's no device connected");
                finish();
                return;
            }
            androidFiles = deviceManager.getAndroidFiles();
            //println("In android:");
            fillDict(androidFiles, androidDict);
            //println("***********************\n");
            WindowsManager windowsManager = new WindowsManager();
            windowsFiles = windowsManager.getLocalFiles();
            //println("In windows:");
            fillDict(windowsFiles, windowsDict);
            //println("***********************\n");

            Dictionary<String, bool> notInWIndows = flipDict(windowsDict, androidDict);
            Dictionary<String, bool> notInAndroid = flipDict(androidDict, windowsDict);
            
            println("Not in windows:");
            foreach (KeyValuePair<String, bool> pair in notInWIndows)
            {
                println(pair.Key);
            }
            println("**************\n");
            println("Not in android:");
            foreach (KeyValuePair<String, bool> pair in notInAndroid)
            {
                println(pair.Key);
            }
            finish();
        }
        static Dictionary<String, bool> flipDict(Dictionary<String, bool> dict1, Dictionary<String, bool> dict2)
        {
            Dictionary<String, bool> tmpDict = new Dictionary<string, bool>(dict2);
            foreach(KeyValuePair<String, bool> pair in dict1)
            {
                if (tmpDict.ContainsKey(pair.Key)) {
                    tmpDict.Remove(pair.Key);
                }
            }
            return tmpDict;
        }
        static void fillDict(List<String> list, Dictionary<String, bool> dict)
        {
            foreach(String s in list)
            {
                //println(s);
                try
                {
                    dict.Add(s, true);
                }
                catch (Exception e) { }
            }
        }
        static bool handleArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-a"))
                {
                    pathToDevice = args[i + 1];
                }
                else if (args[i].Equals("-w"))
                {
                    pathToLocal = args[i + 1];
                }
            }
            if (!checkMainPathsExist())
            {
                return false;
            }
            return true;
        }
        static bool checkMainPathsExist()
        {
            if (pathToLocal == null || pathToDevice == null)
            {
                Console.WriteLine("List the files that doesn't exist in a Windows/Android folder but exist in the Android/Windows  folder.\n");
                Console.WriteLine("diffAW -a androidPath -w windowsPath\n");
                return false;
            }
            return true;
        }
        static void finish()
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
        static void println(Object msg)
        {
            Console.WriteLine(msg);
        }

    }
}
