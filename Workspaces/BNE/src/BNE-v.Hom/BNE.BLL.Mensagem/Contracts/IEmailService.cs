using System;
using System.Collections.Generic;

namespace BNE.BLL.Mensagem.Contracts
{
    public interface IEmailService
    {
        void EnviarEmail(string key, string de, string para, string assunto, string mensagem, bool deveTentarReenvio);
        void EnviarEmail(string key, string de, string para, string assunto, string mensagem, string nomeAnexo, string anexo, bool deveTentarReenvio);
        void EnviarEmail(string key, string de, string para, string assunto, string mensagem, Dictionary<string, string> anexos, bool deveTentarReenvio);
        void EnviarEmail(string key, string de, List<string> para, Guid templateId, dynamic substitutionParameters, dynamic sectionParameters);
        void FalhaAoEnviar(Email email);
        void Deletar(Email objEmail);
        void Salvar(string key, string de, string para, string assunto, string mensagem);
        void Salvar(string key, string de, string para, string assunto, string mensagem, string nomeAnexo, string anexo);
        IEnumerable<Email> GetAllToSend();
    }
}
