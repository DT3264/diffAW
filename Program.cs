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
        public static string pathToDevice=null;
        public static string pathToLocal=null;
        public static string pathToMove = @"\diff\";
        public static bool recursiveAndroid = true;
        public static bool recursiveWindows = true;
        public static bool move = false;
        public static int verbose = 3;
        static void Main(string[] args)
        {
            int cont = 0;
            List<String> androidFiles;
            List<String> windowsFiles;
            Dictionary<String, bool> filesInAndorid = new Dictionary<String, bool>();
            move = true;
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
            println("Files in Android:", 1);
            foreach (string file in androidFiles)
            {
                println(file, 1);
                filesInAndorid.Add(file, true);
            }

            println("", 1);

            WindowsManager windowsManager = new WindowsManager();
            windowsFiles = windowsManager.getLocalFiles();
            println("Files in windows:", 2);
            bool exist;
            for (int i = 0; i < windowsFiles.Count; i++)
            {
                string file = windowsFiles[i];
                filesInAndorid.TryGetValue(file, out exist);
                if (exist)
                {
                    println(file, 1);
                }
                if (!exist)
                {
                    println("*" + file, 2);
                    windowsManager.moveFile(i);
                    cont++;
                }
            }
            Console.WriteLine(cont + " files don't match.");
            finish();
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
                    pathToMove = pathToLocal + pathToMove;
                }
            }
            if (!checkMainPathsExist())
            {
                return false;
            }
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-t"))
                {
                    pathToMove = pathToLocal + args[i + 1];
                }
                else if (args[i].Equals("-ra"))
                {
                    if (args[i + 1].Equals("n"))
                    {
                        recursiveAndroid = false;
                    }
                }
                else if (args[i].Equals("-rw"))
                {
                    if (args[i + 1].Equals("n"))
                    {
                        recursiveWindows = false;
                    }
                }
                else if (args[i].Equals("-m"))
                {
                    if (args[i + 1].Equals("y"))
                    {
                        move = true;
                    }
                }
                else if (args[i].Equals("-v"))
                {
                    int verbose = int.Parse(args[i + 1]);
                    if (verbose >= 1 && verbose <= 3)
                    {
                        Program.verbose = verbose;
                    }
                }
            }
            return true;
        }
        static bool checkMainPathsExist()
        {
            if (pathToLocal == null || pathToDevice == null)
            {
                Console.WriteLine("Copy/Move the files that doesn't exist in a Windows folder but exist in an Android device folder.\n");
                Console.WriteLine("diffAW -a androidPath -w windowsPath [-t diffFolder] [-ra (y|n)] [-ra (y|n)] [-m (y|n)] [-v (1|2|3)]\n");
                Console.WriteLine("-t diffFolder\nthe folder where the files that are in the android folder but no in the windows one. Default: \"diff\"\n");
                Console.WriteLine("-ra {y(es)|n(o)}\nsearch recursively in the android folders. Default: Yes\n");
                Console.WriteLine("-rw {y(es)|n(o)}\nsearch recursively in the windows folders. Default: Yes\n");
                Console.WriteLine("-m {(y(es)|n(o)}\nmove files from the windows folder to the diff folder if doesn't exist, otherwise just copy. Default: No\n");
                Console.WriteLine("-v (1|2|3)\nverbose modes: 1.Print found and diff files, 2.Print diff files, 3.Print only progress. Default: 3\n");
                return false;
            }
            return true;
        }
        static void finish()
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        static void println(Object msg, int requiredLevel)
        {
            if (verbose <= requiredLevel)
            {
                Console.WriteLine(msg);
            }
        }

    }
}
