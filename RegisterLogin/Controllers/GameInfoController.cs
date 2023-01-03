using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using Microsoft.AspNetCore.Mvc.RazorPages;
using exgame.BusinessClass;
using System.Drawing.Printing;

namespace exgame.Controllers
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

        [HttpPost]
        [Route("api/gamedetail/getGameIntro")]
        public Dictionary<string, dynamic> GetGameProfile([FromBody] Game req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string result = "";
            Game game = new Game();
            int ret = Game.GetGameBriefIntro(req.id, game.name, game.publisher_id, game.is_dlc, game.cover, game.general_intro, result);
            resp.Add("name", game.name);
            resp.Add("publisher_id", game.publisher_id);
            resp.Add("is_dlc", game.is_dlc);
            resp.Add("cover", game.cover);
            resp.Add("genral_intro", game.general_intro);
            resp.Add("result", ret);
            resp.Add("reason", result);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/GetGameInfo")]
        public Dictionary<string, dynamic> GetGameInfoController([FromBody] Game req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            Game game = new Game();
            bool result = Game.GetGameInfo(req.id, game, out reason);
            resp.Add("game", game);
            resp.Add("result", result);
            return resp;
        }
        public class GetPublisherGamesRequest
        {
            public string publisher_id;
            public int page;
        }
        [HttpPost]
        [Route("api/gamedetail/GetPublisherGames")]
        public Dictionary<string, dynamic> GetPublisherGamesController([FromBody] GetPublisherGamesRequest req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            List<Game> games = new List<Game>();
            int result = Game.GetPublisherGames(req.publisher_id, req.page, games, out reason);
            resp.Add("game_list", games);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/SavePublishGameRequest")]
        public Dictionary<string, dynamic> SavePublishGameRequestController([FromBody] Game req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            bool result = Game.SavePublishGameRequest(req, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/SaveUpdateGameinfoRequest")]
        public Dictionary<string, dynamic> SaveUpdateGameinfoRequestController([FromBody] Game req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            bool result = Game.SaveUpdateGameinfoRequest(req, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/OffSaleGame")]
        public Dictionary<string, dynamic> OffSaleGameController([FromBody] Game req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            bool result = Game.OffSaleGame(req.id, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        public class SearchGameRequest
        {
            public int page;
            public int sort_opt;
            public int filter_opt;
            public string key;
        }
        [HttpPost]
        [Route("api/gamedetail/SearchGame")]
        public Dictionary<string, dynamic> SearchGameController([FromBody] SearchGameRequest req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            List<string> games = new List<string>();
            int result = Game.SearchGame(req.page, req.sort_opt, req.filter_opt, req.key, games, out reason);
            resp.Add("games", games);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/UploadGameResource")]
        public Dictionary<string, dynamic> UploadGameResourceController([FromBody] Game req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            string NextID;
            bool result = Game.UploadGameResource(out NextID, out reason);
            resp.Add("NextID", NextID);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/GetGameDLC")]
        public Dictionary<string, dynamic> GetGameDLCController([FromBody] Game req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            List<string> games = new List<string>();
            int result = Game.GetGameDLC(req.id, games, out reason);
            resp.Add("games", games);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/GetPostInfo")]
        public Dictionary<string, dynamic> GetPostInfoController([FromBody] Post req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            Post post = new Post();
            bool result = Post.GetPostInfo(req.game_id, req.post_id, post, out reason);
            resp.Add("post", post);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/GetGamePosts")]
        public Dictionary<string, dynamic> GetGamePostsController([FromBody] Post req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            List<Post> posts = new List<Post>();
            int result = Post.GetGamePosts(req.game_id, posts, out reason);
            resp.Add("posts", posts);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/SavePublishPostRequest")]
        public Dictionary<string, dynamic> SavePublishPostRequestController([FromBody] Post req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            List<Post> posts = new List<Post>();
            bool result = Post.SavePublishPostRequest(req, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
        [HttpPost]
        [Route("api/gamedetail/DeletePost")]
        public Dictionary<string, dynamic> DeletePostController([FromBody] Post req)
        {
            Dictionary<string, dynamic> resp = new Dictionary<string, dynamic>();
            string reason = "";
            bool result = Post.DeletePost(req.game_id, req.post_id, out reason);
            resp.Add("result", result);
            resp.Add("reason", reason);
            return resp;
        }
    }
}
