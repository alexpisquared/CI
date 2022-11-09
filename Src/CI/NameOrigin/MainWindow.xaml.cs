namespace NameOrigin;
public partial class MainWindow : Window
{
  public MainWindow() => InitializeComponent();

  void OnLoaded(object sender, RoutedEventArgs e) => tbNm.Focus();
  void OnClose(object sender, RoutedEventArgs e) => Close();
  void OnCreateDB(object sender, RoutedEventArgs e) => DbCreator.Create();
  void OnReadJson(object sender, RoutedEventArgs e) => JsonReadr.ReadArray();
  async void OnGenderApi(object sender, RoutedEventArgs e)
  {
    var (ts, er, root) = await GenderApi.CallOpenAI(new ConfigurationBuilder().AddUserSecrets<App>().Build(), tbNm.Text); //_ = new ConfigurationBuilder().AddUserSecrets<App>().Build()["AppSecrets:MagicNumber"]; //tu: adhoc usersecrets from config

    tbRv.Text = string.IsNullOrEmpty(er) ?
      $"{root?.name_sanitized}   {root?.country_of_origin.FirstOrDefault()?.country_name}   {root?.country_of_origin.FirstOrDefault()?.probability} %    {ts.TotalMilliseconds:N0} ms" :
      $"{er}     {ts.TotalMilliseconds:N0} ms";
  }
  async void OnRapidApi(object sender, RoutedEventArgs e)
  {
    var (ts, er, root) = await RapidApi.CallOpenAI(new ConfigurationBuilder().AddUserSecrets<App>().Build(), tbNm.Text); //_ = new ConfigurationBuilder().AddUserSecrets<App>().Build()["AppSecrets:MagicNumber"]; //tu: adhoc usersecrets from config

    tbRv.Text = string.IsNullOrEmpty(er) ?
      $"{root?.name_sanitized}   {root?.country_of_origin.FirstOrDefault()?.country_name}   {root?.country_of_origin.FirstOrDefault()?.probability} %    {ts.TotalMilliseconds:N0} ms" :
      $"{er}     {ts.TotalMilliseconds:N0} ms";
  }
}