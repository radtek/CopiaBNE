var app = angular.module('log-app', ['ngTable', 'ui.bootstrap']);
var hubserver;
var compareDateTo = new Date();


$(document).ready(function () {
    $.connection.hub.url = 'signalr/hubs';
    hubserver = $.connection.logserver;

    HubStart();

    $.connection.hub.disconnected(function () {
        //try reconnect
        setTimeout(function () {
            HubStart();
        }, 10000); // Restart connection after 10 seconds.
    });

    hubserver.client.pushLog = function (args) {
        var scope = angular.element($("#log-controller")).scope();
        scope.$apply(function () {
            if (args instanceof Array) //Se são vários logs
            {
                angular.forEach(args, function (obj) {
                    scope.addrealtime(obj);
                    scope.addallerrordata(obj);
                });
                return;
            }
            scope.addrealtime(args);
            scope.addallerrordata(args);
        });
    };


});
function HubStart() {
    $.connection.hub.start({ transport: 'auto' }, function () {
        GetAllErrors();
    }).fail(function (ex) {
        toastr.info('Sem Conexão!', { fadeAway: 5000 });
    }).done(function () {
        toastr.info('Conexão efetuada!', { fadeAway: 5000 });
    });
}

function GetAllErrors() {
    hubserver.server.getAllErrors().done(function (response) {
        var scope = angular.element($("#log-controller")).scope();
        scope.$apply(function () {
            if (response instanceof Array) //Se são vários logs
            {
                angular.forEach(response, function (obj) {
                    scope.addallerrordata(obj);
                });
                return;
            }
            scope.addallerrordata(response);
        });
    });
}

function convertLogLevel(value) {
    if (value === 1)
        return "WARN";
    if (value === 2)
        return "ERROR";
    if (value === 3)
        return "FLOW";

    return "INFO";
}
app.filter("translateLogLevel", function () {
    return convertLogLevel;
});

function clone(obj) {
    // Handle the 3 simple types, and null or undefined
    if (null == obj || "object" != typeof obj) return obj;

    // Handle Date
    var copy;
    if (obj instanceof Date) {
        copy = new Date();
        copy.setTime(obj.getTime());
        return copy;
    }

    // Handle Array
    if (obj instanceof Array) {
        copy = [];
        for (var i = 0, len = obj.length; i < len; i++) {
            copy[i] = clone(obj[i]);
        }
        return copy;
    }

    // Handle Object
    if (obj instanceof Object) {
        copy = {
        };
        for (var attr in obj) {
            if (obj.hasOwnProperty(attr)) copy[attr] = clone(obj[attr]);
        }
        return copy;
    }

    throw new Error("Unable to copy obj! Its type isn't supported.");
}

function dumpObject(value) {
    if (value === undefined || value === null)
        return '';

    return JSON.stringify(value, function (key, value) {
        if (value instanceof Date) {
            return convertDate(value);
        }
        else {
            if ("number" == typeof (value))
                return value;

            var parsed = Date.parse(value);
            if (isNaN(parsed) || parsed === undefined)
                return value;

            parsed = new Date(parsed);

            if (parsed.getTime() < getUnexpectedOldDate().getTime() && (key.toLowerCase().indexOf("dt") == -1 && key.toLowerCase().indexOf("dat") == -1 && key.toLowerCase().indexOf("time") == -1))
                return value;

            if (parsed.getDay() == 1 && parsed.getMonth() == 0 && (parsed.getYear() > 9999 || parsed.getYear() < 1972))
                return value;

            return convertDate(parsed);
        }
    }, 4);
};

function leftPad(number, targetLength) {
    var output = number + '';
    while (output.length < targetLength) {
        output = '0' + output;
    }
    return output;
}

function getUnexpectedOldDate() {
    compareDateTo.setYear(2013);
    return compareDateTo;
}

function convertDate(dateObj, year) {

    if (year) {
        return leftPad(dateObj.getDate().toString(), 2) + '/' +
        leftPad((dateObj.getMonth() + 1).toString(), 2) + '/' +
        dateObj.getFullYear() + " - " +
        leftPad(dateObj.getHours().toString(), 2) + ":" +
        leftPad(dateObj.getMinutes().toString(), 2) + ":" +
        leftPad(dateObj.getSeconds().toString(), 2);
    }
    return leftPad(dateObj.getDate().toString(), 2) + '/' +
    leftPad((dateObj.getMonth() + 1).toString(), 2) + ' - ' +
    leftPad(dateObj.getHours().toString(), 2) + ":" +
    leftPad(dateObj.getMinutes().toString(), 2) + ":" +
    leftPad(dateObj.getSeconds().toString(), 2);
}