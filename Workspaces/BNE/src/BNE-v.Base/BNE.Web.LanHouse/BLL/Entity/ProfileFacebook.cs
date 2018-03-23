using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BNE.Web.LanHouse.BLL.Entity
{
    [Serializable]
    public class ProfileFacebook
    {
        [Serializable]
        public class DadosFacebook
        {

            #region Propriedades
            public string CodigoFacebook { get { return id; } }
            public string Nome { get { return name; } }

            public DateTime? DataNascimento
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(birthday))
                        return null;

                    return Convert.ToDateTime(birthday, new DateTimeFormatInfo { ShortDatePattern = "MM/dd/yyyy" });
                }
            }

            public string Email { get { return email; } }

            public string Cidade
            {
                get
                {
                    if (location == null)
                        return string.Empty;

                    return location.name;
                }
            }

            public int Sexo
            {
                get
                {
                    if (gender == null)
                        return (int)Enumeradores.Sexo.Masculino;

                    return gender.ToLower().Equals("masculino") ? (int)Enumeradores.Sexo.Masculino : (int)Enumeradores.Sexo.Feminino;
                }
            }

            public string UltimaFuncao
            {
                get
                {
                    if (work != null && work.Length > 0)
                        return work.Where(w => w.position != null).OrderBy(w => w.end_date).Select(w => w.position.name).FirstOrDefault();
                    return null;
                }
            }

            public int? IdEstadoCivil
            {
                get
                {
                    if (string.IsNullOrWhiteSpace(relationship_status))
                        return null;

                    switch (relationship_status.ToLower())
                    {
                        case "casado":
                            return (int)Enumeradores.EstadoCivil.Casado;
                        case "solteiro":
                            return (int)Enumeradores.EstadoCivil.Solteiro;
                        case "divorciado":
                            return (int)Enumeradores.EstadoCivil.Divorciado;
                        case "separado":
                            return (int)Enumeradores.EstadoCivil.Separado;
                        case "viuvo":
                            return (int)Enumeradores.EstadoCivil.Viúvo;
                    }

                    return null;
                }
            }
            #endregion

            #region Mapeamento

            #region Me
            public string id { get; set; }
            public string name { get; set; }
            public string gender { get; set; }
            public string birthday { get; set; }
            public string link { get; set; }
            public string username { get; set; }
            public Location location { get; set; }
            public string email { get; set; }
            public Education[] education { get; set; }
            public Work[] work { get; set; }
            public string relationship_status { get; set; }
            #endregion

            #region Education
            [Serializable]
            public class Education
            {
                #region Propriedades
                public string NomeInstituicao { get { return school.name; } }
                public int? IdEscolaridade
                {
                    get
                    {
                        switch (type)
                        {
                            case "High School":
                                return (int)Enumeradores.Escolaridade.EnsinoMedioCompleto;
                            case "College":
                                return (int)Enumeradores.Escolaridade.SuperiorCompleto;
                            case "Graduate School":
                                return (int)Enumeradores.Escolaridade.PosGraduacaoEspecializacao;
                        }

                        return null;
                    }
                }
                public string NomeCurso
                {
                    get
                    {
                        if (concentration == null)
                            return string.Empty;

                        return concentration.First().name;
                    }
                }
                #endregion

                public School school { get; set; }
                public string type { get; set; }
                public Degree degree { get; set; }
                public Concentration[] concentration { get; set; }
            }
            [Serializable]
            public class School
            {
                public string id { get; set; }
                public string name { get; set; }
            }
            [Serializable]
            public class Concentration
            {
                public string id { get; set; }
                public string name { get; set; }
            }
            [Serializable]
            public class Degree
            {
                public string id { get; set; }
                public string name { get; set; }
            }
            #endregion

            #region Location
            [Serializable]
            public class Location
            {
                public string id { get; set; }
                public string name { get; set; }
            }
            #endregion

            #region Work
            [Serializable]
            public class Work
            {
                #region Propriedades
                public string DescricaoAtividade { get { return description; } }
                public string RazaoSocial
                {
                    get
                    {
                        if (employer == null)
                            return null;
                        return employer.name;
                    }
                }
                public DateTime DataAdmissao
                {
                    get
                    {
                        if (string.IsNullOrWhiteSpace(start_date) || start_date.Equals("0000-00"))
                            return new DateTime(2000, 01, 01);

                        return Convert.ToDateTime(start_date, new DateTimeFormatInfo { ShortDatePattern = "yyyy-MM-dd" });
                    }
                }
                public DateTime? DataDemissao
                {
                    get
                    {
                        if (string.IsNullOrWhiteSpace(end_date) || end_date.Equals("0000-00"))
                            return null;

                        return Convert.ToDateTime(end_date, new DateTimeFormatInfo { ShortDatePattern = "yyyy-MM-dd" });
                    }
                }
                public string Funcao { get { return position != null ? position.name : string.Empty; } }
                #endregion

                public Employer employer { get; set; }
                public Position position { get; set; }
                public string description { get; set; }
                public string start_date { get; set; }
                public string end_date { get; set; }
                public Project[] projects { get; set; }
                public Location location { get; set; }
            }

            [Serializable]
            public class Employer
            {
                public string id { get; set; }
                public string name { get; set; }
            }

            [Serializable]
            public class Position
            {
                public string id { get; set; }
                public string name { get; set; }
            }

            [Serializable]
            public class Project
            {
                public string id { get; set; }
                public string name { get; set; }
                public ProjectMembers[] with { get; set; }
            }

            [Serializable]
            public class ProjectMembers
            {
                public string id { get; set; }
                public string name { get; set; }
            }
            #endregion

            #endregion

        }

        [Serializable]
        public class FotoFacebook
        {

            public Data data { get; set; }

            [Serializable]
            public class Data
            {
                public string url { get; set; }
            }
        }

    }

}