
angular.module('Azulzinho')
.filter('formatToUrl', function () {
    return function (input) {
        if (!input) return '';

        return FormatToUrl(input);
    }
});