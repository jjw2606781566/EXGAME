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
    }
}
