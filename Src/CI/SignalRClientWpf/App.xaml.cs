using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace SignalRClientWpf
{
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      HubConnection connection = new HubConnectionBuilder()
        .WithUrl($"{Dns.GetHostName()}:11000")
        .Build();

      base.OnStartup(e);
    }
  }
}
