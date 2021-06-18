using Colorful;
using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Console = Colorful.Console;

namespace LDAP0
{
  public class Stuff
  {
    string iii = "OU=Active Users,OU=CI Users,DC=corporate,DC=ciglobe,DC=net";
    readonly StyleSheet _styleSheet = new StyleSheet(Color.DarkGray);
    public Stuff()
    {
      _styleSheet.AddStyle("Terminated", Color.LimeGreen);
      _styleSheet.AddStyle("False", Color.LimeGreen);
      _styleSheet.AddStyle("[D,d]isabled", Color.Red);
      _styleSheet.AddStyle("(?i)CORPORATE", Color.LightBlue);
      _styleSheet.AddStyle("[P-p]igida", Color.Lime);
      _styleSheet.AddStyle("[A,a]lex", Color.LightBlue);
      _styleSheet.AddStyle("Accounts", Color.Lime);
      _styleSheet.AddStyle(iii, Color.Blue);
    }

    public void ModernLdapFinder1st100(string searchStr, bool isEnabledOnly = true)
    {
      safeStyleAdd(searchStr);

      if (!OperatingSystem.IsWindows()) return;

      Console.WriteLineStyled($"\n... Searching: {searchStr}:", _styleSheet);

      try
      {
        using var ctx = new PrincipalContext(ContextType.Domain, "corporate.ciglobe.net");
        using var upf = new UserPrincipal(ctx);
        using var ps = new PrincipalSearcher(upf);

        //GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, IdentityType.DistinguishedName, @"CN=Burgess\, Marcia,OU=Active Users,OU=CI Users,DC=corporate,DC=ciglobe,DC=net");
        //((Principal)u).DistinguishedName = $"*{searchStr}*";

        upf.Enabled = isEnabledOnly;
        if (!string.IsNullOrEmpty(searchStr))
          upf.DisplayName = $"*{searchStr}*";
        ps.QueryFilter = upf;

        var sw1 = Stopwatch.StartNew();
        var lst = ps.FindAll().Take(10000).Where(r => r is UserPrincipal up
          && up.EmailAddress != null
          //&& r.DistinguishedName.Contains("Active Users")
          //&& !up.UserPrincipalName.Equals(up.EmailAddress, StringComparison.InvariantCultureIgnoreCase)
          && !up.DistinguishedName.Contains(iii)
        )
          .OrderBy(r => r.DistinguishedName.Split(new string[] { "OU=", "DC=" }, StringSplitOptions.RemoveEmptyEntries).Skip(1).First()) //.OrderByDescending(r => ((UserPrincipal)r).LastLogon).ThenBy(r => r.Name)
        ;
        sw1.Stop();
        Console.WriteLine($"**{lst.Count(),5:N0} / {sw1.ElapsedMilliseconds,6:N0} ms  ==>  {lst.Count() / sw1.Elapsed.TotalSeconds,6:N0} r/s    LDAP - FindAll()", Color.Cyan);

        //Console.WriteLine($"Name                  VoiceTelephoneNumber      UserPrincipalName              LastLog  Description                         Context.Name           DistinguishedName     ", Color.DarkGray);

        var sw2 = Stopwatch.StartNew();
        int i = 0;
        foreach (UserPrincipal up in lst)
        {
          if (i++ > 11540) break;
          Console.WriteLineStyled($"{i,4} {up.Name,-50}" +
            //$"{up.VoiceTelephoneNumber,-26}" +
            //$"{up.SamAccountName,14}==" +            $"{up.UserPrincipalName,-44}" +
            //$"{up.EmailAddress,-40}" +
            //$"{up.LastLogon:yy-MM-dd} " +
            //$"{up.Description,-50}" +
            //$"{up.Context.Name}  " +
            $"{dn(up)}  ", _styleSheet);

          //if (up.Name != up.DisplayName) Console.WriteLine($"{up.DisplayName}", Color.Lime);
          //if (!up.UserPrincipalName.Equals(up.EmailAddress, StringComparison.InvariantCultureIgnoreCase)) Console.WriteLine($"                                {up.UserPrincipalName}        :upn  ^^ eml", Color.Yellow);
          //if (!up.UserPrincipalName.StartsWith(up.SamAccountName, StringComparison.InvariantCultureIgnoreCase)) Console.WriteLine($"                                                {up.SamAccountName} ", Color.Lime);
        }

        Console.WriteLine($"**{lst.Count(),5:N0} / {sw1.ElapsedMilliseconds,6:N0} ms  ==>  {lst.Count() / sw1.Elapsed.TotalSeconds,6:N0} r/s    LDAP - FindAll()", Color.Cyan);
        Console.WriteLine($"**{lst.Count(),5:N0} / {sw2.ElapsedMilliseconds,6:N0} ms  ==>  {lst.Count() / sw2.Elapsed.TotalSeconds,6:N0} r/s ", Color.Cyan);

        ps.Dispose();
      }
      catch (Exception e) { Console.WriteLine("Error: " + e.Message); }
      Console.ResetColor();
    }

    static string dn(UserPrincipal up)
    {
      var l = up.DistinguishedName.Split(new string[] { "OU=", "DC=" }, StringSplitOptions.RemoveEmptyEntries).Skip(1);
      StringBuilder sb = new();
      foreach (var item in l)
      {
        sb.Append($"{item,-32}");
      }
      return sb.ToString();
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
