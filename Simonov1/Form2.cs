using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simonov1
{
    public partial class Form2 : Form
    {
        Form1 f1;
        string text = "";
        bool exit = false;
        public Form2(Form1 f)
        {
            f1 = f;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            genNewCaptcha();
        }
        private void genNewCaptcha()
        {
            text = "";
            Random rnd = new Random();

            int xpos = rnd.Next(0, pictureBox1.Width - 75);
            int ypos = rnd.Next(0, pictureBox1.Height - 25);

            Bitmap image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(image);

            g.Clear(Color.Gray);

            string chars = "0123456789ABCDEFGHJKLMNP";
            for (int i = 0; i < 5; i++)
                text += chars[rnd.Next(chars.Length)];

            g.DrawString(text,
                new Font("Arial", 18),
                Brushes.Black,
                new Point(xpos, ypos));

            for (int x = 0; x < pictureBox1.Width; x++)
                for (int y = 0; y < pictureBox1.Height; y++)
                    if (rnd.Next() % 20 == 0)
                        image.SetPixel(x, y, Color.White);

            pictureBox1.Image = image;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (text.ToLower() == textBox1.Text.ToLower())
            {
                exit = true;
                this.Close();
            }
            else
            {
                genNewCaptcha();
                textBox1.Text = "";
                MessageBox.Show("Неправильный набор", "Ошибка");
            }
        }
        private void Captcha_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (exit)
            {
                e.Cancel = false;
                f1.startTimer();
            }
            else
                e.Cancel = true;
        }
    }
}
