using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace exgame.BusinessClass
{
    public class Reply
    {
        public string column_id;
        public Player player;
        public string content;
        public DateTime time;


        public static int PublishReply(string uid, string cid, string content, DateTime time, string result)
        {
            int ret = Player.PlayerExists(uid, "", true, result);
            if (ret == 1)
            {
                ret = Column.ColumnExists(cid, result);
                if (ret == 1)
                {
                    DBHelper.isOpened();
                    OracleCommand cmd = DBHelper.con.CreateCommand();
                    cmd.CommandText = "INSERT INTO REPLY_COLUMN VALUES('" + cid + "','" + uid + "','" + content + "',TO_DATE('" + time + "', 'YYYY-MM-DD HH24:MI:SS'))";
                    int cen = 0;
                    try
                    {
                        cen = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        ret = -1;
                        result = e.Message;
                        return ret;
                    }
                    try
                    {
                        cmd.CommandText = "COMMIT";
                        cen = cmd.ExecuteNonQuery();
                        ret = 1;
                    }
                    catch (Exception e)
                    {
                        result= e.Message;
                        ret = -1;
                    }
                }
            }

            return ret;
        }

        public static int DeleteReply(string uid, string cid, string result)
        {
            int ret = Player.PlayerExists(uid, "", true, result);
            if (ret == 1)
            {
                ret = Column.ColumnExists(cid, result);
                if (ret == 1)
                {
                    DBHelper.isOpened();
                    OracleCommand cmd = DBHelper.con.CreateCommand();
                    cmd.CommandText = $"DELETE FROM REPLY_COLUMN WHERE COLUMN_ID = {cid} AND USER_ID = {uid}";
                    int cen = 0;
                    try
                    {
                        cen = cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        ret = -1;
                        result = e.Message;
                        return ret;
                    }
                    try
                    {
                        cmd.CommandText = "COMMIT";
                        cen = cmd.ExecuteNonQuery();
                        ret = 1;
                    }
                    catch (Exception e)
                    {
                        result = e.Message;
                        ret = -1;
                    }
                }
            }

            return ret;
        }

        public static int GetColumnReply(string cid, List<Reply> replies, string result)
        {
            int ret = Column.ColumnExists(cid, result);
            if (ret == 1)
            {
                DBHelper.isOpened();
                OracleCommand cmd = DBHelper.con.CreateCommand();
                cmd.CommandText = "SELECT USER_ID,reply_content,reply_time FROM reply_column WHERE COLUMN_ID = '" + cid + "' order by reply_time";
                OracleDataReader reader = cmd.ExecuteReader();

                try
                {
                    //查找成功，赋值变量
                    while (reader.Read())
                    {
                        Reply reply = new Reply();
                        reply.content = reader[1].ToString();
                        reply.player.id = reader[0].ToString();
                    }
                    ret = 1;
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
