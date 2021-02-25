using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using zEfPoc.Model;

namespace zEfPoc
{
  public partial class MainWindow : Window
  {
    readonly OneBaseContext _db = new OneBaseContext("Server=.\\sqlexpress;Database=OneBase;Trusted_Connection=True;");
    public MainWindow()
    {
      InitializeComponent();//DataContext = this;
#if DEBUG
      if (Environment.MachineName == "RAZER1") { Top = 1700; Left = 1100; }
#endif
    }

    async void Window_Loaded(object sender, RoutedEventArgs e)
    {
      await _db.Users.LoadAsync();

      DataContext = _db.Users.Local.ToList(); //dg1.ItemsSource = _db.Users.Local.ToList();
    }

    void Button_Click(object sender, RoutedEventArgs e) => Close();

    void Button_Click_1(object sender, RoutedEventArgs e) { }
    void Button_Click_2(object sender, RoutedEventArgs e)
    {

    }
  }
}
