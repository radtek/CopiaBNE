<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucWebCallBack.ascx.cs" Inherits="BNE.Web.UserControls.Modais.ucWebCallBack" %>

<%@ Register Src="ucWebCallBack_Modais.ascx" TagName="WebCallBack_Modais" TagPrefix="uc" %>

<div class="containerNosLigamos" style="text-align: center">
    <button id="modalzinha" runat="server" type="button" class="btn-atendimentoOnline" data-toggle="modal" data-target="#nomeDaModal">
        <i class="fa fa-phone fa-3x"></i>
        <br>
        <small>NÓS LIGAMOS<br>
            PARA VOCÊ!</small>
    </button>
</div>

<uc:WebCallBack_Modais ID="ucWebCallBack_Modais" runat="server" />
