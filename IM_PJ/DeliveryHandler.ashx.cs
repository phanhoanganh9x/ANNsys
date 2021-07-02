using System;
using System.Web;
using IM_PJ.Models;
using IM_PJ.Controllers;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

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
                var uploadPath = "/uploads/deliveries/";

                if (!String.IsNullOrEmpty(delivery.Image))
                {
                    // Delete invoice image after upload image new
                    File.Delete(context.Server.MapPath(delivery.Image));
                }

                if (files.Count > 0)
                {
                    // Trường hợp upload image new
                    var filePathNew = String.Format(
                        "{0}{1}-{2:yyyyMMddHHmmss}{3}",
                        uploadPath,
                        delivery.OrderID,
                        DateTime.UtcNow,
                        System.IO.Path.GetExtension(files["ImageNew"].FileName)
                    );
                    files["ImageNew"].SaveAs(context.Server.MapPath(filePathNew));
                    delivery.Image = filePathNew;
                }
                else
                {
                    // Trường hợp là remove image củ
                    delivery.Image = String.Empty;
                }

                string username = context.Request.Cookies["usernameLoginSystem_ANN123"].Value;
                var acc = AccountController.GetByUsername(username);

                // Update transfer infor
                delivery.UUID = Guid.NewGuid();
                delivery.CreatedBy = acc.ID;
                delivery.CreatedDate = DateTime.Now;
                delivery.ModifiedBy = acc.ID;
                delivery.ModifiedDate = DateTime.Now;
                
                DeliveryController.Update(delivery);
                var session = new List<DeliverySession>()
                {
                    new DeliverySession()
                    {
                        OrderID = delivery.OrderID,
                        ShipperID = delivery.ShipperID,
                        CreatedDate = delivery.CreatedDate,
                        DeliveryTimes = delivery.Times.Value,
                        DeliveryStatus = delivery.Status,
                        COD = delivery.COO,
                        ShippingFee = delivery.COD
                    }
                };
                SessionController.updateDeliverySession(acc, session);
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