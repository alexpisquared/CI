using Microsoft.EntityFrameworkCore;
using RMSClient.Model;
using System;
using System.Linq;
using System.Windows;

namespace RMSClient
{
    public partial class MainWindow : Window
    {
        readonly RMSContext _db = new RMSContext();
        public MainWindow()
        {
            InitializeComponent();//DataContext = this;

            dt1.SelectedDate = DateTimeOffset.Now.Date.AddDays(-5);
            dt2.SelectedDate = DateTimeOffset.Now.Date;

#if DEBUG
            if (Environment.MachineName == "RAZER1") { Top = 1700; Left = 1100; }
            else { Top = 1700; Left = 2500; }
#endif
        }

        async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await _db.Requests.LoadAsync();

                DataContext = _db.Requests.Local.ToList(); //dg1.ItemsSource = _db.Users.Local.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void Button_Click(object sender, RoutedEventArgs e) => Close();

        void Button_Click_1(object sender, RoutedEventArgs e) { }
        void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
