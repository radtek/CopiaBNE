
function ImgResizer(element) {
    // Valida a versão do Jquery
    // Pega a versão
    var strjqueryVer = $().jquery;
    // Split para pegar o número do meio
    var jqueryVer = parseInt(strjqueryVer.toString().split(".").join("").toString());

    if (jqueryVer < 100)
        jqueryVer = jqueryVer * 10;

    // Trava o controle conforme a versão do JQuery    
    if (jqueryVer < 144) {
        alert("A versão do JQuery deve ser igual ou superior a 1.4.4 para que este componente possa funcionar corretamente.");
        return;
    }

    // Init
    this._id = element.id;
    this._element = "#" + element.id + "_img";
    this._element_lightbox = "#" + element.id + "_img_lght";
    this._dom_element = document.getElementById(element.id);
    this._thumburl = document.getElementById(element.id + "_thumburl");
    this._x1 = 0;
    this._x2 = 0;
    this._y1 = 0;
    this._y2 = 0;
    this._w = 0;
    this._h = 0;
    this._selection = null;
    this._area = null;
    this._aspect = null;
    // inicializa

    var callback = this.assign;
    var instance = this;
    var cbSetvalue = this.setValues;

    //callback(instance, cbSetvalue);    
    /* setTimeout( 
    function (){ 
    callback(instance, cbSetvalue);
    }  
    ,200);*/

    //    if (Sys.Application.findComponent(this._id, undefined) == null) {
    //        this.assign(this, this.setValues);
    //        ImgResizer.initializeBase(instance, [element]);
    //    }

    this.assign(this, this.setValues);
    ImgResizer.initializeBase(instance, [element]);
}

ImgResizer.prototype.initialize = function () {
    ImgResizer.callBaseMethod(this, "initialize");

},

ImgResizer.prototype.dispose = function () {
    ImgResizer.callBaseMethod(this, "dispose");

}

ImgResizer.prototype.assign = function (inst, callback) {
    // var inst = this;
    //var callback = this.setValues;   
    var parametros = {
        maxWidth: parseInt(inst._dom_element.getAttribute("maxW")),
        maxHeight: parseInt(inst._dom_element.getAttribute("maxH")),
        aspectRatio: inst._dom_element.getAttribute("aspectRatio"),
        handles: true,
        instance: true,
        hide: true,
        parent: '#' + this._id,
        onSelectChange: function (img, selection) { callback(inst, img, selection) }
    }
    inst._area = $(inst._element).imgAreaSelect(parametros);

    //Utilizado para posição inicial da área de corte.
    this._aspect = inst._dom_element.getAttribute("aspectRatio");

    setTimeout(
            function () {
                $find(inst._id).showDefaultSelectionArea()
            }, 100);

    //this.showDefaultSelectionArea();
}

ImgResizer.prototype.setValues = function (instance, img, selection) {
    instance._x1 = selection.x1;
    instance._x2 = selection.x2;
    instance._y1 = selection.y1;
    instance._y2 = selection.y2;
    instance._w = selection.width;
    instance._h = selection.height;
    instance._selection = selection;
}

ImgResizer.prototype.createThumb = function () {
    if (this._x1 == this._x2 && this._y1 == this._y2)
        return false;

    var d = new Date();

    __doPostBack(this._id, 'setImage;' + Math.floor(this._x1) + ";" + Math.floor(this._x2) + ";" + Math.floor(this._y1) + ";" + Math.floor(this._y2) + ";" + Math.floor(this._w) + ";" + Math.floor(this._h) + ";" + d.getTime().toString());
    this.hideSelectionArea();
}

ImgResizer.prototype.hideSelectionArea = function () {
    $(this._area).data().imgAreaSelect.setOptions({ hide: true });
}

ImgResizer.prototype.setRatio = function (ratio) {
    $(this._area).data().imgAreaSelect.setOptions({ aspectRatio: ratio });
}

ImgResizer.prototype.showSelectionArea = function () {
    var inst = this;
    var initial = inst._dom_element.getAttribute("initialSelection");
    var param = null;

    if (initial) {
        var arInitial = initial.split(";");
        inst._x1 = parseInt(arInitial[0]);
        inst._y1 = parseInt(arInitial[1]);
        inst._x2 = parseInt(arInitial[2]);
        inst._y2 = parseInt(arInitial[3]);
        inst._w = inst._x2 - inst._x1;
        inst._h = inst._y2 - inst._y1;

        param = {
            x1: inst._x1,
            y1: inst._y1,
            x2: inst._x2,
            y2: inst._y2,
            show: true
        };


        setTimeout(
            function () {
                $(inst._area).data().imgAreaSelect.setOptions(param);
            }, 100);
    }
}

ImgResizer.prototype.showDefaultSelectionArea = function () {
    if (this._x1 == this._x2 && this._y1 == this._y2) {
        try {
            if (this._aspect != null) {
                var aspectH = this._aspect.split(':')[1];
                var aspectW = this._aspect.split(':')[0];
                var maxH = this._dom_element.getAttribute("maxH");
                var maxW = this._dom_element.getAttribute("maxW");
                var imgH = this._area.height();
                var imgW = this._area.width();
                var areaH;
                var areaW;

                /*Se os limites máximos forem maior que a imagem
                configura os limites para o limite da imagem.*/
                if (maxH > imgH)
                    maxH = imgH;
                if (maxW > imgW)
                    maxW = imgW;

                if (this._aspect != "") {
                    //Multiplica por H
                    var aspecCoe = aspectW / aspectH;
                    if (maxH * aspecCoe <= maxW) {
                        areaH = maxH;
                        areaW = maxH * aspecCoe;
                    }
                    else {
                        areaH = maxW * (aspectH / aspectW);
                        areaW = maxW;
                    }

                    //Calcula a posição inicial
                    this._y1 = (imgH - areaH) / 2;
                    this._x1 = (imgW - areaW) / 2;
                    this._x2 = parseInt(this._x1.toString()) + parseInt(areaW.toString());
                    this._y2 = parseInt(this._y1.toString()) + parseInt(areaH.toString());
                }
                else {

                    this._y1 = 0;
                    this._x1 = 0;
                    this._x2 = parseInt(imgW.toString());
                    this._y2 = parseInt(imgH.toString());
                }

                $(this._area).imgAreaSelect({ x1: this._x1, y1: this._y1, x2: this._x2, y2: this._y2 });

            } else {
                throw '';
            }
        }
        catch (err) {
            return false;
        }
    }
}

ImgResizer.registerClass("ImgResizer", Sys.UI.Control);
if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();