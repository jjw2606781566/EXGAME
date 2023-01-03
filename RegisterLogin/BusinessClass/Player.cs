using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace exgame.BusinessClass
{
    public class Player:User
    {
        public DateTime birthday;
        public string intro;
        public string profile_photo;
        //public List<Order> order_list;
        //public List<Column> column_list;

        public static int PlayerExists(string uid, string name, bool is_uid, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            if (is_uid)
            {
                cmd.CommandText = $"SELECT NAME FROM GAME_USER WHERE ID = '{uid}'";
            }
            else
            {
                cmd.CommandText = $"SELECT ID FROM GAME_USER WHERE NAME = '{name}'";
            }
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
        public static int Register(string email, string name, string password, string time, string area, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT ID FROM GAME_USER WHERE EMAIL = '{email}'";
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

            cmd.CommandText = $"SELECT ID FROM GAME_USER WHERE NAME = '{name}'";
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
            command.CommandText = $"SELECT ID FROM GAME_USER WHERE NAME=BASIC_USER";
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
            cmd.CommandText = $"UPDATE GAME_USER SET ID='{new_id}' WHERE NAME=BASIC_USER";

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

            cmd.CommandText = $"INSERT INTO GAME_USER VALUES('{uid}', '{password}', '{name}', 0, 0, '{email}', null, null, null, '{area}', '{time}')";
            ret = 0;
            return ret;
        }
        public static int getPlayerInfo(string uid, string name, string email, string password, string area, int status, string birthday, int game_num, string intro, string profile_photo, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT NAME, EMAIL, PASSWORD, AREA, STATUS, BIRTHDAY, GAME_NUM, PROFILE_PHOTO, INTRO FROM GAME_USER WHERE ID = '{uid}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                name = reader[0].ToString();
                email = reader[1].ToString();
                password = reader[2].ToString();
                area = reader[3].ToString();
                status = int.Parse(reader[4].ToString());
                birthday = reader[5].ToString();
                game_num = int.Parse(reader[6].ToString());
                profile_photo = reader[7].ToString();
                intro = reader[8].ToString();
                result = "";
            }
            catch (Exception e)
            {
                ret = -1;
                result = e.Message;
            }
            return ret;
        }
        public static int UpdatePlayerInfo(string uid, string name, string password, string area, string birthday, string intro, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT NAME FROM GAME_USER WHERE ID = '{uid}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader.Read())
                {
                    cmd.CommandText = $"UPDATE GAME_USER SET NAME='{name}', PASSWORD='{password}', AREA='{area}', BIRTHDAY='{birthday}', INTRO='{intro}', WHERE ID='{uid}'";
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
        public static int UpdateEmail(string uid, string email,string password, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT ID FROM GAME_USER WHERE EMAIL = '{email}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (!reader.Read())
                {
                    cmd.CommandText = $"SELECT PASSWORD FROM GAME_USER WHERE ID = '{uid}'";
                    reader = cmd.ExecuteReader();
                    if (reader[0].ToString() == password)
                    {
                        cmd.CommandText = $"UPDATE GAME_USER SET EMAIL = '{email}' WHERE ID = '{uid}'";
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
        public static int UpdateAvatar(string uid, string avatar, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT NAME FROM GAME_USER WHERE ID = '{uid}'";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader.Read())
                {
                    cmd.CommandText = $"UPDATE GAME_USER SET PROFILE_PHOTO = '{avatar}' WHERE ID = '{uid}'";
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
