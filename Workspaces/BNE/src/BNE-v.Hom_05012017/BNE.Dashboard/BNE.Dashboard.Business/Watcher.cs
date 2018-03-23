using System;
using BNE.Dashboard.Data.Infrastructure;
using BNE.Dashboard.Data.Repositories;
using BNE.Dashboard.Entities;
using System.Collections.Generic;

namespace BNE.Dashboard.Business
{
    public class Watcher : IWatcherService
    {

        private readonly IWatcherRepository _watcherRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Watcher(IWatcherRepository watcherRepository, IUnitOfWork unitOfWork)
        {
            this._watcherRepository = watcherRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Entities.Watcher> GetAll()
        {
            var watchers = _watcherRepository.GetAll();
            foreach (var watcher in watchers)
            {
                DefineStatus(watcher);
                yield return watcher;
            }
        }

        public Entities.Watcher GetById(int id)
        {
            return _watcherRepository.GetById(id);
        }

        private void DefineStatus(Entities.Watcher watcher)
        {
            if (watcher.WindowsService != null)
            {
                watcher.Status = Status.OK;

                try
                {
                    watcher.Status = VerifyBusinessRule(watcher);
                }
                catch
                {
                    watcher.Status = Status.ERROR;
                }
            }
            else if (watcher.MessageQueue != null)
            {
                watcher.Status = Status.OK;

                try
                {
                    Helper.MessageQueue.VerifyBusinessRule(watcher);
                }
                catch
                {
                    watcher.Status = Status.ERROR;
                }
            }
            else if (watcher.SiteResponse != null)
            {
                watcher.Status = Status.OK;

                try
                {
                    Helper.SiteResponse.VerifyBusinessRule(watcher);
                }
                catch
                {
                    watcher.Status = Status.ERROR;
                }
            }
        }

        private Status VerifyBusinessRule(Entities.Watcher watcher)
        {
            watcher.Amount = _watcherRepository.ExecuteStoredProcedure(watcher.WindowsService.StoredProcedureName);

            if (watcher.Amount > 0)
                return Status.ERROR;

            return Status.OK;
        }
    }

    public interface IWatcherService
    {
        IEnumerable<Entities.Watcher> GetAll();
        Entities.Watcher GetById(int id);
    }
}
