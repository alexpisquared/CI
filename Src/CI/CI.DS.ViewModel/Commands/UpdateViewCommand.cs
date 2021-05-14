using CI.DS.ViewModel.VMs;
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
      StoredProcDetail spd => new DynamicSPUICreatorVM(_mainVM.Logger, _mainVM.Config, _mainVM, spd),
      "Spsl" => new StoredProcListVM(_mainVM.Logger, _mainVM.Config, _mainVM),
      "Dbps" => new DbPrcsSelectorVM(_mainVM.Logger, _mainVM.Config),
      "Demo" => new ValidationDemoVM(_mainVM.Logger, _mainVM.Config),
      "Acbg" => new AllCashByGroupVM(_mainVM.Logger, _mainVM.Config, _mainVM),
#if DEBUG
      _ => new AllCashByGroupVM(_mainVM.Logger, _mainVM.Config, _mainVM),
#else
      _ => throw new MissingFieldException(parameter?.ToString()),
#endif
    };
  }
}
