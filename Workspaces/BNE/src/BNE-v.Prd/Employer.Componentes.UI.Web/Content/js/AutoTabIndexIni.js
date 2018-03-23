
var MsWebForm_AutoFocus = WebForm_AutoFocus;

//ver como implementer em post não ajax
function WebForm_AutoFocus(focusId) {

    window.setTimeout(function () {
        MsWebForm_AutoFocus(focusId);
    }, 500);
}