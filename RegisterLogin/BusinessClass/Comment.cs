using Oracle.ManagedDataAccess.Client;
using System;
using DataBase;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BusinessClass.Comment
{
    public class Comment
    {
        //属性
        public string user_id;
        public string game_id;
        public string content;
        public int agree_num;
        public int disagree_num;
        public int rating;
        public string send_time;
        //方法
        public static int GetComment(string game_id, out int page, List<Comment> comment_list)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "SELECT USERID, GAMEID, CONTENT, AGREE_NUM, DISAGREE_NUM,RATING, SEND_TIME FROM COMMENT WHERE GAMEID ='" + game_id + "' ORDER BY SEND_TIME";
            OracleDataReader reader = cmd.ExecuteReader();
            Comment comment;
            int MaxPageComments = 10; //一页最多显示十个
            int CommentNum = 0;
            try
            {
                //查找成功，赋值变量
                while (reader.Read())
                {
                    comment = new Comment();
                    comment.user_id = reader[0].ToString();
                    comment.game_id = reader[1].ToString();
                    comment.content = reader[2].ToString();
                    comment.agree_num = int.Parse(reader[3].ToString());
                    comment.disagree_num = int.Parse(reader[4].ToString());
                    comment.rating = int.Parse(reader[5].ToString());
                    comment.send_time = reader[6].ToString();
                    CommentNum++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                page = 0;
                return -1;
            }
            page = CommentNum / MaxPageComments + 1;
            return CommentNum;
        }
    }
}
