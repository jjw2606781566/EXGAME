using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace exgame.BusinessClass
{
    public class GameLib
    {
        public Player player;
        public List<Game> game_lib;


        public static int GetPlayerGameLib(string uid, List<string> games, string result)
        {
            int ret = Player.PlayerExists(uid, "", true, result);
            if (ret == 1)
            {
                DBHelper.isOpened();
                OracleCommand cmd = DBHelper.con.CreateCommand();
                cmd.CommandText = $"SELECT GAME_ID FROM OWNERSHIP WHERE USER_ID = '{uid}'";
                OracleDataReader reader = cmd.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        games.Add(reader[0].ToString());
                    }
                }
                catch (Exception e)
                {
                    ret = -1;
                    result = e.Message;
                }
            }

            return ret;
        }
    }
}
