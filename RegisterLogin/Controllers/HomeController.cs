using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BusinessClass.Comment;
using System.Collections.Generic;
using System.Collections;

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
        public Dictionary<string, dynamic> Post([FromBody] GetCommentRequest req)
        {
            Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
            int page = 0;
            string reason;
            List<Comment> comment_list = new List<Comment>();
            int CommentNum = Comment.GetComment(req.game_id, out page, comment_list, out reason);
            result.Add("comment_list", comment_list);
            result.Add("page", page);
            result.Add("CommentNum", CommentNum);
            result.Add("reason", reason);
            return result;
        }
    }

}
