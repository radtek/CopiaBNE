using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace BNE.BLL.Enumeradores
{
    public enum UsuarioSistemaTanque
    {
        [Display(Name="SMS Vaga BNE DDD 41")]
        vddd41,
        [Display(Name="SMS Vaga BNE DDD 11")]
        vddd11,
        [Display(Name="SMS Aviso Saldo BNE")]
        v2,
        [Display(Name="SMS Experiencia BNE")]
        experiencia,
        [Display(Name="AvisoCvsNaoVistos")]
        AvisoCvsNaoVistos,
        [Display(Name="BNE Quem me Viu")]
        QuemMeViu
    }
}
