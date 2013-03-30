<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h2>A group project by Rob Bleasdale, Michael Edionwe, Remi Hameed & Simon Stanford</h2>
            </hgroup>
            <p>
                Please click on the links above to play the game using a <mark>Silverlight</mark> client, view the high scores and explore the game development.
            </p>
        </div>
    </section>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h3>This sites demonstrates:</h3>
    <ol class="round">
        <li class="one">
            <h5>A fully functional prototype of the classic game Tetris</h5>
            This prototype allows you to play numerous levels of Tetris.  All functionality is present: shape left/right movement, rotation and quick-drop is achieved using the keyboard keys.  To play the game  <a href="startscreen.aspx">click here</a>.
        </li>
        <li class="two">
            <h5>A web service back-end</h5>
            All game logic resides in a deployed web service, which returns an array of data that represents the game board. Efficiency and game concurrency is achieved through use of Session state.
        </li>
        <li class="three">
            <h5>A Silverlight front-end</h5>
            Silverlight graphics have been employed to enhance the player experience.  This enables smoother game play and asynchronous web service calls.  To view the web service <a href="http://www.simonjstanford.co.uk/tetriswebservice/TetrisWebService.asmx">click here</a>.
        </li>
        <li class="four">
            <h5>Encrypted high scores</h5>
            Scores are encrypted using an RSA encryption algorithm and public key.  To ensure maximum security, the key is stored in the web.config file.
        </li>

    </ol>
</asp:Content>