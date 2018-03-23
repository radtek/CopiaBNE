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
        nacional. São <strong>56</strong> unidades distribuídas
        em <strong>9 estados</strong> do Brasil.
    
    
    </div>
    <div class="containerTextoPhone">
        <span class="containerTextoLigue">Para entrar em contato com o BNE ligue para</span>
        <span class="containerPhone0800">0800 41 2400</span>
    </div>
    <div class="linha"></div>
    <div class="scrollEstados">
        <div class="containerTextoEstados">

            <span class="enfatizaEstados">Mato Grosso - MT</span>

            Campo Verde
            Cuiabá
            Lucas do Rio Verde
            Primavera do Leste
            Rondópolis
            Sorriso
                        
            <span class="enfatizaEstados">Mato Grosso do Sul - MS</span>

            Dourados
                       
            <span class="enfatizaEstados">Minas Gerais - MG</span>

            Belo Horizonte
            Uberlândia
            
            <span class="enfatizaEstados">Paraná - PR</span>

            Campo Mourão
            Cascavel
            Curitiba
            Guarapuava
            Londrina
            Maringá
            Paranaguá
            Pato Branco
            Ponta Grossa
            São josé dos Pinhais
            
            <span class="enfatizaEstados">Pernambuco - PE</span>

            Recife

            <span class="enfatizaEstados">Rio de Janeiro - RJ</span>

            Macaé
            Rio de Janeiro
            
            <span class="enfatizaEstados">Rio Grande do Sul - RS</span>

            Cruz Alta
            Porto Alegre
            
            <span class="enfatizaEstados">Santa Catarina - SC</span>

            Chapecó
            Florianópolis
            Jaraguá do Sul
            Joinville
            Lages
            Videira
            Xanxerê
            
            <span class="enfatizaEstados">São Paulo - SP</span>

            Campinas
            Jaú
            São Paulo - Barão
            Taubaté
            
        </div>

    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/swfobject-2.2.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/local/OndeEstamos.js" type="text/javascript" />
</asp:Content>
