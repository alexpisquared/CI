#2021-03-23
C:\Users\alex.pigida\OneDrive\Documents\SQL Server Management Studio\Permission Management.sql

Filtering, Sorting, and Grouping w/ Collection Views - EASY WPF (.NET CORE)  https://www.youtube.com/watch?v=fBKW-spQboc

//todo: see hierarchy https://docs.microsoft.com/en-us/ef/core/get-started/wpf


Proper relationships are in DB20\MSSQLDB 

SELECT Users.UserID, PermissionAssignments.Status, Permissions.Name, Application.AppName
FROM   Users INNER JOIN
       PermissionAssignments ON PermissionAssignments.UserID = Users.User_IntID INNER JOIN
       Permissions ON Permissions.PermissionID = PermissionAssignments.PermissionID INNER JOIN
       Application ON Application.AppID = Permissions.AppID
WHERE  (Application.AppName = 'BMS') AND (Users.User_IntID = 230) AND (Users.Status = 'A') AND (Permissions.Name = 'Mutual Fund Order Entry')

Second part of the perm UNION  (fixed grouping join)
SELECT     PermissionAssignments.Status, Permissions.Name AS PermissionName, Application.AppName, Users.UserID AS UserAsGroup, GU.UserID
FROM        GroupUsers INNER JOIN
                  Users AS GU ON GroupUsers.UserID = GU.User_IntID INNER JOIN
                  Users ON Users.User_IntID = GroupUsers.GroupID INNER JOIN
                  PermissionAssignments AS PermissionAssignments ON PermissionAssignments.UserID = Users.User_IntID INNER JOIN
                  Permissions ON Permissions.PermissionID = PermissionAssignments.PermissionID INNER JOIN
                  Application ON Application.AppID = Permissions.AppID
ORDER BY PermissionAssignments.Status, PermissionName, Application.AppName, UserAsGroup, GU.UserID