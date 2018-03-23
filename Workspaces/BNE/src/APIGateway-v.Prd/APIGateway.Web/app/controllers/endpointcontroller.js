app.controller('EndpointController', function ($scope, Endpoint, Api, $timeout, $routeParams) {
    $scope.gatewayUrl = config.gatewayurl;

    if (Endpoint) {
        $scope.endpoint = Endpoint;
    }
    console.log(Endpoint);
});
