app.controller('DashboardController', function ($scope, Estatistica, Api, SistemaApi, $timeout, $routeParams, $uibModal) {
    $scope.gatewayUrl = config.gatewayurl;
    
    $scope.apis = Api.query({});
    $scope.sistemas = SistemaApi.query({});

    $scope.popup2 = {
        opened: false
    };
    $scope.open2 = function () {
        $scope.popup2.opened = true;
    };

    $scope.loadGraphs = function () {
        $scope.estatisticasSistemas = Estatistica.get({ tipo: 'sistema', label: 'data' }, function () {
            $scope.labelsRequisicaoSistemas = $scope.estatisticasSistemas.labels;
            $scope.seriesRequisicaoSistemas = $scope.estatisticasSistemas.series;
            $scope.dataRequisicaoSistemas = $scope.estatisticasSistemas.data;
        });

        $scope.estatisticasApis = Estatistica.get({ tipo: 'api' }, function () {
            $scope.labelsRequisicaoApis = $scope.estatisticasApis.labels;
            $scope.seriesRequisicaoApis = $scope.estatisticasApis.series;
            $scope.dataRequisicaoApis = $scope.estatisticasApis.data;
        });

        $scope.estatisticasEndpoint = Estatistica.get({ tipo: 'endpoint' }, function () {
            $scope.labelsRequisicaoEndpoint = $scope.estatisticasEndpoint.labels;
            $scope.seriesRequisicaoEndpoint = $scope.estatisticasEndpoint.series;
            $scope.dataRequisicaoEndpoint = $scope.estatisticasEndpoint.data;
        });
    }

    $scope.loadGraphs();

    $timeout(function () {
        $('#example-getting-started').multiselect({
            enableClickableOptGroups: true
        });
    });
});