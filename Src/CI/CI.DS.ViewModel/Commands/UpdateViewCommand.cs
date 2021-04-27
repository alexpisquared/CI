using System;
using System.Windows.Input;

namespace CI.DS.ViewModel.Commands
{
  public class UpdateViewCommand : ICommand
  {
    readonly MainVM _mainVM;

    public UpdateViewCommand(MainVM mainVM) => _mainVM = mainVM;

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;
    public void Execute(object? parameter)
    {
      switch (parameter)
      {
        case "Demo": _mainVM.SelectedVM = new DemoVM(_mainVM.Logger, _mainVM.Config); break;
        case "Acbg": _mainVM.SelectedVM = new AllCashByGroupVM(_mainVM.Logger, _mainVM.Config); break;
        default: throw new MissingFieldException(parameter?.ToString()); 
      }
    }
  }
}
