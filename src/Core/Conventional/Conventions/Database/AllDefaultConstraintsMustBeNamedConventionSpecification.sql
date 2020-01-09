SELECT name
FROM sys.default_constraints
WHERE type = 'D' AND is_system_named = 1
ORDER BY name
