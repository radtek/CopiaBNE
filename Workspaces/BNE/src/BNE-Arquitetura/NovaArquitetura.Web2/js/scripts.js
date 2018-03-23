(function(){
    $('.accordion').on('shown.bs.collapse', function (e) {
        console.log(e.target);
    })
})();