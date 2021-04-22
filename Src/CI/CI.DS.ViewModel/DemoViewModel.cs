using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI.DS.ViewModel
{
public  class DemoViewModel : ObservableValidator
  {
    public DemoViewModel() => ValidateAllProperties();

    string _StringMayNotBeEmpty = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string StringMayNotBeEmpty { get => _StringMayNotBeEmpty; set => SetProperty(ref _StringMayNotBeEmpty, value, true); }
  }
}
