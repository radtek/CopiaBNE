using System.Collections.Generic;

namespace Employer.Componentes.UI.Web.Permissions
{
    /// <summary>
    /// Descreve controles que acessam as permissões do sistema
    /// Deve-se evitar amarrar a biblioteca de controles com a lógica de negócio, então os controles a princípio
    /// são os mais genéricos possíveis, podendo ser especilizados conforme a necessidade, entretanto as permissões
    /// devem ser comuns tanto aos controles mais genéricos, tanto aos controles mais especializados
    /// </summary>
    public interface IPermissionConsumer
    {
        #region Properties
        /// <summary>
        /// As permissões do usuário
        /// </summary>
        List<int> Permissions { get; set; }
        /// <summary>
        /// As permissões do usuário e suas respectivas categorias
        /// </summary>
        Dictionary<int,int> PermissionsWithCategory { get; set; }
        /// <summary>
        /// Código da filial logada
        /// </summary>
        int? Idf_Filial { get; set; }
        /// <summary>
        /// Código do usuário logado
        /// </summary>
        int Idf_Usuario { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Aplica as permissões ao controle
        /// </summary>
        void ApplyPermissions();
        #endregion
    }
}
