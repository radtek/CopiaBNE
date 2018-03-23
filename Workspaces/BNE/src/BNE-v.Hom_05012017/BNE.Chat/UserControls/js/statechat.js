function translateStatusMensagem(arg) {
    if (arg == -2)
        return "campanha";

    if (arg == -1)
        return "enviando";

    if (arg == 0 || arg == 3)
        return "enviada";

    if (arg == 1)
        return "criada";

    if (arg == 2) {
        return "na fila";
    }

    if (arg == 4) {
        return "entregue";
    }

    if (arg == 5) {
        return "saldo insuficiente para o envio";
    }

    if (arg == 6) {
        return "erro";
    }

    if (arg == 400) {
        return "falha";
    }

    if (arg == 417) {
        return "não enviada (texto muito grande)";
    }

    if (arg == 401) {
        return "necessário entrar novamente";
    }

    if (arg == 500) {
        return "erro no servidor";
    }

    if (arg == 503) {
        return "servico não disponível";
    }
    return "";
}

function translateMessageDate(newest, oldest) {
    if (!(newest instanceof Date)
        || (!(oldest instanceof Date))) {
        return "";
    }

    var compactNew = new Date(newest.getFullYear(), newest.getMonth(), newest.getDate());
    var compactOld = new Date(oldest.getFullYear(), oldest.getMonth(), oldest.getDate());
    var diffDays = julianDay(compactNew) - julianDay(compactOld);

    if (diffDays == 0) {
        return "hoje";
    }
    else if (diffDays == 1) {
        return "ontem";
    }
    else if (diffDays <= 14) {
        return diffDays + " dias atrás";
    }
    return leftPad(oldest.getDate().toString(), 2) + "/" + leftPad((oldest.getMonth() + 1).toString(), 2);

}

function julianDay(date) {
    return Math.floor((date / 86400000) - (date.getTimezoneOffset() / 1440) + 2440587.5);
}

function leftPad(number, targetLength) {
    var output = number + '';
    while (output.length < targetLength) {
        output = '0' + output;
    }
    return output;
}

(function ($) {
    $.fn.hasScrollBar = function () {
        return this.get(0).scrollHeight > this.height();
    }
})(jQuery);

if (ko != typeof (undefined)) {
    ko.sortedObservableArray = function (sortComparator, initialValues) {
        if (arguments.length < 2) {
            initialValues = [];
        }
        var result = ko.observableArray(initialValues);
        ko.utils.extend(result, ko.sortedObservableArray.fn);
        delete result.unshift;
        result.sort(sortComparator);
        return result;
    };

    ko.sortedObservableArray.fn = {
        push: function (values) {
            if (!$.isArray(values)) {
                values = [values];
            }
            var underlyingArray = this.peek();
            this.valueWillMutate();
            underlyingArray.push.apply(underlyingArray, values);
            underlyingArray.sort(this.sortComparator);
            this.valueHasMutated();
        },
        sort: function (sortComparator) {
            var underlyingArray = this.peek();
            this.valueWillMutate();
            this.sortComparator = sortComparator;
            underlyingArray.sort(this.sortComparator);
            this.valueHasMutated();
        },
        reinitialise: function (values) {
            if (!$.isArray(values)) {
                values = [values];
            }
            var underlyingArray = this.peek();
            this.valueWillMutate();
            underlyingArray.splice(0, underlyingArray.length);
            underlyingArray.push.apply(underlyingArray, values);
            underlyingArray.sort(this.sortComparator);
            this.valueHasMutated();
        },
        reverse: function () {
            var underlyingArrayClone = this.peek().slice();
            underlyingArrayClone.reverse();
            return underlyingArrayClone;
        }
    };

    ko.bindingHandlers.uniqueId = {
        counter: 0,
        prefix: "unique_",
        init: function (element, valueAccessor) {
            var value = valueAccessor();
            if (isUndefined(value.id)) {
                var str1 = value.toString() + ko.bindingHandlers.uniqueId.prefix.toString() + (++ko.bindingHandlers.uniqueId.counter).toString();
                value = str1;
            } else {
                var str2 = value.id || ko.bindingHandlers.uniqueId.prefix.toString() + (++ko.bindingHandlers.uniqueId.counter).toString();
                value = str2;
            }
            element.id = value;
        },
    };

    ko.observableArray.fn.totalVisible = function () {
        var items = this(), count = 0;

        if (isUndefined(items) || typeof items.length === "undefined") return 0;

        for (var i = 0, len = items.length; i < len; i++) {
            if (!isUndefined(items[i]) && items[i]._destroy !== true) count++;
        }

        return count;
    };

    ko.observableArray.fn.validItems = function () {
        var items = this(), valid = [];

        if (isUndefined(items) || typeof items.length === "undefined" || typeof items.length === "undefined") return valid;

        for (var i = 0, len = items.length; i < len; i++) {
            if (!isUndefined(items[i]) && items[i]._destroy !== true) {
                valid.push(items[i]);
            }
        }

        return valid;
    };

    ko.bindingHandlers.returnAction = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = ko.utils.unwrapObservable(valueAccessor());

            $(element).keydown(function (e) {
                if (e.which === 13) {
                    e.cancelBubble = true;
                    if (e.stopPropagation)
                        e.stopPropagation();

                    value(viewModel, $(element).val());

                    setTimeout(function () {
                        $(element).focus();
                    }, 30);
                    return false;
                }
                return true;
            });
        }
    };

    ko.bindingHandlers.escAction = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            var value = ko.utils.unwrapObservable(valueAccessor());

            $(element).keydown(function (e) {
                if (e.keyCode == 27) {
                    e.cancelBubble = true;
                    if (e.stopPropagation)
                        e.stopPropagation();

                    value(viewModel, $(element).val());

                    return false;
                }
                return true;
            });
        }
    };
}

function codeToChar(number) {
    if (number >= 0 && number <= 25) // a-z
        number = number + 97;
    else
        return codeToChar(number - 26);

    return String.fromCharCode(number);
}

function isUndefined(param) {
    if (param == typeof (undefined) || param == null)
        return true;
    return false;
}

function isDateOrParseOrDefault(param) {
    if (param instanceof Date) {
        return param;
    }

    var convertToDate = moment(param);

    if (convertToDate.isValid()) {
        return new Date(convertToDate.format());
    } else {
        return new Date(param);
    }
}

function generateGuid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
                   .toString(16)
                   .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
           s4() + '-' + s4() + s4() + s4();
}

function clone(obj) {
    // Handle the 3 simple types, and null or undefined
    if (null == obj || "object" != typeof obj) return obj;

    // Handle Date
    if (obj instanceof Date) {
        var copy = new Date();
        copy.setTime(obj.getTime());
        return copy;
    }

    // Handle Array
    if (obj instanceof Array) {
        var copy = [];
        for (var i = 0, len = obj.length; i < len; i++) {
            copy[i] = clone(obj[i]);
        }
        return copy;
    }

    // Handle Object
    if (obj instanceof Object) {
        var copy = {};
        for (var attr in obj) {
            if (obj.hasOwnProperty(attr)) copy[attr] = clone(obj[attr]);
        }
        return copy;
    }

    throw new Error("Unable to copy obj! Its type isn't supported.");
}