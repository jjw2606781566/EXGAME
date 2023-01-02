using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BusinessClass.Comment;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessClass.Discount;

namespace Controllers.GameInfoController
{
    [ApiController]
    public class GameInfoController : Controller
    {
        [HttpPost]
        [Route("api/GameInfoSubsys/GetComment")]
        public Dictionary<string, dynamic> GetCommentController([FromBody] Comment req)
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
        [HttpPost]
        [Route("api/GameInfoSubsys/DeleteComment")]
        public Dictionary<string, dynamic> DeleteCommentController([FromBody] Comment req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason;
            bool result = Comment.DeleteComment(req.user_id, req.game_id, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        public class EvaluateCommentRequest
        {
            public string user_id;
            public string game_id;
            public int evaluation;
        }
        [HttpPost]
        [Route("api/GameInfoSubsys/EvaluateComment")]
        public Dictionary<string, dynamic> EvaluateCommentController([FromBody] EvaluateCommentRequest req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason;
            bool result = Comment.EvaluateComment(req.user_id, req.game_id, req.evaluation, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/GameInfoSubsys/GetGameDiscount")]
        public Dictionary<string, dynamic> GetGameDiscountController([FromBody] Discount req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason;
            Discount discount = new Discount();
            bool result = Discount.GetGameDiscount(req.game_id, discount, out reason);
            resp.Add("discount", discount);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/GameInfoSubsys/SavePublishDiscountRequest")]
        public Dictionary<string, dynamic> SavePublishDiscountRequestController([FromBody] Discount req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason;
            bool result = Discount.SavePublishDiscountRequest(req, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/GameInfoSubsys/DeleteDiscount")]
        public Dictionary<string, dynamic> DeleteDiscountController([FromBody] Discount req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason;
            bool result = Discount.DeleteDiscount(req.game_id, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
    }

}
