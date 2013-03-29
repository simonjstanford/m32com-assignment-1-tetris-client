using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TetrisClient.Web.TetrisWebService;

namespace TetrisClient.Web
{
    public partial class HighScores : System.Web.UI.Page
    {
        TetrisWebService.TetrisWebServiceSoapClient webService;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            webService = new TetrisWebServiceSoapClient();

            DataTable dt = webService.GetHighScores();
            HighScoresGridView.DataSource = dt;
            HighScoresGridView.Sorted += new EventHandler(HighScoresGridView_Sorted);
            HighScoresGridView.Sorting += HighScoresGridView_Sorting;

            HighScoresGridView.Sort("Score", SortDirection.Ascending);
            HighScoresGridView.DataBind();
            

            
        }

        private void HighScoresGridView_Sorting(object sender, EventArgs e)
        {
            //do nothing
        }

        private void HighScoresGridView_Sorted(object sender, EventArgs e)
        {
            //do nothing
        }
    }
}