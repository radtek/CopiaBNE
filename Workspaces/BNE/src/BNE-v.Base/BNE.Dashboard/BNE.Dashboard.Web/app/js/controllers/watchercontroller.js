app.controller('WatcherController', ['$scope', 'watcherservice', 'analyticsservice', '$interval', 'notificationFactory', function ($scope, watcherservice, analyticsservice, $interval, notificationFactory) {
    var startedWatchers, startedAnalytics = false;
   

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

    $scope.hasError = function (watcher) {
        if (watcher.Status.Name === 'ERROR') {
            return true;
        }

        return false;
    }

    loadWatchers();
    loadAnalytics();
    
}]);