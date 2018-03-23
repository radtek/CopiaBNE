<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBreadCrumbs.ascx.cs" Inherits="BNE.Web.UserControls.ucBreadCrumbs" %>
<link href="../css/local/UserControls/Breadcrumbs.css" rel="stylesheet" />
<script src="../js/jquery.jcarousel.min.js"></script>
<script src="../js/local/UserControls/Breadcrumbs.js"></script>
<nav id="navBreadcrumbForms" runat="server" class="menu_breadcrumb" ClientIDMode="Static">
    <ol id="breadcrumbList" runat="server" class="breadcrumb" itemscope itemtype="http://schema.org/BreadcrumbList">
        <asp:Literal runat="server" ID="links"></asp:Literal>
        <asp:Literal runat="server" ID="nmePagina"></asp:Literal>
    <%--    <asp:Literal runat="server" ID="video"></asp:Literal>--%>
        <li runat="server" id="liVideo" visible="false"  itemprop="itemListElement" itemscope="" itemtype="http://schema.org/ListItem">
                     <div id="openVideoTutorial" onclick="openModal(); return false;"  > <img src="/img/video/video-play.png" /></div>
                     <div id="openVideoTutorial-tooltip">Instruções em Vídeo</div> 

        </li>
    </ol>
</nav>
<asp:HiddenField runat="server" ID="hdfVideo" />
	<div id="frame-modal" class="modal" style="display:none;"  >
		<!-- START- Modal Premium -->
		<div id="modal-container">			
			<div class="modal-content">						
				<div id="mainvideoPlayer">
					<asp:HtmlIframe width="580" runat="server" id="videoIf" height="330" class="setVideo" frameborder="0" allowfullscreen="true"></asp:HtmlIframe>
				</div>
				<div id="videoList" class="jcarousel">
				    <ul>
				        <li>
				        	<img id="ifTitulo1" src="https://img.youtube.com/vi/y8eaGzSCTR0/maxresdefault.jpg" onclick="setVideo('https://www.youtube.com/embed/y8eaGzSCTR0?enablejsapi=1&html5=1');">
				        	<p>Instruções de uso BNE - Pesquisa de Currículos</p>
				        </li>
				        <li>
				        	<img src="https://img.youtube.com/vi/v_lwNKn2vNA/maxresdefault.jpg" onclick="setVideo('https://www.youtube.com/embed/v_lwNKn2vNA?enablejsapi=1&html5=1');">
				        	<p>Instruções de uso BNE - Resultado de pesquisas e ações.</p>
				        </li>
				        <li>
				        	<img src="https://img.youtube.com/vi/YHh3uWlEEXY/maxresdefault.jpg" onclick="setVideo('https://www.youtube.com/embed/YHh3uWlEEXY?enablejsapi=1&html5=1');">
				        	<p>Instruções de uso BNE - Ações </p>
				        </li>
				        <li>
				        	<img src="https://img.youtube.com/vi/mxw9QE5rbeQ/maxresdefault.jpg" onclick="setVideo('https://www.youtube.com/embed/mxw9QE5rbeQ?enablejsapi=1&html5=1');">
				        	<p>Instruções de uso BNE - Anúncio de Vaga</p>
				        </li>
				        <li>
				        	<img src="https://img.youtube.com/vi/BN7esBEo9js/maxresdefault.jpg" onclick="setVideo('https://www.youtube.com/embed/BN7esBEo9js?enablejsapi=1&html5=1');">
				        	<p>Instruções de uso BNE - Facebook</p>
				        </li>
                        <li>
				        	<img src="https://img.youtube.com/vi/ZRr6ebn4ne4/maxresdefault.jpg" onclick="setVideo('https://www.youtube.com/embed/ZRr6ebn4ne4?enablejsapi=1&html5=1');">
				        	<p>Instruções de uso BNE - Sala da Selecionadora</p>
				        </li>
                        <li>
				        	<img src="https://img.youtube.com/vi/zWZUCeFn5to/maxresdefault.jpg" onclick="setVideo('https://www.youtube.com/embed/zWZUCeFn5to?enablejsapi=1&html5=1');">
				        	<p>Instruções de uso BNE - Chat</p>
				        </li>
				    </ul>
				</div>
				<div id="videoList-prev" class="jcarousel-prev"> 
					<i class="fa fa-chevron-circle-left fa-lg"></i>
	            </div>
	            <div id="videoList-next" class="jcarousel-next">  
					<i class="fa fa-chevron-circle-right fa-lg"></i>
	            </div>
	            <span class="closemodal" id="closePlayer" onclick="closeModal();"><i class="fa fa-times-circle"></i></span>
			</div>	
			
		</div>
		<!-- END - Modal Premium -->
	</div>
	