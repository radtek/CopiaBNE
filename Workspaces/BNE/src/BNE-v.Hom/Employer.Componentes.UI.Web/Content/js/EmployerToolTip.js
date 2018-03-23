Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.EmployerToolTip = function (element) {
    Employer.Componentes.UI.Web.EmployerToolTip.initializeBase(this, [element]);

    this._id = element.id;

    this._ToolTipTitle = null;
    this._ToolTipText = null;
    this._ToolTipPosition = null;
    this._ToolTipTextCss = null;
    this._ToolTipTitleCss = null;

    this._offSetOriginal = {};

    this._onScroll = null;

    this.get_ToolTipTitle = function () {
        return this._ToolTipTitle;
    },
    this.set_ToolTipTitle = function (value) {
        this._ToolTipTitle = value;
    },

    this.get_ToolTipTextCss = function () {
        return this._ToolTipTextCss;
    },
    this.set_ToolTipTextCss = function (value) {
        this._ToolTipTextCss = value;
    },
    this.get_ToolTipTitleCss = function () {
        return this._ToolTipTitleCss;
    },
    this.set_ToolTipTitleCss = function (value) {
        this._ToolTipTitleCss = value;
    },

    this.get_ToolTipText = function () {
        return this._ToolTipText;
    },
    this.set_ToolTipText = function (value) {
        this._ToolTipText = value;
    },
    this.get_ToolTipPosition = function () {
        return this._ToolTipPosition;
    },
    this.set_ToolTipPosition = function (value) {
        this._ToolTipPosition = value;
    },

    this.get_Label = function () {
        return $("#" + this._id);
    },

    this.CriarToolTip = function () {
        var titulo = "<label id='" + this._id + "_lbTitulo' class='" + this.get_ToolTipTitleCss() + "'>" + this.get_ToolTipTitle() + "</label>";
        var texto = "<label id='" + this._id + "_lbTexto' class='" + this.get_ToolTipTextCss() + "'>" + this.get_ToolTipText() + "</label>";

        var div = $("#FixTootip");
        if (div.length == 0) {
            $('body').append("<div id='FixTootip'></div>");
            div = $("#FixTootip");
        }

        div.html(titulo + texto);

        return div;
    },

    this.PositionJquery = function () {
        //TopLeft
        if (this.get_ToolTipPosition() == 11) {
            return { my: "left bottom", at: "right top" };
        }
        //TopCenter
        if (this.get_ToolTipPosition() == 12) {
            return { my: "center bottom", at: "middle top" }
        }
        //TopRight
        if (this.get_ToolTipPosition() == 13) {
            return { my: "right bottom", at: "left top" }
        }

        //MiddleLeft
        if (this.get_ToolTipPosition() == 21) {
            return { my: "left center", at: "right center" };
        }
        //Center
        if (this.get_ToolTipPosition() == 22) {
            return { my: "middle center", at: "middle center" };
        }
        //MiddleRight
        if (this.get_ToolTipPosition() == 23) {
            return { my: "right center", at: "left center" }
        }
        //BottomLeft
        if (this.get_ToolTipPosition() == 31) {
            return { my: "left top", at: "right bottom" }
        }
        //BottomCenter
        if (this.get_ToolTipPosition() == 32) {
            return { my: "center top", at: "center bottom" }
        }
        //BottomRight
        if (this.get_ToolTipPosition() == 33) {
            return { my: "right top", at: "left bottom" }
        }

        return {};
    },

    this.initialize = function () {
        Employer.Componentes.UI.Web.EmployerToolTip.callBaseMethod(this, 'initialize');

        var teste = null;
        // Wireup the event handlers
        var element = this.get_Label();
        if (element) {
            var comp = this;

            element.tooltip({ content:
                function () {
                    return comp.CriarToolTip()
                },
                position: comp.PositionJquery(),
                open: function () {
                    var off = $("#FixTootip").parent().parent().offset();
                    var offHelp = element.offset();
                    comp._offSetOriginal.top = offHelp.top - off.top;
                }
            });

            this._onScroll = Function.createDelegate(this, this.Scroll);
            $addHandler(window, 'scroll', this._onScroll);
        }
    },
    this.dispose = function () {
        var element = this.get_Label();

        $removeHandler(window, 'scroll', this._onScroll);

        $clearHandlers(element[0]);
        element.tooltip("destroy");

        Employer.Componentes.UI.Web.EmployerToolTip.callBaseMethod(this, 'dispose');
    },
    this.Scroll = function () {
        var offHelp = this.get_Label().offset();
        var difTop = offHelp.top - this._offSetOriginal.top;

        var comp = $("#FixTootip").parent().parent();
        comp.offset({ top: difTop });
    }
}

Employer.Componentes.UI.Web.EmployerToolTip.registerClass('Employer.Componentes.UI.Web.EmployerToolTip', Sys.UI.Behavior);