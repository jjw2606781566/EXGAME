using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exgame.BusinessClass;

namespace exgame.Controllers
{
    [ApiController]
    public class CommunityController : Controller
    {
        [HttpPost]
        [Route("api/column/getGameColumns")]
        public Dictionary<string, dynamic> GetColumns([FromBody] Column column)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = Column.GetColumnInfo(column.id, column, result);
            resp.Add("column", column);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }

        [HttpPost]
        [Route("api/column/deletUserColumn")]
        public Dictionary<string, dynamic> DeleteColumns([FromBody] Column column)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = Column.DeleteColumn(column.id, result);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }


        [HttpPost]
        [Route("api/column/uploadColumnContent")]
        public Dictionary<string, dynamic> CreateColumns([FromBody] Column column)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = Column.CreateColumn(column.player.id, column.game.id, column.title, column.content, column.time, result);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }

        [HttpPost]
        [Route("api/column/getUserColumn")]
        public Dictionary<string, dynamic> GetUserColumns([FromBody] Column column)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            List<string> column_list = new List<string>();
            int ret = Column.GetPlayerColumns(column.player.id, column_list, result);
            resp.Add("column_list", column_list);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }

        [HttpPost]
        [Route("api/column/getGameColumn")]
        public Dictionary<string, dynamic> GetGameColumns([FromBody] Column column)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            List<string> column_list = new List<string>();
            int ret = Column.GetPlayerColumns(column.game.id, column_list, result);
            resp.Add("column_list", column_list);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }

        [HttpPost]
        [Route("api/column/replyColumn")]
        public Dictionary<string, dynamic> ReplyColumn([FromBody] Reply reply)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            List<string> column_list = new List<string>();
            int ret = Reply.PublishReply(reply.player.id, reply.column_id, reply.content, reply.time, result);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }

        [HttpPost]
        [Route("api/column/deleteReply")]
        public Dictionary<string, dynamic> DeleteReply([FromBody] Reply reply)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            List<string> column_list = new List<string>();
            int ret = Reply.DeleteReply(reply.player.id, reply.column_id, result);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }
    }
}
