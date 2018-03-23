<%




Session("nomeCores") = request("nome")
Session("empresaCores") = request("empresa")
Session("cargoCores") = request("cargo")
Session("emailCores") = request("email")
Session("celularCores") = request("ddd")&request("celular")



Response.Redirect("cores.asp?nome=" + Request("nome"))



%>