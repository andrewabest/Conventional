SELECT name FROM sys.check_constraints
WHERE is_system_named = 1 AND type = 'C'
ORDER BY name
