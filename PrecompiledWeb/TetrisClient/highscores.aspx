<%@ page title="High Scores" language="C#" masterpagefile="~/Site.master" autoeventwireup="true" inherits="highscores, App_Web_nn5wg2cn" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">

    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <article>
        <asp:GridView ID="HighScoresGridView" runat="server" AllowSorting="True"></asp:GridView>
    </article>

    <aside>
        <h3>Data Encryption</h3>
        <p>        
            High scores are stored by the web service in XML and retrieved by the client. It is encrypted using RSA encryption and a public token.  Please see the <a href="documentation.aspx">documentation</a> for more information.
        </p>
    </aside>


    
</asp:Content>

