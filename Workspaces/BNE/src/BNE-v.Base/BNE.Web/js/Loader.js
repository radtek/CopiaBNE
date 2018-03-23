// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo Loader-pkd.js!


// <![CDATA[

Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

function BeginRequestHandler(sender, args)
{
    var element = $("#" + args.get_postBackElement().id);

    if (element.attr("class").search(/loader/gi) < 0)
    {
        $("#loadGlobal").attr("style", "display:block");
    }
    else
    {
        element.attr("disabled", true);
        element.after("<img src='img/webfopag/loader.gif' title=Aguarde />");
    }
}

function EndRequestHandler(sender, args)
{
    $("#loadGlobal").attr("style", "display:none");
}

if (document.images)
{
    preload_image = new Image(16, 16);
    preload_image.src = 'img/webfopag/loader.gif';
}

// ]]>