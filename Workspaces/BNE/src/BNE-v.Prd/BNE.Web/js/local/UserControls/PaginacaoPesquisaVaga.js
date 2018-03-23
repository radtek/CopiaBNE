btiPrimeiraOn = new Image();
btiPrimeiraOn.src = "/img/btn_telerik_paginacao_primeira_on.png";
btiAnteriorOn = new Image();
btiAnteriorOn.src = "/img/btn_telerik_paginacao_anterior_on.png";
btiProximaOn = new Image();
btiProximaOn.src = "/img/btn_telerik_paginacao_proxima_on.png";
btiUltimaOn = new Image();
btiUltimaOn.src = "/img/btn_telerik_paginacao_ultima_on.png";

function pageLoad() {
    //btiPrimeira
    $("*[id$='btiPrimeira']").mouseover(
        function () {
            $(this).attr("src", "/img/btn_telerik_paginacao_primeira_on.png");
        }
    );
    $("*[id$='btiPrimeira']").mouseout(
        function () {
            $(this).attr("src", "/img/btn_telerik_paginacao_primeira_off.png");
        }
    );

    //btiAnterior
    $("*[id$='btiAnterior']").mouseover(
        function () {
            $(this).attr("src", "/img/btn_telerik_paginacao_anterior_on.png");
        }
    );
    $("*[id$='btiAnterior']").mouseout(
        function () {
            $(this).attr("src", "/img/btn_telerik_paginacao_anterior_off.png");
        }
    );

    //btiProxima
    $("*[id$='btiProxima']").mouseover(
        function () {
            $(this).attr("src", "/img/btn_telerik_paginacao_proxima_on.png");
        }
    );
    $("*[id$='btiProxima']").mouseout(
        function () {
            $(this).attr("src", "/img/btn_telerik_paginacao_proxima_off.png");
        }
    );

    //btiUltima
    $("*[id$='btiUltima']").mouseover(
        function () {
            $(this).attr("src", "/img/btn_telerik_paginacao_ultima_on.png");
        }
    );
    $("*[id$='btiUltima']").mouseout(
        function () {
            $(this).attr("src", "/img/btn_telerik_paginacao_ultima_off.png");
        }
    );
}