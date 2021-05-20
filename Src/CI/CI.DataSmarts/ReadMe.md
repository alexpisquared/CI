# Chrono ASCENDING

I have reviewd the expamce of the SPs: they are very diverse:
1. some params are mandatory, some optional
2. some param values are limited to a set from a DB
 => designing/enforcing proper validation would be problematic
3. return values: some are just a string, number, some are a table, some a mutliple tables
  - not all columns are of interest
  - 
MOST IMPORTANT:
There are SPs which have conctenation of SQL statements ... which opens door to the SQL Injection Attack


Technically, it is doable to create a dynamic SP UI by for the end usrs to deal with it, but:
1. The process for the end users will be elaborate, complicated, error-prone, require time and intimate knowledge to the SP in question.
2. The process itself would require a lolt of troubleshooting on the dev team part
3. The resulted UI will be a crappy user experience.


I can crate a UE in less time for each SP than poor end users trying to deal with trying to translate their understaning/knowldge or the SP and the SP editor.

the result will be a much better UE, data integrity, security




-- params grouped by SP:
SELECT     o.name AS SP, p.parameter_id, p.name AS Param, TYPE_NAME(p.user_type_id) AS Type, p.max_length, p.precision, p.system_type_id, p.user_type_id, p.scale, p.is_output, p.is_cursor_ref, p.has_default_value, p.is_xml_document, p.default_value, p.xml_collection_id, p.is_readonly, p.is_nullable, p.encryption_type, p.encryption_type_desc, p.encryption_algorithm_name, p.column_encryption_key_id, p.column_encryption_key_database_name
FROM        sys.parameters AS p INNER JOIN sys.objects AS o ON p.object_id = o.object_id
WHERE     (p.name <> '') AND (o.type IN ('P', 'X'))
ORDER BY SP, p.parameter_id, Param, o.type, p.max_length DESC
SPs with most params:
usp_AddTransaction    24+ params
usp_UpdateCommission  24
usp_UpdateCompanyProfile
usp_CreateNewCompanyProfile 21

-- DB Permission Audit:
SELECT suser_name() AS [SQL Login], CURRENT_USER AS [User]

SELECT (dp.state_desc + ' ' + dp.permission_name collate latin1_general_cs_as + ' on ' + s.name + '.' + o.name + ' to ' + dpr.name 	) AS [What is granted to whom]
FROM sys.database_permissions AS dp
  INNER JOIN sys.objects AS o ON dp.major_id=o.object_id
  INNER JOIN sys.schemas AS s ON o.schema_id = s.schema_id
  INNER JOIN sys.database_principals AS dpr ON dp.grantee_principal_id=dpr.principal_id
WHERE 1=1
    --	AND o.name IN ('AlexPi_Test')       -- Uncomment to filter to specific object(s)
		--  AND dp.permission_name='EXECUTE'    -- Uncomment to filter to just the EXECUTEs
ORDER BY 1

select top(10) SCHEMA_NAME(schema_id)+'.'+ name,     has_perms_by_name(name, 'OBJECT', 'EXECUTE') as has_execute from sys.procedures  
order by 1

exec [dbo].[AlexPi_Test] 

-- how to GRANT / DENY
GRANT/DENY EXECUTE ON ~OBJECT::[dbo].[AlexPi_Test] TO ~DevDbgLoginUser 



-- A quick peek at SQL Logins visible from the current connection:
select sp.name       as SqlLogin,
       sp.type_desc  as [Login type],
       sp.create_date,
       sp.modify_date,
       case when sp.is_disabled = 1 then 'Disabled' else 'Enabled' end as Status
from sys.server_principals sp left join sys.sql_logins sl on sp.principal_id = sl.principal_id
where sp.type not in ('G', 'R')
order by sp.name;

# //todo: next: progressively script all required perms to run the DBPL.exe for RAZER1\alexp !!!



USE [master]
GO
CREATE LOGIN [EndUserLogin] WITH PASSWORD=N'EndUserLogin', DEFAULT_DATABASE=[Inventory], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO

USE [Inventory]
GO
CREATE USER [EndUserLoginUser] FOR LOGIN [EndUserLogin] WITH DEFAULT_SCHEMA=[dbo]
GO

GRANT EXECUTE ON [dbo].[usp_Report_AllCash] TO [EndUserLoginUser]
GO

GO
GRANT VIEW DEFINITION ON [dbo].[usp_Report_AllCash] TO [EndUserLoginUser]
GO
