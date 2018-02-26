using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Resources;
using Avelango.Models.Accessory;
using Avelango.Models.Enums;

namespace Avelango.Handlers.Email
{
    public class MailSender {

        private readonly SmtpClient _smtpClient = new SmtpClient {
            UseDefaultCredentials = true,
            Credentials = new NetworkCredential(Resources.Email.Connection.Login, Resources.Email.Connection.Password),
            Host = Resources.Email.Connection.Smtp,
            Port = 587
        };


        private string GetNameOf<T>(Expression<Func<T>> property) {
            var memberExpression = property.Body as MemberExpression;
            return memberExpression?.Member.Name ?? string.Empty;
        }


        /// <summary>
        /// Send MAIL
        /// </summary>
        /// <param name="recipientAddress"></param>
        /// <param name="recipientName"></param>
        /// <param name="senderAddress"></param>
        /// <param name="actionName"></param>
        /// <param name="mailType"></param>
        /// <param name="lang"></param>
        /// <param name="dataToReplace"></param>
        public OperationResult<bool> Send(string recipientAddress, string recipientName, string senderAddress, string actionName,
                                                       MailTypes mailType, Langs.LangsEnum lang, Dictionary<string, string> dataToReplace) {
            try {
                var body = GetEmailBody(mailType);

                var from = new MailAddress(senderAddress, Resources.Email.Connection.FromName);
                var to = new MailAddress(recipientAddress, recipientName);
                var message = new MailMessage(from, to) {
                    Subject = actionName,
                    Body = body,
                    IsBodyHtml = true
                };

                body = FullFillBodyContent(body, lang, dataToReplace);
                message = AddImagesToBody(message, body, mailType);

                // Send msg
                _smtpClient.SendMailAsync(message);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        private string GetEmailBody(MailTypes mailType) {
            string body;
            switch (mailType) {
                case MailTypes.Confirm: { body = Resources.Email.Bodies.EmailConfirm; break; }
                case MailTypes.Deactivation: { body = Resources.Email.Bodies.EmailDeactAccount; break; }
                case MailTypes.Delete: { body = Resources.Email.Bodies.EmailDeleteAccount; break; }
                case MailTypes.NewTask: { body = Resources.Email.Bodies.EmailNewTask; break; }
                case MailTypes.News: { body = Resources.Email.Bodies.EmailNews; break; }
                case MailTypes.PasswordRecovery: { body = Resources.Email.Bodies.EmailPasswordRecovery; break; }
                case MailTypes.TaskResponse: { body = Resources.Email.Bodies.EmailTaskResponse; break; }
                case MailTypes.Warning: { body = Resources.Email.Bodies.EmailWarning; break; }
                default: throw new ArgumentOutOfRangeException(nameof(mailType), mailType, null);
            }
            return body;
        }


        private string FullFillBodyContent(string body, Langs.LangsEnum lang, Dictionary<string, string> dataToReplace) {
            switch (lang) {
                case Langs.LangsEnum.Ru: { body = ReplaceContent(body, Resources.Email.Ru.Content.ResourceManager, dataToReplace); break; }
                case Langs.LangsEnum.De: { body = ReplaceContent(body, Resources.Email.De.Content.ResourceManager, dataToReplace); break; }
                case Langs.LangsEnum.Fr: { body = ReplaceContent(body, Resources.Email.Fr.Content.ResourceManager, dataToReplace); break; }
                case Langs.LangsEnum.Es: { body = ReplaceContent(body, Resources.Email.Es.Content.ResourceManager, dataToReplace); break; }
                case Langs.LangsEnum.Ua: { body = ReplaceContent(body, Resources.Email.Ua.Content.ResourceManager, dataToReplace); break; }
                case Langs.LangsEnum.En: { body = ReplaceContent(body, Resources.Email.En.Content.ResourceManager, dataToReplace); break; }
                default: throw new ArgumentOutOfRangeException(nameof(lang));
            }
            return body;
        }


        private string ReplaceContent(string body, ResourceManager resource, Dictionary<string, string> dataToReplace) {
            var resourceSet = resource.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            var resourceDictionary = resourceSet.Cast<DictionaryEntry>().ToDictionary(r => r.Key.ToString(), r => r.Value.ToString());
            if (dataToReplace.Any()) {
                foreach (var i4 in dataToReplace) {
                    resourceDictionary.Add(i4.Key, i4.Value);
                }
            }
            foreach (var i4 in GetDefaultContent()) {
                resourceDictionary.Add(i4.Key, i4.Value);
            }
            foreach (var i4 in resourceDictionary) {
                var marker = "{" + i4.Key + "}";
                if (body.Contains(marker)) {
                    body = body.Replace(marker, i4.Value);
                }
            }
            return body;
        }


        private Dictionary<string, string> GetDefaultContent() {
            var resourceSet = Resources.Email.Common.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            return resourceSet.Cast<DictionaryEntry>().ToDictionary(r => r.Key.ToString(), r => r.Value.ToString());
        }


        private MailMessage AddImagesToBody(MailMessage message, string body, MailTypes mailType) {
            var imagesBodies = GetImagesBodies(mailType);

            // Add Alternate Views
            foreach (var imageBody in imagesBodies) {
                var linkedResource = new LinkedResource(new MemoryStream(imageBody.Value), MediaTypeNames.Image.Jpeg) {
                    ContentId = imageBody.Key,
                    TransferEncoding = TransferEncoding.Base64,
                };
                var av = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                av.LinkedResources.Add(linkedResource);
                message.AlternateViews.Add(av);
            }
            return message;
        }


        private Dictionary<string, byte[]> GetImagesBodies(MailTypes mailType)
        {
            var result = new Dictionary<string, byte[]>();
                // {{ GetNameOf(() => Resources.Email.Image.ImgFacebook), Convert.FromBase64String(Resources.Email.Image.ImgFacebook)},
                // { GetNameOf(() => Resources.Email.Image.ImgVK), Convert.FromBase64String(Resources.Email.Image.ImgVK)},
                // { GetNameOf(() => Resources.Email.Image.ImgTwitter), Convert.FromBase64String(Resources.Email.Image.ImgTwitter)},
                // { GetNameOf(() => Resources.Email.Image.ImgGoogle), Convert.FromBase64String(Resources.Email.Image.ImgGoogle)},
                // { GetNameOf(() => Resources.Email.Image.ImgLinkedin), Convert.FromBase64String(Resources.Email.Image.ImgLinkedin)},
                // { GetNameOf(() => Resources.Email.Image.ImgInstagram), Convert.FromBase64String(Resources.Email.Image.ImgInstagram)}};

            switch (mailType) {
                case MailTypes.Confirm: {
                        result.Add(GetNameOf(() => Resources.Email.Image.ImgLogoBlue), Convert.FromBase64String(Resources.Email.Image.ImgLogoBlue));
                        //result.Add(GetNameOf(() => Resources.Email.Image.ImgProfit), Convert.FromBase64String(Resources.Email.Image.ImgProfit));
                        break;
                    }
                case MailTypes.Deactivation: {
                        result.Add(GetNameOf(() => Resources.Email.Image.ImgLogoPink), Convert.FromBase64String(Resources.Email.Image.ImgLogoPink));
                        break;
                    }
                case MailTypes.Delete: {
                        result.Add(GetNameOf(() => Resources.Email.Image.ImgLogoDark), Convert.FromBase64String(Resources.Email.Image.ImgLogoDark));
                        break;
                    }
                case MailTypes.NewTask: {
                        result.Add(GetNameOf(() => Resources.Email.Image.ImgLogoLGreen), Convert.FromBase64String(Resources.Email.Image.ImgLogoLGreen));
                        break;
                    }
                case MailTypes.News: {
                        result.Add(GetNameOf(() => Resources.Email.Image.ImgLogoGreen), Convert.FromBase64String(Resources.Email.Image.ImgLogoGreen));
                        //result.Add(GetNameOf(() => Resources.Email.Image.ImgProfit), Convert.FromBase64String(Resources.Email.Image.ImgProfit));
                        break;
                    }
                case MailTypes.PasswordRecovery: {
                        result.Add(GetNameOf(() => Resources.Email.Image.ImgLogoDBlue), Convert.FromBase64String(Resources.Email.Image.ImgLogoDBlue));
                        break;
                    }
                case MailTypes.TaskResponse: {
                        result.Add(GetNameOf(() => Resources.Email.Image.ImgLogoLGreen), Convert.FromBase64String(Resources.Email.Image.ImgLogoLGreen));
                        break;
                    }
                case MailTypes.Warning: {
                        result.Add(GetNameOf(() => Resources.Email.Image.ImgLogoYellow), Convert.FromBase64String(Resources.Email.Image.ImgLogoYellow));
                        break;
                    }
            }
            return result;
        }
    }
}
