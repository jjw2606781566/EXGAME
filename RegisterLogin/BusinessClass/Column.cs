using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace exgame.BusinessClass
{
    public class Column
    {
        public string id;
        public Player player;
        public Game game;
        public string title;
        public string content;
        public string time;
        public int has_checked;
        //public List<Reply> reply_list;
        public List<string> image_list;


        public static int ColumnExists(string cid, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT GAME_ID FROM COLUMNS WHERE COLUMN_ID = '{cid}'";
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

        public static int GetColumnInfo(string cid, Column column, string result)
        {
            int ret = ColumnExists(cid, result);
            if (ret == 1)
            {
                /* get basic info */
                DBHelper.isOpened();
                OracleCommand cmd = DBHelper.con.CreateCommand();
                cmd.CommandText = $"SELECT USER_ID, GAME_ID, COLUMN_TITLE, COLUMN_TEXT, POST_TIME FROM COLUMNS WHERE COLUMN_ID = '{cid}'";
                OracleDataReader reader = cmd.ExecuteReader();

                try
                {
                    if (reader.Read())
                    {
                        column.player.id = reader[0].ToString();
                        column.game.id = reader[1].ToString();
                        column.title = reader[2].ToString();
                        column.content = reader[3].ToString();
                        column.time = reader[4].ToString();
                    }                    
                    result = "";
                }
                catch (Exception e)
                {
                    ret = -1;
                    result = e.Message;
                    return ret;
                }

                /* get images */
                cmd.CommandText = $"SELECT COLUMN_PHOTO FROM COLUMN_PHOTOS WHERE COLUMN_ID='{cid}'";
                try
                {
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                        column.image_list.Add(reader[0].ToString());
                }
                catch (Exception e)
                {
                    ret = -1;
                    result = e.Message;
                    return ret;
                }
            }

            return ret;
        }

        public static int GetNextColumnID(string cid, string result)
        {
            string BASIC_NAME = "BASIC_COLUMN";
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT ID FROM GAME_USER WHERE NAME='{BASIC_NAME}'";
            OracleDataReader reader;

            int ret = 0;
            try
            {
                reader = cmd.ExecuteReader();
                if (reader.Read())
                    cid = reader[0].ToString();

                /* get basic cid */
                string new_id = (int.Parse(cid) + 1).ToString("d10");
                cmd.CommandText = $"UPDATE GAME_USER SET ID='{new_id}' WHERE NAME='{BASIC_NAME}'";
                try
                {
                    ret = cmd.ExecuteNonQuery();
                    cmd.CommandText = ret == 0 ? "ROLLBACK" : "COMMIT";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    cmd.CommandText = "ROLLBACK";
                    cmd.ExecuteNonQuery();
                }

                if (ret == 0)
                {
                    cmd.CommandText = "ROLLBACK";       //update failed 
                    cid = "";
                }
                else
                    cmd.CommandText = "COMMIT";
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                cid = "";
            }

            return ret;
        }

        public static int DeleteColumn(string cid, string result)
        {
            int ret = ColumnExists(cid, result);
            if (ret == 1)
            {
                DBHelper.isOpened();
                OracleCommand cmd = DBHelper.con.CreateCommand();
                
                try
                {
                    //查找成功，赋值变量
                    cmd.CommandText = "DELETE FROM COLUMN_PHOTOS WHERE COLUMN_ID = '" + cid + "'";
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"DELETE FROM COLUMNS WHERE COLUMN_ID = '{cid}'";
                    cmd.ExecuteNonQuery();
                    ret = 1;
                }
                catch (Exception e)
                {
                    result = e.Message;
                    ret = 0;
                }
            }

            return ret;
        }

        public static int CreateColumn(string uid, string gid, string title, string content, string time, string result)
        {
            int ret = GameLib.GameInLib(uid, gid, result);

            if (ret == 2)
                ret = 3;
            else if (ret == 0)
                ret = 2;
            else if (ret == 1)
            {
                string cid = "";
                ret = GetNextColumnID(cid, result);
                if (ret == 0)
                {
                    return ret;
                }
                DBHelper.isOpened();
                OracleCommand cmd = DBHelper.con.CreateCommand();

                try
                {
                    cmd.CommandText = "INSERT INTO COLUMNS VALUES('" +  cid + "','" + uid + "','" + gid + "','" + title + "','" + content + "',SYSDATE,'0')";    //初始评论为0
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    result = e.Message;
                    ret = -1;
                }
            }

            return ret;                
        }

        public static int GetPlayerColumns(string uid, List<string> column_list, string result)
        {
            int ret = Player.PlayerExists(uid, "", true, result);

            if (ret == 1)
            {
                DBHelper.isOpened();
                OracleCommand cmd = DBHelper.con.CreateCommand();
                cmd.CommandText = $"SELECT COLUMN_ID, USER_ID, FUN_GET_USER_NAME(USER_ID), COLUMN_TITLE, COLUMN_TEXT, COMMENT_NUM, POST_TIME, ROW_NUMBER() OVER (ORDER BY POST_TIME DESC) R FROM COLUMNS WHERE USER_ID='{uid}'";

                try
                {
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        column_list.Add(reader[0].ToString());
                    }
                }
                catch (Exception e)
                {
                    result = e.Message;
                    ret = -1;
                }
            }

            return ret;
        }

        public static int GetGameColumns(string gid, List<string> column_list, string result)
        {
            int ret = Game.GameExists(gid, result);

            if (ret == 1)
            {
                DBHelper.isOpened();
                OracleCommand cmd = DBHelper.con.CreateCommand();
                cmd.CommandText = $"SELECT COLUMN_ID, USER_ID, FUN_GET_USER_NAME(USER_ID), COLUMN_TITLE, COLUMN_TEXT, COMMENT_NUM, POST_TIME, ROW_NUMBER() OVER (ORDER BY POST_TIME DESC) R FROM COLUMNS WHERE GAME_ID='{gid}'";

                try
                {
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        column_list.Add(reader[0].ToString());
                    }
                }
                catch (Exception e)
                {
                    result = e.Message;
                    ret = -1;
                }
            }

            return ret;
        }
    }

}
