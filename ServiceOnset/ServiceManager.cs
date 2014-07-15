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
        public List<IService> Services
        {
            get;
            private set;
        }

        public ServiceManager(IServiceOnsetConfig config)
        {
            this.Services = config.StartInfos
                .Select(s => ServiceFactory.Instance.Create(s))
                .ToList();
            AppHelper.Log.Info("{0} service(s) initialized: {1}",
                this.Services.Count,
                string.Join(", ", this.Services.Select(s => s.StartInfo.Name + "(" + s.StartInfo.RunMode.ToString() + ")").ToArray()));
        }

        public void RunServices()
        {
            this.Services.ForEach(s => s.Start());
            AppHelper.Log.Info(AppHelper.AppTitle + " started");
        }
        public void StopServices()
        {
            this.Services.ForEach(s => s.Stop());
            AppHelper.Log.Info(AppHelper.AppTitle + " stopped");
        }
    }
}
