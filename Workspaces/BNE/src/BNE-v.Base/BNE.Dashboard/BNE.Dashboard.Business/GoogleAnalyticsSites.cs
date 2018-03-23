using BNE.Dashboard.Data.Infrastructure;
using BNE.Dashboard.Data.Repositories;
using System;
using System.Collections.Generic;

namespace BNE.Dashboard.Business
{
    public class GoogleAnalyticsSites : IGoogleAnalyticsSitesService
    {

        private readonly IGoogleAnalyticsSitesRepository _googleAnalyticsSitesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GoogleAnalyticsSites(IGoogleAnalyticsSitesRepository googleAnalyticsSitesRepository, IUnitOfWork unitOfWork)
        {
            this._googleAnalyticsSitesRepository = googleAnalyticsSitesRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Entities.GoogleAnalyticsSites> GetAll()
        {
            var sites = _googleAnalyticsSitesRepository.GetAll();
            foreach (var site in sites)
            {
                var value = Helper.Google.Analytics.Instance.GetData(site.ViewID, "activeUsers");
                site.ActiveUsers = string.IsNullOrWhiteSpace(value) ? 0 : Convert.ToInt32(value);
                yield return site;
            }
        }

    }

    public interface IGoogleAnalyticsSitesService
    {
        IEnumerable<Entities.GoogleAnalyticsSites> GetAll();
    }
}
