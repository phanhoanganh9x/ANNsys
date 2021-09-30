using CronNET;
using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Models.Pages.ExecuteAPI;
using IM_PJ.Properties;
using IM_PJ.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace IM_PJ.CronJob
{

    public class RunScheduleProductStatus : BaseJob
    {
        private Settings _settings = new Settings();
        private const string CRON_NAME = "Product Status";
        private const string JOB_NAME = "RunScheduleProductStatus";
        private readonly Log _log;
        private readonly string _website;

        /// <summary>
        /// Sử dụng cho việc tìm kiếm sản phẩm cập nhật
        /// </summary>
        public RunScheduleProductStatus() :base()
        {
            _log = Log.Instance();
            this._website = String.Empty;
        }

        /// <summary>
        /// Sử dụng cho việc post api
        /// </summary>
        /// <param name="website"></param>
        public RunScheduleProductStatus(string website)
        {
            _log = Log.Instance();
            this._website = website;
        }

        public override CronExpression Cron
        {
            get
            {
                return CronExpression.EveryMinute;
            }
        }

        public override void Execute()
        {
            try
            {
                _log.Info("Begin excute post API for the websites");

                // Nếu cron job được yêu cầu dưng lại thì sẽ không sử lý nữa
                if (isPause())
                {
                    _log.Info("Run Schedule Web - Cron Job Pause.");
                    return;
                }

                var schedules = CronJobController.getScheduleProductStatus(_website, (int)CronJobStatus.Scheduled, 80);
                _log.Info(String.Format("Run Schedule - Number Schedule: {0:N}", schedules.Count));

                if(schedules.Count > 0)
                {
                    // Lock các dữ liệu chuẩn bị start
                    schedules = schedules.Select(x =>
                    {
                        x.Status = (int)CronJobStatus.Start;
                        return x;
                    })
                    .ToList();
                    CronJobController.updateScheduleProductStatus(schedules);

                    // Chạy post API
                    RunScheduleWeb(schedules);
                }

                _log.Info("End excute post API for the websites");
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }

        private bool isPause()
        {
            // Kiểm tra điều kiện để chạy cron job
            if (_settings.ASPNETCORE_ENVIRONMENT != "Production")
                return true;

            var cron = CronJobController.get(CRON_NAME);

            if (cron != null)
                return cron.Status == 0 ? true : false;
            else
                return false;
        }

        /// <summary>
        /// Thực thi chạy cập nhật trạng thái sản phẩm theo từ web quảng cáo
        /// </summary>
        /// <param name="schedules"></param>
        public void RunScheduleWeb(List<CronJobProductStatu> schedules)
        {
            _log.Info("Begin execute the scheduler for website");
            _log.Info(String.Format("Run Schedule Web - Number Schedule: {0:N}", schedules.Count));

            var scheduleUpdate = new List<CronJobProductStatu>();
            var index = 0;

            foreach (var schedule in schedules)
            {
                // Đếm số lượng thực thi
                index++;

                // Thực thi post API cập nhật thông tin trạng thái sản phẩm
                var response = postAPI(schedule);
                // Lấy thông tin trạng thái lịch trình sau khi thực thi post API
                var scheduleDone = checkResponse(schedule, response);

                scheduleUpdate.Add(scheduleDone);
                _log.Info(String.Format("{0:N0} - Run Schedule Web - {1}", index, JsonConvert.SerializeObject(scheduleDone)));

                Thread.Sleep(500);
            }

            CronJobController.updateScheduleProductStatus(scheduleUpdate);
        }

        /// <summary>
        /// Thực thi post api tới web quảng cáo
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public ResponseProductStatusModel postAPI(CronJobProductStatu schedule)
        {
            ResponseProductStatusModel result = null;

            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(schedule.Web);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                var content = new FormUrlEncodedContent(new[] {
                    new KeyValuePair<string, string>("key", "828327"),
                    new KeyValuePair<string, string>("sku", schedule.SKU),
                    new KeyValuePair<string, string>("visibility", schedule.IsHidden ? "exclude-from-catalog" : "visible")
                });

                var response = client.PostAsync(schedule.API, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    result = response.Content.ReadAsAsync<ResponseProductStatusModel>().Result;
                }
                else
                {
                    result = new ResponseProductStatusModel()
                    {
                        response = "error",
                        content = "HTTP status: " + response.StatusCode.ToString()
                    };
                }
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                _log.Error("Post API for website", ex);
                postAPI(schedule);
            }
            catch (Exception ex)
            {
                _log.Error("Post API for website", ex);
                result = new ResponseProductStatusModel()
                {
                    response = "error",
                    content = ex.Message.Length > 255 ? ex.Message.Substring(0, 254) : ex.Message
                };
            }

            return result;
        }

        /// <summary>
        /// Dựa vào kết quả trả về API
        /// Cập nhật lại cho trạng thái schedule
        /// </summary>
        /// <param name="schedule"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public CronJobProductStatu checkResponse(CronJobProductStatu schedule, ResponseProductStatusModel response)
        {
            if (response == null)
            {
                schedule.Status = (int)CronJobStatus.Fail;
                schedule.Note = "Đã xãy ra lỗi khi gọi thực thi api";
                schedule.ModifiedDate = DateTime.Now;
            }
            else if (response.response == "done")
            {
                schedule.Status = (int)CronJobStatus.Done;
                schedule.Note = response.response;
                schedule.ModifiedDate = DateTime.Now;
            }
            else if (response.response == "notfound")
            {
                schedule.Status = (int)CronJobStatus.Continue;
                schedule.Note = response.response;
                schedule.ModifiedDate = DateTime.Now;
            }
            else if (response.response == "error")
            {
                schedule.Status = (int)CronJobStatus.Fail;
                schedule.Note = response.response;
                schedule.ModifiedDate = DateTime.Now;
            }
            else
            {
                schedule.Status = (int)CronJobStatus.Fail;
                response.content = "Trường hợp success không có trong định nghĩa. " + response.content;
                if (response.content.Length > 255)
                    schedule.Note = response.content.Substring(0, 254);
                else
                    schedule.Note = response.content;
                schedule.ModifiedDate = DateTime.Now;
            }

            return schedule;
        }
    }
}