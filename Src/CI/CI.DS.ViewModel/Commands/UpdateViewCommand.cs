using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        case "Demo": _mainVM.SelectedVM = new DemoVM(); break;
        case "Acbg": _mainVM.SelectedVM = new AllCashByGroupVM(); break;
        default:
          break;
      }
    }
  }
}
