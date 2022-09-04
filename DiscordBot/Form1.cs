using System.Text;

namespace DiscordBot
{
    public partial class Form1 : Form
    {

        public void WriteText(string text)
        {
            this.ConsoleBox.AppendText(text);
        }

        public Form1()
        {
            InitializeComponent();

            this.ConsoleBox.Text = String.Empty;

            StringBuilder sb = new StringBuilder();
            using(StringWriter sw = new StringWriter(sb))
            {
                Console.SetOut(sw);

                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("test: "+i);
                    this.ConsoleBox.AppendText(sw.ToString());
                }
            }
        }

        //TODO redirect output from console

        //TODO  Drag window to allow moving it


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}