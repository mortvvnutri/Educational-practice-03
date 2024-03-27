using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YP3
{
    public partial class LoginForm : Form
    {
        private DatabaseManager databaseManager = new DatabaseManager();
        public LoginForm()
        {
            InitializeComponent();
            //pictureBoxEye.Image = Properties.Resources.eye1;
            txtPassword.UseSystemPasswordChar=true;
            pictureBoxEye.SizeMode = PictureBoxSizeMode.Zoom;

            // Привязываем PictureBox к границам формы
            pictureBoxEye.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lblUsername.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            txtUsername.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            lblPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            btnLogin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !txtPassword.UseSystemPasswordChar;
            if (databaseManager != null)
            {
                Console.WriteLine("Mouse entered DB.");
                string username = txtUsername.Text;
                string password = txtPassword.Text;

                if (CheckLogin(username, password))
                {
                    txtPassword.UseSystemPasswordChar = true;
                    MessageBox.Show("Вы успешно вошли в систему!");
                    OrderForm form = new OrderForm();
                    form.Show();
                    this.Hide(); // Скрыть форму входа
                }
                else
                {
                    txtPassword.UseSystemPasswordChar = true;
                    MessageBox.Show("Неверный логин или пароль. Попробуйте снова.");
                }
            }
        }
        private bool CheckLogin(string username, string password)
        {
            if (databaseManager != null)
            {
                return databaseManager.CheckLogin(username, password); // Проверяем логин и пароль через DatabaseManager
            }
            else
            {
                // Если databaseManager не инициализирован, вернем false
                return false;
            }
        }

        
        private void pictureBoxEye_MouseHover(object sender, EventArgs e)
        {
            pictureBoxEye.Image = Properties.Resources.eye2;
        }

        private void pictureBoxEye_MouseLeave(object sender, EventArgs e)
        {
            Console.WriteLine("Mouse leave pictureBoxEye.");
            txtPassword.UseSystemPasswordChar = true;
            pictureBoxEye.Image = Properties.Resources.eye1;

        }

        private void pictureBoxEye_MouseEnter(object sender, EventArgs e)
        {
            Console.WriteLine("Mouse entered pictureBoxEye."); // Добавляем отладочное сообщение
            pictureBoxEye.Image = Properties.Resources.eye2;
            txtPassword.UseSystemPasswordChar = false;
        }

        private void button_Reg_Click(object sender, EventArgs e)
        {
            ClientRegistrationForm form = new ClientRegistrationForm();
            form.Show();
            this.Hide();
        }
    }

}
