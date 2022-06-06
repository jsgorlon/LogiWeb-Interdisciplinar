
use LogiWeb; 

DROP TABLE IF EXISTS status_entrega;
DROP TABLE IF EXISTS status;
DROP TABLE IF EXISTS entregas;
DROP TABLE IF EXISTS ordens;
DROP TABLE IF EXISTS clientes;
DROP TABLE IF EXISTS funcionarios;
DROP TABLE IF EXISTS cargos;
DROP TABLE IF EXISTS enderecos;
DROP TABLE IF EXISTS telefones;
DROP TABLE IF EXISTS cidades;
DROP TABLE IF EXISTS estados;
DROP TABLE IF EXISTS pessoas;
DROP TABLE IF EXISTS entregas_ordens;

--- Criação de entidades fortes.
CREATE TABLE pessoas(
  id        INT           IDENTITY, 
  nome      VARCHAR(80)   NOT NULL,
  cpf       VARCHAR(11)   NOT NULL,
  rg        VARCHAR(9)        NULL,
  data_nasc DATE          NOT NULL,
  telefone  VARCHAR(16)   NULL,
  email     VARCHAR(100)  NULL,
  dat_cad   DATETIME      NOT NULL DEFAULT GETDATE(),
  CONSTRAINT pkpessoas_id PRIMARY KEY(id),
  CONSTRAINT ukpessoas_cpf  UNIQUE(cpf)
);

CREATE TABLE clientes 
(
  id_pessoa INT NOT NULL,
  ativo     BIT NOT NULL DEFAULT 1,
  CONSTRAINT pkclientes_id_pessoa PRIMARY KEY(id_pessoa),
  CONSTRAINT fkclientes_id_pessoa FOREIGN KEY(id_pessoa) REFERENCES pessoas(id)
);

CREATE TABLE cargos
(
  id        SMALLINT      IDENTITY, 
  nome      VARCHAR(80)   NOT NULL, 
  descricao TEXT          NULL,
  salario   NUMERIC(8,2)  NOT NULL, 
  CONSTRAINT pkcargos_id      PRIMARY KEY(id),
  CONSTRAINT ukcargos_nome    UNIQUE(nome), 
  CONSTRAINT ckcargos_salario CHECK(salario > 0)
);

CREATE TABLE funcionarios 
(
  id_pessoa INT         NOT NULL, 
  id_cargo  SMALLINT    NOT NULL,
  login     VARCHAR(30) NOT NULL, 
  senha     TEXT        NOT NULL,
  ativo     BIT         NOT NULL DEFAULT 1,
  CONSTRAINT pkfuncionarios_id_pessoa PRIMARY KEY(id_pessoa),
  CONSTRAINT fkfuncionarios_id_pessoa FOREIGN KEY(id_pessoa) REFERENCES pessoas(id),
  CONSTRAINT fkfuncionarios_id_cargo FOREIGN KEY(id_cargo) REFERENCES cargos(id),
  CONSTRAINT ukfuncionarios_login UNIQUE(login)    
);

CREATE TABLE estados 
(
  id       INT         NOT NULL,
  nome     VARCHAR(75) NOT NULL,
  sigla_uf VARCHAR(2)  NOT NULL,
  
  CONSTRAINT pkestados_id            PRIMARY KEY(id), 
  CONSTRAINT ukestados_sigla_uf_nome UNIQUE(sigla_uf, nome) 
); 

CREATE TABLE cidades 
(
  id        INT           IDENTITY,
  nome      VARCHAR(120)  NOT NULL, 
  id_estado INT           NOT NULL,
  ibge      INT           NOT NULL, 
  CONSTRAINT pkcidades_id          PRIMARY KEY(id),
  CONSTRAINT ukcidades_ibge_estado UNIQUE(ibge, id_estado),
  CONSTRAINT fkcidades_id_estado   FOREIGN KEY(id_estado) REFERENCES estados(id)
) ;


