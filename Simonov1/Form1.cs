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
        public string log_in = "";//инициализируем логин пустой строкой
        public int role = 0;//инициализируем роль нулевым значением 

        SqlConnection con = new SqlConnection("Server=db.edu.cchgeu.ru;DataBase=193_Starkov;User=193_Starkov;Password=Qq123123");//подключаемся к бд

        int tryCounter = 0;// счетчик на не верный ввод логина пароля

        Timer timer = new Timer();
        int sec = 30;
        public Form1()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';//значения введенное в текст бокс 2 будет скрито знаком звездочка 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")//если поля текст бокс 1 и 2 не пусто
                tryLogin();// то вызываем метод проверки существования записи в бд
        }
        //private void timer_tick(object s, EventArgs e) // метод блокировки формы на 30 сек
        //{
        //    sec += -1;
        //    button1.Text = $"Вход ({sec} сек.)";
        //    if (sec == 0)
        //    {
        //        sec = 30;
        //        timer.Stop();
        //        button1.Enabled = true;
                
        //        button1.Text = "Вход";
        //    }
        //}

       

        public bool isAccHave(string login)
        {
            int id = -1;
            con.Open();// открываем соединение с бд
            SqlCommand cmd = new SqlCommand($"select id from users where login_user = '{login}'", con); // получаем айди из бд по логину
            using (var reader = cmd.ExecuteReader()) // считываем полученное значение из бд
            {
                if (reader.Read())
                    id = Convert.ToInt32(reader.GetInt64(0));
            }
            con.Close();// закрываем соединение с бд обязательно
            if (id != -1)
                return true;
            else
                return false;
        }

        private void newTry()
        {
            tryCounter++; // считаем сколько раз сработало условие не верного ввода учетной записи в методе tryLogin
            if (tryCounter == 3) // если 3 раза условие сработало 
            {
                tryCounter = 0; // обнуляем значение счетчика 
                startTimer();
                Form2 ca = new Form2(this);// открываем форму капчи 
                ca.ShowDialog();
            }
        }

        private void tryLogin()
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            //записываем значения из текст боксов 1 и 2 в переменные 
            if (isAccHave(login)) //вызываем метод который вернет нам логин 

            {
                string passFromTable = "";
                int role_id = 0;
                con.Open(); // опять открываем соедение с бд
                SqlCommand cmd = new SqlCommand($"select pass, id_roles from dbo.users where login_user = '{login}'", con);// получем из бд по логину пароль и ид роли
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        passFromTable = reader.GetString(0); // записали в переменную пароль
                        role_id = Convert.ToInt32(reader.GetInt64(1));//записали в переменную ид права
                    }
                }
                con.Close();//закрыли соединнеие 

                if (passFromTable == password)// проверяем если получаем тот же пароль из бд что и ввели в текст бокс 
                {
                    if (role_id == 1) // проверяем ид прав, если 1 то админ и открывает форму с одними правами
                    {
                        log_in = login;
                        role = role_id;
                        openWindow(0);
                    }
                    else // иначе, если, не админ то другие права
                    {
                        log_in = login;
                        role = role_id;
                        openWindow(1);
                    }
                }
                else
                {
                    MessageBox.Show("Неправильно набран пароль", "Ошибка входа");// если не верно введен пароль
                    newTry();
                }
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует", "Ошибка входа");// если не верно и логин и пароль
                newTry();
            }
            
        }

        public void openWindow(int t)
        {
            if (t == 1)
            {
                MessageBox.Show("1 prava");
            //    Sotr st = new Sotr(this);
            //    st.Show();
            }
            else
            {
                MessageBox.Show("2 prava");
                //  //  Client cl = new Client(this);
                //  //  cl.Show();
            }
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
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
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Start();
            button1.Text = "Вход (30 сек.)";
            button1.Enabled = false;

        }
    }
}
