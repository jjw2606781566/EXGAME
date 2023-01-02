using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BusinessClass.Comment;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Controllers.GameInfoController
{
    [ApiController]
    public class GameInfoController : Controller
    {
        public class GetCommentRequest
        {
            public string game_id { get; set; }
        }
        [HttpPost]
        [Route("api/GameInfoSubsys/GetComment")]
        public Dictionary<string, dynamic> GetCommentController([FromBody] GetCommentRequest req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            int page = 0;
            string reason;
            List<Comment> comment_list = new List<Comment>();
            int CommentNum = Comment.GetComment(req.game_id, out page, comment_list, out reason);
            resp.Add("comment_list", comment_list);
            resp.Add("page", page);
            resp.Add("CommentNum", CommentNum);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/GameInfoSubsys/PublishComment")]
        public Dictionary<string, dynamic> PublishCommentController([FromBody] Comment req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason;
            bool result = Comment.PublishComment(req, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
    }

}
