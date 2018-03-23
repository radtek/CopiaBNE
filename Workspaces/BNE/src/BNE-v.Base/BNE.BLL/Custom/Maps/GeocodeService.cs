using System.Globalization;

namespace BNE.BLL.Custom.Maps
{
    public class GeocodeService
    {

        public static Result RecuperarCoordenada(string descricaoLogradouro, string numeroEndereco, string numeroCEP, string nomeCidade, string siglaEstado, Provider provider)
        {
            Result retorno = null;
            switch (provider)
            {
                case Provider.Bing:
                    var resultadoBing = Bing.Service.RecuperarCoordenada(descricaoLogradouro, numeroEndereco, numeroCEP, nomeCidade, siglaEstado);

                    if (resultadoBing != null)
                    {
                        retorno = new Result
                            {
                                Latitude = resultadoBing.Latitude,
                                Longitude = resultadoBing.Longitude
                            };
                    }
                    break;
                case Provider.Google:
                    var resultadoGoogle = Google.Service.RecuperarCoordenada(descricaoLogradouro, numeroEndereco, numeroCEP, nomeCidade, siglaEstado);

                    if (resultadoGoogle != null)
                    {
                        retorno = new Result
                            {
                                Latitude = resultadoGoogle.Geometry.Location.Latitude,
                                Longitude = resultadoGoogle.Geometry.Location.Longitude
                            };
                    }
                    break;
            }
            return retorno;
        }

        public enum Provider
        {
            Bing,
            Google
        }

        public class Result
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }


}
