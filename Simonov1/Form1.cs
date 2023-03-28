using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simonov1
{
    public partial class Form1 : Form
    {
        public string log_in = "";
        public int role = 0;

        SqlConnection con = new SqlConnection(@"Data Source = pc-2584d8\sysadmin; Initial Catalog = Deteiling; Integrated Security = true");

        int tryCounter = 0;

        Timer timer = new Timer();
        int sec = 30;
        public Form1()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
                tryLogin();
        }
        private void timer_tick(object s, EventArgs e)
        {
            sec += -1;
            button1.Text = $"Вход ({sec} сек.)";
            if (sec == 0)
            {
                sec = 30;
                timer.Stop();
                button1.Enabled = true;
                
                button1.Text = "Вход";
            }
        }

        public void startTimer()
        {
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
            button1.Text = "Вход (30 сек.)";
            button1.Enabled = false;
          
        }

        public bool isAccHave(string login)
        {
            int id = -1;
            con.Open();
            SqlCommand cmd = new SqlCommand($"select id from users where login = '{login}'", con);
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                    id = Convert.ToInt32(reader.GetInt64(0));
            }
            con.Close();
            if (id != -1)
                return true;
            else
                return false;
        }

        private void newTry()
        {
            tryCounter++;
            if (tryCounter == 3)
            {
                tryCounter = 0;
                Form2 ca = new Form2(this);
                ca.ShowDialog();
            }
        }

        private void tryLogin()
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            if (isAccHave(login))
            {
                string passFromTable = "";
                int role_id = 0;
                con.Open();
                SqlCommand cmd = new SqlCommand($"select pass, id_roles from dbo.users where login_user = '{login}'", con);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        passFromTable = reader.GetString(0);
                        role_id = Convert.ToInt32(reader.GetInt64(1));
                    }
                }
                con.Close();

                if (passFromTable == password)
                {
                    if (role_id == 1)
                    {
                        log_in = login;
                        role = role_id;
                        openWindow(0);
                    }
                    else
                    {
                        log_in = login;
                        role = role_id;
                        openWindow(1);
                    }
                }
                else
                {
                    MessageBox.Show("Неправильно набран пароль", "Ошибка входа");
                    newTry();
                }
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует", "Ошибка входа");
                newTry();
            }
        }

        public void openWindow(int t)
        {
            //if (t == 1)
            //{
            //    Sotr st = new Sotr(this);
            //    st.Show();
            //}
            //else
            //{
            //  //  Client cl = new Client(this);
            //  //  cl.Show();
            //}
            //this.Hide();
        }
    }
}
