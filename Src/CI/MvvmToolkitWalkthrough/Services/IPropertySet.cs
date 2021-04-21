namespace MvvmToolkitWalkthrough.Services
{
  internal interface IPropertySet 
  {
    bool ContainsKey(string key);
    void Add<T>(string key, T? value);
    bool TryGetValue(string key, out object value);
  }
}