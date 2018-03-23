app.factory('Api', function ($resource) {
    return $resource(config.apiurl + '/admin/apis/:id', {id:'@id'},
        {
            grantaccess: { method: 'POST', action: 'grantaccess', params: { id: '@id', chaveSistema: '@chaveSistema' }, isArray: false, url: config.apiurl + '/admin/apis/grantaccess/:id?chaveSistema=:chaveSistema' },
            denyaccess: { method: 'POST', action: 'denyaccess', params: { id: '@id', chaveSistema: '@chaveSistema' }, isArray: false, url: config.apiurl + '/admin/apis/denyaccess/:id?chaveSistema=:chaveSistema' }
        }); // Note the full endpoint address
});