using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace exgame.BusinessClass
{
    public class Game
    {
        public string id;
        public string name;
        public string publisher_id;
        public int is_dlc;
        public DateTime publish_date;
        public double price;
        public double discount;
        public Dictionary<string, string> Configuration;
        public double size;
        public string cover;
        public string general_intro;
        public string specific_intro;
        public string level;
        public int has_checked;
        public int like_num;
        public int dislike_num;
        public int tot_deal_num;
        public string rating;
        public List<string> language;
        public List<string> video;
        public List<string> intro_photo;
        public List<string> tag_list;
        public List<string> cdk_list;


        public static int GameExisis(string gid, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT NAME FROM GAME WHERE ID = '{gid}'";
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

        public static int GetGameBriefIntro(string gid, string name, string publisher, int is_dlc, string cover, string general_intro, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT NAME, PUBLISHER_ID, COVER, GENERAL_INTRO, IS_DLC FROM GAME WHERE ID = '{gid}";
            OracleDataReader reader = cmd.ExecuteReader();

            int ret = 0;
            try
            {
                if (reader.Read())
                {
                    name = reader[0].ToString();
                    publisher = reader[1].ToString();
                    cover = reader[2].ToString();
                    general_intro = reader[3].ToString();
                    is_dlc = int.Parse(reader[4].ToString());
                }
                ret = 1;
                result = "";
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
