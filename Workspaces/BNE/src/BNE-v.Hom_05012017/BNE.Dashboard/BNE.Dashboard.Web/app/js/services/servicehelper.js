app.factory('servicehelper', ['$resource', '$http', function ($resource, $http) {
    //var buildurl = function (resourceurl) {
    //    return config.apiurl + resourceurl;
    //};
    //return {
    //    Watcher: $http.get(buildurl('api/watcher')),
    //    Analytics: $http.get(buildurl('api/analytics'))
    //}

    var buildurl = function (resourceurl) {
        return config.apiurl + resourceurl;
    };

    return {
        Watcher: buildurl('api/watcher'),
        Job: buildurl('api/job'),
        Analytics: buildurl('api/analytics'),
        ServerMonitor: buildurl('api/servermonitor')
    }
}]);