CREATE TABLE enderecos
(
  id          INT           IDENTITY,
  id_cidade   INT           NOT NULL, 
  cep         VARCHAR(8)    NOT NULL,
  logradouro  VARCHAR(150)  NULL, 
  nr_casa     VARCHAR(15)   NOT NULL,  
  bairro      VARCHAR(50)   NOT NULL, 
  complemento VARCHAR(100)  NULL, 
  CONSTRAINT pkenderecos_id         PRIMARY KEY(id), 
  CONSTRAINT ukenderecos_cep_nr_casa UNIQUE(cep, nr_casa), 
  CONSTRAINT fkenderecos_id_cidade FOREIGN KEY(id_cidade) REFERENCES cidades(id)
);

CREATE TABLE ordens
(
  id              INT           IDENTITY,
  id_funcionario  INT           NOT NULL,
  id_cliente      INT           NOT NULL,
  id_endereco     INT           NOT NULL,
  qtd_itens       SMALLINT      NOT NULL,  
  peso            NUMERIC(15,4) NOT NULL, 
  volume          VARCHAR(15)   NOT NULL,
  observacao      VARCHAR(100)  NOT NULL,
  CONSTRAINT pkordens_id              PRIMARY KEY(id),
  CONSTRAINT fkordens_id_funcionario  FOREIGN KEY(id_funcionario) REFERENCES funcionarios(id_pessoa),
  CONSTRAINT fkordens_id_cliente      FOREIGN KEY(id_cliente)     REFERENCES clientes(id_pessoa),
  CONSTRAINT fkordens_id_endereco     FOREIGN KEY(id_endereco) REFERENCES enderecos(id),
  CONSTRAINT ckordens_peso            CHECK(peso>0),
  CONSTRAINT ckordens_qtd_itens       CHECK(qtd_itens>=1)
 
); 

CREATE TABLE entregas 
(
  id             INT  IDENTITY, 
  id_funcionario INT  NOT NULL,
  id_motorista   INT  NOT NULL,
  CONSTRAINT pkentregas_id              PRIMARY KEY(id), 

  CONSTRAINT fkentregas_id_funcionario  FOREIGN KEY(id_funcionario) REFERENCES funcionarios(id_pessoa),
  CONSTRAINT fkentregas_id_motorista    FOREIGN KEY(id_motorista) REFERENCES funcionarios(id_pessoa)
); 

CREATE TABLE status 
(
  id        SMALLINT    IDENTITY,
  nome      VARCHAR(50) NOT NULL, 
  descricao TEXT   NULL, 
  CONSTRAINT pkstatus_id   PRIMARY KEY(id),
  CONSTRAINT ukstatus_nome UNIQUE(nome)
); 

create table entregas_ordens(
	ordem_id    int       not null,
	entrega_id  int       not null,
	status_id   SMALLINT  not null,
	constraint pk_entregas_ordens primary key(ordem_id, entrega_id),
	constraint fk_entregas_ordens_ordens foreign key(ordem_id) references ordens(id),
	constraint fk_entregas_ordens_entregas foreign key(entrega_id) references entregas(id),
  constraint fk_entregas_ordens_status_id foreign key(status) references status(id) Declaraçào da fk referenciando status(id)
)

CREATE TABLE status_entrega
(
  id_status  SMALLINT NOT NULL, 
  id_entrega INT      NOT NULL, 
  data_cad   DATETIME     NOT NULL DEFAULT GETDATE(), 
  CONSTRAINT pkstatus_entrega_id_status_id_entrega  PRIMARY KEY(id_status, id_entrega),
  CONSTRAINT fkstatus_entrega_id_status             FOREIGN KEY(id_status)  REFERENCES status(id),
  CONSTRAINT fkstatus_entrega_id_entrega            FOREIGN KEY(id_entrega) REFERENCES entregas(id)
);


-- INSERTS 
INSERT INTO cargos(nome, descricao, salario)
     VALUES ('Operador de Cargas', 'Responsável pelas cargas e os motoristas.', 2000), 
	          ('Atendente', 'Responsável pelo atendimento ao cliente.',           1500),
			      ('Motorista', 'Responsável pela entrega de mercadorias.',           1500);

            
INSERT INTO status(nome, descricao)
     VALUES ('CRIADO', 'Ordem criada pela atendente.'), 
	          ('SEPARAÇÃO', 'Ordem esperando para ser atribuida a um motorista.'),
			      ('SAIU PARA ENTREGA','Ordem atribuida para um motorista.');