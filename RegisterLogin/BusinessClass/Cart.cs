using System;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;
using Oracle.ManagedDataAccess.Client;
namespace exgame.BusinessClass
{
    public class Cart
    {
        public string uid;
        public List<Game> cart;
        public int game_num;
        public double amount;
        public static int AddGame(string uid, string gid)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"INSERT INTO CART VALUES('{uid}', '{gid}')";
            int cen = 0;
            int ret = 0;
            try
            {
                cen = cmd.ExecuteNonQuery();
                if (cen == 0)
                {
                    cmd.CommandText = "ROLLBACK";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "COMMIT";
                    cmd.ExecuteNonQuery();
                    ret = 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                cmd.CommandText = "ROLLBACK";
                cmd.ExecuteNonQuery();
            }

            return ret;
        }
        public static int DeleteGame(string uid, string gid)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"DELETE FROM CART WHERE USER_ID='{uid}' AND GAME_ID='{gid}'";
            int ret = 0;
            int cen = 0;

            try
            {
                cen = cmd.ExecuteNonQuery();
                if (cen == 0)
                {
                    cmd.CommandText = "ROLLBACK";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.CommandText = "COMMIT";
                    cmd.ExecuteNonQuery();
                    ret = 2;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                cmd.CommandText = "ROLLBACK";
                cmd.ExecuteNonQuery();
            }

            return ret;
        }
        public static int BuyGame(string uid, string uid1, List<string> choose_gid_list, string result)
        {
            try
            {
                for (int i = 0; i < choose_gid_list.Count; i++)
                {
                    OracleCommand cmd = DBHelper.con.CreateCommand();
                    OracleDataReader reader;
                    cmd.CommandText = $"SELECT PRICE FROM GAME WHERE ID='{choose_gid_list[i]}'";
                    reader = cmd.ExecuteReader();
                    Order.CreateOrder(uid, choose_gid_list[i], 1, double.Parse(reader[0].ToString()), uid1);
                    GameLib.AddGame2Lib(choose_gid_list[i], uid, result);
                }
            }
            catch (Exception e)
            {
                result = e.Message;
                return 0;
            }
            result = "购买成功";
            return 1;
        }
        public static int GetCart(string uid, List<Game> cart)
        {
            DBHelper.isOpened();
            OracleCommand main_cmd = DBHelper.con.CreateCommand();
            OracleDataReader reader;
            int ret = 0;
            try
            {
                main_cmd.CommandText = $"SELECT GAME_ID FROM CART WHERE USER_ID='{uid}'";
                reader = main_cmd.ExecuteReader();
                string gid = "";
                OracleCommand sub_cmd = DBHelper.con.CreateCommand();
                OracleDataReader sub_reader;
                Game game;

                while (reader.Read())
                {
                    gid = reader[0].ToString();
                    sub_cmd.CommandText = $"SELECT NAME, COVER, PRICE, FUN_GET_CURRENT_DISCOUNT(ID), IS_DLC FROM GAME WHERE ID='{gid}'";
                    sub_reader = sub_cmd.ExecuteReader();
                    while (sub_reader.Read())
                    {
                        game = new Game();

                        game.id = gid;
                        game.name = sub_reader[0].ToString();
                        game.cover = sub_reader[1].ToString();
                        game.price = double.Parse(sub_reader[2].ToString());
                        game.discount = double.Parse(sub_reader[3].ToString());
                        game.is_dlc = int.Parse(sub_reader[4].ToString());

                        cart.Add(game);
                    }
                }
                ret = 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ret = 0;
            }

            return ret;
        }
    }

}

