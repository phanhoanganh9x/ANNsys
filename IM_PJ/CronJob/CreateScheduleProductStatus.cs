using CronNET;
using IM_PJ.Controllers;
using IM_PJ.Models;
using IM_PJ.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IM_PJ.CronJob
{
    public class CreateScheduleProductStatus : BaseJob
    {
        private const string CRON_NAME = "Product Status";
        private const string JOB_NAME = "CreateScheduleProductStatus";
        private Log _log;

        /// <summary>
        /// Sử dụng cho việc tìm kiếm sản phẩm cập nhật
        /// </summary>
        public CreateScheduleProductStatus() :base()
        {
            _log = Log.Instance();
        }

        public override CronExpression Cron
        {
            get
            {
                var cron = CronJobController.get(CRON_NAME);

                if (cron != null)
                    return (CronExpression)cron.CronExpression;
                else
                    return (CronExpression)"0 20 * * *";
            }
        }

        public override void Execute()
        {
            try
            {
                var websites = getWebAdvertisements();
                var tasks = new List<Task>();

                foreach (var website in websites)
                {
                    var apiName = getAPIName(website);
                    var job = new Task(() => {
                        // Lấy ra sản phẩm cần cập nhật trạng thái
                        var products = getProduct();

                        createSchedule(products, website, apiName);

                        // Thực hiện  cập nhật thời gian với product
                        updateProduct(products);
                    });

                    tasks.Add(job);
                    job.Start();
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                _log.Error("Creating the schedule for updating the status of the product", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy ra danh sách trang web quảng cáo
        /// </summary>
        /// <returns></returns>
        public List<string> getWebAdvertisements()
        {
            var web = new List<string>();
            web.Add("https://ann.com.vn");
            web.Add("https://khohangsiann.com");
            web.Add("https://bosiquanao.net");
            web.Add("https://thoitrangann.com");
            web.Add("https://quanaogiaxuong.com");
            web.Add("https://bansithoitrang.net");
            web.Add("https://quanaoxuongmay.com");
            web.Add("https://annshop.vn");
            web.Add("https://panpan.vn");
            //web.Add("https://chuyensidobo.com");
            web.Add("https://damgiasi.vn");
            web.Add("https://nhapsionline.com");
            web.Add("https://thoitrangann.com");
            return web;
        }

        /// <summary>
        /// Lấy ra API dùng để cập nhật trạng thái sản phẩm
        /// </summary>
        /// <param name="web"></param>
        /// <returns></returns>
        private string getAPIName(string web)
        {
            if (web == "https://panpan.vn")
                return "sync-product.html";
            else
                return "sync-product";
        }

        private bool isPause()
        {
            var cron = CronJobController.get(CRON_NAME);
            if (cron != null)
                return cron.Status == 0 ? true : false;
            else
                return false;
        }

        /// <summary>
        /// Lấy thông tin những producte có thay đổi trạng thái để lên lịch trình post API
        /// </summary>
        /// <returns></returns>
        private List<CronJobProductStatu> getProduct()
        {
            try
            {
                _log.Info("Begin finding the products which will be updated");

                using (var con = new inventorymanagementEntities())
                {
                    var cron = CronJobController.get(CRON_NAME);

                    if (cron == null)
                        throw new Exception("Không tìm thấy thông tin cron job " + CRON_NAME);

                    #region Tìm thời điểm cập nhật sản phẩm cuối cùng
                    var lastDatetime = con.tbl_Product.Where(x => x.ModifiedDate.HasValue).Max(x => x.ModifiedDate.Value);
                    #endregion

                    #region Lọc những sản phẩm cần cập nhật
                    var products = con.tbl_Product.Where(x => 1 == 1);
                    #endregion

                    #region Lấy thông tin stock của các sản phẩm cập nhật
                    // Giảm bớt thông tin stock cho sản phẩn biến thể và không biến thể
                    var stocks = con.tbl_StockManager
                        .Where(x => (cron.RunAllProduct == true) || (cron.RunAllProduct == false && x.CreatedDate > lastDatetime));

                    // Lấy ID stock dòng cuối cùng của mỗi sản phẩn (gồm sản phẩm biến thể  và không biến thể)
                    var lastStocks = stocks.GroupBy(x => x.SKU).Select(x => new { stockID = x.Max(m => m.ID) });

                    stocks = stocks
                        .Join(
                            lastStocks,
                            s => s.ID,
                            ls => ls.stockID,
                            (s, ls) => s
                        );

                    // Tính số liệu tồn kho của các sản phẩn cần update (Nếu = 0 thì ẩn or hiện)
                    var quantities = stocks
                        .Select(x => new
                        {
                            productID = x.ParentID.HasValue ? x.ParentID.Value : 0,
                            type = x.Type.HasValue ? x.Type.Value : 0,
                            quantity = x.Quantity.HasValue ? x.Quantity.Value : 0,
                            quantittyCurrent = x.QuantityCurrent.HasValue ? x.QuantityCurrent.Value : 0
                        })
                        .GroupBy(x => x.productID)
                        .Select(x => new
                        {
                            productID = x.Key,
                            quantity = x.Sum(s => s.type == 1 ? s.quantity + s.quantittyCurrent : s.quantittyCurrent - s.quantity)
                        });
                    #endregion

                    #region Lên cái lịch cập nhật và chèn vào table CronJobProductStatus
                    // Dữ liệu chính dùng để cho xửa lý cấp nhật table CronJobProductStatus
                    var data = products
                        .GroupJoin(
                            quantities,
                            p => p.ID,
                            q => q.productID,
                            (p, q) => new { product = p, quantity = q}
                        )
                        .SelectMany(
                            x => x.quantity.DefaultIfEmpty(),
                            (parent, child) => new { parent, child }
                        )
                        .Where(
                            x => (cron.RunAllProduct == true) || (cron.RunAllProduct == false && x.child != null)
                        )
                        .Select(x => new
                        {
                            categoryID = x.parent.product.CategoryID.Value,
                            productID = x.parent.product.ID,
                            sku = x.parent.product.ProductSKU,
                            quantity = x.child != null ? x.child.quantity : 0
                        })
                        .ToList();


                    #endregion

                    var now = DateTime.Now;
                    var result = data.Select(x => new CronJobProductStatu()
                    {
                        Web = String.Empty,
                        API = String.Empty,
                        CategoryID = x.categoryID,
                        ProductID = x.productID,
                        SKU = x.sku,
                        Quantity = (int)x.quantity,
                        IsHidden = x.quantity < cron.MinProduct,
                        Status = (int)CronJobStatus.Scheduled,
                        CreatedDate = now,
                        ModifiedDate = now
                    })
                    .ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Get the products which will be updated", ex);
                throw;
            }
            finally {
                _log.Info("End finding the products which will be updated");
            }
        }

        /// <summary>
        /// Thực thi tạo schedule để post lên các website vệ tinh
        /// </summary>
        /// <param name="product"></param>
        /// <param name="website"></param>
        /// <returns></returns>
        private void createSchedule(List<CronJobProductStatu> products, string website, string apiName)
        {
            try
            {
                _log.Info("Begin posting the schedule new.");

                var index = 0;
                var cronNews = new List<CronJobProductStatu>();

                foreach (var item in products)
                {
                    index++;

                    // Nếu cron job được yêu cầu dưng lại thì sẽ không sử lý nữa
                    if (isPause())
                    {
                        if(cronNews.Count > 0)
                        {
                            CronJobController.postScheduleProductStatus(cronNews);
                            cronNews.Clear();
                        }

                        _log.Info("Cron Job Pause at postSchedule.");
                        return;
                    }

                    // Cập nhật những thông tin còn thiếu để khởi tạo schedule
                    if (website == "https://chuyensidobo.com" && item.CategoryID != 18)
                        continue;
                    else if (website == "https://damgiasi.vn" && item.CategoryID != 17)
                        continue;
                    else if (item.CategoryID == 44)
                        continue;

                    item.Web = website;
                    item.API = apiName;
                    cronNews.Add(item);
                    _log.Info(String.Format("{0:N0} - Schedule New - {1}", index, JsonConvert.SerializeObject(item)));

                    if (cronNews.Count >= 100)
                    {
                        CronJobController.postScheduleProductStatus(cronNews);
                        cronNews.Clear();
                    }
                }

                if (cronNews.Count > 0)
                {
                    CronJobController.postScheduleProductStatus(cronNews);
                    cronNews.Clear();
                }
            }
            catch (ThreadAbortException ex)
            {
                _log.Error("Posting the schedule new", ex);
                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                _log.Error("Posting the schedule new", ex);
            }
            finally
            {
                _log.Info("End posting the schedule new.");
            }
        }

        /// <summary>
        /// Cập nhật lại thời gian chỉnh sửa của sản phẩm
        /// </summary>
        /// <param name="products"></param>
        private void updateProduct(List<CronJobProductStatu> products)
        {
            try
            {
                _log.Info("Begin updating the products which were be scheduled");
                _log.Info(String.Format("Update Product - Number Product: {0:N}", products.Count));

                var cron = CronJobController.get(CRON_NAME);

                if (cron == null)
                    throw new Exception("Không tìm thấy thông tin cron job " + CRON_NAME);

                var size = 100;
                var chunks = new List<List<CronJobProductStatu>>();
                var chunkCount = Math.Ceiling(1.0 * products.Count() / size);

                // Sắp xếp lại product theo thứ tự ID tăng dần
                products = products.OrderBy(x => x.ProductID).ToList();

                for (var i = 0; i < chunkCount; i++)
                    chunks.Add(products.Skip(i * size).Take(size).ToList());

                var now = DateTime.Now;
                var index = 0;

                foreach (var chunk in chunks)
                {
                    using (var con = new inventorymanagementEntities())
                    {
                        foreach (var item in chunk)
                        {
                            index++;

                            var product = con.tbl_Product
                                .Where(x => x.ID == item.ProductID)
                                .FirstOrDefault();

                            if (product != null)
                            {
                                product.IsHidden = item.Quantity < cron.MinProduct;
                                product.ModifiedBy = "CronJob";
                                product.ModifiedDate = now;

                                _log.Info(String.Format("{0:N0} - Update Product - {1}", index, JsonConvert.SerializeObject(item)));
                            }
                        }

                        con.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Updating the products which were be scheduled", ex);
                throw;
            }
            finally
            {
                _log.Info("End updating the products which were be scheduled");
            }
        }
    }
}