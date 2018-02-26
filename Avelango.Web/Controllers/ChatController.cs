using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Avelango.Handlers.Image;
using Avelango.Hubs.Accessory;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.User;
using Avelango.Web.Models;
using Avelango.Web.Models.Attributes;

namespace Avelango.Web.Controllers
{
    [AccessLevelAnyAutorized]
    public class ChatController : Controller
    {
        private readonly IChats _chats;
        private readonly IChatMessages _message;

        public ChatController(IChatMessages message, IChats chats)
        {
            _message = message;
            _chats = chats;
        }


        // /Chat/GetMyChats
        public ActionResult GetMyChats() {
            var myChatsResult = _chats.GetMyChats(new PrivateSession().Current.User.Pk);
            if (!myChatsResult.IsSuccess) return Json(new { IsSuccess = false });
            return Json(new { IsSuccess = true, Chats = myChatsResult.Data });
        }


        // /Chat/GetChatMessages
        public ActionResult GetChatMessages(string chatPk) {
            Guid chatPkGuid;
            Guid.TryParse(chatPk, out chatPkGuid);
            var messagesResult = _message.GetChatMessages(chatPkGuid);
            if (!messagesResult.IsSuccess) return Json(new { IsSuccess = false });

            var chatMessages = new List<object>();

            foreach (var message in messagesResult.Data) {
                chatMessages.Add(new {
                    IsNew = message.IsNew,
                    AttachmentMin = message.AttachmentMin,
                    AttachmentMax = message.AttachmentMax,
                    Message = message.Message,
                    Created = message.Created,
                    MyOwn = message.Owner == new PrivateSession().Current.User.Pk,
                });
            }
            return Json(new { IsSuccess = true, Messages = chatMessages });
        }


        // /Chat/SendMessage
        public ActionResult SendMessage(string chatPk, string collocutorPk, string text, HttpPostedFileBase file) {

            var serializer = new JavaScriptSerializer();
            var chatPkString = string.IsNullOrEmpty(chatPk) ? string.Empty : serializer.Deserialize<string>(chatPk);
            var textString = string.IsNullOrEmpty(text) ? string.Empty : serializer.Deserialize<string>(text);
            var collocutorPkString = string.IsNullOrEmpty(collocutorPk) ? string.Empty : serializer.Deserialize<string>(collocutorPk);

            ImagePair imagePair = null;
            if (file != null) {
                imagePair = SaveFile(file);
                if (imagePair == null) return Json(new { IsSuccess = false });
            }

            Guid chatPkGuid;
            Guid.TryParse(chatPkString, out chatPkGuid);
            var saveResult = _message.SaveMessage(chatPkGuid, textString, imagePair);
            if (!saveResult.IsSuccess) return Json(new {IsSuccess = false});

            HubClient.SendChatMessage(collocutorPkString, textString, imagePair?.Small, imagePair?.Large);
            return Json(new { IsSuccess = true });
        }


        private ImagePair SaveFile(HttpPostedFileBase file) {
            ImagePair imagePair = null;

            var imgMax = Image.FromStream(file.InputStream);
            var attachMaxPath = @"\Storage\Chat\" + Guid.NewGuid() + ".png";
            var resMax = ImgHandler.SaveImage(imgMax, Server.MapPath("~") + attachMaxPath);

            var imgMini = ImgHandler.CreateMiniImage(imgMax, 128, 128);
            var attachMinPath = @"\Storage\Chat\" + Guid.NewGuid() + ".png";
            var resMin = ImgHandler.SaveImage(imgMini, Server.MapPath("~") + attachMinPath);

            if (resMax && resMin) imagePair = new ImagePair { Small = attachMinPath, Large = attachMaxPath };
            return imagePair;
        }
    }
}