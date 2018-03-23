app.controller('log', ['$scope', '$filter', '$q', 'ngTableParams', function ($scope, $filter, $q, ngTableParams) {
    $scope.realtimedata = [];
    $scope.allerrordata = [];
    $scope.realTime = true;
    $scope.showCustomMessage = false;
    $scope.showExceptionDetails = false;
    $scope.dias = 1;

    $scope.changeDataRange = function (value) {
        $scope.dias = value;
        $scope.tablerealtime.reload();
        $scope.tablealldata.reload();
    }

    $scope.names = function () {
        var def = $q.defer(),
            arr = ['INFO', 'WARN', 'ERROR'], /* , 'FLOW' */
            names = [{ 'id': '0', 'title': 'INFO' }, { 'id': '1', 'title': 'WARN' }, { 'id': '2', 'title': 'ERROR' }]; /*, { 'id': '3', 'title': 'FLOW' }*/

        def.resolve(names);
        return def;
    };

    $scope.tablerealtime = new ngTableParams
    (
        {
            page: 1,
            count: 50,
            filter: {}
        },
        {
            counts: [5, 10, 50, 100],
            getData: function ($defer, params) {
                params.showCustomMessage = $scope.showCustomMessage;
                params.showExceptionDetails = $scope.showExceptionDetails;

                var orderedData = params.filter() ? $filter('filter')($scope.realtimedata, params.filter()) : $scope.realtimedata;

                params.total(orderedData.length);
                $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            }
        }
    );

    $scope.tablealldata = new ngTableParams
    (
        {
            page: 1,
            count: 50,
            filter: {},
            sorting: { Quantidade: "desc" }
        },
        {
            counts: [5, 10, 50, 100],
            getData: function ($defer, params) {
                params.showCustomMessage = $scope.showCustomMessage;
                params.showExceptionDetails = $scope.showExceptionDetails;

                var filteredData = params.filter() ? $filter('filter')($scope.allerrordata, params.filter()) : $scope.allerrordata;

                var dateTo = new Date();
                var dateFrom = new Date();
                dateFrom.setDate(dateFrom.getDate() - $scope.dias);
                var rangeData = [];
                angular.forEach(filteredData, function (data) {
                    var dataIncidente = new Date(data.UltimoIncidente);
                    if (dataIncidente >= dateFrom && dataIncidente <= dateTo) {
                        rangeData.push(data);
                    }
                });

                var orderedData = params.sorting() ? $filter('orderBy')(rangeData, params.orderBy()) : rangeData;

                params.total(orderedData.length);
                $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            }
        }
    );

    $scope.addrealtime = function (obj) {
        $scope.realtimedata.unshift(obj);
        $scope.tablerealtime.reload();
    };

    $scope.addallerrordata = function (obj) {
        $scope.allerrordata = $scope.allerrordata.filter(function (element) {
            return element.Guid !== obj.Guid;
        });
        $scope.allerrordata.unshift(obj);
        $scope.tablealldata.reload();
    };

    $scope.changeSelection = function (log) {
        angular.forEach($scope.realtimedata, function (obj) {
            if (log !== obj) {
                obj.$selected = false;
            }
        });
        angular.forEach($scope.allerrordata, function (obj) {
            if (log !== obj) {
                obj.$selected = false;
            }
        });
        var scope = angular.element($("#container_extra_info")).scope();
        scope.open('lg', log);
    }

    $scope.changeCustomMessage = function () {
        $scope.showCustomMessage = !$scope.showCustomMessage;
        $scope.tablealldata.reload();
        $scope.tablerealtime.reload();
    };

    $scope.changeExceptionDetails = function () {
        $scope.showExceptionDetails = !$scope.showExceptionDetails;
        $scope.tablealldata.reload();
        $scope.tablerealtime.reload();
    };


}]);

$('#eTabs').easytabs({
    animate: false,
    updateHash: false
});