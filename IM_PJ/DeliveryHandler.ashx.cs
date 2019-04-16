using System;
using System.Web;
using IM_PJ.Models;
using IM_PJ.Controllers;
using Newtonsoft.Json;

namespace IM_PJ
{
    /// <summary>
    /// Summary description for UploadHander
    /// </summary>
    public class DeliveryHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var files = context.Request.Files;
                var delivery = JsonConvert.DeserializeObject<Delivery>(context.Request.Form["Delivery"]);

                if (files.Count > 0)
                {
                    var filePath = String.Format(
                        "/uploads/deliveries/{0}-{1:yyyyMMddHHmmss}{2}",
                        delivery.OrderID,
                        DateTime.UtcNow,
                        System.IO.Path.GetExtension(files["ImageNew"].FileName)
                    );
                    files["ImageNew"].SaveAs(context.Server.MapPath(filePath));
                    delivery.Image = filePath;
                }

                string username = context.Request.Cookies["userLoginSystem"].Value;
                var acc = AccountController.GetByUsername(username);

                // Update transfer infor
                delivery.UUID = Guid.NewGuid();
                delivery.CreatedBy = acc.ID;
                delivery.CreatedDate = DateTime.Now;
                delivery.ModifiedBy = acc.ID;
                delivery.ModifiedDate = DateTime.Now;

                DeliveryController.Update(delivery);
                context.Response.Write(delivery.Image);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 400;
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}