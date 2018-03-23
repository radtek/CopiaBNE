<%
 Set conn = Server.CreateObject("ADODB.Connection")
 conn.Provider = "SQLOLEDB"
 conn.ConnectionTimeout = 90
 conn.CommandTimeout    = 0
 conn.Open GetConnectionString("CONN_BNE")
%>

<%
Function GetConnectionString(strConnStringName)
  Dim xmlWebConfig
  Dim nodeConnStrings
  Dim nodeChildNode
  Dim strConnStringValue

  Set xmlWebConfig = Server.CreateObject("Msxml2.DOMDocument.6.0")
  xmlWebConfig.async = False
  xmlWebConfig.Load(Server.MapPath("/web.config"))

  If xmlWebConfig.parseError.errorCode = 0 Then
    Set nodeConnStrings = xmlWebConfig.selectSingleNode("//configuration/connectionStrings")
    For Each nodeChildNode In nodeConnStrings.childNodes
      If nodeChildNode.getAttribute("name") = strConnStringName Then
        strConnStringValue = nodeChildNode.getAttribute("connectionString")
        Exit For
      End If
    Next
    Set nodeConnStrings = Nothing
  End If
  Set xmlWebConfig = Nothing
  
  GetConnectionString = strConnStringValue
End Function
%>