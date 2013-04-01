<%@ Page Title="High Scores" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="highscores.aspx.cs" Inherits="highscores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <article>
        <asp:GridView ID="HighScoresGridView" runat="server"></asp:GridView>
    </article>

    <aside>
        <h3>Data Encryption</h3>
        <p>
            High scores are stored by the web service in XML and retrieved by the client. It is encrypted using RSA encryption and a public token.  Please see the <a href="documentation.aspx">documentation</a> for more information.
        </p>
    </aside>

</asp:Content>

