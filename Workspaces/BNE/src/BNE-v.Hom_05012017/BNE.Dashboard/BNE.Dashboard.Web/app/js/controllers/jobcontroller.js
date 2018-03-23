app.controller('JobController', ['$scope', 'jobservice', '$interval', 'notificationFactory', function ($scope, jobservice, $interval, notificationFactory) {
    var startedWatchers, startedAnalytics, startedServers = false;

    $scope.analytics = 0;

    function loadQtdVagas() {
        jobservice.getProjetosQtdVagas().success(function (data) {
            $scope.vagas = data;
        });
    }

    function loadStatusVagas() {
        jobservice.getStatusProjetos().success(function (data) {
            $scope.statusProjetos = data;
        });
    }

    function loadQtdVagasImportadas() {
        jobservice.getTotalVagasImportadas().success(function (data) {
            $scope.vagasImportadas = data;
        });
    }

    function loadTotalProjetosParados() {
        jobservice.getTotalProjetosParados().success(function (data) {
            console.log(data);
            $scope.analytics = data;

            if (!startedAnalytics) {
                startedAnalytics = true;
                $interval(function () {
                }.bind(this), config.refreshIntervalAnalytics);
            }
        });
    }

    $scope.hasError = function (watcher) {
        return true;
    }

    loadQtdVagas();
    loadTotalProjetosParados();
    loadQtdVagasImportadas();
    loadStatusVagas();

    setInterval(function () {
        loadQtdVagas();
        loadTotalProjetosParados();
        loadQtdVagasImportadas();
        loadStatusVagas();
    }, 3600000)


}]);