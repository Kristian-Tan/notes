using System.IO;

#FILE OPERATIONS
##create blank text file
	File.CreateText("C:\Document\myfile.txt").Close();
##copy file
	File.Copy("C:\Document\sourceFile.txt", "C:\Document\targetFile.txt", false); //FALSE HERE IS FOR OVERWRITE OPTION
##cut (move) file / or rename
	File.Move("C:\Document\sourceFile.txt", "C:\Document\targetFile.txt");
##delete file
	File.Delete("C:\fileToBeDeleted.txt");
##open a file as textfile and get the content
	string theContentOfMyFile = File.ReadAllText("C:\Document\myfile.txt");
##edit a file as textfile
	File.WriteAllText("C:\Document\myfile.txt", "here is the content of the file");
##check if a file exist
	bool fileTestExist = File.Exists("C:\test.txt");
##open a file with its default program
	System.Diagnostics.Process.Start("C:\document.pdf");

#DIRECTORY OPERATIONS
##create directory
	Directory.CreateDirectory("C:\MyDirectory\NewDirectory");
##copy directory
	string sourcePath = "C:\SourceFolder";
	string targetPath =  "C:\TargetFolder";
	string[] files = System.IO.Directory.GetFiles(sourcePath);
	// Copy the files and overwrite destination files if they already exist.
	foreach (string s in files)
	{
		// Use static Path methods to extract only the file name from the path.
		fileName = System.IO.Path.GetFileName(s);
		destFile = System.IO.Path.Combine(targetPath, fileName);
		System.IO.File.Copy(s, destFile, true);
	}
	//OR
	//ANOTHER SOLUTION
	foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", 
		SearchOption.AllDirectories))
		Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
	foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", 
		SearchOption.AllDirectories))
		File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
##cut directory
	//JUST DO COPY DIRECTORY AND THEN DELETE THE OLD DIRECTORY
##delete directory
	Directory.Delete("C:\MyDirectory\DeletedDirectory", true); //TRUE IS FOR RECURSIVE (DELETE ALL SUBFOLDERS AND ALL CONTENTS)
##check if a directory exist
	bool testDirExist = Directory.Exists("C:\MyDirectory\NewDirectory");

#BOTH OPERATIONS
##directory listing
    string[] folders = Directory.GetDirectories("C:\MyDirectory\WorkingDirectory", "*", SearchOption.AllDirectories);
    foreach (string directoryName in folders)
    {
        listBoxInfo.Items.Add(directoryName);
    }
##file listing
	//returns string array with files names (full paths)
	string[] filePaths = Directory.GetFiles(@"c:\MyDir\");
	//Get files from directory (with specified extension)
	string[] filePaths = Directory.GetFiles(@"c:\MyDir\", "*.bmp");
	//Get files from directory (including all subdirectories)
	string[] filePaths = Directory.GetFiles(@"c:\MyDir\", "*.bmp", SearchOption.AllDirectories);