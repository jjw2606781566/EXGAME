using System;
using System.Collections.Generic;
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
        public string source_id;
        public int isOffsale;
        public List<String> language;
        public List<String> video;
        public List<String> intro_photo;
        public List<String> tag_list;
        public List<String> cdk_list;


        public static int GameExists(string gid, string result)
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
        public static bool GetGameInfo(string game_id, Game game, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = $"SELECT NAME, PUBLISHER_ID, COVER, GENERAL_INTRO, IS_DLC FROM GAME WHERE ID = '{game_id}";
            OracleDataReader reader = cmd.ExecuteReader();

            try
            {
                if (reader.Read())
                {
                    game.name = reader[0].ToString();
                    game.publisher_id = reader[1].ToString();
                    game.cover = reader[2].ToString();
                    game.general_intro = reader[3].ToString();
                    game.is_dlc = int.Parse(reader[4].ToString());
                    game.discount = double.Parse(reader[5].ToString());
                    game.price = double.Parse(reader[6].ToString());
                    game.rating = reader[7].ToString();
                    game.like_num = int.Parse(reader[7].ToString());
                    game.dislike_num = int.Parse(reader[8].ToString());
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
        public static int GetPublisherGames(string publisher_id, int page, List<Game> game_list, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "SELECT GAMEID FROM GAME WHERE PUBLISHER_ID = '"+ publisher_id + "'";
            OracleDataReader reader = cmd.ExecuteReader();
            List<Game> games = new List<Game>();
            List<string>games_id = new List<string>();
            Game game;
            try
            {
                //查找成功，赋值变量
                while (reader.Read())
                {
                    games_id.Add(reader[0].ToString());
                }
            }
            catch (Exception e)
            {
                reason = e.Message;
                return -1;
            }
            for(int i = 0; i<games_id.Count; i++)
            {
                game = new Game();
                if(Game.GetGameInfo(games_id[i], game, out reason))
                {
                    return -1;
                }
                games.Add(game);
            }
            reason = "查找成功";
            return games_id.Count;
        }
        public static bool SavePublishGameRequest(Game game, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();

            int cen = 0;
            cmd.CommandText = "INSERT INTO GAME VALUES('" + game.id + "'," + game.is_dlc + ",to_date('" + game.publish_date + "','yyyy-mm-dd')," + game.price + "," + game.size + ",'" + game.general_intro + "','" + game.intro_photo + "','" + game.specific_intro + "'," + game.level + ",'" + game.cover + "',0,0,0,'" + ",)";
            try
            {
                cen = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                cmd.CommandText = "ROLLBACK";
                cen = cmd.ExecuteNonQuery();
                reason = e.Message.ToString();
                return false;
            }
            if (cen == 0)
            {
                cmd.CommandText = "ROLLBACK";
                cen = cmd.ExecuteNonQuery();
                reason = "添加新游戏失败";
                return false;
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
            if (game.is_dlc == 1)//添加dlc从属关系
            {
                cmd.CommandText = "INSERT INTO SUBJECT_DLC VALUES('" + game.source_id + "','" + game.id + "')";
                try
                {
                    cen = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    reason = e.Message.ToString();
                    return false;
                }
                if (cen == 0)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = "添加dlc从属关系失败";
                    return false;
                }
                else
                {
                    try
                    {
                        cmd.CommandText = "COMMIT";
                        cen = cmd.ExecuteNonQuery();
                        reason = "添加成功";
                        return false;
                    }
                    catch (Exception e)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = e.Message.ToString();
                        return false;
                    }
                }
            }//end of if(hreq.Form["isDLC"] == 1)
            return true;
        }
        public static bool SaveUpdateGameinfoRequest(Game game, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();

            int cen = 0;
            cmd.CommandText = "UPDATE GAME VALUES('" + game.id + "'," + game.is_dlc + ",to_date('" + game.publish_date + "','yyyy-mm-dd')," + game.price + "," + game.size + ",'" + game.general_intro + "','" + game.intro_photo + "','" + game.specific_intro + "'," + game.level + ",'" + game.cover + "',0,0,0,'" + ",)";
            try
            {
                cen = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                cmd.CommandText = "ROLLBACK";
                cen = cmd.ExecuteNonQuery();
                reason = e.Message.ToString();
                return false;
            }
            if (cen == 0)
            {
                cmd.CommandText = "ROLLBACK";
                cen = cmd.ExecuteNonQuery();
                reason = "更新游戏失败";
                return false;
            }
            else
            {
                try
                {
                    cmd.CommandText = "COMMIT";
                    cen = cmd.ExecuteNonQuery();
                    reason = "更新成功";
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
        public static bool OffSaleGame(string game_id, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();

            int cen = 0;
            cmd.CommandText = "UPDATE GAME VALUES ISOFFSALE = 1 WHERE GAMEID = '"+ game_id +"'" ;
            try
            {
                cen = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                cmd.CommandText = "ROLLBACK";
                cen = cmd.ExecuteNonQuery();
                reason = e.Message.ToString();
                return false;
            }
            if (cen == 0)
            {
                cmd.CommandText = "ROLLBACK";
                cen = cmd.ExecuteNonQuery();
                reason = "下架游戏失败";
                return false;
            }
            else
            {
                try
                {
                    cmd.CommandText = "COMMIT";
                    cen = cmd.ExecuteNonQuery();
                    reason = "下架成功";
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
        public static int SearchGame(int page, int sort_opt, int filter_opt, string key, List<string> result, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand Ocmd = DBHelper.con.CreateCommand();
            OracleDataReader reader;

            int GAME_NUM_PER_PAGE = 10;
            string rank = "";
            switch (sort_opt)
            {
                case 0:
                    rank = "FUN_GET_GAME_PRICE(ID) ASC";
                    break;
                case 1:
                    rank = "TOT_DEAL_NUM DESC";
                    break;
                case 2:
                    rank = "PUBLISH_DATE DESC";
                    break;
                default:
                    break;
            }

            string cmd = $"SELECT ID, ROW_NUMBER() OVER (ORDER BY {rank}) R FROM GAME WHERE ";

            switch (filter_opt)
            {
                case 0:
                    cmd += "AND PRICE=FUN_GET_GAME_PRICE(id) ";
                    break;
                case 1:
                    cmd += "AND PRICE<>FUN_GET_GAME_PRICE(id) ";
                    break;
                case 2:
                    cmd += "AND IS_DLC=0 ";
                    break;
                case 3:
                    cmd += "AND IS_DLC=1 ";
                    break;
                default:
                    break;
            }

            string command1 = $"SELECT ID FROM ({cmd}) WHERE R BETWEEN {GAME_NUM_PER_PAGE * (page - 1) + 1} AND {GAME_NUM_PER_PAGE * page} ";
            string command2 = $"SELECT CEIL(COUNT(ID)/{GAME_NUM_PER_PAGE}) FROM ({cmd})";
            string[] commands = { command1, command2 };

            try
            {

                /* execute command1: get gane_ids corresponds */
                Ocmd.CommandText = commands[0];
                reader = Ocmd.ExecuteReader();
                while (reader.Read())
                    result.Add(reader[0].ToString());

                /* execute command2: get number of pages */
                Ocmd.CommandText = commands[1];
                reader = Ocmd.ExecuteReader();

                reason = "搜索成功";
            }
            catch (Exception e)
            {
                reason = e.Message.ToString();
                return -1;
            }

            return result.Count;
        }
        public static bool UploadGameResource(out string game_id, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "SELECT ID FROM GAME WHERE NAME = 'testNext'";
            OracleDataReader reader = cmd.ExecuteReader();
            if (!reader.HasRows)
            {
                reason = "无法查询到下一个game_id";
                game_id = "";
                return false;
            }
            else
            {
                reader.Read();
                string NextID = reader[0].ToString();
                int outid = System.Convert.ToInt32(NextID);
                outid += 1;
                string AddID = outid.ToString("0000000000");

                int cen = 0;
                cmd.CommandText = "UPDATE GAME SET ID = '" + AddID + "' WHERE NAME = 'testNext'";
                cen = cmd.ExecuteNonQuery();
                if (cen == 0)
                {
                    cmd.CommandText = "ROLLBACK";
                    cen = cmd.ExecuteNonQuery();
                    reason = "修改GAME记录ID失败";
                    game_id = "";
                    return false;
                }
                else
                {
                    try
                    {
                        cmd.CommandText = "COMMIT";
                        cen = cmd.ExecuteNonQuery();
                        game_id = NextID;
                        reason = "获取成功";
                    }
                    catch (Exception e)
                    {
                        cmd.CommandText = "ROLLBACK";
                        cen = cmd.ExecuteNonQuery();
                        reason = e.Message.ToString();
                        game_id = "";
                        return false;
                    }
                }
                return true;
            }
        }
        public static int GetGameDLC(string game_id, List<string> game_list, out string reason)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();
            cmd.CommandText = "SELECT ID FORM GAME WHERE SOURCEID = '"+ game_id;
            OracleDataReader reader = cmd.ExecuteReader();
            try
            {
                //查找成功，赋值变量
                while (reader.Read())
                {
                    game_list.Add(reader[0].ToString());
                }
                reason = "查找成功";
            }
            catch (Exception e)
            {
                reason = e.Message;
                return -1;
            }
            return game_list.Count;
        }
        public static int CDKExchange(string uid, string cdk, string result)
        {
            DBHelper.isOpened();
            OracleCommand cmd = DBHelper.con.CreateCommand();

            OracleDataReader reader;
            string gid = "";        //game id corresponds to the cdk
            cmd.CommandText = $"SELECT HAVE_USED, GAME_ID FROM CDKS WHERE CDK_VALUE='{cdk}'";
            try
            {
                reader = cmd.ExecuteReader();
                if (!reader.HasRows)        //cdk not exists
                {
                    result = "CDK不存在";
                    return 0;
                }

                if (reader.Read())
                {
                    int used = int.Parse(reader[0].ToString());

                    //check if cdk is already used 
                    if (used == 1)    
                    {
                        result = "CDK已被使用";
                        return -1;
                    }

                    /* check whether user already owns the game */
                    gid = reader[1].ToString();
                    int has_game = GameLib.GameInLib(uid, gid, result);
                    if (has_game == 1)
                    {
                        result = "玩家已拥有该游戏";
                        return 2;
                    }
                    /* set cdk invalid and add game to user lib */
                    cmd.CommandText = $"UPDATE CDKS SET HAVE_USED=1 WHERE CDK_VALUE='{cdk}'";
                    cmd.ExecuteNonQuery();
                    if (Order.CreateOrder(uid, gid, 0, 0, uid) == 1 && GameLib.AddGame2Lib(gid, uid, result) == 1)
                        result = "兑换成功";
                    else
                        result = "兑换失败";
                }
            }
            catch (Exception e)
            {
                result = e.Message;
                return -1;
            }

            return 1;
        }
    }
}
