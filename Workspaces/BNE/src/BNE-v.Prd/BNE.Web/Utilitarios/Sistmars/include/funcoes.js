function AbrirPopup(pagina, h, w)
{
	var iTop, iLeft, features
	
	iTop = (window.screen.availHeight - h) / 2
	iLeft = (window.screen.availWidth - w) / 2
	
	features = 'top=' + iTop + ',left=' + iLeft + ',height=' + h + ',width=' + w + ',scrollbars=yes';
	
	novaJanela = window.open(pagina, 'pop3', features);
	novaJanela.focus();
}

function dt_onkeyup(obj)
{
	var dtnasc = obj;
	var tecla;
	var BACKSPACE= 8;  
	var key; 
	
	CheckTAB=true; 
	if(navigator.appName.indexOf("Netscape")!= -1) 
		tecla= event.which; 
	else
	{ 
		tecla= event.keyCode; 
		key = String.fromCharCode(tecla); 
	}
	if ( tecla == 13 ) 
		return false; 
	if ( tecla == BACKSPACE ) 
		return true; 
	if ( tecla == 46 ) 
		return true; 
		
	if(dtnasc.value.substring(0,2)>31 || dtnasc.value.substring(0,2)=="00")
	{
		alert('Dia Invalido');
		dtnasc.value="";
		return false;
	}
	else
	{
		if(dtnasc.value.substring(3,5)>12 || dtnasc.value.substring(3,5)=="00")
		{
			alert("Mes Invalido");
			dtnasc.value=dtnasc.value.substring(0,3);
			return false;
		}
		else
		{
			if((dtnasc.value.substring(6,10)>2003 && dtnasc.value.substring(6,10).length>3) ||
				(dtnasc.value.substring(6,10)<1900 && dtnasc.value.substring(6,10).length>3))
			{
				alert("Ano Invalido");
				dtnasc.value=dtnasc.value.substring(0,6);
				dtnasc.focus(); 
				return false;
			}
			else
			{
				if(dtnasc.value.length==2)
					dtnasc.value+="/";
				if(dtnasc.value.length==5)
					dtnasc.value+="/";
			}
		}		
	}
}
