using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace RdpFacility
{
  public partial class MainWindow : Window
  {
#if DEBUG
    const int _periodSec = 1, _till = 20;
    readonly int _dx = 50, _dy = 50;
#else
    const int _periodSec = 60, _till = 20;
    int _dx = 1, _dy = 1;
#endif
    readonly MediaElement _mediaplayer = new MediaElement();
    readonly Insomniac _dr = new Insomniac();
    readonly DispatcherTimer _timer = new DispatcherTimer();
    readonly SpeechSynthesizer _synth = new SpeechSynthesizer();
    readonly IReadOnlyList<VoiceInformation> _av = SpeechSynthesizer.AllVoices;
    DateTime _since;
    Point _prevPosition;
    readonly int _voice = 0;

    public MainWindow()
    {
      InitializeComponent();
      _timer.Tick += onTick;
      _timer.Interval = TimeSpan.FromSeconds(_periodSec);
      _timer.Start();
    }

    async void onLoaded(object s, RoutedEventArgs e)
    {
      await Task.Delay(999);
      checkBox.IsChecked = true; // setDR(true);
    }
    protected override void OnClosed(EventArgs e)
    {
      base.OnClosed(e);
      setDR(false);
    }

    void onTick(object s, object e)
    {
      if (DateTime.Now.Hour >= _till && checkBox.IsChecked == true)
        setDR(false);

      //Debug.WriteLine($"** XY: {_prevPosition}");
    }
    void onStrt(object s, RoutedEventArgs e) => setDR(true);
    void onStop(object s, RoutedEventArgs e) => setDR(false);
    void onMove(object s, RoutedEventArgs e) { }
    void onExit(object s, RoutedEventArgs e) { Close(); }


    void setDR(bool isOn)
    {      
      if (isOn)
      {
        _dr.RequestActive();
        tbkLog.Text = $"{DateTime.Now:HH:mm:ss}\r\n";
      }
      else
      {
        _dr.RequestRelease();
      }

      tbkBig.Text = isOn ? $"On {(_since = DateTime.Now):HH:mm} ÷ {_till}:00" : $"Was On for {(DateTime.Now - _since):hh\\:mm}";
      Title = isOn ? $"{(_since = DateTime.Now):HH:mm}···" : $"Off";
    }
    async Task readText(string mytext)
    {
      var ssml = $"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='en-US'>{mytext}</speak>";
      //var stream = await _synth.SynthesizeSsmlToStreamAsync(ssml);
      //_mediaplayer.SetSource(stream, stream.ContentType);
      _mediaplayer.Play();
    }
  }
}
