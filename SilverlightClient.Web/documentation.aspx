<%@ Page Title="Documentation" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="documentation.aspx.cs" Inherits="documentation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <div id="main2">
            <p>
        <span class="label">Original brief: </span><a href="documentation/M32COM_CW_Group.pdf">M32COM_CW_Group.pdf</a>
    </p>
    <p>
        <span class="label">Draft plan: </span><a href="documentation/Draft%20Plan.pdf">Draft Plan.pdf</a>
    </p>
    <p>
        <span class="label">Supporting document: </span><a href="documentation/Supporting%20Document.pdf">Supporting Document.pdf</a>
    </p>
    <p>
        <span class="label">Sample output: See the youtube video and </span><a href="documentation/Sample%20Output.pdf">Sample Output.pdf</a>
    </p>
    <p>
        <span class="label">Source: </span><a href="https://bitbucket.org/robbleasdale/m32com-assignment-1">Web service, game logic and high scores</a> and <a href="https://bitbucket.org/sstanford/m32com-assignment-1-tetris-client">Silverlight client</a>
    </p>
    </div>

    <div id="leftCol">
        <asp:Label ID="EmbeddedVideo" runat="server"></asp:Label>
    </div>
        





</asp:Content>

