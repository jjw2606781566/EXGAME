using System;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;
namespace exgame.BusinessClass
{
    public class Order
    {
        public string id;
        public string uid;
        public string gid;
        public string rid;
        public int method;
        public string time;
        public double amount;
        public static int CreateOrder(string uid, string gid, int method, double amount, string rid)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "SELECT ID FROM GAME_ORDER WHERE USER_ID = '" + uid + "'";
            OracleDataReader reader = cmd.ExecuteReader();
            string nextid = "00000000000000000000";
            while (reader.Read())
            {
                nextid = (int.Parse(reader[0].ToString()) + 1).ToString();
            }

            int ret = 0;
            cmd.CommandText = $"INSERT INTO GAME_ORDER VALUES('{nextid}', '{uid}', '{gid}',SYSDATE, {method}, {amount}, '{rid}')";
            try
            {
                ret = cmd.ExecuteNonQuery();
                cmd.CommandText = ret == 0 ? "ROLLBACK" : "COMMIT";
                cmd.CommandText = "UPDATE GAME SET TOT_DEAL_NUM=TOT_DEAL_NUM+1 WHERE ID='" + gid + "'";
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                ret = 0;
                Console.WriteLine(e);
                cmd.CommandText = "ROLLBACK";
            }
            cmd.ExecuteNonQuery();

            return ret;
        }
        public static int GetOrder(string uid, List<Order> order_list, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"ID, USER_ID,GAME_ID, ORDER_TIME, METHOD, AMOUNT FROM GAME_ORDER WHERE GAMEID = '{uid}'";
            OracleDataReader reader = cmd.ExecuteReader();
            Order order;
            try
            {
                //查找成功，赋值变量
                while (reader.Read())
                {
                    order = new Order();
                    order.id = reader[0].ToString();
                    order.uid = reader[1].ToString();
                    order.gid = reader[2].ToString();
                    order.time = reader[3].ToString();
                    order.method = int.Parse(reader[4].ToString());
                    order.amount = double.Parse(reader[5].ToString());
                    order_list.Add(order);
                }
            }
            catch (Exception e)
            {
                result = e.Message;
                return 0;
            }
            result = "查找成功";
            return 1;
        }

    }
}
