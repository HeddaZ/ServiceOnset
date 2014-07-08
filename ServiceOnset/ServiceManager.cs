using ServiceOnset.Config;
using ServiceOnset.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset
{
    public class ServiceManager
    {
        private List<IService> InnerServices { get; set; }

        public ServiceManager(IServiceOnsetConfig config)
        {
            this.InnerServices = config.StartInfos
                .Select(s => ServiceFactory.Instance.Create(s))
                .ToList();
        }

        public void RunServices()
        {
            //？？？？？？？？？？？？？？？？？？？？
            this.InnerServices.First().Start();
        }
    }
}
