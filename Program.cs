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

        //Android paths are linux style
        static string pathToDevice=null;// = "/storage/49ED-1907/musica uwu/";
        static string pathToLocal=null;// = @"C:\Users\Dani\Music\musica uwu";
        static string pathToMove = @"\temp\";
        static bool recursiveAndroid = true;
        static bool recursiveWindows = true;
        static bool move = false;
        static int verbose = 3;
        static void Main(string[] args)
        {
            for(int i=0; i<args.Length; i++)
            {
                //-a /storage.../sd 
                //-w C:\Users\GGG 
                //-t temp(default name: diff) 
                //-r x(recursive: y(es):default n(o))
                //-m x(y(es, move) n(o, copy):default) 
                //-v(verbose type->1:all, 2:only diff files, 3:none:default)
                if (args[i].Equals("-a"))
                {
                    pathToDevice = args[i + 1];
                }
                else if (args[i].Equals("-w"))
                {
                    pathToLocal = args[i + 1];
                }
                else if (args[i].Equals("-t"))
                {
                    pathToMove = args[i + 1];
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
                else if (args[i].Equals("-m")){
                    if (args[i + 1].Equals("y"))
                    {
                        move = true;
                    }
                }
                else if (args[i].Equals("-v"))
                {
                    int verbose = int.Parse(args[i + 1]);
                    if (verbose>=1 && verbose<=3)
                    {
                        Program.verbose = verbose;
                    }
                }
            }
            if(pathToLocal==null || pathToDevice == null)
            {
                Console.WriteLine("Copy/Move the files that doesn't exist in a Windows folder but exist in an Android device folder.\n");
                Console.WriteLine("diffAW -a androidPath -w windowsPath [-t diffFolder] [-ra (y|n)] [-ra (y|n)] [-m (y|n)] [-v (1|2|3)]\n");
                Console.WriteLine("-t diffFolder\nthe folder where the files that are in the android folder but no in the windows one. Default: \"diff\"\n");
                Console.WriteLine("-ra {y(es)|n(o)}\nsearch recursively in the android folders. Default: Yes\n");
                Console.WriteLine("-rw {y(es)|n(o)}\nsearch recursively in the windows folders. Default: Yes\n");
                Console.WriteLine("-m {(y(es)|n(o)}\nmove files from the windows folder to the diff folder if doesn't exist, otherwise just copy. Default: No\n");
                Console.WriteLine("-v (1|2|3)\nverbose modes: 1.Print found and diff files, 2.Print diff files, 3.Print only progress. Default: 3\n");
                finish();
                return;
            }
            List<String> androidFiles;
            List<String> windowsFiles;
            Dictionary<String, bool> filesInAndorid = new Dictionary<String, bool>();
            AndroidManager deviceManager = new AndroidManager(pathToDevice, true);
            if (!deviceManager.prepareADB())
            {
                Console.WriteLine("There's no device connected");
                finish();
                return;
            }
            androidFiles = deviceManager.getAndroidFiles();
            Console.WriteLine("Files in Android:");
            foreach (string file in androidFiles)
            {
                Console.WriteLine(file);
                filesInAndorid.Add(file, true);
            }

            Console.WriteLine("");

            WindowsManager windowsManager = new WindowsManager(pathToLocal);
            windowsFiles = windowsManager.getLocalFiles();
            Console.WriteLine("Files in windows:");
            bool exist;
            for (int i=0; i<windowsFiles.Count; i++)
            {
                string file = windowsFiles[i];
                filesInAndorid.TryGetValue(file, out exist);
                Console.WriteLine((exist ? "" : "*") + file);
                if (!exist)
                {
                    windowsManager.moveSong(i);
                }
            }
            finish();
        }

        static void finish()
        {
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

    }
}
