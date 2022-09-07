using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AllInOneMwtPOC
{
  public class ViewModel : ObservableValidator
  {
    public ViewModel() => ValidateAllProperties();

    string _StringMayNotBeEmpty = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "This field {0} may not be empty.")] public string StringMayNotBeEmpty { get => _StringMayNotBeEmpty; set => SetProperty(ref _StringMayNotBeEmpty, value, true); }
  }
}