$(document).ready(function () {
    $(".vaga").each(
        function () {
            var self = $(this);
            var link = self.find(".link");
            self.find(".rrssb-buttons").rrssb({ url: link.attr('href'), title: link.attr('title') });
        }
    );
});