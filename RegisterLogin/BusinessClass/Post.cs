using System;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;
namespace exgame.BusinessClass
{
    public class Post
    {
        public string game_id;
        public string post_id;
        public string post_cover;
        public string post_title;
        public string time;
        public string content;
        public int check;
        public static bool GetPostInfo(string game_id, string post_id, Post post, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT POST_COVER, POST_TITLE, TIME, CONTENT, FROM GAME WHERE GAMEID = '{game_id}' AND POST_ID = '{post_id}'";
            OracleDataReader reader = cmd.ExecuteReader();

            try
            {
                if (reader.Read())
                {
                    post.post_cover = reader[0].ToString();
                    post.post_title = reader[1].ToString();
                    post.time = reader[2].ToString();
                    post.content = reader[3].ToString();
                }
                reason = "获取信息成功";
            }
            catch (Exception e)
            {
                reason = e.Message;
                return false;
            }

            return true;
        }
        public static int GetGamePosts(string game_id, List<Post> post_list, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT POST_COVER, POST_TITLE, TIME, CONTENT, FROM GAME WHERE GAMEID = '{game_id}'";
            OracleDataReader reader = cmd.ExecuteReader();
            Post post;
            try
            {
                //查找成功，赋值变量
                while (reader.Read())
                {
                    post = new Post(); 
                    post.post_cover = reader[0].ToString();
                    post.post_title = reader[1].ToString();
                    post.time = reader[2].ToString();
                    post.content = reader[3].ToString();
                    post_list.Add(post);
                }
            }
            catch (Exception e)
            {
                reason = e.Message;
                return -1;
            }
            reason = "查找成功";
            return post_list.Count;
        }
        public static bool SavePublishPostRequest(Post post, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "SELECT POST_ID FROM COMMENTS WHERE GAME_ID = '00000000'";
            OracleDataReader reader = cmd.ExecuteReader();
            string nextid = "";
            if (reader.HasRows)
            {
                reader.Read();
                nextid = reader[0].ToString();
            }
            cmd.CommandText = "INSERT INTO DISCOUNT VALUES('" + post.game_id + "'," + nextid + ",'" + "',to_date('" + post.time + "','yyyy-mm-dd'))";
            int cen = cmd.ExecuteNonQuery();
            if (cen == 0)
            {
                cmd.CommandText = "ROLLBACK";
                cen = cmd.ExecuteNonQuery();
                reason = "添加新post失败";
                return false;  //修改失败
            }
            else
            {
                try
                {
                    cmd.CommandText = "COMMIT";
                    cen = cmd.ExecuteNonQuery();
                    reason = "添加成功";
                }
                catch (Exception e)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = e.Message.ToString();
                    return false;
                }
            }
            return true;
        }
        public static bool DeletePost(string game_id, string post_id, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"DELETE FROM POST WHERE GAME_ID='{game_id}'AND POSTID = '{post_id}'";
            try
            {
                int rslt = cmd.ExecuteNonQuery();
                cmd.CommandText = rslt == 1 ? "COMMIT" : "ROLLBACK";
                if (rslt == 0)
                {
                    reason = "删除失败";
                    return false;
                }
                else
                {
                    reason = "删除成功";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                cmd.CommandText = "ROLLBACK";
                reason = e.Message.ToString();
                return false;
            }
            cmd.ExecuteNonQuery();
            return true;
        }
    }
}
