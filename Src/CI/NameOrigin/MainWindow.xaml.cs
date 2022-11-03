using System.Windows;
using GenderApiLib;
using NameOrigin.Code1stModelGen;
namespace NameOrigin;
public partial class MainWindow : Window
{
  public MainWindow() => InitializeComponent();

  void OnCreateDB(object sender, RoutedEventArgs e) => DbCreator.Create();
  void OnReadJson(object sender, RoutedEventArgs e) => JsonReadr.ReadArray();
  async void OnGenderApi(object sender, RoutedEventArgs e)
  {
    var key = new ConfigurationBuilder().AddUserSecrets<App>().Build()["AppSecrets:MagicNumber"]; //tu: adhoc usersecrets from config

    var rv = await GenderApi.CallOpenAI(new ConfigurationBuilder().AddUserSecrets<App>().Build(), "Mina");

  }

  void OnClose(object sender, RoutedEventArgs e) => Close();
}
