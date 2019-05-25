# DIFFAW
Copy/Move the files that doesn't exist in a Windows folder but exist in an Android device folder.

# Where to download
[Here](https://github.com/DT3264/diffAW/releases/tag/1.0)

# Example
```
diffAW -a "/storage/49ED-1907/All Music" -w "C:\Users\Dani\Music\Phone Music"
diffAW -a androidPath -w windowsPath
```
The first time it would create an adb folder, that's because the app comunicates with the device through adb

# Thanks to
* [AndroidCtrl](https://forum.xda-developers.com/showthread.php?t=2772502) - .NET functions to communicate with an Android device
