using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterP
{
    public partial class Page3 : Page
    {

        private readonly string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=12345;Database=MasterPolG";
        public Page3()
        {
            InitializeComponent();
            UpdateDateTime();
            LoadHistoryData();
        }

        private void UpdateDateTime()
        {
            txtDateTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        }

        private void LoadHistoryData()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT p.name_products AS product_name, pp.quantity, pp.sale_date
                FROM partner_products pp
                JOIN products p ON pp.id = p.id";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    using (var adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        dataGridHistory.ItemsSource = dataTable.DefaultView;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPartners()
        {
            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT id, name_partner FROM partners";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbPartners.Items.Add(new ComboBoxItem
                            {
                                Content = reader["name_partner"].ToString(),
                                Tag = reader["id"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки партнёров: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this)?.Close();
        }
    }
}