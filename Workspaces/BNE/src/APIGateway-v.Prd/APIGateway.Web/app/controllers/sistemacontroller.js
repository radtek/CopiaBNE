app.controller('SistemaController', function ($scope, SistemaApi, $timeout, $routeParams, $uibModal) {
    $scope.gatewayUrl = config.gatewayurl;
    $scope.IsAddFormVisible = false;
    
    $scope.carregarSistemas = function () {
        $scope.sistemas = SistemaApi.query({}, function () {
            for (iSistema in $scope.sistemas) {
                var li = "";
                for (iApi in $scope.sistemas[iSistema].Apis) {
                    li += "<li>" + $scope.sistemas[iSistema].Apis[iApi].UrlSuffix + "</li>";
                }
                if (li.length>0)
                    $scope.sistemas[iSistema].UlApis = '<ul class="list-unstyled">' + li + '</ul>';
            }

            $timeout(function () {
                $("[rel=tooltip]").tooltip({ placement: 'bottom' });
            }, 0); 
        });
        console.log($scope.sistemas);
    }
    

    $scope.delete = function (chave) {
        SistemaApi.delete({ id: chave }, function () { $scope.carregarSistemas(); });
    }


    $scope.showAddModal = function () {

        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'app/view/sistemasAdd.html',
            controller: 'SistemaAddController',
            resolve: {
                Operation: function () { return 'AddForm'; }
            }
        });

        modalInstance.result.then(function (sucess) {
            if (sucess) $scope.carregarSistemas();
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });

    };

    $scope.carregarSistemas();
});

app.controller('SistemaAddController', function ($scope, SistemaApi, $timeout, $routeParams, $uibModalInstance) {
    $scope.gatewayUrl = config.gatewayurl;

    $scope.AddSistema = function (formAddSistema) {
        if (!formAddSistema.$valid)
            return;

        $scope.IsFormInvalid = false;

        var sistema = new SistemaApi({ Nome: this.nomeNovoSistema });

        sistema.$save(function (sistema, responseHeaders) {
            $uibModalInstance.close(true);
        });
    };

    $scope.closeModal = function () { $uibModalInstance.dismiss(false) };
});