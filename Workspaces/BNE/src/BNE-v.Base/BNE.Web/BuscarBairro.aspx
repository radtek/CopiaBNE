<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="BuscarBairro.aspx.cs"
    Inherits="BNE.Web.BuscarBairro" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/conteudo.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/geral.css" type="text/css" rel="stylesheet" />

    <title>BNE -
        Busca por
        bairro
    </title>

    <script type="text/javascript">

        //<![CDATA[

        function QueryString(variavel) {
            qs = new Array()
            variaveis = location.search.replace(/\x3F/, "").replace(/\x2B/g, " ").split("&")
            if (variaveis != "") {
                for (i = 0; i < variaveis.length; i++) {
                    nvar = variaveis[i].split("=")
                    qs[nvar[0]] = unescape(nvar[1])
                }
            }

            return qs[variavel];
        }

        var geocoder;
        var map;

        var address = QueryString("pCidade");

        function load() {
            if (GBrowserIsCompatible()) {
                map = new GMap2(document.getElementById("map"));
                geocoder = new GClientGeocoder();
                geocoder.getLocations(address, addToMap);
                map.addControl(new GSmallMapControl());
                map.addControl(new GHierarchicalMapTypeControl());
                GEvent.addListener(map, "click", clicked);
            }
        }

        function addToMap(response) {
            place = response.Placemark[0];
            point = new GLatLng(place.Point.coordinates[1], place.Point.coordinates[0]);
            map.setCenter(point, 13);

            marker = new GMarker(point);
            map.addOverlay(marker);
            map.openInfoWindow(map.getCenter(), "Clique nos bairros para selecioná-los");
        }

        function Trim(str) { return str.replace(/^\s+|\s+$/g, ""); }

        function clicked(overlay, latlng) {
            if (latlng) {
                geocoder.getLocations(latlng, function(addresses) {
                    if (addresses.Status.code != 200) {
                        alert("Falha em localizar a coordenada: " + latlng.toUrlValue());
                    }
                    else {
                        address = addresses.Placemark[0];

                        var strEnderecoCompleto = address.address;
                        var arrEndereco = '';
                        var arrBairro = '';

                        arrEndereco = strEnderecoCompleto.split(',');

                        if (arrEndereco[1] != undefined && arrEndereco[4] != undefined) {
                            arrBairro = arrEndereco[1].split('-');
                            if (arrBairro[2] != undefined) {
                                //var iconBlue = new GIcon(); iconBlue.image = 'http://labs.google.com/ridefinder/images/mm_20_blue.png'; iconBlue.shadow = 'http://labs.google.com/ridefinder/images/mm_20_shadow.png'; iconBlue.iconSize = new GSize(12, 20); iconBlue.shadowSize = new GSize(22, 20); iconBlue.iconAnchor = new GPoint(6, 20); iconBlue.infoWindowAnchor = new GPoint(5, 1); var options = { icon: iconBlue };
                                Marcador = new GMarker(latlng);
                                map.addOverlay(Marcador);
                                map.openInfoWindow(latlng, Trim(arrBairro[2]));
                                //map.removeOverlay(Marcador);
                                document.forms[0].pBairro.value = document.forms[0].pBairro.value + Trim(arrBairro[2]) + ', ';
                            }
                        }
                        else {
                            map.openInfoWindow(latlng, 'O Google não possui um bairro associado a esta localidade');
                        }
                    }
                });
            }
        }

        function SetarBairroParent() {
            //Está sendo feito hardcode pois dependendo do form/usercontrol que chama o Name do controle é diferente.

            //ASPX Normais
            //PesquisaCurriculo
            if (typeof (window.opener.document.forms[0].ctl00$cphConteudo$txtBairro$txtValor) != 'undefined') {
                window.opener.document.forms[0].ctl00$cphConteudo$txtBairro$txtValor.value = document.forms[0].pBairro.value.substring(0, document.forms[0].pBairro.value.length - 2);
                window.opener.__doPostBack('ctl00$cphConteudo$txtBairro$txtValor', '');
                if (typeof (window.opener.document.forms[0].ctl00$cphConteudo$btnAplicarFiltro) != 'undefined') 
                    window.opener.document.forms[0].ctl00$cphConteudo$btnAplicarFiltro.click();
            }
            
            //UserControls
            //RastreioCV
            if (typeof (window.opener.document.forms[0].ctl00$cphConteudo$ucRastreioCV$txtBairro$txtValor) != 'undefined') {
                window.opener.document.forms[0].ctl00$cphConteudo$ucRastreioCV$txtBairro$txtValor.value = document.forms[0].pBairro.value.substring(0, document.forms[0].pBairro.value.length - 2);
                window.opener.__doPostBack('ctl00$cphConteudo$ucRastreioCV$txtBairro$txtValor', '');
            }
            
            
            window.close();
        }
        //]]>
    </script>
</head>

<body onload="load();">
    <form id="form1"
    runat="server">
    <input type="hidden"
        name="pBairro"
        value="" />
    <div id="map"
        style="width: 500px;
        height: 400px">
    </div>
    <input type="button"
        name="btnConfirmar"
        value="OK"
        class="botao_padrao"
        onclick="javascript: SetarBairroParent();" />
    </form>
</body>
</html>
