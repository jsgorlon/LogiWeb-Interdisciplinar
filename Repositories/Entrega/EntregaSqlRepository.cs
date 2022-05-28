using System.Data.SqlClient;
using logiWeb.Models;

namespace logiWeb.Repositories
{
    public class EntregaSqlRepository : DBContext, IEntregaRepository
    {
        private SqlCommand cmd = new SqlCommand();
        private IOrdemRepository ordemRepository;
        private IStatusRepository statusRepository;

        public EntregaSqlRepository(IOrdemRepository ordemRepository,IStatusRepository statusRepository)
        {
            this.ordemRepository = ordemRepository;
            this.statusRepository = statusRepository;
        }

        public void Cadastrar(Entrega entrega, int[] idOrdem)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"INSERT INTO ENTREGAS (id_funcionario, id_motorista) 
                                    VALUES (@id_funcionario, @id_motorista); ";

                cmd.Parameters.AddWithValue("@id_funcionario", entrega.IdFuncionario);
                cmd.Parameters.AddWithValue("@id_motorista", entrega.IdMotorista);
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"select SCOPE_IDENTITY()";
                SqlDataReader reader = cmd.ExecuteReader();
                int idEntrega = 0;

                if (reader.Read())
                {
                    idEntrega = (int)reader["ID"];
                }
                //status padrao 1
                cmd.CommandText = @"INSERT INTO status_entrega (id_entrega, id_status) 
                                    VALUES (@id_entrega, 1); ";
                cmd.Parameters.AddWithValue("@id_entrega", idEntrega);
                cmd.ExecuteNonQuery();

                foreach (var idOrd in idOrdem)
                {
                    cmd.CommandText = @"INSERT INTO entregas_ordens (ordem_id, entrega_id, status) 
                                        VALUES (@id_ordem,  @id_entrega , 1); ";
                    cmd.Parameters.AddWithValue("@id_ordem", idOrd);
                    cmd.Parameters.AddWithValue("@id_entrega", idEntrega);
                    cmd.ExecuteNonQuery();
                }
            }
              catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public void Excluir(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE ENTREGAS SET ATIVO = 0 WHERE ID_ENTREGA = @ID";
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
              catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }

        public List<Entrega> MostrarEntregas()
        {
            try{
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT E.ID, E.ID_FUNCIONARIO, E.ID_MOTORISTA, 
                                    F.NOME NOME_FUNCIONARIO, M.NOME NOME_MOTORISTA,
                                    SE.ID_STATUS, SE.DATA_CAD, S.NOME STATUS, S.DESCRICAO
                                    FROM ENTREGAS E
                                    INNER JOIN PESSOAS F
                                        ON E.ID_FUNCIONARIO = F.ID 
                                    INNER JOIN PESSOAS M
                                        ON E.ID_MOTORISTA = M.ID 
                                    INNER JOIN status_entrega SE
                                        ON E.ID = SE.ID_ENTREGA
                                    INNER JOIN STATUS S
                                        ON SE.ID_STATUS = S.ID
                                    WHERE E.ATIVO > 0
                ";

                SqlDataReader reader = cmd.ExecuteReader();

                List<Entrega> lista = new List<Entrega>();

                while (reader.Read())
                {
                    Entrega item = new Entrega();
                    item.Id = (int)reader["ID"];
                    item.IdFuncionario = (int)reader["ID_FUNCIONARIO"];
                    item.IdMotorista = (int)reader["ID_MOTORISTA"];
                    item.Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"];
                    item.Motorista.Nome = (string)reader["NOME_MOTORISTA"];
                    item.Status.Id = (int)reader["ID_STATUS"];
                    item.DataCadastro = (DateTime)reader["DATA_CAD"];
                    item.Status.Nome = (string)reader["STATUS"];
                    item.Status.Descricao = (string)reader["DESCRICAO"];
                    lista.Add(item);
                }
                return lista;
            }catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
            
        }

