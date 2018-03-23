using System;

namespace BNE.ValidaCelular
{
    internal interface IValidaCelular
    {

        string GerarCodigo(string numeroDDD, string numeroCelular);

        bool ValidarCodigo(string numeroDDD, string numeroCelular, string codigoValidacao);

        bool UtilizarCodigo(string numeroDDD, string numeroCelular, string codigoValidacao);

        bool CodigoEnviado(string numeroDDD, string numeroCelular, out DateTime? dataEnvio);


    }
}
