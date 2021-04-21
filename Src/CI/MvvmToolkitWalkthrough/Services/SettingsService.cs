using MvvmSample.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmToolkitWalkthrough.Services
{
  public sealed class SettingsService : ISettingsService
  {
    /// <summary>
    /// The <see cref="IPropertySet"/> with the settings targeted by the current instance.
    /// </summary>
    readonly IPropertySet SettingsStorage = new PropertySet(); //  ApplicationData.Current.LocalSettings.Values;

    /// <inheritdoc/>
    public void SetValue<T>(string key, T value)
    {
      if (!SettingsStorage.ContainsKey(key)) SettingsStorage.Add(key, value);
      //todo: else SettingsStorage[key] = value;
    }

    /// <inheritdoc/>
    public T GetValue<T>(string key)
    {
      if (SettingsStorage.TryGetValue(key, out object value))
      {
        return (T)value;
      }

      return default;
    }
  }
}
