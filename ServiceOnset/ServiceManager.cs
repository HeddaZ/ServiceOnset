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
            AppHelper.Log.Info("{0} service(s) initialized: {1}",
                this.InnerServices.Count,
                string.Join(", ", this.InnerServices.Select(s => s.StartInfo.Name + "(" + s.StartInfo.RunMode.ToString() + ")").ToArray()));
        }

        public void RunServices()
        {
            this.InnerServices.ForEach(s => s.Start());
        }
        public void StopServices()
        {
            this.InnerServices.ForEach(s => s.Stop());
        }
    }
}
