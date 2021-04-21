using System.Collections.Generic;

namespace MvvmToolkitWalkthrough.Services
{
  internal class PropertySet : Dictionary<string, object>, IPropertySet
  {
    Dictionary<string, object> __ = new();

    public void Add<T>(string key, T? value) => __.Add(key, value);

    public object this[string key] => __.GetValueOrDefault(key);
  }
}