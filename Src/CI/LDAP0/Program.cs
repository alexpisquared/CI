if (!System.OperatingSystem.IsWindows()) return;



//CI.LDAP.Lib.LdapHelper.GetAll().ForEach(r=> Colorful.Console.WriteLine(r));
//CI.LDAP.Lib.LdapHelper.ModernLdapFinder("char").ForEach(r => Colorful.Console.WriteLine(r));


var c = new LDAP0.Stuff();

c.FromProd("alex");
c.FromProd("pigi");
c.FromProd("Eng");
c.FromProd("zen");
//c.FromProd("chart");

//c.allUsersSimple("char");
////c.ModernLdapFinder1st100("");
////c.ModernLdapFinder1st100("q");
////c.ModernLdapFinder1st100("");
////c.ModernLdapFinder1st100("eng");
////c.ModernLdapFinder1st100("said");
////c.ModernLdapFinder1st100("");
////c.ModernLdapFinder1st100("mazi");
//c.ModernLdapFinder1st100("char");
////c.list0_Yellow();
////c.allAttributes();
