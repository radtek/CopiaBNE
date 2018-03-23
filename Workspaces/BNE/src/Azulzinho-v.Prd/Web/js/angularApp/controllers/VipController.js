var VipController = angular.module('VipController', []);

VipController.controller('VipController', ['$scope', '$http', '$location', '$route', '$rootScope', '$filter', function ($scope, $http, $location, $route, $rootScope, $filter) {

    var store = this;

    $scope.processandoVip = false;
    $scope.vipOK = false;
    $scope.vipErro = false;
    $scope.vipSemCodigo = false;


    if (sessionStorage.curriculo != null)
        store.curriculo = angular.fromJson(sessionStorage.curriculo);
    else
        $location.path('/Login');

    $scope.AtivarVip = function () {

        $scope.vipOK = false;
        $scope.vipErro = false;
        $scope.vipSemCodigo = false;
        $scope.processandoVip = false;

        var codigo = $('#codigoDesconto').val();
        
        if (codigo == "")
        {
            $scope.vipSemCodigo = true;
            return;
        }
        $scope.processandoVip = true;
        var idCurriculo = store.curriculo.idCurriculo;

        $http.post(urlAPI + "Curriculo/LiberarVip?idCurriculo=" + idCurriculo + "&codigo=" + codigo)
            .success(function (data, status, headers, config) {
                $.each(data, function (i, v) {
                    $rootScope.vagas.push(v);
                })
                store.curriculo.isVip = true;
                sessionStorage.curriculo = angular.toJson(store.curriculo);
                $scope.processandoVip = false;
                $scope.vipOK = true;
            })
            .error(function (data, status, headers, config) {
                $scope.descErroVip = data;
                $scope.processandoVip = false;
                $scope.vipErro = true;
            });
    };

    $scope.VerMaisVagas = function () {
        $location.path('/Vagas');
    };
}]);