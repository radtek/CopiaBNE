﻿--CREATE PROCEDURE [dashboard].[SPVerificarPlanosPessoaFisicaNaoEncerradoCorretamente]
--	@param1 int = 0,
--	@param2 int
--AS
--	SELECT  COUNT(*)
--    FROM    BNE_Curriculo C WITH(NOLOCK)
--            JOIN TAB_Pessoa_Fisica PF WITH(NOLOCK) ON C.Idf_Pessoa_Fisica = PF.Idf_Pessoa_Fisica
--            JOIN TAB_Usuario_Filial_Perfil UFP WITH(NOLOCK) ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica
--            JOIN BNE_Plano_Adquirido PA WITH(NOLOCK) ON UFP.Idf_Usuario_Filial_Perfil = PA.Idf_Usuario_Filial_Perfil  
--    WHERE   Dta_Fim_Plano < GETDATE()
--            AND PA.Idf_Plano_Situacao = 1 
--            AND PA.Idf_Filial IS NULL
--RETURN 0
