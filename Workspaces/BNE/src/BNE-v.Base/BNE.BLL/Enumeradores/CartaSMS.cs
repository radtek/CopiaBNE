namespace BNE.BLL.Enumeradores
{
    public enum CartaSMS : int
    {
        BoasVindasVIP = 1,
        ValidacaoCelularEnvioCodigo = 2,
        SMSAviso_SaldoZerou = 3,
        SMSAviso_SaldoDisponivelMod1 = 4,
        SMSAviso_SaldoDisponivelMod2 = 5,
        SMSAviso_SaldoDisponivelMod3 = 6,     
        AlertaExperienciaProfissional = 7,
        AlertaExperienciaProfissional2dias = 8,
        AlertaExperienciaProfissionalFraca2dias = 9,
        SMSAlerta_CvsNaoVistos = 10
    }
}
