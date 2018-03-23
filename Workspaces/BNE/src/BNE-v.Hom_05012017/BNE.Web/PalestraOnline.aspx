<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="PalestraOnline.aspx.cs" Inherits="BNE.Web.PalestraOnline" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;</div>
        <div id="divPlayer" style="width: 720px; height: 405px; position: relative; text-align: center;">
            <div style="width: 720px; height: 405px; position: absolute; top: 0; left: 0; background: #000;">
                <div id="svp_no_flash_layer5jc4w7b8gf0g" style="margin: auto; position: absolute;
                    top: 50%; left: 50%; margin-left: -150px; margin-top: -50px; color: #fff; width: 300px;
                    height: 120px; text-align: center; font-size: 11px; font-family: Arial, Helvetica, sans-serif;
                    display: none;">
                    <!--googleoff: index-->
                    <p align="left" style="font-size: 11px;">
                        <a href="http://get.adobe.com/flashplayer/" target="_blank">
                            <img border="0" src="http://play.webvideocore.net/img/flash_icon.png" style="float: left;
                                width: 80px; margin-right: 11px;" /></a> <b style="font-size: 14px;">ADOBE FLASH PLAYER</b><br />
                        You are either missing or need to update your <b>ADOBE Flash Player</b> in order
                        to see this video .<br />
                        <br />
                        <a style="color: #3ac9f8; text-decoration: none; font-size: 11px; font-weight: bold;"
                            href="http://get.adobe.com/flashplayer/" target="_blank">Download Now</a>
                    </p>
                    <!--googleon: index-->
                </div>
            </div>
        </div>
    </div>
    <script language="javascript" type="text/javascript" src="http://play.webvideocore.net/js/dplayer.js"></script>
    <script language="javascript" type="text/javascript">
<!--
        var vars = { clip_id: "5jc4w7b8gf0g", player_color1: "#c8c8c8", player_color: "#a6a6a6", transparent: "false", pause: "0", repeat: "", bg_color: "#FFFFFF", fs_mode: "2", no_fullscreen: "0", no_controls: "", start_img: "0", start_volume: "100", close_button: "", brand_new_window: "1", auto_hide: "1", prebuffer: "", stretch_video: "", player_align: "NONE", offset_x: "", offset_y: "", bg_transp: "", player_color_ratio: 0.6, skinAlpha: "80", colorBase: "#202020", colorIcon: "#FFFFFF", colorHighlight: "#fcad37", direct: "false", is_responsive: "true" };
        var svp_player = new SVPDynamicPlayer("divPlayer", "", "720", "405", { use_div: "divPlayer", skin: "2" }, vars);
        svp_player.execute();
//-->
    </script>
    <noscript>
        Your browser does not support JavaScript! JavaScript is needed to display this video
        player!</noscript>
    <!--player code end-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
