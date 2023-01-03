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
    }
}
