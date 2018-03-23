//-- Data: 13/05/2014 11:13
//-- Autor: Lennon Vidal

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BNE.BLL
{
    public partial class ConversasAtivas // Tabela: BNE_Conversas_Ativas
    {
        #region [ SP's Static ]
        #region Spcarregaconversasativasporusuariofilialperfil
        private const string Spcarregaconversasativasporusuariofilialperfil = @"
        SELECT *
        FROM [BNE_Conversas_Ativas] AS ca
        WHERE ca.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
        AND ca.Flg_Mensagem_Pendente = 1";
        #endregion

        #region Spatualizarouinserirconversaativa
        private const string Spatualizarouinserirconversaativa = @"
        IF EXISTS(SELECT TOP 1 1 
          FROM [BNE_Conversas_Ativas] AS ca
          WHERE ca.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
          AND ca.Idf_Curriculo = @Idf_Curriculo)
	        BEGIN
		        UPDATE [BNE].[BNE_Conversas_Ativas]
		        SET Flg_Mensagem_Pendente = 1
		        WHERE Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
				        AND Idf_Curriculo = @Idf_Curriculo
	        END  
        ELSE
            BEGIN
		        INSERT INTO [BNE].[BNE_Conversas_Ativas]
	                ( Idf_Curriculo ,
	                  Idf_Usuario_Filial_Perfil ,
	                  Flg_Mensagem_Pendente
	                )
	        VALUES  ( @Idf_Curriculo ,
	                  @Idf_Usuario_Filial_Perfil ,
	                  1
	                )        
	        END
        SELECT SCOPE_IDENTITY()";
        #endregion

        #region Spatualizarconversainativa
        private const string Spatualizarconversainativa = @"
		UPDATE [BNE].[BNE_Conversas_Ativas]
		SET Flg_Mensagem_Pendente = 0
		WHERE Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
				AND Idf_Curriculo = @Idf_Curriculo
         SELECT SCOPE_IDENTITY()";
        #endregion

        #region [ SpGetConversasArmazenadas ]
        private const string SpPegarCurriculosComConversasArmazenadas = @"
        SELECT ca.Idf_Curriculo, ca.Dta_Ultima_Atualizacao FROM BNE.BNE_Conversas_Ativas ca
        WHERE ca.Flg_Armazenado = 'true' AND CA.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil      
        ";
        #endregion

        #region SpInativarConversa
        private const string SpInativarConversa = @"
        IF EXISTS(SELECT TOP 1 1 
                FROM [BNE_Conversas_Ativas] AS ca
                WHERE ca.Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
                AND ca.Idf_Curriculo = @Idf_Curriculo)
	            BEGIN
		            UPDATE [BNE].[BNE_Conversas_Ativas]
		            SET Flg_Mensagem_Pendente = 0, Dta_Ultima_Atualizacao = GETDATE(), Flg_Armazenado = 'true' 
		            WHERE Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil
				            AND Idf_Curriculo = @Idf_Curriculo
	            END  
            ELSE
                BEGIN
		            INSERT INTO [BNE].[BNE_Conversas_Ativas]
	                    ( Idf_Curriculo ,
	                        Idf_Usuario_Filial_Perfil ,
	                        Flg_Mensagem_Pendente,
						    Flg_Armazenado,
						    Dta_Ultima_Atualizacao
							
	                    )
	            VALUES  ( @Idf_Curriculo ,
	                        @Idf_Usuario_Filial_Perfil ,
	                        0,
						    'true',
							GETDATE()
	                    )        
	            END
        SELECT SCOPE_IDENTITY()";
        #endregion
        #endregion

        #region [ Public Static Methods ]
        public static ConversasAtivas[] PegarConversasAtivas(int usuarioFilialPerfilId)
        {
            var sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter
            {
                ParameterName = "@Idf_Usuario_Filial_Perfil",
                SqlDbType = SqlDbType.Int,
                Size = 4,
                Value = usuarioFilialPerfilId
            });

            var listConversasAtivas = new List<ConversasAtivas>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spcarregaconversasativasporusuariofilialperfil, sqlParams))
            {
                while (dr.Read())
                {
                    var ativa = new ConversasAtivas
                    {
                        _idConversaAtiva = Convert.ToInt32(dr["Idf_Conversa_Ativa"]),
                        _curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"])),
                        _usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"])),
                        _flagMensagemPendente = Convert.ToBoolean(dr["Flg_Mensagem_Pendente"]),
                        _persisted = true,
                        _modified = false
                    };

                    listConversasAtivas.Add(ativa);
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return listConversasAtivas.ToArray();
        }

        public static void AtualizarOuInserirVarios(ConversasAtivas[] conversasAtivas)
        {
            var filter = conversasAtivas.Where(a => a.UsuarioFilialPerfil != null
                                                    && a.Curriculo != null).ToArray();

            var toSaveWithid = filter.Where(a => a.IdConversaAtiva > 0);
            var toSaveWithoutId = filter.Where(a => a.IdConversaAtiva <= 0)
                .GroupBy(a => new { a.UsuarioFilialPerfil.IdUsuarioFilialPerfil, a.FlagMensagemPendente })
                .OrderBy(a => a.Key.IdUsuarioFilialPerfil).ThenBy(b => b.Key.FlagMensagemPendente)
                .ToDictionary(a => a.Key, b => b.ToArray());

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    foreach (var item in toSaveWithid)
                    {
                        item.Update(trans);
                    }

                    foreach (var item in toSaveWithoutId)
                    {
                        Func<SqlTransaction, int, int, int> toInvoke = item.Key.FlagMensagemPendente
                            ? (Func<SqlTransaction, int, int, int>)AtualizarOuInserirMensagemPendente
                            : (Func<SqlTransaction, int, int, int>)AtualizarParaSemMensagemPendente;

                        foreach (var conversa in item.Value)
                        {
                            conversa._idConversaAtiva = toInvoke(trans, conversa.UsuarioFilialPerfil.IdUsuarioFilialPerfil, conversa.Curriculo.IdCurriculo);
                        }
                    }
                    trans.Commit();
                }
            }
        }
        #endregion

        #region [ Private Static Methods ]
        private static int AtualizarParaSemMensagemPendente(SqlTransaction trans, int idUsuarioFilialPerfil, int curriculoId)
        {
            var sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter
            {
                ParameterName = "@Idf_Usuario_Filial_Perfil",
                SqlDbType = SqlDbType.Int,
                Size = 4,
                Value = idUsuarioFilialPerfil
            });

            sqlParams.Add(new SqlParameter
            {
                ParameterName = "@Idf_Curriculo",
                SqlDbType = SqlDbType.Int,
                Size = 4,
                Value = curriculoId
            });

            var res = DataAccessLayer.ExecuteScalar(trans, new CommandType(), Spatualizarconversainativa, sqlParams);
            if (res != null && !Convert.IsDBNull(res))
                return Convert.ToInt32(res);

            return 0;
        }

        private static int AtualizarOuInserirMensagemPendente(SqlTransaction trans, int idUsuarioFilialPerfil, int curriculoId)
        {
            var sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter
            {
                ParameterName = "@Idf_Usuario_Filial_Perfil",
                SqlDbType = SqlDbType.Int,
                Size = 4,
                Value = idUsuarioFilialPerfil
            });

            sqlParams.Add(new SqlParameter
            {
                ParameterName = "@Idf_Curriculo",
                SqlDbType = SqlDbType.Int,
                Size = 4,
                Value = curriculoId
            });

            var res = DataAccessLayer.ExecuteScalar(trans, new CommandType(), Spatualizarouinserirconversaativa, sqlParams);
            if (res != null && !Convert.IsDBNull(res))
                return Convert.ToInt32(res);

            return 0;
        }
        #endregion

        public static IEnumerable<int> RemoverConversasInativas(int idUsuarioFilialPerfil, IEnumerable<KeyValuePair<int, DateTime>> curriculoComUltimaMensagem, DateTime? pesquisaApartirDe)
        {
            var sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter
            {
                ParameterName = "@Idf_Usuario_Filial_Perfil",
                SqlDbType = SqlDbType.Int,
                Size = 4,
                Value = idUsuarioFilialPerfil
            });

            var sql = new StringBuilder(SpPegarCurriculosComConversasArmazenadas);
            if (pesquisaApartirDe.HasValue)
            {
                sqlParams.Add(new SqlParameter
                {
                    ParameterName = "@Data_Limite",
                    SqlDbType = SqlDbType.Date,
                    Value = pesquisaApartirDe.Value
                });

                sql.Append(" AND ca.Dta_Ultima_Atualizacao > @Data_Limite");
            }

            var conversasArmazenadas = new SortedSet<int>();
            using (var reader = DataAccessLayer.ExecuteReader(CommandType.Text, sql.ToString(), sqlParams))
            {
                while (reader.Read())
                {
                    DateTime lastUpdate;
                    int idfCurriculo = Convert.ToInt32(reader["Idf_Curriculo"]);

                    if (Convert.IsDBNull(reader["Dta_Ultima_Atualizacao"]))
                    {
                        lastUpdate = new DateTime();
                    }
                    else
                    {
                        lastUpdate = Convert.ToDateTime(reader["Dta_Ultima_Atualizacao"]);
                    }

                    var found = curriculoComUltimaMensagem.FirstOrDefault(a => a.Key == idfCurriculo);
                    if (found.Key > 0 && lastUpdate >= found.Value)
                    {
                        conversasArmazenadas.Add(found.Key);
                    }
                }
            }

            return curriculoComUltimaMensagem.Select(a => a.Key)
                .Except(conversasArmazenadas);
        }

        public static void InativarConversa(int usuarioFilialPerfilId, int curriculoId)
        {
            var sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter
            {
                ParameterName = "@Idf_Usuario_Filial_Perfil",
                SqlDbType = SqlDbType.Int,
                Size = 4,
                Value = usuarioFilialPerfilId
            });

            sqlParams.Add(new SqlParameter
            {
                ParameterName = "@Idf_Curriculo",
                SqlDbType = SqlDbType.Int,
                Size = 4,
                Value = curriculoId
            });

            using (var con = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                con.Open();
                using (var trans = con.BeginTransaction())
                {
                    try
                    {
                        DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpInativarConversa, sqlParams);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}