app.controller('WatcherController', ['$scope', 'watcherservice', 'servermonitorservice', '$interval', 'notificationFactory', function ($scope, watcherservice, servermonitorservice, $interval, notificationFactory) {
    var startedWatchers, startedAnalytics, startedServers = false;

    $scope.analytics = [];

    $scope.notifyError1 = function () {
        var error = false;
        angular.forEach($scope.watchers, function (watcher) {
            if ($scope.hasError(watcher)) {
                error = true;
                notificationFactory.error('O serviço ' + watcher.Name + ' está com erro!');
            }
        });

        if (error) {
            notificationFactory.error(null, true);
        }
    }

    $scope.lastServerWarning = new Date(2015,1,1,0,0,0,0);
    $scope.notifyWarning1 = function () {
        var diff = new Date() - $scope.lastServerWarning;
        if (Math.round(((diff % 86400000) % 3600000) / 60000) < 5)
            return;
        
        var error = false;
        angular.forEach($scope.servers, function (server) {
            if (server.status == 2) {
                error = true;
                notificationFactory.warning('O servidor ' + server.ip + ' está com uso de memória/cpu muito alto!');
            }
        });

        if (error) {
            notificationFactory.warning(null, true);
        }

        $scope.lastServerWarning = new Date();
    }

    function loadWatchers() {
        watcherservice.getItems().success(function (data) {
            $scope.watchers = data;
            $scope.notifyError1();

            if (!startedWatchers) {
                startedWatchers = true;
                $interval(function () {
                    loadWatchers();
                }.bind(this), config.refreshInterval);
            }
        });
    }
    function loadAnalytics() {
        analyticsservice.getItems().success(function (data) {
            $scope.analytics = data;

            if (!startedAnalytics) {
                startedAnalytics = true;
                $interval(function () {
                    loadAnalytics();
                }.bind(this), config.refreshIntervalAnalytics);
            }
        });
    }

    var serversOrder = ['SINE', 'SINE Services', 'BNE'];
    var nextOrderIndex = serversOrder.length;
    var lastHeader = "";

    function loadServers() {
        $scope.serverMessages = new Array();
        servermonitorservice.getItems().success(function (retorno) {
            lastHeader = "";
            var apps = new Array();
            var lastserver;

            for (i in retorno.servers) {
                if (!lastserver || retorno.servers[i].applicationName != lastserver || true) {
                    var orderIndex = serversOrder.indexOf(retorno.servers[i].applicationName);
                    if (orderIndex < 0)
                        orderIndex = nextOrderIndex++;

                    apps.push({ applicationName: retorno.servers[i].applicationName, order: orderIndex, servers: new Array() })
                    lastserver = retorno.servers[i].applicationName;
                }

                apps[apps.length - 1].servers.push(retorno.servers[i]);
            }            

            console.log(apps);

            $scope.apps = apps;
            $scope.servers = retorno.servers;
            $scope.messages = retorno.messages;
            $scope.notifyWarning1();

            if (!startedServers) {
                startedServers = true;
                $interval(function () {
                    loadServers();
                }.bind(this), config.refreshIntervalAnalytics);
            }
        });
    }

    $scope.printServerHeader = function (app) {
        var ret = app.applicationName == lastHeader;
        lastHeader = app.applicationName;
        return !ret;
    }

    $scope.hasError = function (watcher) {
        if (watcher.Status.Name === 'ERROR') {
            return true;
        }

        return false;
    }

    loadWatchers();
    //loadAnalytics();
    loadServers();
    
}]);