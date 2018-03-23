function BaseControl(element) {
    this._id = element.id;
    this._dom_element = document.getElementById(element.id);

    this._dom_element.onMouseOver = this.mouseOver;
    this._dom_element.onMouseOut = this.mouseOut;
    this._dom_element.onBlur = this.blur;
    this._dom_element.onFocus = this.focus;

    BaseControl.initializeBase(this, [element]);
}

BaseControl.prototype.initialize = function () {
    // ImgResizer.callBaseMethod(this, "initialize");
}

BaseControl.prototype.dispose = function () {
    // ImgResizer.callBaseMethod(this, "dispose");
}

BaseControl.prototype.mouseOver = function(){
}

BaseControl.prototype.mouseOut = function () {
}

BaseControl.prototype.blur = function () {
}

BaseControl.prototype.focus = function () {
}