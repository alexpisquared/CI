# Hello
## Hello
### Hello

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

