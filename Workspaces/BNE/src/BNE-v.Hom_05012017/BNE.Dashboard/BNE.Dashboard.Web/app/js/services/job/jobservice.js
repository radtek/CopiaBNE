app.factory('jobservice', ['$resource', 'servicehelper', '$http', function ($resource, servicehelper, $http) {
    var serviceUrl = servicehelper.Job;

    var getProjetosQtdVagas = function () {
        return $http.get(serviceUrl + "/getprojetosqtdvagas");
    };

    var getStatusProjetos = function () {
        return $http.get(serviceUrl + "/getstatusprojetos");
    };

    var getTotalVagasImportadas = function () {
        return $http.get(serviceUrl + "/gettotalvagasimportadas");
    };

    var getTotalProjetosParados = function () {
        return $http.get(serviceUrl + "/gettotalprojetosparados");
    };

    return {
        getProjetosQtdVagas: getProjetosQtdVagas,
        getStatusProjetos: getStatusProjetos,
        getTotalVagasImportadas: getTotalVagasImportadas,
        getTotalProjetosParados: getTotalProjetosParados

    };
}]);