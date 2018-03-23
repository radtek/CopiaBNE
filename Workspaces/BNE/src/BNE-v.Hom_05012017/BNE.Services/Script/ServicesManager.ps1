Param(
  [string]$op
)

if($op -ne "install" -And $op -ne "remove" -And $op -ne "start" -And $op -ne "stop"){
"	USO: BNEServicesManager <option>
		-OPTIONS:
			- install: Instala os serviços
			- remoce: Remove os serviços
			- start: Inicia todos os serviços
			- stop: Para todos os serviços"
	return
}

$exe = "E:\ProgramFiles(x86)\Bne.Services\BNE.Services.exe"
$svcs = "BNE.Services.AllinEmailQuemMeViu",
"BNE.Services.AllInEmailSincronizacaoLista",
"BNE.Services.ArquivarVaga",
"BNE.Services.AtualizarCurriculo",
"BNE.Services.AtualizarEmpresa",
"BNE.Services.AtualizarPlano",
"BNE.Services.AtualizaSitemap",
"BNE.Services.BuscaCoordenada",
"BNE.Services.ControleFinanceiro",
"BNE.Services.ControleParcelas",
"BNE.Services.DebitoOnlineBradesco",
"BNE.Services.DestravaSMSPlanoEmployer",
"BNE.Services.EmailFiliais",
"BNE.Services.EmailsInvalidos",
"BNE.Services.EnviarCurriculo",
"BNE.Services.EnviarEmailAlertaExperienciaProfissional",
"BNE.Services.EnviarEmailConfirmacaoCandidatura",
"BNE.Services.EnviarEmailOportunidade",
"BNE.Services.EnvioSMSEmailEmpresasCvsNaoVistos",
"BNE.Services.EnvioSMSEmpresas",
"BNE.Services.EnvioSMSSemanal",
"BNE.Services.InativarCurriculo",
"BNE.Services.IntegracaoWebfopag",
"BNE.Services.IntegrarVagas",
"BNE.Services.RastreadorCurriculo",
"BNE.Services.SondaBancoDoBrasil",
"BNE.Services.EnvioBoletoAntesDeVencerPlano";

foreach($svc in $svcs)
{
	if($op -eq "stop" -Or $op -eq "remove"){
		"Parando $svc";
		sc.exe stop "$svc"
	}
	if($op -eq "install"){
		"Instalando $svc";
		sc.exe create "$svc" DisplayName= "$svc" binpath= "$exe $svc" start= auto
		sc.exe failure "$svc" reset= 86400 actions= restart/1000/restart/1000/run/1000
		sc.exe failure "$svc" command= "$exe"
	}
	if($op -eq "remove"){
		"Desinstalando $svc";
		sc.exe delete "$svc"
	}
	if($op -eq "start" -Or $op -eq "install"){
		"Iniciando $svc";
		sc.exe start "$svc"
	}
	
}

if($op -eq "stop" -Or $op -eq "remove"){
	"Parando processos BNE.Services";
	kill -processname "Bne.Services" -Force
}