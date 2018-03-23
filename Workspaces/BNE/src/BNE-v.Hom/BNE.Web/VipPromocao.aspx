<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/Principal.Master" CodeBehind="VipPromocao.aspx.cs" Inherits="BNE.Web.VipPromocao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link href="//fonts.googleapis.com/css?family=Open+Sans+Condensed:300" rel='stylesheet' type='text/css' />
    <link href="http://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet" type="text/css">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/VipTelaMagica.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:LinkButton ID="lnkBtn" runat="server" OnClick="btnContinuar_Click">
        <div class="containerImgVipPromocao"></div>
       <h1 class="promocao_vip_nome_candidato" id="ltNomeCandidato"></h1>
    </asp:LinkButton>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var dadosCandidato = $('#pnlNomeUsuarioLogado').html();
                var nomeCandidato = dadosCandidato.split(',');
                $('#ltNomeCandidato').html(nomeCandidato[1]);
        });
</script>
</asp:Content>
