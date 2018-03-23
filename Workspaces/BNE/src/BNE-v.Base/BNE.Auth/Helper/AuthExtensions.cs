using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Auth.Helper
{
    public static class AuthExtension
    {
        public static int? GetUnchekedPessoaFisicaId(this ClaimsIdentity identity)
        {
            var claimCur = identity.Claims.FirstOrDefault(obj => obj.ClaimType == BNEClaimTypes.PessoaFisicaId);
            if (claimCur == null)
                return null;

            int curId;
            if (int.TryParse(claimCur.Value, out curId))
            {
                return curId;
            }

            return null;
        }

        public static int? GetPessoaFisicaId(this ClaimsIdentity identity)
        {
            if (identity == null || !identity.IsAuthenticated || identity.Claims == null)
                return null;

            return GetUnchekedPessoaFisicaId(identity);
        }

        public static int? GetCurriculoId(this ClaimsIdentity identity)
        {
            if (identity == null || !identity.IsAuthenticated || identity.Claims == null)
                return null;

            return GetUncheckedCurriculoId(identity);
        }

        public static int? GetUncheckedCurriculoId(this ClaimsIdentity identity)
        {
            var claimCur = identity.Claims.FirstOrDefault(obj => obj.ClaimType == BNEClaimTypes.CurriculoId);
            if (claimCur == null)
                return null;

            int curId;
            if (int.TryParse(claimCur.Value, out curId))
            {
                return curId;
            }

            return null;
        }

        public static bool? TipoCandidato(this ClaimsIdentity identity)
        {
            if (identity == null || !identity.IsAuthenticated || identity.Claims == null)
                return null;

            var claimPerf = identity.Claims.FirstOrDefault(obj => obj.ClaimType == BNEClaimTypes.PerfilUsuario);
            if (claimPerf == null)
                return null;

            AuthPerfilType perfType;
            if (Enum.TryParse<AuthPerfilType>(claimPerf.Value, out perfType))
            {
                return perfType.HasFlag(AuthPerfilType.Candidato);
            }

            return null;
        }

    }
}
