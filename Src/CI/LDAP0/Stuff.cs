using Colorful;
using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using Console = Colorful.Console;

namespace LDAP0
{
  public class Stuff
  {
    readonly StyleSheet _styleSheet = new StyleSheet(Color.Gray);
    public Stuff()
    {
      _styleSheet.AddStyle("Terminated", Color.LimeGreen);
      _styleSheet.AddStyle("False", Color.LimeGreen);
      _styleSheet.AddStyle("[D,d]isabled", Color.Red);
      _styleSheet.AddStyle("(?i)CORPORATE", Color.LightBlue);
      _styleSheet.AddStyle("[P-p]igida", Color.LightBlue);
      _styleSheet.AddStyle("[A,a]lex", Color.LightBlue);
      _styleSheet.AddStyle("Service Accounts", Color.White);
      _styleSheet.AddStyle("OU=Active Users,OU=CI Users,DC=corporate,DC=ciglobe,DC=net", Color.Blue);
    }

    public void ModernLdapFinder1st100(string searchStr, bool isEnabledOnly = true)
    {
      safeStyleAdd(searchStr);

      Console.WriteLineStyled($"... search: {searchStr}:", _styleSheet);
      try
      {
        using var ctx = new PrincipalContext(ContextType.Domain, "corporate.ciglobe.net");
        using var u = new UserPrincipal(ctx);
        using var ps = new PrincipalSearcher(u);

        //GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, IdentityType.DistinguishedName, @"CN=Burgess\, Marcia,OU=Active Users,OU=CI Users,DC=corporate,DC=ciglobe,DC=net");
        //((Principal)u).DistinguishedName = $"*{searchStr}*";

        u.Enabled = isEnabledOnly;
        if (!string.IsNullOrEmpty(searchStr))
          u.DisplayName = $"*{searchStr}*";
        ps.QueryFilter = u;

        var sw = Stopwatch.StartNew();
        var lst = ps.FindAll().Take(100);

        Console.WriteLine($"Name                  VoiceTelephoneNumber      UserPrincipalName              LastLog  Description                         Context.Name           DistinguishedName     ", Color.DarkGray);

        sw = Stopwatch.StartNew();
        foreach (UserPrincipal up in lst)
        {
          Console.WriteLineStyled($"{up.Name,-22}{up.VoiceTelephoneNumber,-26}" +
            $"{up.UserPrincipalName,-34}" +
            $"{up.EmailAddress,-31}" +
            $"{up.LastLogon:yy-MM-dd} {up.Description,-36}{up.Context.Name}  {up.DistinguishedName}  ", _styleSheet);

          if (up.Name != up.DisplayName) Console.WriteLine($"{up.DisplayName}", Color.Lime);

          if (!up.UserPrincipalName.Equals(up.EmailAddress, StringComparison.InvariantCultureIgnoreCase)) Console.WriteLine($"                                                                                  {up.UserPrincipalName}", Color.Yellow);

          if (!up.UserPrincipalName.StartsWith(up.SamAccountName, StringComparison.InvariantCultureIgnoreCase)) Console.WriteLine($"                                                {up.SamAccountName} ", Color.Lime);
        }

        Console.WriteLine($"\n**   {lst.Count():N0} / {sw.ElapsedMilliseconds,6:N0} ms  ==>  {lst.Count() / sw.Elapsed.TotalSeconds,6:N2} r/s \n\n", Color.DarkCyan);
        Console.WriteLine($"\n**   {lst.Count():N0} / {sw.ElapsedMilliseconds,6:N0} ms  ==>  {lst.Count() / sw.Elapsed.TotalSeconds,6:N2} r/s \n\n", Color.DarkCyan);

        ps.Dispose();
      }
      catch (Exception e) { Console.WriteLine("Error: " + e.Message); }
      Console.ResetColor();
    }

    private void safeStyleAdd(string searchStr)
    {
      var need = true;
      foreach (var item in _styleSheet.Styles)
      {
        if (item.Target.Value == $"(?i){searchStr}")
          need = false;
      }
      if (need)
        _styleSheet.AddStyle($"(?i){searchStr}", Color.Orange); // styleSheet.AddStyle("rain[a-z]*", Color.MediumSlateBlue, match => match.ToUpper());
    }

