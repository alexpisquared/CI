using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DS.ViewModel
{
  public class DemoVM : ObservableValidator
  {
    public DemoVM() => ValidateAllProperties();

    string _stringMayNotBeEmpty = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string StringMayNotBeEmpty { get => _stringMayNotBeEmpty; set => SetProperty(ref _stringMayNotBeEmpty, value, true); }
  }
}
