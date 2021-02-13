using System;
using System.IO;
using System.Media;
using System.Windows;

namespace EventLogger
{
  public partial class EventLoggerMainWindow : Window
  {
    public EventLoggerMainWindow() { InitializeComponent(); SystemSounds.Asterisk.Play(); Loaded += onLoaded; }
    async void onLoaded(object s, RoutedEventArgs e) { tbkBig.Text = $"{App.Started:yyyy-MM-dd HH:mm:ss}  {string.Join(' ', Environment.GetCommandLineArgs())} \n"; await File.AppendAllTextAsync(App.TextLog, $"\n{tbkBig.Text}"); SystemSounds.Exclamation.Play(); }
    async void onMark(object z, RoutedEventArgs e) { var s = $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss}  Mark \n"; tbkBig.Text += s; await File.AppendAllTextAsync(App.TextLog, s); }
    async void onExit(object s, RoutedEventArgs e) { await File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss}  Closd manually\n"); Close(); }
    protected override async void OnClosed(EventArgs e) /**/   { await File.AppendAllTextAsync(App.TextLog, $"{DateTimeOffset.Now:yyyy-MM-dd HH:mm:ss} {(DateTimeOffset.Now - App.Started):hh\\:mm\\:ss}  OnClosed  () \n"); base.OnClosed(e); SystemSounds.Hand.Play(); }
  }
}
