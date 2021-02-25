using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace RMSClient.Views
{
  public partial class ProcessOrderPopup : Window
  {
    public ProcessOrderPopup()
    {
      InitializeComponent();

      MouseWheel += (s, e) => { if (!(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))) return; ZVa += (e.Delta * .001); e.Handled = true; Debug.WriteLine(Title = $">>ZVa:{ZVa}"); }; //tu:
      MouseLeftButtonDown += (s, e) => DragMove();
      DataContext = this;
      c1.Focus();
      c1.IsDropDownOpen = true;
    }

    public static readonly DependencyProperty ZVaProperty = DependencyProperty.Register("ZVa", typeof(double), typeof(ProcessOrderPopup), new PropertyMetadata(1.25)); public double ZVa { get => (double)GetValue(ZVaProperty); set => SetValue(ZVaProperty, value); }

    public static readonly DependencyProperty OrderIdProperty = DependencyProperty.Register("OrderId", typeof(int), typeof(ProcessOrderPopup)); public int OrderId { get => (int)GetValue(OrderIdProperty); set => SetValue(OrderIdProperty, value); }
    public static readonly DependencyProperty OrderStatusProperty = DependencyProperty.Register("OrderStatus", typeof(string), typeof(ProcessOrderPopup)); public string OrderStatus { get => (string)GetValue(OrderStatusProperty); set => SetValue(OrderStatusProperty, value); }
    public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register("Symbol", typeof(string), typeof(ProcessOrderPopup)); public string Symbol { get => (string)GetValue(SymbolProperty); set => SetValue(SymbolProperty, value); }
    public static readonly DependencyProperty AdpaccountCodeProperty = DependencyProperty.Register("AdpaccountCode", typeof(string), typeof(ProcessOrderPopup)); public string AdpaccountCode { get => (string)GetValue(AdpaccountCodeProperty); set => SetValue(AdpaccountCodeProperty, value); }
    public static readonly DependencyProperty QuantityProperty = DependencyProperty.Register("Quantity", typeof(int?), typeof(ProcessOrderPopup)); public int? Quantity { get => (int?)GetValue(QuantityProperty); set => SetValue(QuantityProperty, value); }
    public static readonly DependencyProperty AvgPxProperty = DependencyProperty.Register("AvgPx", typeof(double?), typeof(ProcessOrderPopup)); public double? AvgPx { get => (double?)GetValue(AvgPxProperty); set => SetValue(AvgPxProperty, value); }
    public static readonly DependencyProperty NoteProperty = DependencyProperty.Register("Note", typeof(string), typeof(ProcessOrderPopup)); public string Note { get => (string)GetValue(NoteProperty); set => SetValue(NoteProperty, value); }
    public static readonly DependencyProperty NewOrderStatusProperty = DependencyProperty.Register("NewOrderStatus", typeof(OrderStatusEnum), typeof(ProcessOrderPopup)); public OrderStatusEnum NewOrderStatus { get => (OrderStatusEnum)GetValue(NewOrderStatusProperty); set => SetValue(NewOrderStatusProperty, value); }
    public static readonly DependencyProperty NewOrderActionProperty = DependencyProperty.Register("NewOrderAction", typeof(OrderActionEnum), typeof(ProcessOrderPopup)); public OrderActionEnum NewOrderAction { get => (OrderActionEnum)GetValue(NewOrderActionProperty); set => SetValue(NewOrderActionProperty, value); }

    void Button_Click1(object s, RoutedEventArgs e) { DialogResult = true; NewOrderAction = OrderActionEnum.SendUpdate; NewOrderStatus = getIt; Close(); }
    void Button_Click2(object s, RoutedEventArgs e) { DialogResult = true; NewOrderAction = OrderActionEnum.Acknowledge; NewOrderStatus = getIt; Close(); }
    void Button_Click3(object s, RoutedEventArgs e) { DialogResult = true; NewOrderAction = OrderActionEnum.UnlockOrder; NewOrderStatus = getIt; Close(); }
    void Button_Click4(object s, RoutedEventArgs e) { DialogResult = false; NewOrderAction = OrderActionEnum.Cancel; NewOrderStatus = getIt; Close(); }

    OrderStatusEnum getIt => (OrderStatusEnum)c1.SelectedIndex + 1;
  }
}
