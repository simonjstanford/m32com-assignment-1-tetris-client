<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TetrisClient.Web.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Name:
        <asp:TextBox ID="TextBoxName" runat="server" Width="127px" ValidationGroup="NameValidation"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxName" ErrorMessage="RequiredFieldValidator" ValidationGroup="NameValidation"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:Button ID="ButtonPlay" runat="server" OnClick="ButtonPlay_Click" Text="Play!" ValidationGroup="NameValidation" />
    </div>
    </form>
</body>
</html>
