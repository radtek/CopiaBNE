app.controller('ApiController', function ($scope, Api, SistemaApi, $timeout, $routeParams, $uibModal) {
    $scope.gatewayUrl = config.gatewayurl;

    if ($routeParams.urlPreffix) {
        var prefix = $routeParams.urlPreffix;
        $scope.urlPreffix = $routeParams.urlPreffix;
        $scope.api = Api.get({ id: prefix }, function () {
            $scope.carregarSistemas();
        }); // get() returns a single entry

    } else {
        $scope.apis = Api.query({}, function () {
            console.log($scope.apis);
            $timeout(function () {
                $("[rel=tooltip]").tooltip({ placement: 'bottom' });
            }, 0); // time here
        }); // query() returns all apis
    }

    $scope.carregarSistemas = function () {
        $scope.sistemas = SistemaApi.query({}, function () {
            console.log($scope.sistemas);
            for (var iSistema in $scope.sistemas) {
                var result = $.grep($scope.api.Sistemas, function (s) { return s.Chave == $scope.sistemas[iSistema].Chave; });
                $scope.sistemas[iSistema].isPermited = result.length > 0;
            }
        });
    }

    $scope.togglePermission = function (sistema) {
        if (sistema.isPermited) {
            Api.grantaccess({ id: $scope.urlPreffix, chaveSistema: sistema.Chave });
        } else {
            Api.denyaccess({ id: $scope.urlPreffix, chaveSistema: sistema.Chave });
        }
    }

    $scope.showEndpointModal = function (endpoint) {
        // Just provide a template url, a controller and call 'showModal'.
        ModalService.showModal({
            templateUrl: "app/view/endpoint.html",
            controller: "EndpointController",
            inputs: {
                Endpoint: endpoint
            }
        }).then(function (modal) {
            // The modal object has the element built, if this is a bootstrap modal
            // you can call 'modal' to show it, if it's a custom modal just show or hide
            // it as you need to.
            modal.element.modal();
            modal.close.then(function (result) {
                $scope.message = result ? "You said Yes" : "You said No";
            });
        });

    };

    $scope.showSistemasModal = function () {

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
});