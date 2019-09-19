//more information: http://csharp.net-informations.com/statements/enum.htm

//setting

	enum Tempurature
	{
		Low,
		Medium,
		High,
	};



//checking

    Temperature value = Temperature.Medium;
	if (value == Tempurature.Medium)
	{
	    Console.WriteLine("Temperature is Medium..");
	}
	

	
//simple example

using System;
using System.Windows.Forms;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        enum Tempurature
        {
            Low,
            Medium,
            High,
        };
        private void button1_Click(object sender, EventArgs e)
        {
            Temperature value = Tempurature.Medium;
            if (value == Tempurature.Medium)
            {
                MessageBox.Show ("Temperature is Mediuam..");
            }
        }
    }
}