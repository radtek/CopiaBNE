app.factory('analyticsservice', ['$resource', 'servicehelper', '$http', function ($resource, servicehelper, $http) {
    var serviceUrl = servicehelper.Analytics;

    var getItems = function () {
        return $http.get(serviceUrl);
    };

    return {
        getItems: getItems
    };
}]);