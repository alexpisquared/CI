using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CI.PermissionManager
{
  public partial class MainWindow : Window
  {
    ICollectionView _cv;

    readonly InventoryContext _context = new();
    CollectionViewSource categoryViewSource;

    public MainWindow()
    {
      InitializeComponent();


      categoryViewSource = (CollectionViewSource)FindResource(nameof(categoryViewSource));


      _cv = CollectionViewSource.GetDefaultView(_context.Permissions.Local.ToObservableCollection());
      CV.Filter = filterPerms;
      //demo only - wrecks sorting -      CV.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Permission.Name)));
      CV.SortDescriptions.Add(new SortDescription(nameof(Permission.Name), ListSortDirection.Ascending));

      DataContext = this;

      Loaded += onLoaded;
    }
    public static readonly DependencyProperty PermissionsFilterProperty = DependencyProperty.Register("PermissionsFilter", typeof(string), typeof(MainWindow), new PropertyMetadata("", propChngdCallback)); public string PermissionsFilter { get { return (string)GetValue(PermissionsFilterProperty); } set { SetValue(PermissionsFilterProperty, value); } }
    static void propChngdCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((MainWindow)d).RefreshView();

    public void RefreshView() => CV.Refresh();

    public ICollectionView CV { get => _cv; }

    bool filterPerms(object obj)
    {
      if (obj is Permission permission)
      {
        return permission.Name.Contains(PermissionsFilter, StringComparison.InvariantCultureIgnoreCase);
      }


      return false;
    }

    void onLoaded(object sender, RoutedEventArgs e)
    {
      _context.Permissions.Load();      //await _context.Permissions.LoadAsync();
      Title = $"{_context.Permissions.Local.Count}";

      //_context.Database.EnsureCreated();
      categoryViewSource.Source = _context.Permissions.Local.ToObservableCollection();
    }
    void onSave(object s, RoutedEventArgs e)
    {
      _context.SaveChanges();

      categoryDataGrid.Items.Refresh();      // this forces the grid to refresh to latest values
      productsDataGrid.Items.Refresh();
    }
    void onExit(object s, RoutedEventArgs e) => Close();

    protected override void OnClosing(CancelEventArgs e)
    {
      _context.Dispose();
      base.OnClosing(e);
    }
  }
}
