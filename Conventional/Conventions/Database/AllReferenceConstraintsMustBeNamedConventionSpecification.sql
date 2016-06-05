SELECT name
FROM sys.foreign_keys
WHERE is_system_named = 1
ORDER BY name
