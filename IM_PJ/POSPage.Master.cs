using IM_PJ.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IM_PJ
{
    public partial class POSPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            if (Request.Cookies["usernameLoginSystem_ANN123"] != null)
            {
                string username = Request.Cookies["usernameLoginSystem_ANN123"].Value;
                var acc = AccountController.GetByUsername(username);
                if (acc != null)
                {
                    var accountInfo = AccountInfoController.GetByUserID(acc.ID);

                    hdfUserID.Value = acc.ID.ToString();

                }
            }
        }
    }
}