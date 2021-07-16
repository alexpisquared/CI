using DB.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace CI.PermissionManager
{
  public partial class MainWindow : Visual.Lib.Base.WindowBase
  {
    readonly ICollectionView _cv;
    readonly InventoryContext _context = new(@"Server=mtUATsqldb;Database=Inventory;Trusted_Connection=True;");
    readonly CollectionViewSource _permissionViewSource;

    public MainWindow()
    {
      InitializeComponent();

      _permissionViewSource = (CollectionViewSource)FindResource(nameof(_permissionViewSource));

      _cv = CollectionViewSource.GetDefaultView(_context.Permissions.Local.ToObservableCollection());
      CV.Filter = filterPerms;
      //demo only - wrecks sorting -      CV.GroupDescriptions.Add(new PropertyGroupDescription(nameof(Permission.Name)));
      CV.SortDescriptions.Add(new SortDescription(nameof(Permission.Name), ListSortDirection.Ascending));

      DataContext = this;

      Loaded += onLoaded;
    }
    public static readonly DependencyProperty PermissionsFilterProperty = DependencyProperty.Register("PermissionsFilter", typeof(string), typeof(MainWindow), new PropertyMetadata("", propChngdCallback)); public string PermissionsFilter { get => (string)GetValue(PermissionsFilterProperty); set => SetValue(PermissionsFilterProperty, value); }
    static void propChngdCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) => ((MainWindow)d).RefreshView();
    public void RefreshView() => CV.Refresh();
    public ICollectionView CV => _cv;
    bool filterPerms(object obj) => (obj is Permission permission) && permission.Name.Contains(PermissionsFilter, StringComparison.InvariantCultureIgnoreCase);

    void onLoaded(object s, RoutedEventArgs e)
    {
      _context.Applications.Load();           //await _context.Permissions.LoadAsync();
      _context.Permissions.Load();            //await _context.Permissions.LoadAsync();
      _context.PermissionAssignments.Load();  //await _context.Permissions.LoadAsync();
      Title = $"P:{_context.Permissions.Local.Count} pa:{_context.PermissionAssignments.Local.Count}";

      //_context.Database.EnsureCreated();
      _permissionViewSource.Source = _context.Permissions.Local.ToObservableCollection();
    }
    void onSave(object s, RoutedEventArgs e)
    {
      _context.SaveChanges();

      _permissionDataGrid.Items.Refresh();      // this forces the grid to refresh to latest values
      _applicationPermissionsDataGrid.Items.Refresh();
    }
    //void OnExit(object s, RoutedEventArgs e) => App.Current.Shutdown();

    protected override void OnClosing(CancelEventArgs e)
    {
      _context.Dispose();
      base.OnClosing(e);
    }
  }
}
