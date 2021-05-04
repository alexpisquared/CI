﻿using CI.GUI.Support.WpfLibrary.Extensions;
using CI.GUI.Support.WpfLibrary.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace CI.GUI.Support.WpfLibrary.Base
{
  public partial class WindowBase : Window
  {
    const double _defaultZoomV = 1.25;
    const string _defaultTheme = "No Theme";
    const int _swShowNormal = 1, _swShowMinimized = 2, _margin = 0;
    static int _currentTop = 0, _currentLeft = 0;

    protected bool IgnoreEscape { get; set; }
    protected bool IgnoreWindowPlacement { get; set; } = false;
    string _isoFilenameONLY => $"{GetType().Name}.xml";

#if MockingCore3
    class Logger
    {
      internal void LogError(Exception ex, string v) => Trace.WriteLine($"{ex}  {v}");
    }

    readonly Logger _logger = new Logger();
    public WindowBase()
    {
#else
    readonly ILogger<WindowBase> _logger;
    public WindowBase() : this(new LoggerFactory().CreateLogger<WindowBase>()) { }
    public WindowBase(ILogger<WindowBase> logger)
    {
      _logger = logger;
#endif

      //Loaded += (s, e) => { applyTheme(Thm); };
      //useDoubleClick += (s, e) => WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal; <= too obnoxious (Jan2020)

      MouseLeftButtonDown += (s, e) => { if (e.LeftButton == MouseButtonState.Pressed) DragMove(); }; //tu: workaround for  "Can only call DragMove when primary mouse button is down." (2021-03-10: pre-opened dropdown seemingly caused the error)

      MouseWheel += (s, e) => { if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) return; ZV += (e.Delta * .001); e.Handled = true; Debug.WriteLine(Title = $">>ZV:{ZV}"); }; //tu:

      KeyUp += (s, e) =>
      {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
          switch (e.Key)
          {
            default: break;
            case Key.OemMinus:  /**/ ZV /= 1.1; break;
            case Key.OemPlus:   /**/ ZV *= 1.1; break;
            case Key.D0:        /**/ ZV = 1; break;
          }
        else
          switch (e.Key)
          {
            default: break;
            case Key.Escape:
              if (!IgnoreEscape) Close();
              base.OnKeyUp(e); break;
          }
      };
    }
    public static readonly DependencyProperty ZVProperty = DependencyProperty.Register("ZV", typeof(double), typeof(WindowBase), new PropertyMetadata(_defaultZoomV)); public double ZV { get => (double)GetValue(ZVProperty); set => SetValue(ZVProperty, value); }
    public static readonly DependencyProperty ThmProperty = DependencyProperty.Register("Thm", typeof(string), typeof(WindowBase), new PropertyMetadata(_defaultTheme)); public string Thm { get => (string)GetValue(ThmProperty); set => SetValue(ThmProperty, value); }

    protected void ApplyTheme(string themeName)
    {
      const string pref = "/CI.GUI.Support.WpfLibrary;component/ColorScheme/Theme.Color.";

      try
      {
        if (_defaultTheme.Equals(themeName, StringComparison.Ordinal) || Thm.Equals(themeName, StringComparison.Ordinal))
        {
          return;
        }

        //~Trace.Write($"    ~> ThemeApplier()   '{themeName}'  to  '{_isoFilenameONLY}' ... Dicts --/++:\r\n");
        //~Application.Current.Resources.MergedDictionaries.ToList().ForEach(r => Trace.WriteLine($"    ~> -- Removing: {((System.Windows.Markup.IUriContext)r)?.BaseUri?.AbsolutePath.Replace(pref, "..."/*, StringComparison.OrdinalIgnoreCase*/)}"));

        var suri = $"{pref}{themeName}.xaml";
        if (Application.LoadComponent(new Uri(suri, UriKind.RelativeOrAbsolute)) is ResourceDictionary dict)
        {
          ResourceDictionary? rd;
          while ((rd = Application.Current.Resources.MergedDictionaries.FirstOrDefault(r => ((System.Windows.Markup.IUriContext)r)?.BaseUri?.AbsolutePath?.Contains(pref
#if MockingCore3
#else
            , StringComparison.OrdinalIgnoreCase
#endif
            ) == true)) != null)
            Application.Current.Resources.MergedDictionaries.Remove(rd);

          Application.Current.Resources.MergedDictionaries.Add(dict);
        }
        Thm = themeName;

        //~Application.Current.Resources.MergedDictionaries.ToList().ForEach(r => Trace.WriteLine($"    ~> ++ Adding:   {((System.Windows.Markup.IUriContext)r)?.BaseUri?.AbsolutePath.Replace(pref, "..."/*, StringComparison.OrdinalIgnoreCase*/)}"));
        //~Trace.Write($"    ~> ThemeApplier()   '{themeName}'  to  '{_isoFilenameONLY}' is done. \r\n");
      }
      catch (Exception ex) { _logger.LogError(ex, $""); ex.Pop(this); throw; }
    }

    protected void onWindowMinimize(object s, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    protected void onExit(object s, RoutedEventArgs e) => Close();

    protected override void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);

      if (IgnoreWindowPlacement) return;

      NativeMethods.WindowPlacement winPlcmnt;
      try
      {
        try
        {
          var wpc = XmlIsoFileSerializer.Load<NativeMethods.WPContainer>(_isoFilenameONLY);
          ZV = wpc.Zb == 0 ? 1 : wpc.Zb;
          winPlcmnt = wpc.WindowPlacement;

          ApplyTheme(string.IsNullOrEmpty(wpc.Thm) ? _defaultTheme : wpc.Thm); // -- for Mail.sln - causes the error: Cannot find resource named 'WindowStyle_Aav0'. Resource names are case sensitive 
        }
        catch (InvalidOperationException ex)
        {
          ex.Log();
          ZV = 1d;
          winPlcmnt = XmlIsoFileSerializer.Load<NativeMethods.WindowPlacement>(_isoFilenameONLY);
        }
        catch (Exception ex) { ex.Log(); throw; }

        winPlcmnt.length = Marshal.SizeOf(typeof(NativeMethods.WindowPlacement));
        winPlcmnt.flags = 0;
        winPlcmnt.showCmd = (winPlcmnt.showCmd == _swShowMinimized ? _swShowNormal : winPlcmnt.showCmd);

        if (winPlcmnt.normalPosition.Bottom == 0 && winPlcmnt.normalPosition.Top == 0 && winPlcmnt.normalPosition.Left == 0 && winPlcmnt.normalPosition.Right == 0)
        {
          Trace.WriteLine($"{_isoFilenameONLY,20}: 1st time: Window Positions - all zeros!   {SystemParameters.WorkArea.Width}x{SystemParameters.WorkArea.Height} is this the screen dims?");

          winPlcmnt.normalPosition.Left = _currentLeft + _margin;
          winPlcmnt.normalPosition.Top = _currentTop + _margin;

          winPlcmnt.normalPosition.Right = winPlcmnt.normalPosition.Left + (int)ActualWidth;
          if (winPlcmnt.normalPosition.Right > SystemParameters.WorkArea.Width)
          {
            winPlcmnt.normalPosition.Left = _margin;
            winPlcmnt.normalPosition.Top += (_margin + (int)ActualHeight);
            _currentTop += (int)ActualHeight;
            winPlcmnt.normalPosition.Right = winPlcmnt.normalPosition.Left + (int)ActualWidth;
            _currentLeft = _margin;
          }

          winPlcmnt.normalPosition.Bottom = winPlcmnt.normalPosition.Top + (int)ActualHeight;
          if (winPlcmnt.normalPosition.Bottom > SystemParameters.WorkArea.Height)
          {
            _currentLeft =
            winPlcmnt.normalPosition.Top =
              winPlcmnt.normalPosition.Left = _margin;
          }

          _currentLeft += (int)ActualWidth;
        }
        //else
        //  Trace.WriteLine($"{_isoFilenameONLY,20}: Window Positions NOT all zeros: btm:{winPlcmnt.normalPosition.Bottom,-4} top:{winPlcmnt.normalPosition.Top,-4} lft:{winPlcmnt.normalPosition.Left,-4} rht:{winPlcmnt.normalPosition.Right,-4}.  {SystemParameters.WorkArea.Width}x{SystemParameters.WorkArea.Height} is this the screen dims?");

        NativeMethods.SetWindowPlacement_(new WindowInteropHelper(this).Handle, ref winPlcmnt); //Note: if window was closed on a monitor that is now disconnected from the computer, SetWindowPlacement will place the window onto a visible monitor.
      }
      catch (InvalidOperationException ex) { ex.Log(); }
      catch (Exception ex) { ex.Log(); throw; }
    }
    protected override void OnClosing(CancelEventArgs e) // WARNING - Not fired when Application.SessionEnding is fired
    {
      base.OnClosing(e);

      NativeMethods.GetWindowPlacement_(new WindowInteropHelper(this).Handle, out var wp);
      XmlIsoFileSerializer.Save(new NativeMethods.WPContainer { WindowPlacement = wp, Zb = ZV, Thm = Thm }, _isoFilenameONLY);
    }
  }
}