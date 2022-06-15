Income Payment Manager Application Description

The application is written in C# and uses the .NET.
The architecture of the application is based on the Model-View-ViewModel (MVVM), a pattern that allows you to separate the view from the model. The solution is split into multiple projects, reflecting the structure of the application and its components as well as emphasizing the separation of concerns provided by the MVVM pattern. 

Dependency injection has been employed to inject the dependencies into the view models.

A significant amount of effort has been put into the application's user interface. The application's user interface is implemented using WPF and is designed to be easy to use and intuitive.

User preferences are stored in the application's settings file. There is no need to update the settings file manually, as the application will automatically update the settings file when the user changes any of the personal preferences.


Event Log
The running application maintains an event log. The event log is a list of events that have occurred in the application. The event log is used to track the application's progress and to provide the user with feedback about the application's progress. Currently, the event log is set to the Information level. In case of a need to investigate an issue, the event log can be set to the Verbose level. After the application has been deployed and demonstrated stability in operations, the event log can be set to the Warning level in order to save space.

SQL Server Connection
SQL server connection string is stored in the application's settings file. The connection string is used to connect to the SQL server. 
Windows Authentication is used to connect to the SQL server. Thus, the user does not need to enter a username and password. And there is no need to store the password in the application's settings file.

Security
The application's security is based on the exisiting BMS security framework as well as native SQL server security built-in features such as SQL Server Login, SQL Server Role, etc. All users of the application must belong to the SQL Server Role named IpmRole. The IpmRole role is used to grant access to the objects in the database used by the application. The IpmRole role must be created in the following SQL Server databases: BS, Inventory, VBCM. There is a SQL script that is used to create the IpmRole role in all the databases.

I have also created a helper application that is used to manage the IpmRole role and synchronise the users of the role and their privileges with the BMS security framework. it is called IpmRoleSync. It support the following modes:
- Editing BMS security framework roles and privileges and synchronising them with the SQL Server IpmRole role.
- Editing SQL Server IpmRole role and synchronising it with the BMS security framework roles and privileges.
The IpmRoleSync application is currently under development, but the main functionality is implemented and is ready to be tested.