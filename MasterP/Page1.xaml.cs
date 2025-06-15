using Npgsql;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterP
{
    public partial class Page1 : Page
    {
        private string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=12345;Database=MasterPolG";

        public Page1()
        {
            InitializeComponent();
            LoadPartners();
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            txtDateTime.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm");
        }

        private string CalculateDiscount(int totalSales)
        {
            if (totalSales < 10000)
                return "0%";
            if (totalSales < 50000)
                return "5%";
            if (totalSales < 300000)
                return "10%";
            return "15%";
        }

        private void LoadPartners(string searchQuery = "")
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, name_partner, address, inn, director, phone, email, type_partner, rating, total_sales FROM partners";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    query += " WHERE LOWER(name_partner) LIKE LOWER(@search)";
                }

                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(searchQuery))
                    {
                        command.Parameters.AddWithValue("@search", "%" + searchQuery + "%");
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        PartnersStackPanel.Children.Clear();

                        while (reader.Read())
                        {
                            int totalSales = reader["total_sales"] != DBNull.Value ? Convert.ToInt32(reader["total_sales"]) : 0;

                            string discount = CalculateDiscount(totalSales);

                            Grid partnerGrid = new Grid
                            {
                                Margin = new Thickness(10),
                                Background = new SolidColorBrush(Colors.LightGoldenrodYellow)
                            };

                            partnerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                            partnerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                            var partnerInfo = new TextBlock
                            {
                                Text = $"ID: {reader["id"]}\n" +
                                       $"Название: {reader["name_partner"]}\n" +
                                       $"Адрес: {reader["address"]}\n" +
                                       $"ИНН: {reader["inn"]}\n" +
                                       $"Директор: {reader["director"]}\n" +
                                       $"Телефон: {reader["phone"]}\n" +
                                       $"Email: {reader["email"]}\n" +
                                       $"Тип: {reader["type_partner"]}\n" +
                                       $"Рейтинг: {reader["rating"]}\n" +
                                       $"Продажи: {totalSales}",
                                Margin = new Thickness(10),
                                Padding = new Thickness(10),
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = new SolidColorBrush(Colors.Black)
                            };
                            Grid.SetColumn(partnerInfo, 0);
                            partnerGrid.Children.Add(partnerInfo);

                            var discountText = new TextBlock
                            {
                                Text = discount,
                                FontSize = 18,
                                FontWeight = FontWeights.Bold,
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Right,
                                Padding = new Thickness(10),
                                Foreground = new SolidColorBrush(Colors.DarkGreen)
                            };
                            Grid.SetColumn(discountText, 1);
                            partnerGrid.Children.Add(discountText);

                            PartnersStackPanel.Children.Add(partnerGrid);
                        }
                    }
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadPartners(SearchBox.Text);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this)?.Close();
        }

        private void PartnerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page2());
            this.NavigationService.RemoveBackEntry();
        }
    }
}