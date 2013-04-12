using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// The documentation page contains download links for project documents and an embedded YouTube video demonstrating the game
/// </summary>
public partial class documentation : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //display the embedded YouTube video, taken from http://forums.asp.net/p/1341599/2717427.aspx
        string YouTubeID = "9SAF9jEKLVA";
        EmbeddedVideo.Text = "<object width='425' height='355'><param name='movie' value='http://www.youtube.com/v/" + YouTubeID + "'></param><param name='wmode' value='transparent'></param><embed src='http://www.youtube.com/v/" + YouTubeID + "' type='application/x-shockwave-flash' wmode='transparent' width='425' height='355'></embed></object>";
    }
}