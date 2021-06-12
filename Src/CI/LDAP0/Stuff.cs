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
      _styleSheet.AddStyle("[D,d]isabled", Color.Red);
      _styleSheet.AddStyle("(?i)CORPORATE", Color.LightBlue);
      _styleSheet.AddStyle("[P-p]igida", Color.LightBlue);
      _styleSheet.AddStyle("[A,a]lex", Color.LightBlue);
    }

    public void allUsersModern(string searchStr) // slow but ...
    {
      var need = true;
      foreach (var item in _styleSheet.Styles)
      {
        if (item.Target.Value == $"(?i){searchStr}")
          need = false;
      }
      if (need)
        _styleSheet.AddStyle($"(?i){searchStr}", Color.Orange); // styleSheet.AddStyle("rain[a-z]*", Color.MediumSlateBlue, match => match.ToUpper());

      Console.WriteLineStyled($"... search: {searchStr}:", _styleSheet);
      try
      {
        var AD = new PrincipalContext(ContextType.Domain, "corporate.ciglobe.net");
        var u = new UserPrincipal(AD);
        using var search = new PrincipalSearcher(u);
        var sw = Stopwatch.StartNew();

        var allcount = search.FindAll().Count();
        Console.WriteLine($"\n** {sw.ElapsedMilliseconds,6:N0} ms  to find allcount: {allcount}\n\n", Color.DarkGreen);

        Console.WriteLine($"Name                SamAcntName       UserPrincipalName          Enabld BadAtmp  Description                  DistinguishedName     ", Color.DarkGray);

        sw = Stopwatch.StartNew();

        foreach (UserPrincipal up in search.FindAll().Where(r => r != null && (
          (r.DistinguishedName != null && r.DistinguishedName.Contains(searchStr, StringComparison.InvariantCultureIgnoreCase)) ||
          (r.Description != null && r.Description.Contains(searchStr, StringComparison.InvariantCultureIgnoreCase)))))
        {
          Console.WriteLineStyled($"{up.Name,-20}{up.SamAccountName,-18}{up.UserPrincipalName,-26} {up.Enabled,-5} {up.LastBadPasswordAttempt,9:yy-MM-dd} {up.Description,-26}   {up.DistinguishedName}  ", _styleSheet);
        }

        Console.WriteLine($"\n** {sw.ElapsedMilliseconds,6:N0} ms \n\n", Color.DarkGreen);

        search.Dispose();
      }
      catch (Exception e) { Console.WriteLine("Error: " + e.Message); }
      Console.ResetColor();
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
