using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// The introduction screen for the game.  Asks for the player's name, that is passed onto the Silverlight client and is used when logging high scores
/// </summary>
public partial class startscreen : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void ButtonPlay_Click(object sender, EventArgs e)
    {
        //create a query string called 'Name' and assign it the players name.  Redirect the player to the game.aspx page to play the game along with the query string
        Response.Redirect("game.aspx?Name=" + TextBoxName.Text);
    }
}