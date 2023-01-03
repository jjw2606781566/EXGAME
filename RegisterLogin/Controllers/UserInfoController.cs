using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using exgame.BusinessClass;

namespace exgame.Controllers
{
    [ApiController]
    public class UserInfoController : Controller
    {
        [HttpPost]
        [Route("api/user/login")]
        public Dictionary<string, dynamic> LoginController([FromBody] ExUser req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = ExUser.Login(req.email, req.password, result);
            resp.Add("result", ret);
            resp.Add("name", req.name);
            resp.Add("id", req.id);

            return resp;
        }

        [HttpPost]
        [Route("api/user/register")]
        public Dictionary<string, dynamic> PlayerRegisterController([FromBody] Player req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            string time = "";
            string area = "";
            int ret = Player.Register(req.email, req.name, req.password, time, area,result);
            resp.Add("result", ret);
            return resp;
        }

        [HttpPost]
        [Route("api/publisher/register")]
        public Dictionary<string, dynamic> PublisherRegisterController([FromBody] Publisher req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            string time = "";
            string area = "";
            int ret = Publisher.Register(req.name, req.email, req.telephone, req.password, time, area, result);
            resp.Add("result", ret);
            return resp;
        }

        [HttpPost]
        [Route("api/user/getUserInfo")]
        public Dictionary<string, dynamic> GetPlayerInfoController([FromBody] Player req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "", birthday = "";
            int status = 0, game_num = 0;
            int ret = Player.getPlayerInfo(req.id, req.name, req.email, req.password, req.area, status, birthday, game_num, req.intro, req.profile_photo, result);
            resp.Add("result", ret);
            resp.Add("result", req.name);
            resp.Add("result", req.email);
            resp.Add("result", req.password);
            resp.Add("result", req.area);
            resp.Add("result", status);
            resp.Add("result", birthday);
            resp.Add("result", game_num);
            resp.Add("result", req.intro);
            resp.Add("result", req.profile_photo);
            return resp;
        }

        [HttpPost]
        [Route("api/publisher/getPublisherInfo")]
        public Dictionary<string, dynamic> GetPublisherInfoController([FromBody] Publisher req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = Publisher.getPublisherInfo(req.id, req.name, req.email, req.password, req.telephone, req.area, req.register_time, req.brief_info, req.logo, result);
            resp.Add("result", ret);
            resp.Add("result", req.name);
            resp.Add("result", req.email);
            resp.Add("result", req.password);
            resp.Add("result", req.area);
            resp.Add("result", req.telephone);
            resp.Add("result", req.register_time);
            resp.Add("result", req.brief_info);
            resp.Add("result", req.logo);
            return resp;
        }

        [HttpPost]
        [Route("api/user/editProfile")]
        public Dictionary<string, dynamic> UpdatePlayerInfoController([FromBody] Player req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "", time = "", area = "",birthday="";
            int status = 0, game_num = 0;
            int ret = Player.UpdatePlayerInfo(req.id, req.name, req.password, area, birthday, req.intro, result);
            resp.Add("result", ret);
            return resp;
        }

        [HttpPost]
        [Route("api/publisher/editProfile")]
        public Dictionary<string, dynamic> UpdatePublisherInfoController([FromBody] Publisher req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "", time = "", area = "", birthday = "";
            int status = 0, game_num = 0;
            int ret = Publisher.UpdatePublisherInfo(req.id, req.name, req.password, req.telephone, area, req.brief_info, result);
            resp.Add("result", ret);
            return resp;
        }

        [HttpPost]
        [Route("api/user/editEmail")]
        public Dictionary<string, dynamic> UpdatePlayerEmailController([FromBody] Player req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = Player.UpdateEmail(req.id, req.email, req.password, result);
            resp.Add("result", ret);
            return resp;
        }

        [HttpPost]
        [Route("api/publisher/editEmail")]
        public Dictionary<string, dynamic> UpdatePublisherEmailController([FromBody] Publisher req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = Publisher.ApplyUpdateEmail(req.id, req.email, req.password, result);
            resp.Add("result", ret);
            return resp;
        }

        [HttpPost]
        [Route("api/user/editAvatar")]
        public Dictionary<string, dynamic> UpdatePlayerAvatarController([FromBody] Player req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = Player.UpdateAvatar(req.id, req.profile_photo, result);
            resp.Add("result", ret);
            return resp;
        }

        [HttpPost]
        [Route("api/publisher/editAvatar")]
        public Dictionary<string, dynamic> UpdatePublisherAvatarController([FromBody] Publisher req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            int ret = Publisher.ApplyUpdateLogo(req.id, req.logo, result);
            resp.Add("result", ret);
            return resp;
        }
    }
}
