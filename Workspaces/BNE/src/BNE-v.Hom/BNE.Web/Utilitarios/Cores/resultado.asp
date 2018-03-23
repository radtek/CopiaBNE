<%@  language="VBScript" %>
<!--#INCLUDE File="ConnBD.asp"-->
<html>
<head>
    <link href="../../css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
    <link type="text/css" href="css/Bne.css" rel="stylesheet" />
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <script type="text/javascript" src="../../js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
            m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        var uri = document.baseURI || document.URL;
        var baseAddress = uri.replace("http://", "").replace("https://", "");
        if (baseAddress.indexOf("www.bne.com.br") == 0 || baseAddress.indexOf("bne.com.br") == 0)
            ga('create', 'UA-1937941-6', 'auto');
        else
            ga('create', 'UA-1937941-8', 'auto');

        ga('send', 'pageview');

        function trackEvent(category, action, label) {
            try {
                ga('send', 'event', category, action, label);
            } catch (err) { }
        }


        function imprimir() {
            $('.imprimir').css('display', 'none');

            window.print();
            $('.imprimir').css('display', 'block');
        };

        function BNE() {
            location.href = "../../Default.aspx"
        }
    </script>

    <title>O Teste das Cores</title>
    <style>
        .h {
            margin-left: 16%;
            font-family: "proxima-nova",sans-serif;
            color: #808080;
        }

        .justificar {
            margin-left: 130%;
            font-weight: normal;
            font-size: 10pt;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            text-align: justify;
        }

        .caderneta {
            left: 100%;
            margin-left: -350px;
            margin-top: -250px;
            top: auto;
            position: absolute;
            width: 350px;
            height: 350px;
        }

        .logo {
            margin-top: 3%;
            position: absolute;
            margin-left: 16%;
            left: 0px;
            height: 46px;
        }

        .imagem_teclado {
            width: 250px;
            height: 250px;
        }
    </style>
</head>
<body leftmargin="0" topmargin="0" style="background-color: #e3e8ed;" class=" @print">


    <div align="center" class="imprimir">
        <p>
            <img id="top" src="images/logo-testedascores.png" height="179" width="314" class="img-responsive"><br>
        </p>
    </div>
    <br>
    <br>

    <h3 class="h ">Resultado!</h3>

    <div class="imp" id="txt_result">
        <table width="60%" border="0" style="margin-left: 16%; max-height: 20%; overflow: auto;">
            <tr>
                <td>
                    <font color="#006DCC" face="Sans-serif" size="5"><b>
                        <%=Request.QueryString("nome")%></b></font>
                    <br>
                    <br>
                </td>
                <tr>
                    <td class="justificar">
                        <%
dim seq(2,5)

seq(1,1) = "+" & cint(request.querystring("cor[1][1]")) -1 & "+" & cint(request.querystring("cor[1][2]")) -1
seq(1,2) = "X" & cint(request.querystring("cor[1][3]")) -1 & "X" & cint(request.querystring("cor[1][4]")) -1
seq(1,3) = "=" & cint(request.querystring("cor[1][5]")) -1 & "=" & cint(request.querystring("cor[1][6]")) -1
seq(1,4) = "-" & cint(request.querystring("cor[1][7]")) -1 & "-" & cint(request.querystring("cor[1][8]")) -1
seq(1,5) = "+" & cint(request.querystring("cor[1][1]")) -1 & "-" & cint(request.querystring("cor[1][8]")) -1

seq(2,1) = "+" & cint(request.querystring("cor[2][1]")) -1 & "+" & cint(request.querystring("cor[2][2]")) -1
seq(2,2) = "X" & cint(request.querystring("cor[2][3]")) -1 & "X" & cint(request.querystring("cor[2][4]")) -1
seq(2,3) = "=" & cint(request.querystring("cor[2][5]")) -1 & "=" & cint(request.querystring("cor[2][6]")) -1
seq(2,4) = "-" & cint(request.querystring("cor[2][7]")) -1 & "-" & cint(request.querystring("cor[2][8]")) -1
seq(2,5) = "+" & cint(request.querystring("cor[2][1]")) -1 & "-" & cint(request.querystring("cor[2][8]")) -1


for i = 1 to 5
	SQL = "SELECT * FROM BNE_TESTE_CORES WHERE "
	SQL = SQL & "titulo='" & seq(1,i) & "' OR " 
	SQL = SQL & "titulo='" & seq(2,i) & "'"
	Set rs = conn.Execute(SQL)

	x = 0

	do while not rs.eof
		if x = 0 then
			if rs("titulo") = seq(1,1) or rs("titulo") = seq(2,1) then
				response.write "<font color=#006DCC face=Sans-serif size=2><b>&bull;&nbsp;" & "Como você opera, age, frente aos seus objetivos e desejos:" & "</b></font>" & "<BR>"
			elseif rs("titulo") = seq(1,2) or rs("titulo") = seq(2,2) then
				response.write "<font color=#006DCC face=Sans-serif size=2><b>&bull;&nbsp;" & "Suas preferências reais:" & "</b></font>" & "<BR>"
			elseif rs("titulo") = seq(1,3) or rs("titulo") = seq(2,3) then
				response.write "<font color=#006DCC face=Sans-serif size=2><b>&bull;&nbsp;" & "Sua situação real:" & "</b></font>" & "<BR>"
			elseif rs("titulo") = seq(1,4) or rs("titulo") = seq(2,4) then
				response.write "<font color=#006DCC face=Sans-serif size=2><b>&bull;&nbsp;" & "O que você quer evitar:" & "</b></font>" & "<BR>"
			elseif rs("titulo") = seq(1,5) or rs("titulo") = seq(2,5) then
				response.write "<font color=#006DCC face=Sans-serif size=2><b>&bull;&nbsp;" & "Seu problema real:" & "</b></font>" & "<BR>"
			end if
		end if
			response.write rs("texto") & "<BR><BR>"
		rs.movenext
		x = 1
	loop
next

                        %>
                    </td>
                </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="imprimir" align="center">
                    <img src="images/btn-bne.png" onclick="javascript: BNE(); trackEvent('Teste das Cores','Click','resultado botao Voltar para o BNE');" id="btnBNE" alt="Voltar para o BNE" name="btiBne" style="cursor: pointer;" />&nbsp;&nbsp;
                    <img src="images/btn-imprimir.png" name="btiImprimir" id="btnImprimir" alt="Imprimir" onclick="imprimir(); trackEvent('Teste das Cores','Click','resultado botao Imprimir resultado');" style="cursor: pointer;" />&nbsp;&nbsp;
                    <!--                    <img src="images/btn-email.png" name="btiImprimir" id="btnImprimir" alt="Imprimir" onclick="imprimir()" style="cursor: pointer;" />-->
                    <a href="http://www.bne.com.br/vagas-de-emprego" class="VejaVagasBNE VejaVagasBNE_AjusteResultado" target="_blank" title="Vagas de Emprego" onclick="trackEvent('Teste das Cores', 'Click', 'resultado link BNE');">
                        <b>Vagas de Emprego</b></a>
                </td>
            </tr>
        </table>

        <div class="logo imprimir">
            <img id="logo" src="images/logo_employer.png"  />&nbsp;&nbsp;&nbsp;&nbsp;
            <font face="sans-serif" color="#8B8989" size="2">Employer Group © 2015 - All Rights Reserved</font>
        </div>
    </div>
    <!--Include file="cia_menu_bnenovo.htm"-->

    <br>
</body>
</html>
