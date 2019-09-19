Tools:
- dex2jar: https://github.com/pxb1988/dex2jar
- JD-GUI (java decompiler): http://jd.benow.ca/

Requirements:
- Install Java + add to path
- Add dex2jar to path (or cd to it's directory)

Steps:
- Rename .apk to .zip
- Extract "classes.dex"
- Run command "d2j-dex2jar classes.dex"
- Open JD-GUI.exe, open file "classes-dex2jar.jar"

To protect against reverse engineering:
- Obfuscation (change name of all variables) => open build.gradle (module:app), change android.buildTypes.release.minifyEnabled=true