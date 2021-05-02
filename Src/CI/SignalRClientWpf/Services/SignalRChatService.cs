using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace SignalRClientWpf.Services
{
  public class SignalRChatService
  {
    readonly HubConnection _connection;

    public event Action<ColorChatColor> ColorMessageRecieved;
    public SignalRChatService(HubConnection connection) => _connection = connection;
    public async Task Connect() => await _connection.StartAsync();
    public async Task SendColorMessage(ColorChatColor color) => await Task.Yield();       //_connection.SendAsync(nameof(SendColorMessage)) /// https://youtu.be/7fLnF7VBjas?t=705  ~~ SignalR cannot talk to raw sockets ...easily at least. So the project is on hold at this moment in the video.
  }
}