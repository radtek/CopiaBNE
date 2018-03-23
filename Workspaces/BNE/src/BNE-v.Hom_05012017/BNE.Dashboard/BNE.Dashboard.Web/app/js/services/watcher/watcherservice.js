app.factory('watcherservice', ['$resource', 'servicehelper', '$http', function ($resource, servicehelper, $http) {
    var serviceUrl = servicehelper.Watcher;

    var getItems = function () {
        return $http.get(serviceUrl);
    };

    return {
        getItems: getItems
    };
}]);