using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YP3
{
    public partial class ClientRegistrationForm : Form
    {
        private bool isValidTelephone = false;
        private DatabaseManager databaseManager = new DatabaseManager();
        public ClientRegistrationForm()
        {
            InitializeComponent();
        }
        private bool IsValid(string text)
        {
            if (!Regex.IsMatch(text, @"^\+7\(\d{3}\)\s\d{3}-\d{2}-\d{2}$"))
            {
                return false;
            }

            return true;
        }
        private void button_Regisration_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем значения из формы
                string clientFullName = txtClientFullName.Text;
                string clientPhone = masked_number.Text;

                // Проверяем, хочет ли пользователь продолжить регистрацию
                DialogResult result = MessageBox.Show("Вы уверены, что хотите зарегистрировать клиента?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Создаем команду для вставки данных в таблицу Client
                    string query = "INSERT INTO Client (client_full_name, client_phone) " +
                                    "VALUES (@clientFullName, @clientPhone)";

                    using (MySqlCommand command = new MySqlCommand(query, databaseManager.GetConnection()))
                    {
                        // Добавляем параметры к команде
                        command.Parameters.AddWithValue("@clientFullName", clientFullName);
                        command.Parameters.AddWithValue("@clientPhone", clientPhone);

                        // Открываем соединение и выполняем запрос
                        databaseManager.OpenConnection();
                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Клиент успешно зарегистрирован!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка регистрации клиента: " + ex.Message);
            }
            finally
            {
                LoginForm form = new LoginForm();
                form.Show();
                this.Hide();
                // В любом случае закрываем соединение
                databaseManager.CloseConnection();
            }
        }


        private void ClientRegistrationForm_MouseMove(object sender, MouseEventArgs e)
        {

            if (IsValid(masked_number.Text))
            {
                masked_number.BackColor = Color.LightGreen;
                isValidTelephone = true;
            }
            else
            {
                masked_number.BackColor = Color.Salmon;
                isValidTelephone = false;
            }


            button_Registration.Enabled = false;
            if (isValidTelephone)
            {
                button_Registration.Enabled = true;
            }
        }

        private void ClientRegistrationForm_Load(object sender, EventArgs e)
        {
            txtClientFullName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            masked_number.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            button_Registration.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            button_exit.Anchor = AnchorStyles.Top| AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            LoginForm form = new LoginForm();
            form.Show();
            this.Hide();
        }
    }
}

