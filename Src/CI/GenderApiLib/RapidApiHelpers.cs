namespace GenderApiLib;

public static class RapidApiHelpers
{

    public static bool IsBadName(string firstName)
    {
        var rv = true;

        if (new Regex("^[a-zA-Z]*$").Match(firstName).Success == false) return false;

        var badNames = new string[] {
  "bmo",
  "dice",
  "domain",
  "hr",
  "ibm",
  "info",
  "it",
  "madam",
    "monster",
  "no",
    "noreply",
    "sql",
    "stack",
  "the",
  "sir"};
        badNames.ToList().ForEach(name =>
        {
            if (firstName.Equals(name, StringComparison.OrdinalIgnoreCase))
                rv = false;
        });

        if (!rv) return false;

        var badParts = new string[] {
  "admin",
  "career",
  "cgi",
  "cibc",
  "contact",
  "custom",
  "data",
  "email",
  "glass",
  "human",
  "linke",
  "madam",
  "market",
  "option",
  "quest",
  "recru",
  "remove",
  "resou",
  "sales",
  "servi",
  "suppor",
  "subsc",
  "team",
  "tech",
  "sir"};
        badParts.ToList().ForEach(name =>
        {
            if (firstName.Contains(name, StringComparison.OrdinalIgnoreCase))
                rv = false;
        });

        if (!rv) return false;

        return rv;
    }
}