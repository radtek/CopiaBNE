using System.ComponentModel;

namespace BNE.Services.Base.ProcessosAssincronos
{
    /// <summary>
    /// Enumeração dos tipos de saída permitidos
    /// </summary>
    public enum TipoSaida
    {
        [Description("PDF")]
        Pdf = 1,
        [Description("Excel")]
        Excel = 2,
        [Description("Matricial")]
        Matricial = 3,
        [Description("Email")]
        Email = 4,
        [Description("SMS")]
        SMS = 5,
    }
}