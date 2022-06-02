
use LogiTech; 

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
  rg        VARCHAR(9)    NOT NULL,
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
  CONSTRAINT ukfuncionarios_login UNIQUE(login)    
);

CREATE TABLE estados 
(
  id       INT         IDENTITY,
  nome     VARCHAR(75) NOT NULL,
  sigla_uf VARCHAR(2)  NOT NULL,
  
  CONSTRAINT pkestados_id            PRIMARY KEY(id), 
  CONSTRAINT ukestados_sigla_uf_nome UNIQUE(sigla_uf, nome) 
); 

CREATE TABLE cidades 
(
  id        INT           NOT NULL,
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
  logradouro  VARCHAR(150)  NOT NULL, 
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
  volume			VARCHAR(15)   NOT NULL,
  observacao      VARCHAR(100)  NOT NULL,
  qtd_itens       SMALLINT      NOT NULL,  
  peso            NUMERIC(15,4) NOT NULL, 
  ativo     BIT         NOT NULL DEFAULT 1,
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
  ativo     BIT         NOT NULL DEFAULT 1,
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
	ordem_id int not null,
	entrega_id int not null,
	status int not null,
	constraint pk_entregas_ordens PRIMARY KEY(ordem_id, entrega_id), 
	constraint fk_entregas_ordens_ordens foreign key(ordem_id) references ordens(id),
	constraint fk_entregas_ordens_entregas foreign key(entrega_id) references entregas(id),
	CONSTRAINT fkstatus_entregas_ordens FOREIGN KEY(status)  REFERENCES status(id),
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

Insert into Estados ( Nome, sigla_uf) values ('Acre', 'AC');
Insert into Estados ( Nome, sigla_uf) values ('Alagoas', 'AL');
Insert into Estados ( Nome, sigla_uf) values ('Amapá', 'AP');
Insert into Estados ( Nome, sigla_uf) values ('Amazonas', 'AM');
Insert into Estados ( Nome, sigla_uf) values ('Bahia', 'BA');
Insert into Estados ( Nome, sigla_uf) values ('Ceará', 'CE');
Insert into Estados ( Nome, sigla_uf) values ('Distrito Federal', 'DF');
Insert into Estados ( Nome, sigla_uf) values ('Espírito Santo', 'ES');
Insert into Estados ( Nome, sigla_uf) values ('Goiás', 'GO');
Insert into Estados ( Nome, sigla_uf) values ('Maranhão', 'MA');
Insert into Estados ( Nome, sigla_uf) values ('Mato Grosso', 'MT');
Insert into Estados ( Nome, sigla_uf) values ('Mato Grosso do Sul', 'MS');
Insert into Estados ( Nome, sigla_uf) values ('Minas Gerais', 'MG');
Insert into Estados ( Nome, sigla_uf) values ('Pará', 'PA');
Insert into Estados ( Nome, sigla_uf) values ('Paraíba', 'PB');
Insert into Estados ( Nome, sigla_uf) values ('Paraná', 'PR');
Insert into Estados ( Nome, sigla_uf) values ('Pernambuco', 'PE');
Insert into Estados ( Nome, sigla_uf) values ('Piauí', 'PI');
Insert into Estados ( Nome, sigla_uf) values ('Rio de Janeiro', 'RJ');
Insert into Estados ( Nome, sigla_uf) values ('Rio Grande do Norte', 'RN');
Insert into Estados ( Nome, sigla_uf) values ('Rio Grande do Sul', 'RS');
Insert into Estados ( Nome, sigla_uf) values ('Rondônia', 'RO');
Insert into Estados ( Nome, sigla_uf) values ('Roraima', 'RR');
Insert into Estados ( Nome, sigla_uf) values ('Santa Catarina', 'SC');
Insert into Estados ( Nome, sigla_uf) values ('São Paulo', 'SP');
Insert into Estados ( Nome, sigla_uf) values ('Sergipe', 'SE');
Insert into Estados ( Nome, sigla_uf) values ('Tocantins', 'TO');
insert into cidades values (1,'São José do Rio Preto', 25, 1);
insert into enderecos values (1,'15055120', 'Rua aleatoria', '123', 'Jardim Perdido', 'Próximo a auto mecânica');
insert into enderecos values (1,'15055021', 'Rua Undefined', 'NaN', 'Jardim Undefined', 'Próximo a Undefined');

insert into cargos values ('Administrador', 'Administra', 6000);
insert into cargos values ('Motorista', 'Dirige', 1500);
insert into cargos values ('Operador Logistico', 'Opera a logistica', 2000);

insert into status values ('Pendente', 'Objeto não foi alocado em uma entrega');
insert into status values ('Aguardando', 'Objeto está em uma entrega, aguardando a mesma');
insert into status values ('A caminho', 'Objeto está em uma entrega, aguardando a mesma');
insert into status values ('Entregue', 'Objeto foi entregue');
insert into status values ('Destinatário Ausente', 'Objeto não foi entregue pois destinatário nao se encontrava');
insert into status values ('Cancelado', 'Cancelado');

insert into pessoas values ('Joao', '1111111', '11111', '02/02/2002', '(11)1111-1111', 'joao@email.com', GETDATE());
insert into funcionarios values (SCOPE_IDENTITY(), 1,'joao@logitech.com', '123456',1);
insert into pessoas values ('Joana', '2222222', '22222', '03/02/2002', '(22)2222-222', 'joana@email.com', GETDATE());
insert into funcionarios values (SCOPE_IDENTITY(), 1,'joana@logitech.com', '123456',1);
insert into pessoas values ('Josias', '333333', '33333', '04/02/2002', '(33)3333-333', 'josias@email.com', GETDATE());
insert into funcionarios values (SCOPE_IDENTITY(), 1,'josias@logitech.com', '123456',1);

insert into pessoas values ('Jonas', '4444444', '44444', '05/02/2002', '(44)4444-4444', 'jonas@email.com', GETDATE());
insert into clientes values (SCOPE_IDENTITY(), 1);
insert into pessoas values ('Josefina', '55555', '5555', '06/02/2002', '(55)5555-55555', 'josefina@email.com', GETDATE());
insert into clientes values (SCOPE_IDENTITY(), 1);

INSERT INTO ORDENS (id_cliente, id_endereco, id_funcionario, qtd_itens, volume, peso, observacao, ativo) 
                                    VALUES (4, 2, 3, 1,'50 cm^3', 2, 'Cuidado no transporte',1)
INSERT INTO ORDENS (id_cliente, id_endereco, id_funcionario, qtd_itens, volume, peso, observacao, ativo) 
                                    VALUES (5, 1, 3, 1,'680 cm^3', 15, 'Cuidado fragil',1)
									
INSERT INTO ENTREGAS (id_funcionario, id_motorista) 
                                    VALUES (2,3);
INSERT INTO status_entrega (id_entrega, id_status) 
                                    VALUES (SCOPE_IDENTITY(), 1); 
INSERT INTO entregas_ordens (ordem_id, entrega_id, status) 
                                    VALUES (1,  1 , 1);
INSERT INTO entregas_ordens (ordem_id, entrega_id, status) 
                                    VALUES (2,  1 , 1);
INSERT INTO ENTREGAS (id_funcionario, id_motorista) 
                                    VALUES (2,3);
INSERT INTO status_entrega (id_entrega, id_status) 
                                    VALUES (SCOPE_IDENTITY(), 1); 
INSERT INTO entregas_ordens (ordem_id, entrega_id, status) 
                                    VALUES (4,  2 , 1);
INSERT INTO entregas_ordens (ordem_id, entrega_id, status) 
                                    VALUES (3,  2 , 1);              
