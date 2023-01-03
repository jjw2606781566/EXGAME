using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using Microsoft.AspNetCore.Mvc.RazorPages;
using exgame.BusinessClass;

namespace exgame.Controllers
{
    [ApiController]
    public class GameLibController : Controller
    {
        [HttpPost]
        [Route("api/user/getUserGameInfo")]
        public Dictionary<string, dynamic> GetPlayerLibController([FromBody] Player req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            List<string> games = new List<string>();
            int ret = GameLib.GetPlayerGameLib(req.id, games, result);
            resp.Add("games", games);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }

        public class Add2LibRequest
        {
            public string user_id;
            public string game_id;
        }
        [HttpPost]
        [Route("api/shopingcart/addToLibrary")]
        public Dictionary<string, dynamic> AddGame2Lib([FromBody] Add2LibRequest req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = GameLib.AddGame2Lib(req.user_id, req.game_id, result);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }               
    }
}
