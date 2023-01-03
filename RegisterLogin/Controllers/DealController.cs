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
    public class DealController : Controller
    {
        public class CDKRequest
        {
            public string uid;
            public string cdk;
        }
        [HttpPost]
        [Route("api/CDK/CDKExchange")]
        public Dictionary<string, dynamic> CDKExchangeController([FromBody] CDKRequest req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            List<string> games = new List<string>();
            int ret = Game.CDKExchange(req.uid, req.cdk, result);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }

        [HttpPost]
        [Route("api/Order/CreateOrder")]
        public Dictionary<string, dynamic> CreateOrder([FromBody] Order req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            int ret = Order.CreateOrder(req.uid, req.gid, req.method, req.amount, req.rid);
            resp.Add("result", ret);

            return resp;
        }

        [HttpPost]
        [Route("api/Order/GetOrder")]
        public Dictionary<string, dynamic> GetOrder([FromBody] Order req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            List<Order> order_list = new List<Order>();
            int ret = Order.GetOrder(req.uid, order_list, result);
            resp.Add("order_list", order_list);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }

        public class AddGameRequest
        {
            public string uid;
            public string gid;
        }
        [HttpPost]
        [Route("api/Cart/AddGame")]
        public Dictionary<string, dynamic> AddGame([FromBody] AddGameRequest req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            int ret = Cart.AddGame(req.uid, req.gid);
            resp.Add("result", ret);

            return resp;
        }

        public class DeleteGameRequest
        {
            public string uid;
            public string gid;
        }
        [HttpPost]
        [Route("api/Cart/DeleteGame")]
        public Dictionary<string, dynamic> DeleteGame([FromBody] DeleteGameRequest req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            int ret = Cart.DeleteGame(req.uid, req.gid);
            resp.Add("result", ret);

            return resp;
        }

        public class BuyGameRequest
        {
            public string uid;
            public string uid1;
            public List<string> choose_gid_list;
            public string gid;
        }
        [HttpPost]
        [Route("api/Cart/BuyGame")]
        public Dictionary<string, dynamic> BuyGame([FromBody] BuyGameRequest req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = Cart.BuyGame(req.uid, req.uid1,req.choose_gid_list, result);
            resp.Add("result", ret);
            resp.Add("reason", result);

            return resp;
        }

        [HttpPost]
        [Route("api/Cart/GetCart")]
        public Dictionary<string, dynamic> GetCart([FromBody] Cart req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();

            int ret = Cart.GetCart(req.uid, req.cart);
            resp.Add("result", ret);

            return resp;
        }
    }
}
