using System;
using System.Linq;
using BNE.BLL.BINGGeocodeService;

namespace BNE.BLL.Custom.Maps.Bing
{
    internal class Service
    {

        public class Request
        {
            public Address Address { get; set; }
        }

        public class Address
        {
            public string Route { get; set; }
            public string StreetNumber { get; set; }
            public string PostalCode { get; set; }
            public string Locality { get; set; }
            public string StateShortName { get; set; }
        }

        #region GetResponse
        public static GeocodeResponse GetResponse(Request geoCodeRequest)
        {
            var bingMapsAPIKey = Parametro.RecuperaValorParametro(Enumeradores.Parametro.BingMapsAPIKey);

            if (string.IsNullOrEmpty(bingMapsAPIKey))
                throw new Exception("A chave do BING não está configurada.");

            GeocodeResponse resposta;
            using (var client = new GeocodeServiceClient("BasicHttpBinding_IGeocodeService"))
            {
                resposta = client.Geocode(new GeocodeRequest
                {
                    Address = new BINGGeocodeService.Address
                    {
                        AddressLine = string.Format("{0}, {1}", geoCodeRequest.Address.Route, geoCodeRequest.Address.StreetNumber),
                        Locality = geoCodeRequest.Address.Locality,
                        AdminDistrict = geoCodeRequest.Address.StateShortName,
                        PostalCode = geoCodeRequest.Address.PostalCode
                    },
                    Credentials = new Credentials { ApplicationId = bingMapsAPIKey }
                });
            }

            return resposta;
        }
        #endregion

        #region RecuperarCoordenada
        public static GeocodeLocation RecuperarCoordenada(string descricaoLogradouro, string numeroEndereco, string numeroCEP, string nomeCidade, string siglaEstado)
        {
            var apiRequest = new Request
            {
                Address = new Address
                {
                    PostalCode = numeroCEP,
                    Route = descricaoLogradouro,
                    StreetNumber = numeroEndereco,
                    Locality = nomeCidade,
                    StateShortName = siglaEstado
                }
            };

            var apiResponse = GetResponse(apiRequest);

            var retorno = apiResponse.Results.FirstOrDefault();

            //Se não tem retorno, abre para buscar sem o número
            if (retorno == null)
            {
                apiRequest = new Request
                {
                    Address = new Address
                    {
                        PostalCode = numeroCEP,
                        Route = descricaoLogradouro,
                        Locality = nomeCidade,
                        StateShortName = siglaEstado
                    }
                };

                apiResponse = GetResponse(apiRequest);

                retorno = apiResponse.Results.FirstOrDefault();
            }

            if (retorno != null)
            {
                var location = retorno.Locations.FirstOrDefault();
                if (location != null)
                    return location;
            }
            return null;
        }
        #endregion

    }
}
