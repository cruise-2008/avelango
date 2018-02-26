using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Enums;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpCommonNews : ICommonNews
    {
        private readonly IRepository<CommonNews> _news;

        public ImpCommonNews(IRepository<CommonNews> news)
        {
            _news = news;
        }


        public OperationResult<bool> Add(string newsText) {
            try {
                _news.Add(new CommonNews {
                    Created = DateTime.Now,
                    Html = newsText,
                    PublicKey = Guid.NewGuid()
                });
                _news.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<List<ApplicationNews>> GetAll(Langs.LangsEnum lang) {
            try {
                var news = _news.GetAll(x => x.Lang == lang.ToString());
                var nws = news.Select(x => (ApplicationNews)x).ToList();
                return new OperationResult<List<ApplicationNews>>(nws);
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationNews>>(ex);
            }
        }


        public OperationResult<bool> Remove(Guid pk) {
            try {
                var news = _news.GetFiltered(x => x.PublicKey == pk).ToList();
                if (!news.Any()) return new OperationResult<bool>();
                foreach (var i4 in news) {
                    _news.Remove(i4);
                }
                _news.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }
    }
}
