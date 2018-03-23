app.factory('SistemaApi', function ($resource) {
    return $resource(config.apiurl + '/admin/sistema/:id'); // Note the full endpoint address
});