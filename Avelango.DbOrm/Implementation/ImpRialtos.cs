using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Application;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpRialtos : IRialtos
    {
        private readonly IRepository<Users> _users;

        private readonly IRepository<RialtoState> _rialtoState;
        private readonly IRepository<RialtoAction> _rialtoAction;
        private readonly IRepository<RialtoChat> _rialtoChat;
        private readonly IRepository<RialtoUsersStates> _rialtoUsersState;
        private readonly IRepository<RialtoUserAssets> _rialtoUsersAssets;

        public ImpRialtos(IRepository<RialtoState> rialtoState, IRepository<RialtoAction> rialtoAction, IRepository<Users> users, IRepository<RialtoUsersStates> rialtoUsersState, IRepository<RialtoUserAssets> rialtoUsersAssets, IRepository<RialtoChat> rialtoChat) {
            _rialtoState = rialtoState;
            _rialtoAction = rialtoAction;
            _users = users;
            _rialtoUsersState = rialtoUsersState;
            _rialtoUsersAssets = rialtoUsersAssets;
            _rialtoChat = rialtoChat;
        }


        public void AddUserIfNotExist(Guid userPk) {

            const int startAves = 1000;
            var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
            var rialtoUser = _rialtoUsersState.GetSingleOrDefault(x => x.BelongsToUser == user.ID);

            if (rialtoUser != null) return;
            _rialtoUsersState.Add(new RialtoUsersStates { Ballance = startAves, Equity = startAves, BelongsToUser = user.ID });
            _rialtoUsersState.UnitOfWork.Commit();

            var data = _rialtoState.GetAll().First();
            data.TotalAveCount += startAves;
            _rialtoState.UnitOfWork.Commit();
        }


        public ApplicationRialtoState GetCommonData(Guid userPk) {
            var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
            var appRialtoUser = _rialtoUsersState.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
            var data = _rialtoState.GetAll().First();
            var assets = _rialtoUsersAssets.GetFiltered(x => x.BelongsToUser == user.ID);
            var uassets = assets.Select(asset => (ApplicationRialtoUserAssets) asset).ToList();
            var rdata = (ApplicationRialtoState)data;
            rdata.UserState = (ApplicationRialtoUsersStates)appRialtoUser;
            rdata.Assets = uassets;
            return rdata;
        }


        public string GetChampionName() {
            var rialtochampion = new RialtoUsersStates();
            var rialtoState = _rialtoState.GetAll().First();
            var users = _rialtoUsersState.GetAll();
            
            double topBallance = 0;
            foreach (var user in users) {
                var assets = _rialtoUsersAssets.GetFiltered(x => x.BelongsToUser == user.ID);
                var userAssetsSumm = assets.Sum(asset => asset.Value*rialtoState.AveRate*rialtoState.Shoulder);

                var userBallance = user.Ballance + userAssetsSumm;
                if (!(userBallance > topBallance)) continue;
                topBallance = userBallance;
                rialtochampion = user;
            }
            var champion = _users.GetSingleOrDefault(x => x.ID == rialtochampion.BelongsToUser);
            return champion == null ? string.Empty : champion.Name + " " + champion.SurName;
        }


        public Tuple<double, List<ApplicationRialtoUserAssets>> Bid(Guid userPk, double bidValue) {
            var unsignedBidValue = bidValue > 0 ? bidValue : bidValue * -1;
            var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
            var rialtoUser = _rialtoUsersState.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
            if (!(rialtoUser.Ballance > unsignedBidValue)) return new Tuple<double, List<ApplicationRialtoUserAssets>>(0, null);

            var aveRate = ChangeCommonRate(bidValue);

            var data = _rialtoState.GetAll().First();
            var siteBenefit = data.BenefitPersent * unsignedBidValue / 100;

            rialtoUser.Ballance -= unsignedBidValue + siteBenefit;
            rialtoUser.Equity -= unsignedBidValue + siteBenefit;
            _rialtoUsersState.UnitOfWork.Commit();

            _rialtoUsersAssets.Add(new RialtoUserAssets {
                BelongsToUser = user.ID,
                Benefit = 0,
                PublicKey = Guid.NewGuid(),
                Rate = data.AveRate,
                Sign = bidValue > 0,
                Value = unsignedBidValue
            });
            _rialtoUsersAssets.UnitOfWork.Commit();

            AddPointToChart(aveRate, user.ID, bidValue);

            CalculateUsersBallance(aveRate, userPk);

            var assets = _rialtoUsersAssets.GetFiltered(x => x.BelongsToUser == user.ID);
            var uassets = assets.Select(asset => (ApplicationRialtoUserAssets)asset).ToList();
            return new Tuple<double, List<ApplicationRialtoUserAssets>>(aveRate, uassets);
        }


        private double ChangeCommonRate(double bidValue) {
            var data = _rialtoState.GetAll().First();
            var aveRate = Math.Round(data.AveRate + bidValue / data.TotalAveCount, 5);
            data.AveRate = aveRate;
            _rialtoState.UnitOfWork.Commit();
            return aveRate;
        }


        private void AddPointToChart(double aveRate, int userId, double bidValue) {
            _rialtoAction.Add(new RialtoAction {
                AveRate = aveRate,
                BelongsToUser = userId,
                Bid = bidValue,
                BidTime = DateTime.Now
            });
            _rialtoAction.UnitOfWork.Commit();
        }


        public double CloseBid(Guid userPk, Guid bidId) {
            var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
            var asset = _rialtoUsersAssets.GetSingleOrDefault(x => x.BelongsToUser == user.ID && x.PublicKey == bidId);
            var rialtoUser = _rialtoUsersState.GetSingleOrDefault(x => x.BelongsToUser == user.ID);

            rialtoUser.Ballance += asset.Value + asset.Benefit;
            rialtoUser.Equity = rialtoUser.Ballance;
            _rialtoUsersState.UnitOfWork.Commit();

            // Remove asset
            _rialtoUsersAssets.Remove(asset);
            _rialtoUsersAssets.UnitOfWork.Commit();

            // Change all users
            var bidValue = asset.Sign ? asset.Value * -1 : asset.Value;
            var aveRate = ChangeCommonRate(bidValue);
            AddPointToChart(aveRate, user.ID, bidValue);

            CalculateUsersBallance(aveRate, userPk);
            return aveRate;
        }


        public double CloseBids(Guid userPk) {
            var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
            var assets = _rialtoUsersAssets.GetFiltered(x => x.BelongsToUser == user.ID);
            return assets.Sum(asset => CloseBid(userPk, asset.PublicKey ?? Guid.NewGuid()));
        }


        private void CalculateUsersBallance(double newRate, Guid myuserPk) {
            var noextUser = _users.GetSingleOrDefault(x => x.PublicKey == myuserPk);
            var users = _rialtoUsersState.GetFiltered(x => x.BelongsToUser != noextUser.ID);
            var data = _rialtoState.GetAll().First();

            foreach (var user in users) {
                var rialtoUser = _users.GetSingleOrDefault(x => x.ID == user.BelongsToUser);
                var assets = _rialtoUsersAssets.GetFiltered(x => x.BelongsToUser == user.BelongsToUser);

                foreach (var asset in assets) {
                    double equity;
                    if (asset.Sign) {
                        equity = (asset.Rate - newRate) * asset.Value * data.Shoulder * -1;
                    }
                    else {
                        equity = (asset.Rate - newRate) * asset.Value * data.Shoulder;
                    }
                    asset.Benefit = equity;
                    user.Equity += equity;
                }
                if (!(user.Equity < 1)) continue;
                var ballance = CloseBids(rialtoUser.PublicKey ?? Guid.NewGuid());
                user.Ballance = ballance;
                user.Equity = ballance;
            }
            _rialtoUsersAssets.UnitOfWork.Commit();
            _rialtoUsersState.UnitOfWork.Commit();
        }


        public List<KeyValuePair<int, double>> GetChartData() {
            var actions = new List<KeyValuePair<int, double>> ();
            var allActions = _rialtoAction.GetAll().ToList();
            foreach (var i4Action in allActions) {
                var unixTimestamp = (int)i4Action.BidTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                actions.Add(new KeyValuePair<int, double>(unixTimestamp, i4Action.AveRate));
            }
            return actions;
        }


        public RialtoChat AddChatMessage(string userName, string message) {
            try {
                var msg = new RialtoChat {
                    Created = DateTime.Now,
                    Message = message,
                    SenderName = userName
                };
                _rialtoChat.Add(msg);
                _rialtoChat.UnitOfWork.Commit();
                return msg;
            }
            catch {
                return null;
            }
        }


        public List<RialtoChat> GetChatMessages() {
            try {
                return _rialtoChat.GetAll().Take(33).ToList();
            }
            catch {
                return new List<RialtoChat>();
            }
        }
    }
}
