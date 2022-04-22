create table Pessoas (
	id integer not null identity,
	nome varchar(80) not null,
	documento varchar(18) not null,
	tipo_documento varchar(4) not null,
	endereco varchar(200) not null,
	telefone varchar(15) not null,
	add constraint pk_pessoas primary key (id)
);

create table Cargos(
	id integer not null identity,
	nome varchar(50) not null,
	descricao varchar(150) not null,
	add constraint pk_cargos primary key (id)
);

create table Funcionarios(
	pessoa_id int not null,
	salario decimal(10,2) not null,
	cargo_id int not null,
	login varchar(50) not null,
	senha varchar(100) not null
	add constraint fk_funcionarios_pessoas foreign key(pessoa_id) references pessoas (id),
	add constraint fk_funcionarios_cargos foreign key(cargo_id) references cargos (id),
);

create table Clientes(
	pessoa_id int not null

);

create table Entregas(
	id int not null identity,
	funcionario_id int not null,
	data datetime not null,
	add constraint pk_entregas primary key (id)
);

create table Ordens (
	id int not null identity,
	cliente_id int not null,
	tamanho decimal(10,2) not null,
	peso decimal(10,2) not null,
	destino varchar(200) not null,
	origem varchar(200) not null,
	add constraint pk_ordens primary key(id)
);

create table entregas_ordens(
	ordem_id int not null,
	entrega_id int not null,
	status int not null,
	add constraint pk_entregas_ordens primary key (ordem_id, entrega_id),
	add constraint fk_entregas_ordens_ordens foreign key (ordem_id) references ordens (id),
	add constraint fk_entregas_ordens_entregas foreign key (entrega_id) references entregas (id),
);



