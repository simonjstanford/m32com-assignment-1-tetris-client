using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TetrisWebService;

public partial class highscores : System.Web.UI.Page
{
    TetrisWebService.TetrisWebServiceSoapClient webService;

    protected void Page_Load(object sender, EventArgs e)
    {
        webService = new TetrisWebServiceSoapClient();

        DataTable dt = webService.GetHighScores();
        dt.DefaultView.Sort = "Score desc";
        HighScoresGridView.DataSource = dt;
        HighScoresGridView.DataBind();
    }
}