namespace Conventional.Tests.Conventional.Conventions.Database
{
    public static class SqlScripts
    {
        public static string CreateDb(string dbName)
        {
            return string.Format("if not exists (select * from sys.databases where name='{0}')\r\ncreate database {0}",
                dbName);
        }

        public static string DropDb(string dbName)
        {
            return string.Format("if exists (select * from sys.databases where name='{0}')\r\nalter database {0} set SINGLE_USER with rollback IMMEDIATE\r\ndrop database {0}", dbName);
        }
    }
}