<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebPage.aspx.cs" Inherits="ServiceManagement_WebApp.WebPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Service Monitoring</title>

    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <link href="css/style.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">

        <div class="container-fluid table-bordered">
            <div class="row">
                <div class="col-md-12">
                    <div class="page-header text-left text-success">
                        <h3>Application Title
				</h3>
                        <br class="border-right-0" />
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="page-header text-left text-success">
                        <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn-link btn-sm" OnClick="btnRefresh_Click" />
                    </div>
                </div>
            </div>
            <div class="row border">
                <div class="col-md-6">
                    <h4 class="text-left text-danger">Non-Production Environments
                    </h4>
                    <div class="small">
                        <asp:GridView ID="grdPreProdDetails" runat="server" CellPadding="3" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>

                </div>
                <div class="col-md-6">
                    <h4 class="text-left text-danger">Production Environments
                    </h4>
                    <div class="small">
                        <asp:GridView ID="grdPrdDetails" runat="server" CellPadding="3" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            <PagerStyle ForeColor="#000066" HorizontalAlign="Left" BackColor="White" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle Font-Size="X-Small" BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <script src="js/jquery.min.js"></script>
        <script src="js/bootstrap.min.js"></script>
        <script src="js/scripts.js"></script>
    </form>
</body>
</html>
