var LanFunctions = angular.module('LanFunctions', []);

LanFunctions.getListaObjetos = function(controller, objDestino) {
    $http.get(urlAPI + controller)
        .success(function (data) {
            objDestino = data;
        })
        .error(function (data) {
            console.log(data);
        });
}
