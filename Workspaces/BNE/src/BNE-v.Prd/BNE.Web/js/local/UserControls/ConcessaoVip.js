

//maxL = 0;
var bName = navigator.appName;
function taLimit(taObj, maxL_) {
    maxL = maxL_;
    if (taObj.value.length == maxL || taObj.value.length > maxL) {

        return false;
    }
    return true;
}

function taCount(taObj, Cnt, maxL_) {
    maxL = maxL_;
    objCnt = createObject(Cnt);
    objVal = taObj.value;

    if (objVal.length > maxL) {

        objCnt.innerText = objVal.length;
        objCnt.innerText = objVal.length - maxL;

    }
    else {
        objCnt.innerText = objVal.length;
    }

    if (objCnt) {

        objCnt.innerText = maxL - objVal.length;
    }

    return true;
}