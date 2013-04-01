<%@ Page Title="Documentation" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="documentation.aspx.cs" Inherits="documentation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="Server">

    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <p>
        <span class="label">Original brief: </span><a href="documentation/M32COM_CW_Group.pdf">M32COM_CW_Group.pdf</a>
    </p>
    <p>
        <span class="label">Draft plan: </span><a href="documentation/Draft%20Plan.pdf">Draft Plan.pdf</a>
    </p>
    <p>
        <span class="label">Presentation: </span>
    </p>
    <p>
        <span class="label">Supporting document: </span>
    </p>
    <p>
        <span class="label">Source code: </span>
    </p>

</asp:Content>

