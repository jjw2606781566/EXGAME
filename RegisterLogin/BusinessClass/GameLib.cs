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

        public static int GameInLib(string uid, string gid, string result)
        {
            int ret = Player.PlayerExists(uid, "", true, result);
            if (ret == 1)
            {
                DBHelper.isOpened();
                OracleCommand cmd = DBHelper.con.CreateCommand();
                cmd.CommandText = $"SELECT GAME_ID FROM OWNERSHIP WHERE USER_ID = '{uid}' AND GAME_ID = '{gid}'";
                OracleDataReader reader = cmd.ExecuteReader();

                try
                {
                    ret = reader.Read() ? 1 : 0;
                }
                catch (Exception e)
                {
                    ret = -1;
                    result = e.Message;
                }
            }
            else if (ret == 0)
            {
                ret = 2;
            }

            return ret;
        }

        public static int AddGame2Lib(string uid, string gid, string result)
        {
            int ret = Player.PlayerExists(uid, "", true, result);
            if (ret == 1)
            {
                ret = Game.GameExists(gid, result);
                if (ret == 1)
                {
                    ret = GameInLib(uid, gid, result);
                    if (ret == 1)
                    {
                        ret = 0;
                    }
                    else
                    {
                        DBHelper.isOpened();
                        OracleCommand cmd = DBHelper.con.CreateCommand();
                        /* add game to ownership */
                        cmd.CommandText = $"INSERT INTO OWNERSHIP VALUES('{uid}', '{gid}', {0})";
                        try
                        {
                            ret = cmd.ExecuteNonQuery();
                            if (ret == 0)
                            {
                                cmd.CommandText = "ROLLBACK";
                                cmd.ExecuteNonQuery();
                                return ret;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            cmd.CommandText = "ROLLBACK";
                            cmd.ExecuteNonQuery();
                            return ret;
                        }

                        /* update number of games user owns */
                        cmd.CommandText = $"UPDATE GAME_USER SET GAME_NUM=GAME_NUM+1 WHERE ID='{uid}'";
                        try
                        {
                            ret = cmd.ExecuteNonQuery();
                            if (ret == 0)
                            {
                                cmd.CommandText = "ROLLBACK";
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                cmd.CommandText = "COMMIT";
                                cmd.ExecuteNonQuery();
                            }
                        }
                        catch (Exception e)
                        {
                            ret = 0;
                            Console.WriteLine(e);
                            cmd.CommandText = "ROLLBACK";
                            cmd.ExecuteNonQuery();
                        }
                    }                    
                }
                else if (ret == 0)
                {
                    ret = 2;
                }                
            }
            else if (ret == 0)
            {
                ret = 3;
            }

            return ret;
        }
    }
}
