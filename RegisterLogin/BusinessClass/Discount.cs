using Oracle.ManagedDataAccess.Client;
using System;
using DataBase;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessClass.Comment;
using Microsoft.AspNetCore.Mvc;

namespace BusinessClass.Discount
{
    public class Discount
    {
        public string game_id;
        public double rate;
        public string start_date;
        public string end_date;
        public static bool GetGameDiscount(string game_id, Discount discount, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "SELECT RATE, START_DATE, END_DATE FROM DISCOUNT WHERE GAMEID ='" + game_id + "'";
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                //查找成功，赋值变量
                while (reader.Read())
                {
                    discount.rate = double.Parse(reader[0].ToString());
                    discount.start_date = reader[1].ToString();
                    discount.end_date = reader[2].ToString();
                }
            }
            catch (Exception e)
            {
                reason = e.Message;
                return false;
            }
            reason = "查找成功";
            return true;
        }
        public static bool SavePublishDiscountRequest(Discount discount, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "SELECT RATING FROM COMMENTS WHERE GAME_ID = '" + discount.game_id + "'";
            OracleDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reason = "该游戏已存在折扣";
                return false;
            }
            cmd.CommandText = "INSERT INTO DISCOUNT VALUES('" + discount.game_id + "'," + discount.rate + ",'" + "',to_date('" +discount.start_date + "','yyyy-mm-dd'))";
            int cen = cmd.ExecuteNonQuery();
            if (cen == 0)
            {
                cmd.CommandText = "ROLLBACK";
                cen = cmd.ExecuteNonQuery();
                reason = "添加新discount失败";
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
        public static bool DeleteDiscount(string game_id, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"DELETE FROM COMMENTS WHERE GAME_ID='{game_id}'";
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
