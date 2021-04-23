using CI.DS.ViewModel.Commands;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace CI.DS.ViewModel
{
  public class MainVM : ObservableValidator
  {
    ObservableValidator _selectedVM = new DemoVM();

    public MainVM() => UpdateViewCommand = new UpdateViewCommand(this);

    public ObservableValidator SelectedVM { get => _selectedVM; set => SetProperty(ref _selectedVM, value); }

    public ICommand UpdateViewCommand { get; set; }
  }
}
