<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="Campanha.aspx.cs" Inherits="BNE.Web.Campanha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphExperimentos" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <script type="text/javascript">
        $(document).ready(
            function () {
                var pathCampanha = '/campanha/';
                var pathName = window.location.pathname;
                if (pathName.indexOf(pathCampanha) >= 0) {
                    var campanha = pathName.replace(pathCampanha, '');
                    $('#' + campanha).show();
                }
            }
        );
    </script>
    <style>
        #vagarapida {
            padding-top:32px;
            display:flex;
            flex-direction:column;
            align-items:center;
            justify-content:center;
        }
        .campanha__title { 
            text-align:center;
        }
        .campanha__actions {
            margin-top:32px;
        }
        .campanha__actions a + a{
            margin-top:8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphConteudo" runat="server">
    <div id="vagarapida" class="container_small" style="display: none;" >
        <h1 class="campanha__title">Por questões de segurança, é necessário que você esteja <strong>cadastrado</strong> para visualizar os currículos recomendados para sua vaga.</h1>
        <div class="campanha__img"><img src="/img/campanha/campanha-vaga-rapida.png" class="img-responsive"/></div>
        <div class="campanha__actions">           
            <asp:LinkButton runat="server" ID="btnVagaRapidaCadastrarAgora" OnClick="btnVagaRapidaCadastrarAgora_Click" OnClientClick="trackEvent('Campanha','Click','CadastrarAgora_Botao'); return true;">
                <div class="btn btn-full btn-primary">Cadastrar Agora</div>
            </asp:LinkButton>
             <asp:LinkButton runat="server" ID="btnVagaRapidaNaoQueroCadastrar" OnClick="btnVagaRapidaNaoQueroCadastrar_Click" OnClientClick="trackEvent('Campanha','Click','NaoQueroCadastrar_Botao'); return true;">
                <div class="btn btn-full btn-default">Não quero cadastrar</div>
            </asp:LinkButton>
        </div>
        
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
