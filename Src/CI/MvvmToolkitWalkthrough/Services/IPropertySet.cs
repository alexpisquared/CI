using System.Collections.Generic;

namespace MvvmToolkitWalkthrough.Services
{
  internal interface IPropertySet 
  {
    bool ContainsKey(string key);
    void Add<T>(string key, T? value);
    bool TryGetValue(string key, out object value);
  }
  internal class PropertySet : Dictionary<string, object>, IPropertySet
  {
    public void Add<T>(string key, T? value) => throw new System.NotImplementedException();

    public object this[int i] => throw new System.NotImplementedException();
  }
}