        public Entrega MostrarEntrega(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SELECT E.ID, E.ID_ORDEM, E.ID_FUNCIONARIO, E.ID_MOTORISTA, 
                                    F.NOME NOME_FUNCIONARIO, M.NOME NOME_MOTORISTA,
                                    SE.ID_STATUS, SE.DATA_CAD, S.NOME STATUS, S.DESCRICAO
                                    FROM ENTREGAS E
                                    INNER JOIN PESSOAS F
                                        ON E.ID_FUNCIONARIO = F.ID 
                                    INNER JOIN PESSOAS M
                                        ON E.ID_MOTORISTA = M.ID 
                                    INNER JOIN status_entrega SE
                                        ON E.ID = SE.ID_ENTREGA
                                    INNER JOIN STATUS S
                                        ON SE.ID_STATUS = S.ID                          
                                    WHERE E.ID = @ID_ENTREGA AND E.ATIVO > 0";

                cmd.Parameters.AddWithValue("@ID_ENTREGA", id);
                cmd.ExecuteNonQuery();

                SqlDataReader reader = cmd.ExecuteReader();

                Entrega entrega = new Entrega();

                while (reader.Read())
                {                        
                    entrega.Id = (int)reader["ID"];
                    entrega.IdOrdem = (int)reader["ID_ORDEM"];
                    entrega.IdFuncionario = (int)reader["ID_FUNCIONARIO"];
                    entrega.IdMotorista = (int)reader["ID_MOTORISTA"];
                    entrega.Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"];
                    entrega.Motorista.Nome = (string)reader["NOME_MOTORISTA"];
                    entrega.Status.Id = (int)reader["ID_STATUS"];
                    entrega.DataCadastro = (DateTime)reader["DATA_CAD"];
                    entrega.Status.Nome = (string)reader["STATUS"];
                    entrega.Status.Descricao = (string)reader["DESCRICAO"];               
                }
                return entrega;
            }   
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
         public Entrega MostrarDetalheEntrega(int id)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"SSELECT ent.id, ent.entrega_id, ent.ordem_id, ent.status,
                                    e.id_funcionario, e.id_motorista,
                                    p.nome nome_motorista,
                                    f.nome nome_funcionario,
                                    o.peso, o.volume, o.qtd_itens, o.observacao,
                                    c.nome nome_cliente, c.telefone,
                                    EN.logradouro, EN.nr_casa, EN.complemento, EN.bairro, EN.cep, 
                                    EN.ID_CIDADE, CID.nome NOME_CIDADE, 
                                    CID.ID_ESTADO, EST.NOME NOME_ESTADO EST.sigla_uf,
                                    s.nome, s.descricao
                                    FROM entregas_ordens ent
                                    inner join entregas e
                                        on e.id = ent.entrega_id
                                    inner join pessoas p
                                        on p.id = e.id_motorista
                                    inner join pessoas f
                                        on f.id = e.id_funcionario
                                    inner join ordens o
                                        on o.id = ent.ordem_id
                                    inner join pessoas c
                                        on c.id = o.id_cliente
                                    INNER JOIN enderecos EN
                                        ON O.id_endereco = EN.id
                                    INNER JOIN cidades CID
                                        ON EN.id_cidade = CID.id
                                    INNER JOIN estados EST
                                        ON CID.id_estado = EST.id
                                    inner join status s
                                        on s.id = ent.status
                                    where ent.entrega_id = @ID_ENTREGA ";

                cmd.Parameters.AddWithValue("@ID_ENTREGA", id);
                cmd.ExecuteNonQuery();

                SqlDataReader reader = cmd.ExecuteReader();

                Entrega entrega = new Entrega();

                while (reader.Read())
                {
                        
                    entrega.Id = (int)reader["entrega_id"];
                    entrega.IdOrdem = (int)reader["entrega_id"];
                    entrega.IdFuncionario = (int)reader["ID_FUNCIONARIO"];
                    entrega.IdMotorista = (int)reader["ID_MOTORISTA"];
                    entrega.Funcionario.Nome = (string)reader["NOME_FUNCIONARIO"];
                    entrega.Motorista.Nome = (string)reader["NOME_MOTORISTA"];
                    entrega.Ordem.Volume = (string)reader["VOLUME"];
                    entrega.Ordem.Observacao = (string)reader["OBSERVACAO"];
                    entrega.Ordem.Peso = (decimal)reader["PESO"];
                    entrega.Ordem.Qtd_itens =  (int)reader["qtd_itens"];
                    entrega.Ordem.Cliente.Nome = (string)reader["NOME_CLIENTE"];
                    entrega.Ordem.Cliente.Telefone = (string)reader["TELEFONE"];
                    entrega.Ordem.Endereco.Id = (int)reader["ID_ENDERECO"];
                    entrega.Ordem.Endereco.Logradouro =  (string)reader["LOGRADOURO"];
                    entrega.Ordem.Endereco.Nr_casa =  (string)reader["NR_CASA"];
                    entrega.Ordem.Endereco.Bairro =  (string)reader["BAIRRO"];
                    entrega.Ordem.Endereco.Complemento =  (string)reader["COMPLEMENTO"];
                    entrega.Ordem.Endereco.Cep =  (string)reader["CEP"];
                    entrega.Ordem.Endereco.IdCidade = (int)reader["ID_CIDADE"];
                    entrega.Ordem.Endereco.Cidade =  (string)reader["NOME_CIDADE"];
                    entrega.Ordem.Endereco.IdEstado = (int)reader["ID_ESTADO"];
                    entrega.Ordem.Endereco.Estado =  (string)reader["NOME_ESTADO"];
                    entrega.Ordem.Endereco.Uf =  (string)reader["SIGLA_UF"];
                    
                }
                return entrega;
            }   
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
        public void StatusOrdem(Ordem ordem)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE entregas_ordens SET STATUS = @ID_STATUS 
                                    WHERE ordem_id = @ID_ORDEM";
                cmd.Parameters.AddWithValue("@ID_STATUS", ordem.IdStatus);
                cmd.Parameters.AddWithValue("@ID_ORDEM", ordem.Id);
                cmd.ExecuteNonQuery();
            }
              catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
         public void StatusEntrega(Entrega entrega)
        {
            try
            {
                cmd.Connection = connection;
                cmd.CommandText = @"UPDATE status_entrega SET ID_STATUS = @ID_STATUS 
                                    WHERE ID_ENTREGA = @ID_ENTREGA";
                cmd.Parameters.AddWithValue("@ID_STATUS", entrega.IdStatus);
                cmd.Parameters.AddWithValue("@ID_ENTREGA", entrega.Id);
                cmd.ExecuteNonQuery();
            }
              catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }
        }
    }
}