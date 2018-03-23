app.factory('Estatistica', function ($resource) {
    return $resource(config.apiurl + '/admin/estatisticas/:tipo', { tipo: '@tipo' });
});