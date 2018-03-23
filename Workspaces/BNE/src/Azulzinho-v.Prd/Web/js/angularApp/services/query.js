angular.module('Azulzinho')
.factory('query', function ($routeParams) {

    var queryService = {};

    queryService.text = '';

    if ($routeParams && $routeParams.filtro)
        queryService.text = $routeParams.filtro;

    
    queryService.getTextForUrl = function(){
        FormatToUrl(queryService.text);
    };

    return queryService;
});