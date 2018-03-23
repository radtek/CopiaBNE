<%@  language="VBScript" %>
<html>
<head>
    <title>O Teste das Cores</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
      <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <link href="../../css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
    <script language="JavaScript">
<!--

    var seconds = 61;

    function pular() {
        var url = "cores.asp?" + document.clock.pula.value
        window.location.href = url
    }

    function showtime() {
        var now = new Date();
        seconds = seconds - 1
        var timeValue = ""
        timeValue += ((seconds < 10) ? "0" : "") + seconds
        document.clock.face.value = timeValue;
        timerID = setTimeout("showtime()", 1000);
        timerRunning = true;
        if (seconds == 0) {
            clearTimeout(timerID);
            timerRunning = false;
            pular()
        }
    }

    //-->
    </script>
        <link href="css/Bne.css" rel="stylesheet" />
       <style>
                .img
                {
                    position: relative;
                    top: 37%;
                    left: 15%;
                    height: 46px;
                }
            </style>
</head>
<body bgcolor="#FFFFFF" onload="showtime()" topmargin="0" leftmargin="0">

 
    <div align="center">
        <p>
            <img src="images/logo-testedascores.png" height="179" width="314" class="img-responsive"><br>
        </p>
    </div>
    <center>
        <form name="clock">
        <p>
            &nbsp;</p>
        <p>
            <b><font face="sans-serif" size="4" color="#8B8989">Tempo Restante:</font></b>
            
            <input type="hidden" name="pula" value="<%=request.querystring%>">
            <input type="text" name="face" size="3"  value="" style="border-radius:5px;">
                
            <br>
            <br>
            <input type="button"  class="btn btn-primary" value="Próxima" onclick="pular()" style="width:150px; margin-left:7%; height:35px;">
        </p>
        </form>
       
    </center>
        <div id="footer">
        <div class="container">
          <div class="col-md-12 column">
                <p><img src="images/logo_employer.png" class="img-responsive" /><small style="color:#8B8989">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Employer Group © 2015 - All Rights Reserved</small></p>
          </div>
        </div>
    </div>
 
</body>
</html>
