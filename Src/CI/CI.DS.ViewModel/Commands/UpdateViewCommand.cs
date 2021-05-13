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
      string key when (key == "Spsl") => new StoredProcListVM(_mainVM.Logger, _mainVM.Config, _mainVM),
      "Spsl" => new StoredProcListVM(_mainVM.Logger, _mainVM.Config, _mainVM),
      "Dbps" => new DbProcessSelectorVM(_mainVM.Logger, _mainVM.Config),
      "Demo" => new DemoVM(_mainVM.Logger, _mainVM.Config),
      "Acbg" => new AllCashByGroupVM(_mainVM.Logger, _mainVM.Config, _mainVM),
      "usp_Report_AllCashByGroup" => new AllCashByGroupVM(_mainVM.Logger, _mainVM.Config, _mainVM),
#if DEBUG
      _ => new AllCashByGroupVM(_mainVM.Logger, _mainVM.Config, _mainVM),
#else
      _ => throw new MissingFieldException(parameter?.ToString()),
#endif
    };
  }
}
