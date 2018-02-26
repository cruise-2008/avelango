using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpCommonAdvertising : ICommonAdvertising
    {
        private readonly IRepository<CommonAdvertising> _advertising;

        public ImpCommonAdvertising(IRepository<CommonAdvertising> advertising)
        {
            _advertising = advertising;
        }

        public OperationResult<bool> Add(DateTime expired, string companyName, int companyRate, string firstData, string secondData, string icon, string imagePath) {
            try {
                _advertising.Add(new CommonAdvertising {
                    Expired = DateTime.Now,
                    PublicKey = Guid.NewGuid(),
                    CompanyName = companyName,
                    CompanyRate = companyRate,
                    FirstData = firstData,
                    SecondData = secondData,
                    Icon = icon,
                    ImagePath = imagePath
                });
                _advertising.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<List<ApplicationAdvertising>> GetAll() {
            try {
                var news = _advertising.GetAll();
                var nws = news.Select(x => (ApplicationAdvertising)x).ToList();
                return new OperationResult<List<ApplicationAdvertising>>(nws);
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationAdvertising>>(ex);
            }
        }


        public OperationResult<bool> Remove(Guid pk) {
            try {
                var news = _advertising.GetSingleOrDefault(x => x.PublicKey == pk);
                if (news == null) return new OperationResult<bool>();
                _advertising.Remove(news);
                _advertising.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }
    }
}
