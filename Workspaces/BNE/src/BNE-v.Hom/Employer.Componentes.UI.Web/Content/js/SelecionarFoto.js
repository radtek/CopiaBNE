function showSelectionArea() {
    setTimeout(function () {
        var slic =  $("[id$='imgSlicer']");
        $find(slic[0].id).showSelectionArea();
    }, 250);
}

function hideSelectionArea() {
    var slic = $("[id$='imgSlicer']");
       $find(slic[0].id).hideSelectionArea();
}

function btnConfirmar_OnClientClick(componente) {
    $find($("*[id$='" + componente + "']").attr("id")).createThumb();
}