using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRUDWeb
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void modal_Click(object sender, EventArgs e)
        {
            mymodal.Style["display"] = "block";
        }

        protected void modal_close(object sender, EventArgs e)
        {
            mymodal.Style["display"] = "none";
        }
    }
}