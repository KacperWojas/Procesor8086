namespace Procesor8086
{
    public partial class Form1 : Form
    {
        string input;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            input = button1.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}