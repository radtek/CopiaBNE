<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PagamentoDebitoOnlineBB.aspx.cs" Inherits="BNE.Web.PagamentoBB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form action="https://mpag.bb.com.br/site/mpag/" method="post"
        name="pagamento">
        <input runat="server" id="idConv" type="hidden" name="idConv" />
        <input runat="server" id="refTran" type="hidden" name="refTran" />
        <input runat="server" id="valor" type="hidden" name="valor" />
        <input runat="server" id="dtVenc" type="hidden" name="dtVenc" />
        <input runat="server" id="tpPagamento" type="hidden" name="tpPagamento" />
        <input runat="server" id="urlRetorno" type="hidden" name="urlRetorno" />
        <input runat="server" id="urlInforma" type="hidden" name="urlInforma" />
    </form>
    <script type="text/javascript">
            document.pagamento.submit();
    </script>
</body>
</html>
