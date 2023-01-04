using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace exgame.BusinessClass
{
    public class Publisher:ExUser
    {
        public string general_info;
        public string brief_info;
        public string logo;
        public string telephone;
        public string intro_pic;
        public string ali_account;
        public static int Register(string name, string email, string telephone, string password, string time, string area, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT ID FROM PUBLISHER WHERE EMAIL = '{email}'";
            OracleDataReader reader1 = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader1.Read())
                {
                    ret = 2;
                    result = "";
                    return ret;
                }
            }
            catch (Exception e)
            {
                ret = -1;
                result = e.Message;
                return ret;
            }

            cmd.CommandText = $"SELECT ID FROM PUBLISHER WHERE NAME = '{name}'";
            OracleDataReader reader2 = cmd.ExecuteReader();
            try
            {
                if (reader2.Read())
                {
                    ret = 1;
                    result = "";
                    return ret;
                }
            }
            catch (Exception e)
            {
                ret = -1;
                result = e.Message;
                return ret;
            }

            OracleCommand command = DBHelper.con.CreateCommand();
            command.CommandText = $"SELECT ID FROM PUBLISHER WHERE NAME=BASIC_PUBLISHER";
            string uid = "";
            OracleDataReader reader;

            try
            {
                reader = command.ExecuteReader();
                if (reader.Read())
                    uid = reader[0].ToString();
                /*int rslt = SetBasicUID(uid);
                if (rslt == 0)
                {
                    command.CommandText = "ROLLBACK";       //update failed 
                    uid = "";
                }
                else
                    command.CommandText = "COMMIT";*/
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                uid = "";
            }
            int res = 0;
            string new_id = (int.Parse(uid) + 1).ToString("d10");

            //OracleCommand cmd2 = DBHelper.con.CreateCommand();
            cmd.CommandText = $"UPDATE PUBLISHER SET ID='{new_id}' WHERE NAME=BASIC_PUBLISHER";

            try
            {
                res = cmd.ExecuteNonQuery();
                cmd.CommandText = res == 0 ? "ROLLBACK" : "COMMIT";
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                cmd.CommandText = "ROLLBACK";
                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = $"INSERT INTO PUBLISHER VALUES('{uid}', '{password}', '{name}', null, null, '{email}', '{time}', '{area}', '{telephone}')";
            ret = 0;
            return ret;
        }
        public static int getPublisherInfo(string uid, string name, string email, string password, string telephone, string area, string time, string intro, string logo, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT NAME, EMAIL, PASSWORD, AREA, TELEPHONE, REGISTER_DATE, LOGO, INTRO_TEXT FROM GAME_USER WHERE ID = '{uid}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                name = reader[0].ToString();
                email = reader[1].ToString();
                password = reader[2].ToString();
                area = reader[3].ToString();
                telephone = reader[4].ToString();
                time = reader[5].ToString();
                logo = reader[6].ToString();
                intro = reader[7].ToString();
                result = "";
            }
            catch (Exception e)
            {
                ret = -1;
                result = e.Message;
            }
            return ret;
        }
        public static int UpdatePublisherInfo(string uid, string name, string password, string telephone, string area, string intro, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT NAME FROM PUBLISHER WHERE ID = '{uid}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader.Read())
                {
                    cmd.CommandText = $"UPDATE PUBLISHER SET PUBLISHER_NAME='{name}', PASSWORD='{password}', TELEPHONE='{telephone}', AREA='{area}', INTRO_TEXT='{intro}', WHERE ID='{uid}'";
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
        public static int ApplyUpdateEmail(string uid, string email, string password, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT ID FROM PUBLISHER WHERE EMAIL = '{email}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (!reader.Read())
                {
                    cmd.CommandText = $"SELECT PASSWORD FROM PUBLISHER WHERE ID = '{uid}'";
                    reader = cmd.ExecuteReader();
                    if (reader[0].ToString() == password)
                    {
                        cmd.CommandText = $"UPDATE PUBLISHER SET EMAIL = '{email}' WHERE ID = '{uid}'";
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
        public static int ApplyUpdateLogo(string uid, string logo, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT NAME FROM PUBLISHER WHERE ID = '{uid}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader.Read())
                {
                    cmd.CommandText = $"UPDATE PUBLISHER SET LOGO = '{logo}' WHERE ID = '{uid}'";
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
