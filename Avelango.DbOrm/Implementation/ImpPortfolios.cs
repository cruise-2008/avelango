using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Orm;
using Avelango.Models.User;

namespace Avelango.DbOrm.Implementation
{
    public class ImpPortfolios : IPortfolios
    {
        private readonly IRepository<Users> _users;
        private readonly IRepository<Portfolios> _portfolios;
        private readonly IRepository<PortfolioJobs> _portfolioJobs;
        private readonly IRepository<PortfolioJobImages> _portfolioJobImages;



        public ImpPortfolios(IRepository<Portfolios> portfolios, IRepository<Users> users, IRepository<PortfolioJobs> portfolioJobs,
                             IRepository<PortfolioJobImages> portfolioJobImages) {
            _portfolios = portfolios;
            _users = users;
            _portfolioJobs = portfolioJobs;
            _portfolioJobImages = portfolioJobImages;
        }


        public OperationResult<bool> AddPortfolioData(Guid userPk, string title, string description) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("[ImpPortfolios]/[AddPortfolioData] User with Pk:" + userPk + " doesnt found."));


                var portfolio = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
                if (portfolio == null) {
                    _portfolios.Add(new Portfolios {
                        BelongsToUser = user.ID,
                        Title = title,
                        Description = description
                    });
                }
                else {
                    portfolio.Title = title;
                    portfolio.Description = description;
                }
                _portfolios.UnitOfWork.Commit();
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<ApplicationPortfolio> GetPortfolio(Guid userPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<ApplicationPortfolio>(new Exception("[ImpPortfolios]/[GetPortfolio] User with Pk:" + userPk + " doesnt found."));
                var porto = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
                if (porto == null) return new OperationResult<ApplicationPortfolio>(new ApplicationPortfolio());

                var aporto = (ApplicationPortfolio) porto;
                aporto.Jobs = new List<ApplicationPortfolioJobs>();
                var portoJobs = _portfolioJobs.GetFiltered(x => x.BelongsToPortfolio == porto.ID);

                foreach (var portoJob in portoJobs) {
                    var aportoJob = (ApplicationPortfolioJobs) portoJob;
                    aportoJob.Images = new List<ApplicationPortfolioJobImages>();
                    var appPortfolioJobImages = _portfolioJobImages.GetFiltered(x => x.BelongsToPortfolioJob == portoJob.ID);
                    foreach (var appPortfolioJobImage in appPortfolioJobImages) {
                        aportoJob.Images.Add((ApplicationPortfolioJobImages)appPortfolioJobImage);
                    }
                    aporto.Jobs.Add(aportoJob);
                }
                return new OperationResult<ApplicationPortfolio>(aporto);
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationPortfolio>(ex);
            }
        }



        public OperationResult<string> JobUpLoad(Guid userPk, string title, string description, List<ImagePair> imagePairs) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<string>(new Exception("[ImpPortfolios]/[JobUpLoad] User with Pk:" + userPk + " doesnt found."));

                var portfolio = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
                if (portfolio == null) {
                    _portfolios.Add(new Portfolios {
                        PublicKey = Guid.NewGuid(),
                        BelongsToUser = user.ID,
                        Title = string.Empty,
                        Description = string.Empty
                    });
                    _portfolios.UnitOfWork.Commit();
                    portfolio = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
                }
                var newJobPk = Guid.NewGuid();
                var job = new PortfolioJobs {
                    PublicKey = newJobPk,
                    Title = title,
                    Description = description,
                    BelongsToPortfolio = portfolio.ID
                };
                _portfolioJobs.Add(job);
                _portfolioJobs.UnitOfWork.Commit();

                if (imagePairs.Any()) {
                    job = _portfolioJobs.GetSingleOrDefault(x => x.PublicKey == newJobPk);
                    foreach (var imagePair in imagePairs) {
                        _portfolioJobImages.Add(new PortfolioJobImages {
                            PublicKey = Guid.NewGuid(),
                            BelongsToPortfolioJob = job.ID,
                            Large = imagePair.Large,
                            Small = imagePair.Small
                        });
                    }
                    _portfolioJobImages.UnitOfWork.Commit();
                }
                return new OperationResult<string>(newJobPk.ToString());
            }
            catch (Exception ex) {
                return new OperationResult<string>(ex);
            }
        }


        public OperationResult<bool> ChangePortfolioJobData(Guid userPk, Guid jobPk, string title, string description) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("[ImpPortfolios]/[ChangePortfolioJobData] User with Pk:" + userPk + " doesnt found."));

                var portfolio = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
                if (portfolio == null) return new OperationResult<bool>(new Exception("[ImpPortfolios]/[ChangePortfolioJobData]: Portfolio does not found"));

                var job = _portfolioJobs.GetSingleOrDefault(x => x.BelongsToPortfolio == portfolio.ID && x.PublicKey == jobPk);
                job.Title = title;
                job.Description = description;
                _portfolioJobs.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> AddPortfolioJobImage(Guid userPk, Guid jobPk, List<ImagePair> imagePairs) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("[ImpPortfolios]/[ChangePortfolioJobData] User with Pk:" + userPk + " doesnt found."));

                var portfolio = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
                if (portfolio == null) return new OperationResult<bool>(new Exception("AddPortfolioJobImage: Portfolio does not found")); {
                    var job = _portfolioJobs.GetSingleOrDefault(x => x.BelongsToPortfolio == portfolio.ID && x.PublicKey == jobPk);

                    if (job != null && imagePairs.Any()) {
                        foreach (var imagePair in imagePairs) {
                            _portfolioJobImages.Add(new PortfolioJobImages {
                                PublicKey = Guid.NewGuid(),
                                Small = imagePair.Small,
                                Large = imagePair.Large,
                                BelongsToPortfolioJob = job.ID
                            });
                        }
                        _portfolioJobImages.UnitOfWork.Commit();
                    }
                }
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<ImagePair> RemovePortfolioJobImage(Guid userPk, Guid jobPk, Guid imagePk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<ImagePair>(new Exception("[ImpPortfolios]/[RemovePortfolioJobImage] User with Pk:" + userPk + " doesnt found."));

                var portfolio = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
                if (portfolio == null) return new OperationResult<ImagePair>(new Exception("[ImpPortfolios]/[RemovePortfolioJobImage]: portfolio does not found"));

                var job = _portfolioJobs.GetSingleOrDefault(x => x.PublicKey == jobPk && x.BelongsToPortfolio == portfolio.ID);
                if (job == null) return new OperationResult<ImagePair>(new Exception("[ImpPortfolios]/[RemovePortfolioJobImage]: job does not found"));

                var result = new ImagePair();
                var image = _portfolioJobImages.GetSingleOrDefault(x => x.BelongsToPortfolioJob == job.ID && x.PublicKey == imagePk);
                if (image != null) {
                    result.Small = image.Small;
                    result.Large = image.Large;

                    _portfolioJobImages.Remove(image);
                    _portfolioJobImages.UnitOfWork.Commit();
                }
                return new OperationResult<ImagePair>(result);
            }
            catch (Exception ex) {
                return new OperationResult<ImagePair>(ex);
            }
        }


        public OperationResult<bool> RemovePortfolioJob(Guid userPk, Guid jobPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("[ImpPortfolios]/[RemovePortfolioJobImage] User with Pk:" + userPk + " doesnt found."));

                var portfolio = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
                if (portfolio == null) return new OperationResult<bool>(new Exception("[ImpPortfolios]/[RemovePortfolioJobImage]: portfolio does not found"));

                var job = _portfolioJobs.GetSingleOrDefault(x => x.PublicKey == jobPk && x.BelongsToPortfolio == portfolio.ID);
                if (job == null) return new OperationResult<bool>(new Exception("[ImpPortfolios]/[RemovePortfolioJobImage]: job does not found"));

                var images = _portfolioJobImages.GetFiltered(x => x.BelongsToPortfolioJob == job.ID).ToList();
                if (images.Any()) {
                    foreach (var image in images) {
                        _portfolioJobImages.Remove(image);
                    }
                    _portfolioJobImages.UnitOfWork.Commit();
                }
                _portfolioJobs.Remove(job);
                _portfolioJobs.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }
      
    }
}