    public void allUsersSimple(string searchStr, string property = "mail") // fast but nobody is findable
    {
      _styleSheet.AddStyle($"(?i){searchStr}", Color.Orange); // styleSheet.AddStyle("rain[a-z]*", Color.MediumSlateBlue, match => match.ToUpper());
      _styleSheet.AddStyle(property, Color.Lime);

      Console.WriteLineStyled($"property: {property} ... search: {searchStr}:", _styleSheet);

      try
      {
        DirectoryEntry myLdapConnection = createDirectoryEntry();

        var search = new DirectorySearcher(myLdapConnection);
        search.PropertiesToLoad.Add("cn");
        search.PropertiesToLoad.Add(property);

        SearchResultCollection allUsers = search.FindAll();

        var sw = Stopwatch.StartNew();
        var i = 0;
        foreach (SearchResult result in allUsers)
        {
          if (result.Properties["cn"].Count > 0 && result.Properties[property].Count > 0 && result.Properties[property][0].ToString().Contains(searchStr, StringComparison.InvariantCultureIgnoreCase))
          {
            Console.WriteLineStyled($"{++i,4}  {result.Properties["cn"][0],-26} {result.Properties[property][0],-28} {result.Properties["adspath"][0]}", _styleSheet);
          }
        }
        Console.WriteLine($"** {sw.ElapsedMilliseconds,6:N0} ms \n\n", Color.Cyan);
      }
      catch (Exception e)
      {
        Console.WriteLine("Exception caught:\n\n" + e.ToString());
      }
    }

    public void list0_Yellow(string username = "Pigida, Alex") // https://www.ianatkinson.net/computing/adcsharp/retrieve_all_info.cs
    {
      _styleSheet.AddStyle(username.Split(new char[] { ',', ' ' }).First(), Color.Orange); // styleSheet.AddStyle("rain[a-z]*", Color.MediumSlateBlue, match => match.ToUpper());
      _styleSheet.AddStyle(username.Split(new char[] { ',', ' ' }).Last(), Color.Lime); // styleSheet.AddStyle("rain[a-z]*", Color.MediumSlateBlue, match => match.ToUpper());

      DirectoryEntry myLdapConnection = createDirectoryEntry();

      try
      {
        var search = new DirectorySearcher(myLdapConnection) { Filter = "(cn=" + username + ")" }; // create search object which operates on LDAP connection object  and set search object to only find the user specified  
        SearchResult result = search.FindOne(); // create results objects from search object  

        if (result != null) // user exists, cycle through LDAP fields (cn, telephonenumber etc.)  
        {
          var array = new string[result.Properties.PropertyNames.Count];
          result.Properties.PropertyNames.CopyTo(array, 0);
          foreach (var ldapField in array.OrderBy(r => r)) // result.Properties.PropertyNames)
          {
            foreach (var myCollection in result.Properties[ldapField]) // cycle through objects in each field e.g. group membership  (for many result.Properties there will only be one object such as name)  
            {
              Console.WriteLineStyled($" {ldapField,-26}  {myCollection}", _styleSheet);
            }
          }
        }
        else
        {
          Console.WriteLine("\n\n");
          Console.WriteLine("User not found!", Color.Red);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine("\n\n");
        Console.WriteLine("Exception caught:\n\n" + e.ToString());
      }
      Console.ResetColor();
    }

    DirectoryEntry createDirectoryEntry() => new DirectoryEntry("LDAP://corporate.ciglobe.net") { AuthenticationType = AuthenticationTypes.Secure };      //string ctx22 = ldapConnection.Properties["defaultNamingContext"].Value as string;      //      ldapConnection.Path = "LDAP://OU=staff,DC=ca";// create and return new LDAP connection with desired settings  

    public void allAttributes() // LDAP - Retrieve a list of all attributes/values?
    {
      Console.WriteLine($"\n\n {WindowsIdentity.GetCurrent().Name}  {Environment.UserName}", Color.Blue);
      var currentUserSid = WindowsIdentity.GetCurrent().User.Value;

      var ctx = new PrincipalContext(ContextType.Domain, "corporate.ciglobe.net");
      var up = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, currentUserSid);

      var entry = up.GetUnderlyingObject() as DirectoryEntry;
      PropertyCollection props = entry.Properties;
      var array = new string[props.PropertyNames.Count];
      props.PropertyNames.CopyTo(array, 0);
      foreach (var propName in array.OrderBy(r => r))
      {
        Console.WriteLineStyled($" {propName,-26}  {entry.Properties[propName].Value}", _styleSheet);
      }
    }

  }
}
