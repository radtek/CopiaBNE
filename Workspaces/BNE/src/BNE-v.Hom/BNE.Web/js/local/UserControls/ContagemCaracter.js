var bName = navigator.appName;
function LimitaCaracteres(target, maxL_) {
    maxL = maxL_;
    if (target.value.length == maxL || target.value.length > maxL) {
        return false;
    }
    return true;
}

function Contador(target, Cnt, maxL_) {
    maxL = maxL_;
    objCnt = createObject(Cnt);
    objVal = target.value;

    if (objVal.length > maxL) {
        objCnt.innerText = objVal.length + ' caracteres';
        objCnt.innerText = objVal.length - maxL + ' caracteres';
    }
    else {
        objCnt.innerText = objVal.length + ' caracteres';
    }

    if (objCnt) {
        objCnt.innerText = maxL - objVal.length + ' caracteres';
    }

    return true;
}

function createObject(objId) {
    if (document.getElementById) return document.getElementById(objId);
    else if (document.layers) return eval("document." + objId);
    else if (document.all) return eval("document.all." + objId);
    else return eval("document." + objId);
}