app.factory('servermonitorservice', ['$resource', 'servicehelper', '$http', function ($resource, servicehelper, $http) {
    var serviceUrl = servicehelper.ServerMonitor;

    var getItems = function () {
        return $http.get(serviceUrl);

    };

    return {
        getItems: getItems
    };
}]);