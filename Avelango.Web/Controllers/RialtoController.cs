using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Avelango.Hubs.Accessory;
using Avelango.Models;
using Avelango.Models.Abstractions.Db;
using Avelango.Web.Models;
using Avelango.Web.Models.Attributes;

namespace Avelango.Web.Controllers
{
    public class RialtoController : Controller
    {
        private readonly IRialtos _rialtos;


        public RialtoController(IRialtos rialtos) {
            _rialtos = rialtos;
        }


        // GET: Rialto
        [AccessLevelAnyAutorized]
        public ActionResult Index() {
            _rialtos.AddUserIfNotExist(new PrivateSession().Current.User.Pk);
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult GetActualData() {
            var userPk = new PrivateSession().Current.User.Pk;
            var rialtoData = _rialtos.GetCommonData(userPk);
            var chartData = _rialtos.GetChartData();
            var champion = _rialtos.GetChampionName();
            var chatMessages = _rialtos.GetChatMessages();
            return Json(new { IsSuccess = true, RialtoData = rialtoData, ChartData = chartData, Champion = champion, ChatMessage = chatMessages });
        }


        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult Bid(double data) {
            if (Math.Abs(data) < 0.001) return Json(new { IsSuccess = false });
            var bidRes = _rialtos.Bid(new PrivateSession().Current.User.Pk, data);
            HubClient.AveRateChanges(new JavaScriptSerializer().Serialize(new { AveRate = bidRes.Item1 }));
            return Json(new { IsSuccess = true, Assets = bidRes.Item2 });
        }



        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult CloseBid(Guid bidId) {
            var aveRate = _rialtos.CloseBid(new PrivateSession().Current.User.Pk, bidId);
            HubClient.AveRateChanges(new JavaScriptSerializer().Serialize(new { AveRate = aveRate }));
            return Json(new { IsSuccess = true });
        }


        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult ChatMessage(string message)
        {
            var chatMessage = _rialtos.AddChatMessage(new PrivateSession().Current.User.Name, message);

            HubClient.RialtoChatMessage(new JavaScriptSerializer().Serialize(new { Message = chatMessage.Message, SenderName = chatMessage.SenderName }));
            return Json(new { IsSuccess = true });
        }
    }
}