using CronNET;
using IM_PJ.Controllers;
using IM_PJ.CronJob;

namespace IM_PJ
{
    public partial class Startup
    {

        public void ConfigureCron()
        {
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