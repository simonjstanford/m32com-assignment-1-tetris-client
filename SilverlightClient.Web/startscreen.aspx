<%@ Page Title="Start Game" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="startscreen.aspx.cs" Inherits="startscreen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" Runat="Server">

    Please enter your name:
        <asp:TextBox ID="TextBoxName" runat="server" Width="127px" ValidationGroup="NameValidation"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxName" ErrorMessage="Your name is needed in case you get a high score. Please enter it!" ValidationGroup="NameValidation" Font-Bold="True" ForeColor="#CC0000"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Button ID="ButtonPlay" runat="server" OnClick="ButtonPlay_Click" Text="Play!" ValidationGroup="NameValidation" />

</asp:Content>

