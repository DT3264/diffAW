# DIFFAW
Copy/Move the files that doesn't exist in a Windows folder but exist in an Android device folder.

# Where to download
[Here](https://github.com/DT3264/diffAW/releases/tag/1.0)

# Example
```
diffAW -a "/storage/49ED-1907/musica uwu" -w "C:\Users\Dani\Music\musica uwu" -t "\music deleted" -ra y -ra y -m y -v 2
```
The first time it would create an adb folder, that's because the app comunicates with the device through adb

| Parameter | Use |
| ------ | ------ |
|-t diffFolder	|	the folder where the files that are in the android folder but no in the windows one. Default: "diff"|
|-ra {y(es)\|n(o)} |	search recursively in the android folders. Default: Yes|
|-rw {y(es)\|n(o)}	|search recursively in the windows folders. Default: Yes
|-m {(y(es)\|n(o)}	|move files from the windows folder to the diff folder if doesn't exist, otherwise just copy. Default: No
-v (1\|2\|3)	|verbose modes: 1.Print found and diff files, 2.Print diff files, 3.Print only progress. Default: 3

# Thanks to
* [AndroidCtrl](https://forum.xda-developers.com/showthread.php?t=2772502) - .NET functions to communicate with an Android device
