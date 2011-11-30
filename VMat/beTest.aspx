<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="beTest.aspx.cs" Inherits="VMat.beTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>
        <asp:Button ID="rename" runat="server" onclick="rename_Click" Text="rename" />
        VM Info Test</h1>
    <asp:Button runat="server" ID="MakeServer" Text="MakeServer" 
            onclick="MakeServer_Click" />
        <br />
        <br />
    <asp:Literal ID="mainText" runat="server" />
    </div>
    </form>
</body>
</html>
