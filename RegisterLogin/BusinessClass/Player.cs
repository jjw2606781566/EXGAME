using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace exgame.BusinessClass
{
    public class Player
    {
        public string id;
        public string name;
        public DateTime birthday;
        public string intro;
        public string profile_photo;
        //public List<Order> order_list;
        //public List<Column> column_list;

        public static int PlayerExists(string uid, string name, bool is_uid, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            if (is_uid)
            {
                cmd.CommandText = $"SELECT NAME FROM GAME_USER WHERE ID = '{uid}'";
            }
            else
            {
                cmd.CommandText = $"SELECT ID FROM GAME_USER WHERE NAME = '{name}'";
            }
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                ret = reader.Read() ? 1 : 0;
                result = "";
            }
            catch (Exception e)
            {
                ret = -1;
                result = e.Message;
            }

            return ret;
        }
    }
}
