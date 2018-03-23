app.controller('ExtraInfo', ['$scope', '$uibModal', function ($scope, $uibModal) {
    $scope.open = function (size, log, rowItem) {
        var parsedLog = clone(log);
        parsedLog.$$hashKey = function () { };
        parsedLog.$selected = function () { };
        if (parsedLog.Level === undefined)
            parsedLog.Level = convertLogLevel(parsedLog.Nivel);

        parsedLog.Nivel = function () { };
        parsedLog.nivel = function () { };
        parsedLog.$id = function () { };

        if (parsedLog.UltimoIncidente === undefined) {
            parsedLog.UltimoIncidente = parsedLog.DataIncidente;
        }

        if (parsedLog.Ocorrencias !== undefined) {
            angular.forEach(parsedLog.Ocorrencias, function (obj) {
                if (obj.Usuario === null) {
                    obj.Usuario = function () { };
                }
                if (obj.Payload === null || obj.Payload === '') {
                    obj.Payload = function () { };
                }
            });
        }

        var modalInstance = $uibModal.open({
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: size,
            resolve: {
                items: function () {
                    return {
                        main: parsedLog, rowItem: rowItem
                    };
                }
            }
        });

        modalInstance.result.then(function (res) {
        }, function () {
            if (rowItem === undefined)
                return;

            if (rowItem.selected) {
                rowItem.selected = false;
                if (!$scope.$$phase) {
                    $scope.apply();
                    return;
                }
            }
        });

    };
}]);


app.controller('ModalInstanceCtrl', ['$scope', '$uibModalInstance', 'items', function ($scope, $uibModalInstance, items) {
    $scope.main = items.main;
    $scope.rowItem = items.rowItem;
    $scope.mainDumpInfo = dumpObject(items.main);

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
        if ($scope.rowItem !== undefined) {
            $scope.rowItem.selected = false;
            if (!$scope.$$phase) {
                $scope.apply();
                return;
            }
        }
    };
}]);