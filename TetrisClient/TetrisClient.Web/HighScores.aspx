<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HighScores.aspx.cs" Inherits="TetrisClient.Web.HighScores" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="HighScoresGridView" runat="server" AllowSorting="True"></asp:GridView>
    </div>
    </form>
</body>
</html>
