using System;
using System.Data;
using System.Net;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace YP3
{
    public partial class OrderForm : Form
    {
        private DatabaseManager databaseManager = new DatabaseManager();
        private DataTable appointmentsTable;
        public OrderForm()
        {
            InitializeComponent();
            LoadServices();
            LoadClients();
            LoadOrders();
        }
        private void LoadOrders()
        {
            try
            {
                // Открываем соединение с базой данных
                databaseManager.OpenConnection();

                // Запрос для выборки всех данных из таблицы Orders
                string query = "SELECT * FROM Orders";

                // Создаем адаптер данных
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, databaseManager.GetConnection());

                // Создаем объект DataTable для хранения результатов запроса
                DataTable dataTable = new DataTable();

                // Заполняем DataTable данными из базы данных
                adapter.Fill(dataTable);

                // Назначаем DataTable источником данных для DataGridView
                dataGridViewOrders.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных о заказах: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Закрываем соединение с базой данных
                databaseManager.CloseConnection();
            }
        }
        private void LoadClients()
        {
            try
            {
                // Подключение к базе данных
                databaseManager.OpenConnection();

                // Запрос для загрузки списка клиентов из базы данных
                string query = "SELECT client_id, client_full_name FROM Client";

                // Выполнение запроса и получение данных в DataTable
                DataTable dataTable = databaseManager.ExecuteQuery(query);

                // Заполнение ComboBox с клиентами
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int clientId = Convert.ToInt32(row["client_id"]);
                        string clientName = row["client_full_name"].ToString();
                        cmbClient.Items.Add(new Client(clientId, clientName));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки клиентов: " + ex.Message);
            }
            finally
            {
                // Закрытие соединения с базой данных
                databaseManager.CloseConnection();
            }
        }
        private void RefreshAppointmentsData()
        {
            // Очищаем DataGridView
            dataGridViewOrders.DataSource = null;

            // Заново загружаем данные о заказах
            LoadOrders();
        }
        private void LoadServices()
        {
            try
            {
                // Подключение к базе данных
                databaseManager.OpenConnection();

                // Запрос для загрузки списка услуг из базы данных, включая стоимость
                string query = "SELECT service_id, service_name, service_cost FROM Services";

                // Выполнение запроса и получение данных в DataTable
                DataTable dataTable = databaseManager.ExecuteQuery(query);

                // Заполнение ComboBox с услугами
                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        int serviceId = Convert.ToInt32(row["service_id"]);
                        string serviceName = row["service_name"].ToString();
                        decimal serviceCost = Convert.ToDecimal(row["service_cost"]); // Получаем стоимость из базы данных
                        cmbService.Items.Add(new Service(serviceId, serviceName, serviceCost));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки услуг: " + ex.Message);
            }
            finally
            {
                // Закрытие соединения с базой данных
                databaseManager.CloseConnection();
            }
        }

        public class Service
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Cost { get; set; }

            public Service(int id, string name, decimal cost)
            {
                Id = id;
                Name = name;
                Cost = cost;
            }

            public override string ToString()
            {
                return Name;
            }
        }
        public class Client
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public Client(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }


        private void cmbService_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTotalCost();
            try
            {
                // Получаем выбранную услугу из выпадающего списка
                Service selectedService = (Service)cmbService.SelectedItem;
                if (selectedService == null)
                {
                    return;
                }

                // Обновляем значение в txtTotalCost с учетом стоимости выбранной услуги
                decimal serviceCost = selectedService.Cost;
                decimal discount = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
                decimal totalCost = serviceCost * (1 - discount / 100);
                txtTotalCost.Text = totalCost.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении стоимости: " + ex.Message);
            }
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            UpdateTotalCost();
        }

        private void UpdateTotalCost()
        {
            try
            {
                Service selectedService = (Service)cmbService.SelectedItem;
                if (selectedService == null)
                {
                    return;
                }

                decimal serviceCost = selectedService.Cost;
                decimal discount = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
                decimal totalCost = serviceCost * (1 - discount / 100);

                txtTotalCost.Text = totalCost.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления общей стоимости: " + ex.Message);
            }
        }

        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbClient.SelectedItem == null)
                {
                    MessageBox.Show("Выберите клиента!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Проверяем, выбрана ли услуга
                if (cmbService.SelectedItem == null)
                {
                    MessageBox.Show("Выберите услугу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // Получаем значения из формы
                string clientName = cmbClient.Text;
                int clientId = GetClientIdByName(clientName);

                Service selectedService = (Service)cmbService.SelectedItem;
                decimal discount = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
                decimal serviceCost = selectedService.Cost;
                decimal totalCost = serviceCost * (1 - discount / 100); // Вычисляем общую стоимость заказа
                decimal ordersCost = totalCost; // Стоимость заказа для записи в таблицу Orders

                // Создаем команду для вставки данных в таблицу Orders
                string query = "INSERT INTO Orders (orders_date, client_id, service_id, employee_id, discount_percentage, processing_status, orders_cost) " +
                               "VALUES (@orderDate, @clientId, @serviceId, @employeeId, @discount, @status, @ordersCost)";

                using (MySqlCommand command = new MySqlCommand(query, databaseManager.GetConnection()))
                {
                    // Добавляем параметры к команде
                    command.Parameters.AddWithValue("@orderDate", DateTime.Now.Date);
                    command.Parameters.AddWithValue("@clientId", clientId);
                    command.Parameters.AddWithValue("@serviceId", selectedService.Id);
                    command.Parameters.AddWithValue("@employeeId", GetEmployeeId());
                    command.Parameters.AddWithValue("@discount", discount);
                    command.Parameters.AddWithValue("@status", "Новый");
                    command.Parameters.AddWithValue("@ordersCost", ordersCost); // Добавляем новый параметр для стоимости заказа

                    // Открываем соединение и выполняем запрос
                    databaseManager.OpenConnection();
                    command.ExecuteNonQuery();

                    // Получаем ID только что созданного заказа
                    query = "SELECT LAST_INSERT_ID()";
                    command.CommandText = query;
                    int orderId = Convert.ToInt32(command.ExecuteScalar());

                    // Обновляем значение в txtCost с учетом стоимости заказа
                    txtOrderId.Text = orderId.ToString();
                    txtCost.Text = ordersCost.ToString("0.00");
                }
                RefreshAppointmentsData();
                MessageBox.Show("Заказ успешно создан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка создания заказа: " + ex.Message);
            }
            finally
            {
                // В любом случае закрываем соединение
                databaseManager.CloseConnection();
            }
        }







        private int GetClientIdByName(string clientName)
        {
            try
            {
                databaseManager.OpenConnection();

                string query = "SELECT client_id FROM Client WHERE client_full_name = @clientName";

                MySqlParameter parameter = new MySqlParameter("@clientName", clientName);

                object result = databaseManager.ExecuteScalar(query, parameter);

                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("Клиент с таким именем не найден в базе данных.");
                }
            }
            finally
            {
                databaseManager.CloseConnection();
            }
        }


        private int GetEmployeeId()
        {
            return 1; // В данном примере возвращается ID первого сотрудника
        }

        private void cmbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Получаем выбранного клиента из выпадающего списка
                Client selectedClient = (Client)cmbClient.SelectedItem;
                if (selectedClient == null)
                {
                    return;
                }

                // Обновляем значение в txtCustomerFullName
                cmbClient.Text = selectedClient.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении имени клиента: " + ex.Message);
            }
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            RefreshAppointmentsData();
            // Запрет на редактирование столбца id
            dataGridViewOrders.Columns["orders_id"].ReadOnly = true;
            txtTotalCost.ReadOnly = true;
            txtOrderId.ReadOnly = true;
            cmbClient.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbService.DropDownStyle = ComboBoxStyle.DropDownList;
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            txtOrderId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            cmbClient.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            cmbService.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            txtTotalCost.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            txtDiscount.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            txtCost.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            btnCreateOrder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
            button_exit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private void button_exit_Click(object sender, EventArgs e)
        {
            LoginForm form = new LoginForm();
            form.Show();
            this.Hide();
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // Если введенный символ не является цифрой и не является клавишей Backspace,
                // то отменяем ввод
                e.Handled = true;
            }
            if (!string.IsNullOrEmpty(txtDiscount.Text))
            {
                // Получаем значение из TextBox
                int value;
                if (int.TryParse(txtDiscount.Text + e.KeyChar, out value))
                {
                    // Если значение больше 100, то отменяем ввод
                    if (value > 100 || (txtDiscount.Text.Length == 2 && txtDiscount.Text[0] == '1'))
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void dataGridViewOrders_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }


    }
}

