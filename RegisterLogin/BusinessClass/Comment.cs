using Oracle.ManagedDataAccess.Client;
using System;
using DataBase;
using System.Collections.Generic;


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
        public static int GetComment(string game_id, out int page, List<Comment> comment_list, out string reason)
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
                reason = e.Message;
                page = 0;
                return -1;
            }
            page = CommentNum / MaxPageComments + 1;
            reason = "";
            return CommentNum;
        }
        public static bool PublishComment(Comment comment, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "SELECT RATING FROM COMMENTS WHERE USER_ID = '" + comment.user_id + "' AND GAME_ID = '" + comment.game_id + "'";
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reason = "该用户已发表过该游戏评论";
                return false;
            }
            cmd.CommandText = "INSERT INTO COMMENTS VALUES('" + comment.user_id + "','" + comment.game_id + "'," + comment.rating + ",'" + comment.content + "',0,0,'" + "',to_date('" + DateTime.Now.ToString("yyyy-MM-dd") + "','yyyy-mm-dd'))";
            int cen = cmd.ExecuteNonQuery();
            if (cen == 0)
            {
                cmd.CommandText = "ROLLBACK";
                cen = cmd.ExecuteNonQuery();
                reason = "添加新comment失败";
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
        public static bool DeleteComment(string user_id, string game_id, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"DELETE FROM COMMENTS WHERE USER_ID='{user_id}' AND GAME_ID='{game_id}'";
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
        public static bool EvaluateComment(string user_id, string game_id, int evaluation, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "select LIKE_CMT from COMMENT_LIKES WHERE USER_ID = '" + user_id + ";";
            OracleDataReader reader = cmd.ExecuteReader();
            int like_cmt = 0;
            if (!reader.HasRows)
            {
                reason = "用户并未评论过该帖子";
                return false;       
            }
            else
            {
                try
                {
                    //查找成功
                    if (reader.Read())
                    {
                        like_cmt = System.Convert.ToInt32(reader[0].ToString());
                    }
                }
                catch (Exception e)
                {
                    reason = e.Message.ToString();
                    return false;
                }
            }
            string newview = "";
            if (evaluation == 1)
                newview = "AGREE_NUM";
            else if (evaluation == -1)
                newview = "DISAGREE_NUM";
            int cen;
            if (like_cmt == evaluation)//若赞/踩了还赞/踩，则是取消
            {
                cmd.CommandText = "DELETE FROM COMMENT_LIKES WHERE USER_ID = '" + user_id + ";";
                try
                {
                    cen = cmd.ExecuteNonQuery();
                    if (cen == 0)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = "取消操作失败1!";
                        return false;
                    }
                }
                catch (Exception e)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = e.Message;
                    return false;
                }
                cmd.CommandText = "UPDATE COMMENTS SET " + newview + "= " + newview + "-1 ;";
                try
                {
                    cen = cmd.ExecuteNonQuery();
                    if (cen == 0)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = "取消操作失败2!";
                        return false;
                    }
                    else//取消操作成功
                    {
                        cmd.CommandText = "COMMIT";
                        cen = cmd.ExecuteNonQuery();
                        reason = "取消操作成功！";
                    }
                }
                catch (Exception e)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = e.Message;
                    return false;
                }
                return true;
            }
            else if (like_cmt == 0)//没有点过赞/踩，则新增数据
            {
                cmd.CommandText = "insert into COMMENT_LIKES values('" + user_id + "," + evaluation + ")";
                try
                {
                    cen = cmd.ExecuteNonQuery();
                    if (cen == 0)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = "新增操作失败1!";
                        return false;
                    }
                }
                catch (Exception e)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = e.Message;
                    return false;
                }
                cmd.CommandText = "UPDATE COMMENTS SET " + newview + "= " + newview + "+1";
                try
                {
                    cen = cmd.ExecuteNonQuery();
                    if (cen == 0)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = "新增操作失败2!";
                        return false;
                    }
                    else//取消操作成功
                    {
                        cmd.CommandText = "COMMIT";
                        cen = cmd.ExecuteNonQuery();
                        reason = "新增操作成功！";
                    }
                }
                catch (Exception e)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = e.Message;
                    return false;
                }
                return true;
            }
            else//点过踩/赞，但此时点了另一个，则取消上一个，新增下一个
            {
                string lastview = "";
                if (like_cmt == -1)
                    lastview = "DISAGREE_NUM";
                else
                    lastview = "AGREE_NUM";
                cmd.CommandText = "DELETE FROM COMMENT_LIKES WHERE USER_ID = '" + user_id + "'";
                try
                {
                    cen = cmd.ExecuteNonQuery();
                    if (cen == 0)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = "修改操作失败1!";
                        return false;
                    }
                }
                catch (Exception e)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = e.Message;
                    return false;
                }
                cmd.CommandText = "insert into COMMENT_LIKES values('" + user_id + "'," + evaluation + ")";
                try
                {
                    cen = cmd.ExecuteNonQuery();
                    if (cen == 0)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = "修改操作失败2!";
                        return false;
                    }
                }
                catch (Exception e)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = e.Message;
                    return false;
                }
                cmd.CommandText = "UPDATE COMMENTS SET " + lastview + "= " + lastview + "-1";
                try
                {
                    cen = cmd.ExecuteNonQuery();
                    if (cen == 0)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = "修改操作失败3!";
                        return false;
                    }
                }
                catch (Exception e)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = e.Message;
                    return false;
                }
                cmd.CommandText = "UPDATE COMMENTS SET " + newview + "= " + newview + "+1";
                try
                {
                    cen = cmd.ExecuteNonQuery();
                    if (cen == 0)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = "修改操作失败4!";
                        return false;
                    }
                    else//取消操作成功
                    {
                        cmd.CommandText = "COMMIT";
                        cen = cmd.ExecuteNonQuery();
                        reason = "修改操作成功！";
                    }
                }
                catch (Exception e)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = e.Message;
                    return false;
                }
                return true;//防止编译错误
            }
        }
    }
}
