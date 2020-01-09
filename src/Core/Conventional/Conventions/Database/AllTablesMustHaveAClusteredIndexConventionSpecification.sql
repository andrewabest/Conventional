SELECT [o].[Name]
FROM [sys].[indexes] [i]
INNER JOIN [sys].[objects] [o] ON [i].[object_id] = [o].[object_id]
WHERE [o].[type_desc] = 'USER_TABLE'
AND [i].[type_desc] = 'HEAP'
ORDER BY [o].[name]