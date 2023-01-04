using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace exgame.BusinessClass
{
    public class ExUser
    {
        public string id;
        public string name;
        public string email;
        public string password;
        public string area;
        public string register_time;
        public string last_login_time;

        public static int Login(string email, string password, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT PASSWORD FROM GAME_USER WHERE EMAIL = '{email}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader.Read())
                {
                    if(reader[0].ToString() == password)
                    {
                        
                        ret = 0;
                        result = "";
                    }
                    else
                    {
                        ret = 1;
                        result = "";
                    }
                }
                else
                {
                    ret = 2;
                    result = "";
                }
            }
            catch (Exception e)
            {
                ret = -1;
                result = e.Message;
            }
            return ret;
        }
        public static int UpdatePassword(string uid, string password_old, string password, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT PASSWORD FROM GAME_USER WHERE ID = '{uid}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader.Read())
                {
                    if (reader[0].ToString() == password_old)
                    {
                        cmd.CommandText = $"UPDATE GAME_USER SET PASSWORD = '{password}' WHERE ID = '{uid}'";
                        ret = 1;
                        result = "";
                        StateOn(uid,"");
                    }
                    else
                    {
                        ret = 0;
                        result = "";
                    }
                }
                else
                {
                    ret = -1;
                    result = "用户不存在";
                }
            }
            catch (Exception e)
            {
                ret = -1;
                result = e.Message;
            }

            return ret;
        }
        public static int StateOn(string uid, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT STATUS FROM GAME_USER WHERE ID = '{uid}'";
            
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader.Read())
                {
                    cmd.CommandText = $"UPDATE GAME_USER SET STATUS = '1' WHERE ID = '{uid}'";
                    ret = 0;
                    result = "";
                }
                else
                {
                    ret = -1;
                    result = "用户不存在";
                }
            }
            catch (Exception e)
            {
                ret = -1;
                result = e.Message;
            }

            return ret;
        }
        public static int StateOff(string uid, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT STATUS FROM GAME_USER WHERE ID = '{uid}'";

            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader.Read())
                {
                    cmd.CommandText = $"UPDATE GAME_USER SET STATUS = '0' WHERE ID = '{uid}'";
                    ret = 0;
                    result = "";
                }
                else
                {
                    ret = -1;
                    result = "用户不存在";
                }
            }
            catch (Exception e)
            {
                ret = -1;
                result = e.Message;
            }

            return ret;
        }
    }
}
