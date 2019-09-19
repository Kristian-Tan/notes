Installing Framework7:

download from
https://github.com/framework7io/framework7
http://framework7.io

## Installing Cordova (mobile app development framework / html5 wrapper)
- Install Node.js (from nodejs.org)
- Set up PATH variable to enable command line call to npm
```[CONSOLE] npm install -g cordova```
- Install http-serve to enable local development server (like laravel's artisan serve)
```[CONSOLE] npm install -g http-serve```
- Try serving current directory:
```[CONSOLE] http-serve```
- Add npm and nodejs to PATH
```
PATH=C:\Program Files\nodejs\
PATH=C:\Program Files\nodejs\node_modules\npm\bin
PATH=C:\Users\Kristian\AppData\Roaming\npm
```

## Creating new cordova project
```[CONSOLE] cordova create DirectoryNameHere com.example.package.name.here AppNameHere```
- Fill www directory with the project's html files

## Setting up android sdk (to export as .apk)
- Set cordova to add android as one of the target platform
```[CONSOLE] cordova platform add android```
- Download android sdk (no need to do this if Android Studio already installed)
- Set up android sdk PATH
```
ANDROID_HOME=C:\Users\Kristian\AppData\Local\Android\Sdk
ANDROID_HOME=C:\Users\Kristian\AppData\Local\Android\Sdk\platform-tools
JAVA_HOME=C:\Program Files\java\jdk1.8.0_151
PATH=C:\Program Files (x86)\Common Files\Oracle\Java\javapath
PATH=C:\Users\Kristian\AppData\Local\Android\Sdk\platform-tools
```

### Install gradle
- Download from https://gradle.org/install/
- Select binary-only download, extract at any directory and add to PATH
- Verify installation
```[CONSOLE] gradle -v```

### Build APK:
```[CONSOLE] cordova build android```
- Wait for gradling / building apk, might be long (about 5-30 minute)
- Get the finished apk at: ```platforms\android\app\build\outputs\apk\debug\app-debug.apk```

#### Quick paste to build and install to adb
- cordova build android
```[CONSOLE] adb install platforms\android\app\build\outputs\apk\debug\app-debug.apk```

- install to specific adb device and auto replace old app
```[CONSOLE] adb -s dd2b4cbb install -r platforms\android\app\build\outputs\apk\debug\app-debug.apk```
