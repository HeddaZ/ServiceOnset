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
            Services = config.StartInfos
                .Where(s => !s.Disable)
                .Select(s => ServiceFactory.Instance.Create(s))
                .ToList();
            AppHelper.Log.Info("{0} service(s) initialized. {1}",
                Services.Count,
                string.Join(", ", Services.Select(s => s.StartInfo.Name + "(" + s.StartInfo.RunMode.ToString() + ")").ToArray()));
        }

        public void RunServices()
        {
            Services.ForEach(s => s.Start());
            AppHelper.Log.Info(AppHelper.AppTitle + " started");
        }
        public void StopServices()
        {
            Services.ForEach(s => s.Stop());
            AppHelper.Log.Info(AppHelper.AppTitle + " stopped");
        }
    }
}
