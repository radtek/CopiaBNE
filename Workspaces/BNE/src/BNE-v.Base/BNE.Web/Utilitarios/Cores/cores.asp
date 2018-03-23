<%@  language="VBScript" %>
<!--#INCLUDE File="random.asp"-->
<html>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Expires" content="-1">
<link href="css/Bne.css" rel="stylesheet" />
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<head>
    <title>O Teste das Cores</title>
    <link href="../../css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
    <script src="../../js/bootstrap/bootstrap.js"></script>
     <link type="text/css" href="css/Bne.css" rel="stylesheet" />
 
</head>
<body topmargin="0" leftmargin="0">
<%
    dim order(8)
    dim corOk(2,8)

    if Request.QueryString("level") <> "" then
	    level = cint(Request.QueryString("level"))	
    else
	    level = 0
    end if
    if Request.QueryString("indice") <> "" then
	    indice = cint(Request.QueryString("indice"))
    else
	    indice = 1
    end if

    If level = 0 And indice = 1 Then
	    Nome = "&nome=" & Server.URLEncode(Request.QueryString("nome"))
    Else
	    Nome = ""
    End if

    corRef = "cor[" & indice & "][" & (level + 1) & "]"

    'coloca o conteúdo randômico na matriz 
    if level = 0 then
	    getRandom = fRandom()
	    for i = 1 to 8	
		    order(i) = cInt(mid(getRandom,i,1))
	    next
    else
	    getRandom = request.querystring("order")
	    for i = 1 to 8
		    order(i) = cInt(mid(getRandom,i,1))
	    next
    end if

    'grava as cores selecionadas
    if indice > 1 then
	    if level = 0 then
		    hist = mid(request.querystring,18,len(request.querystring)-1)
	    else
		    hist = mid(request.querystring,33,len(request.querystring)-1) & "&"
	    end if
    else
	    if level > 0 then
		    hist = mid(request.querystring,33,len(request.querystring)) & "&"
	    else
		    hist = ""
	    end if
    end if

    'verifica quais as cores que já foram escolhidas
    for i = 1 to 2
	    for j = 1 to 8
		    for k = 1 to 8
			    if cint(request.querystring("cor[" & i & "][" & j & "]")) = k then
				    corOk(i,k) = true
				    k = 8
			    end if
		    next
	    next
    next

    if indice > 1 then
	    if request.querystring("cor[2][8]") > 0 then
		    response.redirect "resultado.asp?" & hist & "grava=1"
	    end if
    else
	    if request.querystring("cor[1][8]") > 0 then
		    response.redirect "aguarde.asp?level=0&" & "indice=2&" & hist
	    end if
    end if

%>
    <!-- monta a tabela com as cores -->
 <!--   <table width="100%" border="0">
        <tr>
            <td bgcolor="#79A5D2">
                <font face="Verdana" color="#FFFFFF"><b><font size="4">Teste das cores</font></b></font>
            </td>
        </tr>
        <tr>
            <td bgcolor="#C9E1EF">
                <font size="2" color="#000000" face="verdana">Escolha as cores na sequência que você
                    preferir.</font>
            </td>
        </tr>
    </table>-->
    <!--<div style="position: absolute;">
        <img class="imagem_teclado" src="images/teclado_mouse.png" />
    </div>-->

    <div align="center">
        <p>
            <img src="images/logo-testedascores.png" height="179" width="314" class="img-responsive"><br>
        </p>
    </div>
   
        <br>
        <br>

    <%if indice <> 1 then%>
    <center>
        <table width="65%" border="0">
            <tr>
                <td>
                    <p align="center">
                        <font face="sans-serif" color="#8B8989"
                            size="4"><b>Repita</b> o procedimento, anterior clicando sobre a cor escolhida até selecionar todas novamente.</font></br>
                           <font face="sans-serif" color="#8B8989" size="2"> <br /><i>Tente não repetir sua primeira seleção, escolha as cores como se estivesse vendo
                            pela primeira vez.</i></p></font>
                </td>
            </tr>
        </table>
    </center>
    <p>
    </p>
    <%else%>
    <center>
        <table width="65%" border="0">
            <tr>
                <td>
                    <p align="center">
                        <font face="Sans-serif" color="#8B8989" size="4">Para fazer a escolha,<b> clique sobre a cor
                            escolhida</b>, fazendo o mesmo com as demais, até que todas as cores sejam escolhidas.</font></p>
                </td>
            </tr>
        </table>
    </center>
    <%end if%>
    <center>
    <table width="228">
            <% for i = 1 to 2 %>
            <tr>
                <% for j = (1+s) to (4*i) %>
                <td class="td_cores">
                    <!-- só mostra se a cor não foi selecionada -->
                    <% if corOk(indice,order(j)) = false then %>
                    <a href="cores.asp?level=<%=level+1%>&amp;indice=<%=indice%>&amp;order=<%=getRandom%>&amp;<%=hist%><%=corRef%>=<%=order(j)%><%=Nome%>">
                        <img src="images/<%=order(j)%>.gif" class="cores" ></a>
                    <% else %>
                    <img src="images/0.gif" class="cores">
                    <% end if %>
                </td>
                <% next %>
            </tr>
            <% s = 4 %>
            <% next %>
        </table>
        </center>
    <!--Include file="cia_menu_bnenovo.htm"-->
  
    <div id="footer">
        <div class="container">
          <div class="col-md-12 column">
                <p><img src="images/logo_employer.png" class="img-responsive" /><small style="color:#8B8989">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Employer Group © 2015 - All Rights Reserved</small></p>
          </div>
        </div>
    </div>
   <!--  <div >
        <img class="caderneta" src="images/caderneta.png"><br>
    </div>-->
</body>
</html>
