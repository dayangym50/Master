using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Npgsql;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterP
{
    public partial class Page2 : Page
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=12345;Database=MasterPolG";

        public Page2()
        {
            InitializeComponent();
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            txtDateTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Page1());
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtInn.Text) || string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtAddress.Text) || string.IsNullOrEmpty(txtPhone.Text) ||
                string.IsNullOrEmpty(txtRating.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtRating.Text, out int rating) || rating < 1 || rating > 10)
            {
                MessageBox.Show("Рейтинг должен быть числом от 1 до 5.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Partners (type_partner, name_partner, director, email, phone, address, inn, rating) " +
                                   "VALUES (@type_partner, @name_partner, @director, @email, @phone, @address, @inn, @rating)";

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("type_partner", "Тип партнера");
                        cmd.Parameters.AddWithValue("name_partner", txtName.Text);
                        cmd.Parameters.AddWithValue("director", txtInn.Text);
                        cmd.Parameters.AddWithValue("email", txtEmail.Text);
                        cmd.Parameters.AddWithValue("phone", txtPhone.Text);
                        cmd.Parameters.AddWithValue("address", txtAddress.Text);
                        cmd.Parameters.AddWithValue("inn", txtInn.Text);
                        cmd.Parameters.AddWithValue("rating", rating);

                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Партнер успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при добавлении данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}