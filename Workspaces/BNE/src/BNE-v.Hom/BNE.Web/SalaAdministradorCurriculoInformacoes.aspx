<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="SalaAdministradorCurriculoInformacoes.aspx.cs" Inherits="BNE.Web.SalaAdministradorCurriculoInformacoes" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphExperimentos" runat="server">
  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">

    <link href="../css/local/SalaAdministradorInformacoesCurriculo.css" rel="stylesheet" />
    <link href="../css/local/Bootstrap/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    
    <script src="../js/bootstrap/bootstrap-datetimepicker.min.js"></script>
    <script src="../js/bootstrap/bootstrap-datetimepicker.pt-BR.js"></script>
    <script src="../js/local/Forms/SalaAdministrador/SalaAdministradorInfoCurriculo.js"></script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphConteudo" runat="server">

    <!--    top nav end===========-->

    <asp:UpdatePanel ID="upInformacoes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="container-2">
                <div id="page-wrapper">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="page-title">
                                <asp:UpdatePanel runat="server" ID="upDtas" UpdateMode="Conditional">
                                    <ContentTemplate>
                                          <asp:HiddenField runat="server" ID="hdfDtaFim" ClientIDMode="Static" />
                                          <asp:HiddenField runat="server" ID="hdfDtaInicio" ClientIDMode="Static" />
                                            <asp:HiddenField runat="server" ID="hdfCpf" ClientIDMode="Static" />
                                            <asp:HiddenField runat="server" ID="hdfCV" ClientIDMode="Static" />
                                        <h2>
                                            <asp:Label runat="server" ID="lblNome"></asp:Label></h2>
                                        <div class="breadcrumb">
                                            <div class='col-sm-3'>
                                                <div class='input-group date' id='datetimepicker1'>
                                                    <asp:TextBox runat="server" ID="txtDataInicio" onchange="upDate('Inicio');" onclick='$("#txtDataInicio").datetimepicker("show");' ReadOnly placeholder="Data de Inicio" Style="padding: 6px;" class="form-control" ClientIDMode="Static"></asp:TextBox>
                                                    <span class="input-group-addon" onclick='$("#txtDataInicio").datetimepicker("show");'><span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class='col-sm-3'>
                                                <div class='input-group date' id='datetimepicker2'>
                                                    <asp:TextBox runat="server" ID="txtDataFim" onchange="upDate('Fim');" onclick='$("#txtDataFim").datetimepicker("show");' ReadOnly placeholder="Data Fim " Style="padding: 6px;" class="form_datetime" ClientIDMode="Static"></asp:TextBox>
                                                    <span class="input-group-addon" onclick='$("#txtDataFim").datetimepicker("show");'><span class="glyphicon glyphicon-calendar"></span>
                                                    </span>
                                                </div>
                                            </div>

                                            <asp:LinkButton runat="server" ID="lnkFiltro" CssClass="btn btn-default btn-lg" OnClick="lnkFiltro_Click" Style="padding: 5px;"><i class="fa fa-search"></i></asp:LinkButton>
                                            <asp:LinkButton runat="server" ID="lnkVoltar" OnClick="lnkVoltar_Click" CssClass="btn btn-default pull-right" ><i class="fa fa-arrow-left" aria-hidden="true"></i> Voltar</asp:LinkButton>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-3 col-sm-6">
                            <div class="circle-tile">
                                <a href="#">
                                    <div class="circle-tile-heading dark-blue">
                                        <i class="fa fa-address-book-o fa-fw fa-3x"></i>
                                    </div>
                                </a>
                                <div class="circle-tile-content dark-blue">
                                    <div class="circle-tile-description text-faded">
                                        Vagas Candidatadas
                               
                                    </div>
                                    <div class="circle-tile-number text-faded">
                                        <asp:Label runat="server" ID="lblCandidaturas"></asp:Label>
                                        <span id="sparklineA"></span>
                                    </div>
                                    <a onclick="ExibirCandidaturas(1);" id="btnInformacoesCand" style="cursor: pointer;" class="circle-tile-footer">Mais Informações <i class="fa fa-chevron-circle-right"></i></a>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-sm-6">
                            <div class="circle-tile">
                                <a href="#">
                                    <div class="circle-tile-heading blue">
                                        <i class="fa fa-briefcase fa-fw fa-3x"></i>
                                    </div>
                                </a>
                                <div class="circle-tile-content blue">
                                    <div class="circle-tile-description text-faded">
                                        Empresas Visualizaram
                               
                                    </div>
                                    <div class="circle-tile-number text-faded">
                                        <asp:Label runat="server" ID="lblEmpresaVisualizaram"></asp:Label>
                                        <span id="sparklineB"></span>
                                    </div>
                                    <a onclick="ExibirVisualizacoes(1);"  id="btnInformacaoesVisualizacao" style="cursor: pointer;" class="circle-tile-footer">Mais Informações  <i class="fa fa-chevron-circle-right"></i></a>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-sm-6">
                            <div class="circle-tile">
                                <a href="#">
                                    <div class="circle-tile-heading green">
                                        <i class="fa fa-share fa-fw fa-3x"></i>
                                    </div>
                                </a>
                                <div class="circle-tile-content green">
                                    <div class="circle-tile-description text-faded">
                                        Empresas que enviou o currículo
                               
                                    </div>
                                    <div class="circle-tile-number text-faded">
                                        <asp:Label runat="server" ID="lblEmpresaEnviadaCV"></asp:Label>
                                    </div>
                                    <a onclick="ExibirEnvioEmpresa(1);"  id="btnInformacaoesEnvioCV" class="circle-tile-footer" style="cursor: pointer;">Mais Informações <i class="fa fa-chevron-circle-right"></i></a>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-sm-6">
                            <div class="circle-tile">
                                <a href="#">
                                    <div class="circle-tile-heading orange">
                                        <i class="fa fa-bell fa-fw fa-3x"></i>
                                    </div>
                                </a>
                                <div class="circle-tile-content orange">
                                    <div class="circle-tile-description text-faded">
                                        Alerta de Vagas
                               
                                    </div>
                                    <div class="circle-tile-number text-faded">
                                        <asp:Label runat="server" ID="lblQtdFuncoes"></asp:Label> Funcões /
                                        <asp:Label runat="server" ID="lblQtdCidades"></asp:Label> Cidades
                                    </div>
                                    <a onclick="ExibirAlertaVaga();" id="btnInformacaoesAlerta" class="circle-tile-footer" style="cursor: pointer;">Mais Informações <i class="fa fa-chevron-circle-right"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>


                </div>
                <!-- page-wrapper END-->
            </div>





            <!-- CANDIDATURAS-->

            <div id="divCandidaturas" style="display: none;">
                <div class="container">
                    <div class="row">
                        <div class="col-md-12">

                            <h4>
                                <div id="divTitulo"></div>
                            </h4>
                            <div class="table-responsive">
                                <table id="mytable" class="table table-bordred table-striped table-hover">
                                    <thead>
                                        <th>Cod Vaga</th>
                                        <th>Função</th>
                                        <th>Cidade</th>
                                        <th>Data Candidatura</th>
                                        <th>Link</th>
                                        <th>Status da Vaga</th>
                                    </thead>
                                    <tbody id="tbRegistros">
                                    </tbody>
                                    <div class="clearfix"></div>

                                    <ul class="pagination pull-right" id="divPaginacao">
                                    </ul>

                                    <%--<ul class="pagination pull-right">
                                <li class="disabled"><a href="#"><span class="glyphicon glyphicon-chevron-left"></span></a></li>
                                <li class="active"><a href="#">1</a></li>
                                <li><a href="#">2</a></li>
                                <li><a href="#">3</a></li>
                                <li><a href="#">4</a></li>
                                <li><a href="#">5</a></li>
                                <li><a href="#"><span class="glyphicon glyphicon-chevron-right"></span></a></li>
                                   
                            </ul>--%>
                                </table>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <%--EMPRESAS VISUALIZADAS--%>

            <div id="divEmpresasVisualizadas" style="display: none;">
                <div class="container">
                    <div class="row">
                        <div class="col-md-12">

                            <h4>
                                <div id="divTituloVisualizado"></div>
                            </h4>
                            <div class="table-responsive">
                                <table id="mytableVisualizado" class="table table-bordred table-striped table-hover">
                                    <thead>
                                        <th>Empresa</th>
                                        <th>Data De Visualização</th>
                                    </thead>
                                    <tbody id="tbRegistrosVisualizados">
                                    </tbody>
                                    <div class="clearfix"></div>

                                    <ul class="pagination pull-right" id="divPaginacaoVisualizados">
                                    </ul>
                                </table>
                            </div>

                        </div>
                    </div>
                </div>
            </div>


            <%--EMPRESAS Enviou curriculo--%>

            <div id="divEmpresasEnvio" style="display: none;">
                <div class="container">
                    <div class="row">
                        <div class="col-md-12">

                            <h4>
                                <div id="divTituloEmpresaEnvio"></div>
                            </h4>
                            <div class="table-responsive">
                                <table id="mytableEmpresaEnvio" class="table table-bordred table-striped table-hover">
                                    <thead>
                                        <th>Empresa</th>
                                        <th>Data de Envio</th>
                                    </thead>
                                    <tbody id="tbRegistrosEmpresaEnvio">
                                    </tbody>
                                    <div class="clearfix"></div>

                                    <ul class="pagination pull-right" id="divPaginacaoEmpresaEnvio">
                                    </ul>
                                </table>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <%--EMPRESAS ALERTAS DE VAGA--%>

            <div id="divAlertaCvs" style="display: none;">
                <div class="container">
                    <div class="row">
                        <div class="col-md-12">

                            <h4>
                                <div id="divTituloAlertaCvs">Alerta De Vagas</div>
                            </h4>
                            <div class="table-responsive " id="divAlertaDiaSemana">
                                <div class=" btn-group " >
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <h4>
                                <div>Funções</div>
                            </h4>
                            <div class="table-responsive " id="divAlertaFuncoes">
                                <div class=" btn-group " >
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">

                            <h4>
                                <div>Cidades</div>
                            </h4>
                            <div class="table-responsive " id="divAlertaCidade">
                                <div class=" btn-group " >
                                </div>
                            </div>
                        </div>

                    </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
