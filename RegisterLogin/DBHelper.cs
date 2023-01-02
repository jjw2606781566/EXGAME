using Oracle.ManagedDataAccess.Client;
using System;

namespace DataBase
{
    public class DBHelper
    {
        private static string connString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=154.204.60.213)(PORT=1521))(CONNECT_DATA=(SID=cdb1)));Persist Security Info=True;User ID=c##jjw;Password=1234;";
        public static OracleConnection con = new OracleConnection(connString);
        public DBHelper()
        {
            try
            {
                con.Open();
                Console.WriteLine("Successfully connected to Oracle Database");
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection Failed!");
                Console.WriteLine(e);
                con.Close();
            }
        }
        public static bool isOpened()
        {
            if(con.State == System.Data.ConnectionState.Open)
                return true;
            else
            {
                try
                {
                    con.Open();
                    Console.WriteLine("Successfully connected to Oracle Database");
                    Console.WriteLine();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Connection Failed!");
                    Console.WriteLine(e);
                    con.Close();
                    return false;
                }
            }
        }
    }
}
