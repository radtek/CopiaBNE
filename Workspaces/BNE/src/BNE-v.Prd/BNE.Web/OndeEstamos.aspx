<%@ Page Title="Onde Estamos" Language="C#" MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true" CodeBehind="OndeEstamos.aspx.cs" Inherits="BNE.Web.OndeEstamos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/OndeEstamos.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="containerImgMapa"></div>

    <div class="containerTextoOndeEstamos">
        <span class="headPresencaNacional">Presença Nacional</span>
        Somos uma empresa de abrangência
        nacional. São <strong>30</strong> unidades distribuídas
        estrategicamente pelo Brasil.
    
    
    </div>
    <div class="containerTextoPhone">
        <span class="containerTextoLigue">Para entrar em contato com o BNE ligue para</span>
        <span class="containerPhone0800">0800 41 2400</span>
    </div>
    
    <div class="bg_list_filial">
	<ul class="menu_onde">
		<li class="filiais_bne">
    	<span class="enfatizaEstados">Distrito Federal - DF</span>
    	<Br>
            Brasília
		</li>
		<li class="filiais_bne">	
     	<span class="enfatizaEstados">Goiás - GO</span>
     	<Br>
            Rio Verde<Br>
            Formosa
        </li>
        <li class="filiais_bne">   
    	<span class="enfatizaEstados">Mato Grosso - MT</span>
    	<Br>
            Rondópolis<Br>
            Sorriso
        </li>
        <li class="filiais_bne">               
        <span class="enfatizaEstados">Mato Grosso do Sul - MS</span>
        <Br>
            Campo Grande<Br>
            Dourados
        </li>
        <li class="filiais_bne">             
        <span class="enfatizaEstados">Minas Gerais - MG</span>
        <Br>
            Belo Horizonte<Br>
            Uberlândia
        </li>
        <li class="filiais_bne">   
        <span class="enfatizaEstados">Paraná - PR</span>
        <Br>
            Campo Mourão<Br>
            Cascavel<Br>
            Curitiba<Br>
            Guarapuava<Br>
            Londrina<Br>
            Maringá<Br>
            Paranaguá<Br>
            Pato Branco<Br>
            Ponta Grossa<Br>
            Rio Negro
        </li>
        <li class="filiais_bne">   
        <span class="enfatizaEstados">Pernambuco - PE</span>
        <Br>
            Recife
        </li>
        <li class="filiais_bne">
        <span class="enfatizaEstados">Rio de Janeiro - RJ</span>
        <Br>
            Rio de Janeiro
            
        </li>  
        <li class="filiais_bne">  
        <span class="enfatizaEstados">Santa Catarina - SC</span>
		<Br>
            Florianópolis<Br>
            Itápolis<Br>
            Jaraguá do Sul<Br>
            Joinville<Br>
            Lages<Br>
            Xanxerê
        </li>
       	<li class="filiais_bne">  
        <span class="enfatizaEstados">São Paulo - SP</span>
        <Br>
            Jaú<Br>
            São Paulo - Barão<Br>
            Taubaté
        </li>
</ul>
	<div id="controller"> <a href="#" class="next"><i class="fa fa-sort-desc"></i></a>

	</div>	
</div>	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/swfobject-2.2.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/local/OndeEstamos.js" type="text/javascript" />
</asp:Content>
