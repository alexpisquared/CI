using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;

allUsers();
//allAttributes();
//list0();
Console.ResetColor();


static void allUsers()
{
  try
  {
    PrincipalContext AD = new PrincipalContext(ContextType.Domain, "corporate.ciglobe.net");
    UserPrincipal u = new UserPrincipal(AD);
    PrincipalSearcher search = new PrincipalSearcher(u);

    foreach (UserPrincipal result in search.FindAll().Where(r => r!= null && r.DisplayName != null && r.DisplayName.Contains("Pig", StringComparison.InvariantCultureIgnoreCase)))
      //if (result.VoiceTelephoneNumber != null)
        Console.WriteLine($"{result.DisplayName,-32}{result.UserPrincipalName,-26} {result.DisplayName,-26} " );

    search.Dispose();
  }

  catch (Exception e)
  {
    Console.WriteLine("Error: " + e.Message);
  }
}
static void allUsersSimple()
{
  Console.Write("Enter property: ");
  String property = "mail";//  Console.ReadLine();

  try
  {
    DirectoryEntry myLdapConnection = createDirectoryEntry();

    DirectorySearcher search = new DirectorySearcher(myLdapConnection);
    search.PropertiesToLoad.Add("cn");
    search.PropertiesToLoad.Add(property);

    SearchResultCollection allUsers = search.FindAll();

    foreach (SearchResult result in allUsers)
    {
      if (result.Properties["cn"].Count > 0 && result.Properties[property].Count > 0)
      {
        Console.WriteLine(String.Format("{0,-20} : {1}",
                          result.Properties["cn"][0].ToString(),
                          result.Properties[property][0].ToString()));
      }
    }
  }
  catch (Exception e)
  {
    Console.WriteLine("Exception caught:\n\n" + e.ToString());
  }
}


void list0() // https://www.ianatkinson.net/computing/adcsharp/retrieve_all_info.cs
{
string username = "Pigida, Alex"; // Console.ReadLine();
  Console.ForegroundColor = ConsoleColor.Green;

  DirectoryEntry myLdapConnection = createDirectoryEntry();

  try
  {
    DirectorySearcher search = new DirectorySearcher(myLdapConnection) { Filter = "(cn=" + username + ")" }; // create search object which operates on LDAP connection object  and set search object to only find the user specified  
    SearchResult result = search.FindOne(); // create results objects from search object  

    if (result != null) // user exists, cycle through LDAP fields (cn, telephonenumber etc.)  
    {
      var array = new string[result.Properties.PropertyNames.Count];
      result.Properties.PropertyNames.CopyTo(array, 0);
      foreach (string ldapField in array.OrderBy(r => r)) // result.Properties.PropertyNames)
      {
        foreach (object myCollection in result.Properties[ldapField]) // cycle through objects in each field e.g. group membership  (for many result.Properties there will only be one object such as name)  
        {
          Console.WriteLine(string.Format(" {0,-26}  {1}", ldapField, myCollection.ToString()));
        }
      }
    }
    else
    {
      Console.ForegroundColor = ConsoleColor.DarkRed;
      Console.WriteLine("User not found!");
    }
  }
  catch (Exception e)
  {
    Console.BackgroundColor = ConsoleColor.Yellow;
    Console.ForegroundColor = ConsoleColor.DarkRed;
    Console.WriteLine("Exception caught:\n\n" + e.ToString());
    Console.ResetColor();
  }

}

static DirectoryEntry createDirectoryEntry() => new DirectoryEntry("LDAP://corporate.ciglobe.net") { AuthenticationType = AuthenticationTypes.Secure };      //string ctx22 = ldapConnection.Properties["defaultNamingContext"].Value as string;      //      ldapConnection.Path = "LDAP://OU=staff,DC=ca";// create and return new LDAP connection with desired settings  

static void allAttributes() // LDAP - Retrieve a list of all attributes/values?
{
  Console.ForegroundColor = ConsoleColor.Cyan;
  string currentUserSid = WindowsIdentity.GetCurrent().User.Value;

  PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "corporate.ciglobe.net");
  UserPrincipal up = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, currentUserSid);

  DirectoryEntry entry = up.GetUnderlyingObject() as DirectoryEntry;
  PropertyCollection props = entry.Properties;
  var array = new string[props.PropertyNames.Count];
  props.PropertyNames.CopyTo(array, 0);
  foreach (string propName in array.OrderBy(r => r))
    Console.WriteLine($" {propName,-26}  {entry.Properties[propName].Value}");
}
