$(document).ready(
    function () {
        $(".icone_pagamento img").click(
            function () {
                $(this).parent().parent().find("input").click();
            }
        );
    }
);