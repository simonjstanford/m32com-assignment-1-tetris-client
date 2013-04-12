using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TetrisWebService;

/// <summary>
/// Displays a table of the current high scores that have been retrieved from the web service
/// </summary>
public partial class highscores : System.Web.UI.Page
{
    TetrisWebService.TetrisWebServiceSoapClient webService;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Create the web service proxy
        webService = new TetrisWebServiceSoapClient();

        //Create a DataTable object and populate it with the high scores DataTable returned from the web service
        DataTable dt = webService.GetHighScores();
        //sort the data using the Score field, and arrange it in descending order - this makes the highest score appear at the top of the table 
        dt.DefaultView.Sort = "Score desc"; 
        //assign the dataTable as the datasource to the GridView and bind - this makes the data viewable
        HighScoresGridView.DataSource = dt;
        HighScoresGridView.DataBind();
    }
}