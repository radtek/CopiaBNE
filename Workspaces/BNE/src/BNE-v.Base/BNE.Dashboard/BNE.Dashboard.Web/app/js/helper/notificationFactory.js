app.factory('notificationFactory', ['ngAudio', function (ngAudio) {
    return {
        success: function (text) {
            toastr.success(text, "Success");
        },
        error: function (text, playSound) {
            if (playSound) {
                ngAudio.load('/Content/sounds/beep.mp3').play();

            }
            if (text) {
                toastr.error(text, "Error");
            }
        }
    };
}]);