SELECT sys.tables.name
    
FROM sys.identity_columns
    INNER JOIN sys.tables
        ON sys.identity_columns.object_id = sys.tables.object_id

WHERE sys.identity_columns.name <> sys.tables.name + 'Id'