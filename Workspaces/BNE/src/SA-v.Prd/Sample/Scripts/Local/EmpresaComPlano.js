﻿

function Ordenar(order) {
    document.getElementById('Ordenacao').value = order;
    console.log(document.getElementById('Ordenacao').value);
    document.getElementById('btnSubmit').click();
}
function Pagina(pag) {
    document.getElementById('pag').value = pag;
}

$(document).click(function (event) {
    if (event.target.className == 'page-link') {
        document.getElementById('btnSubmit').click();
    }
});

$("#idForm").submit(function (e) {
    $("#div_carregando").show();
});

$(function () {
    //Datemask dd/mm/yyyy
    $("#datemask").inputmask("dd/mm/yyyy", { "placeholder": "dd/mm/yyyy" });
    //Datemask2 mm/dd/yyyy
    $("#datemask2").inputmask("mm/dd/yyyy", { "placeholder": "mm/dd/yyyy" });
    //Money Euro
    $("[data-mask]").inputmask();

    //Date range picker
    $('#FlDataRetorno').daterangepicker();
    $('#FlDataCadastro').daterangepicker();
    //Date range picker with time picker
    $('#FlDtaRetornotime').daterangepicker({ timePicker: true, timePickerIncrement: 30, format: 'MM/DD/YYYY h:mm A' });
    //Date range as a button
    $('#daterange-btn').daterangepicker(
        {
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
                'Last 7 Days': [moment().subtract('days', 6), moment()],
                'Last 30 Days': [moment().subtract('days', 29), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
            },
            startDate: moment().subtract('days', 29),
            endDate: moment()
        },
        function (start, end) {
            $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        }
    );

    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal',
        radioClass: 'iradio_minimal'
    });
    //Red color scheme for iCheck
    $('input[type="checkbox"].minimal-red, input[type="radio"].minimal-red').iCheck({
        checkboxClass: 'icheckbox_minimal-red',
        radioClass: 'iradio_minimal-red'
    });
    //Flat red color scheme for iCheck
    $('input[type="checkbox"].flat-red, input[type="radio"].flat-red').iCheck({
        checkboxClass: 'icheckbox_flat-red',
        radioClass: 'iradio_flat-red'
    });

    //Colorpicker
    $(".my-colorpicker1").colorpicker();
    //color picker with addon
    $(".my-colorpicker2").colorpicker();

    //Timepicker
    $(".timepicker").timepicker({
        showInputs: false
    });
});

$(document).ready(function () {
    $("li").removeClass("active");
    $("#tables").addClass("active");
    $("#tables_menu").css("display", "block");
    $("#tables_datatables").addClass("active");
    $("#tables_arrow").addClass("fa-angle-down");

    var listCheck = document.getElementsByClassName("iCheck-helper");
    listCheck[0].addEventListener("click", checkTodo, false);
    for (var i = 1; i < listCheck.length; i++) {
        listCheck[i].addEventListener("click", addEmpresaEnvio, false);
    }
});








