using ServiceOnset.Common;
using ServiceOnset.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceOnset
{
    public class ServiceManager
    {
        private List<IService> Services { get; set; }

        public ServiceManager(IServiceOnsetConfig config)
        {
            this.Services = config.StartInfos.Select(s => ServiceFactory.Instance.Create(s)).ToList();
        }

        public void Run()
        {
            //？？？？？？？？？？？？？？？？？？？？
            this.Services.First().Start();
        }
    }
}
