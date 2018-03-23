//Recupera o componente usando jQuery pelo ID original definido no .aspx
function RecuperarComponente(id_original)
{
    return $("*[id*='" + id_original + "']");
}

function RecuperarComponenteValor(campo)
{
    var i = 0;
    var re = "/*txtValor$";
    if(campo != null && campo != undefined)
    {
        while(campo.childNodes[i] != undefined)
        {
            if(campo.childNodes[i].id.match(re) != null)
            {
                return campo.childNodes[i];
            }
            i++;
        }
    }  
}
function RecuperarComponenteCustomValidator(campo)
{
    var i = 0;
    var j = 0;
    var re = "/*cvValor$";
    if(campo != null && campo != undefined)
    {
        while(campo.childNodes[i] != undefined)
        {
            if (campo.childNodes[i].childNodes.length > 0) 
            {
                while (campo.childNodes[i].childNodes[j] != undefined) 
                {
                    if(campo.childNodes[i].childNodes[j].id.match(re) != null)
                        return campo.childNodes[i].childNodes[j];
                }
                j++;
            } 
            else 
            {
                if(campo.childNodes[i].id.match(re) != null)
                    return campo.childNodes[i];
            }
            i++;
        }
    }  
}
function RecuperarComponenteFilho(campo, nome)
{
    var i = 0;
    while(campo.childNodes[i] != undefined)
    {
        if(campo.childNodes[i].id.match(nome) != null)
        {
            return campo.childNodes[i];
        }
        i++;
    } 
}
function RecuperarNomeComponenteUltimoFoco()
{
    var componente = $get('ctl00_hfUltimoFoco');
    if(componente != undefined)
        return componente.value;
    else
        return undefined;
    
}
function DefinirValorComponenteTelefone(campo, ddd, fone)
{
    var i = 0;
    while(campo.childNodes[i] != undefined)
    {
        if(campo.childNodes[i].id.match('txtDDD') != null)
        {
            campo.childNodes[i].value = ddd;
        }
        if(campo.childNodes[i].id.match('txtFone') != null)
        {
            campo.childNodes[i].value = fone;
            break;
        }
        i++;
    }
}
function HabilitarComponenteTelefone(campo, desabilitar, limparValor)
{
    var i = 0;
    while(campo.childNodes[i] != undefined)
    {
        if(campo.childNodes[i].id.match('txtDDD') != null)
        {
            campo.childNodes[i].disabled = desabilitar;
            if(limparValor)
                campo.childNodes[i].value = "";
        }
        if(campo.childNodes[i].id.match('txtFone') != null)
        {
            campo.childNodes[i].disabled = desabilitar;
            if(limparValor)
                campo.childNodes[i].value = "";
            break;
        }
        i++;
    }
}
function HabilitarComponente(campo, desabilitar, limparValor)
{
    campo.disabled = desabilitar;   
    if(limparValor)
    {
        campo.value = "";
    }
}
function CalculaDiferencaAnos(data1, data2)
{
    var diferencaDias = CalculaDiferencaDias(data1,data2);
    var diferencaAnos = diferencaDias/365;
    var diferencaAnosArredondado = Math.round(diferencaAnos);
    
    if(diferencaAnosArredondado > diferencaAnos)
        return diferencaAnosArredondado-1;
    else
        return diferencaAnosArredondado;
}
function CalculaDiferencaDias(data1, data2)
{
    // Calcula o número de milisegundos em um dia
    var milisegundosDia = 1000 * 60 * 60 * 24
    
    //Recupera a diferença em milisegundos
    var diferencaMS = CalculaDiferencaMilisegundos(data1,data2);
    var diferencaDias = diferencaMS/milisegundosDia;
    var diferencaDiasArredondado = Math.round(diferencaDias);
    
    if(diferencaDiasArredondado > diferencaDias)
        return diferencaDiasArredondado-1;
    else
        return diferencaDiasArredondado;
}
function CalculaDiferencaMilisegundos(data1, data2)
{
    // Converte as duas datas para milisegundos
    var data1MS = data1.getTime();
    var data2MS = data2.getTime();

    // Calcula a diferença entre os milisegundos
    return Math.abs(data1MS - data2MS);
}
function ValidarCampo(validadores)
{
    var i;
    for(i = 0; i < validadores.length; i++)
    {
        if(!$get(validadores[i]).isvalid)
            return false;
    }
    return true;
}
function AtualizarDescricaoItemLista(componenteLista, valor)
{
    var i = 0;
    for(i = 0; i < componenteLista.childNodes.length; i++)
    {
        if(componenteLista.childNodes[i].id.match('hfSugestao'))
        {
            var valores = componenteLista.childNodes[i].value.split('Æ');
            var j = 0;
            for (j = 0; j < valores.length; j += 2)
            {
                if (valores[j] == valor)
                {
                    RecuperarLabelDescricaoLS(componenteLista).innerText = valores[j + 1];
                    return;
                }
            }
        }
    }
    RecuperarLabelDescricaoLS(componenteLista).innerText = "";
    
} 
function RecuperarLabelDescricaoLS(componenteLista)
{
    var i = 0;
    for(i = 0; i < componenteLista.childNodes.length; i++)
    {
        if(componenteLista.childNodes[i].id.match('lblValor'))
        {
            return componenteLista.childNodes[i];
        }
    }
}

function AtivarValidadoresData(componenteData, ativar)
{
    var i;
    var j;
    for(j = 0; j < componenteData.childNodes.length; j++)
    {
        if(componenteData.childNodes[j].id.match('pnlValidador'))
        {
            var painelValidadores = componenteData.childNodes[j];
            
            for(i = 0; i < painelValidadores.childNodes.length; i++)
            {
                if(painelValidadores.childNodes[i].id != undefined)
                {
                    if(painelValidadores.childNodes[i].id.match('rfValor') || 
                       painelValidadores.childNodes[i].id.match('cvValor') || 
                       painelValidadores.childNodes[i].id.match('rvValor'))  
                    {
                        ValidatorEnable(painelValidadores.childNodes[i],ativar);
                    }
                }
            }
        }
        return;
    }
}

function AtivarValidadoresListaSugestao(componenteListaSugestao, ativar)
{
    var i;
    var j;
    for(j = 0; j < componenteListaSugestao.childNodes.length; j++)
    {
        if(componenteListaSugestao.childNodes[j].id.match('pnlValidador'))
        {
            var painelValidadores = componenteListaSugestao.childNodes[j];
            for(i = 0; i < painelValidadores.childNodes.length; i++)
            {
                if(painelValidadores.childNodes[i].id != undefined)
                {
                    if(painelValidadores.childNodes[i].id.match('rfValor') || 
                       painelValidadores.childNodes[i].id.match('reValor'))  
                    {
                        ValidatorEnable(painelValidadores.childNodes[i],ativar);
                    }
                }
            }
        }
        return;
    }
}