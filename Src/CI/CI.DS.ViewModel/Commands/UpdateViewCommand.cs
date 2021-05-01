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
    public void Execute(object? parameter) => _mainVM.SelectedVM = parameter switch
    {
      "Demo" => new DemoVM(_mainVM.Logger, _mainVM.Config),
      "Acbg" => new AllCashByGroupVM(_mainVM.Logger, _mainVM.Config),
      _ => throw new MissingFieldException(parameter?.ToString()),
    };
  }
}
