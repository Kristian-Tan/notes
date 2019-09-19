using System.IO;

OpenFileDialog openFileDialog = new OpenFileDialog();
openFileDialog.Title = "Browse for file";
openFileDialog.InitialDirectory = "C:";
openFileDialog.Filter = ".jpg picture files (*.jpg)|*.jpg|.png picture files (*.png)|*.png"; //use this for example if you want to get jpg or png only
openFileDialog.FilterIndex = 0;
openFileDialog.RestoreDirectory = true;
if (openFileDialog.ShowDialog() == DialogResult.OK)
{
    string fileHasilBrowseWithPath = openFileDialog.FileName; //example: "C:\Pictures\sample.jpg"
    string fileHasilBrowseNoPath = openFileDialog.SafeFileName; //example: "sample.jpg"
    //File.Copy(fileHasilBrowseWithPath, Application.StartupPath, false);
}