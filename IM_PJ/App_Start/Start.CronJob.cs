using CronNET;
using IM_PJ.Controllers;
using IM_PJ.CronJob;
using IM_PJ.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IM_PJ
{
    public partial class Startup
    {
        private Settings _settings = new Settings();

        public void ConfigureCron()
        {
            // Kiểm tra điều kiện để chạy cron job
            if (_settings.ASPNETCORE_ENVIRONMENT != "Production")
                return;

            CronManager.Start();
            var jobs = CronManager.GetJobs();

            foreach (var job in jobs)
                CronManager.RemoveJob(job.JobID);

            var productStatus = new CreateScheduleProductStatus();
            CronManager.AddJob(productStatus);
            CronJobController.update("Product Status", productStatus.JobID);

            var websites = productStatus.getWebAdvertisements();
            foreach (var website in websites)
                CronManager.AddJob(new RunScheduleProductStatus(website));
        }
    }
}