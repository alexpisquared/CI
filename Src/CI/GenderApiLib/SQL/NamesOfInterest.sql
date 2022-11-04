USE [QStatsRls]
GO
SELECT     COUNT(*) AS Count, FName
FROM        EMail
GROUP BY FName
HAVING     (NOT (FName LIKE '%0%')) AND (NOT (FName LIKE '%1%')) AND (NOT (FName LIKE '%2%')) AND (NOT (FName LIKE '%3%')) AND (NOT (FName LIKE '%4%')) AND (NOT (FName LIKE '%5%')) AND (NOT (FName LIKE '%6%')) AND (NOT (FName LIKE '%7%')) AND (NOT (FName LIKE '%8%')) AND 
                  (NOT (FName LIKE '%9%')) AND (NOT (FName LIKE '% %')) AND (COUNT(*) > 1) AND (COUNT(*) < 4) AND (LEN(FName) > 5)
ORDER BY Count DESC, FName