namespace RMSClient.Models
{
  public class AppSettings
  {
    public string IpAddress { get; set; } = "10.10.19.152";
    public ushort Port { get; set; } = 6756;
    public string RmsDebug { get; set; } = "Server=.\\sqlexpress;Database=RMS;Trusted_Connection=True;";
    public string RmsRelease { get; set; } = "Server=MTdevSQLDB;Database=RMS;Trusted_Connection=True;";
    public string Inventory { get; set; } = "Server=MTdevSQLDB;Database=Inventory;Trusted_Connection=True;";
    public string BR { get; set; } = "Server=MTdevSQLDB;Database=BR;Trusted_Connection=True;";
    public bool ForceSocketReconnect { get; set; } = true;
    public int ForceSocketReconnectTime { get; set; } = 300000;
    public string KeyVaultURL { get; set; } = "<moved to a safer place>";
  }
}
