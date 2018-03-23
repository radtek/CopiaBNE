// Previne a digitação após o maxLength estourar
function doKeypress(control){
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
     if(maxLength && value.length > maxLength-1){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
     }
}
// Cancela o Paste para nao estourar o maxLength
// BETA
function doBeforePaste(control){
    maxLength = control.attributes["maxLength"].value;
     if(maxLength)
     {
          event.returnValue = false;
     }
}

//Efetua o Paste se for houver espaço livre na textbox
//BETA
function doPaste(control){
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
     if(maxLength){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
          var range = control.document.selection.createRange();
          var pasteLen = maxLength - value.length + range.text.length;
          var data = window.clipboardData.getData("Text").substr(0,pasteLen);
          range.text = data;
     }
}
