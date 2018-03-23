var breadcrumbsDirectives = angular.module('breadcrumbsDirectives', []);

breadcrumbsDirectives.directive('breadcrumbLanhouse', function () {
    return {
        restrict: 'E',
        templateUrl: 'views/templates/breadcrumbs/breadcrum_lanhouse.html'
    }
});