let clientes = []; 

class Cliente {
    constructor(){
       this.Nome = $("#nome").val(); 
       this.Telefone = $("#telefone").val().replaceAll(/[^0-9]/gi,''); 
       this.Email = $("#email").val().trim(); 
       this.DatNasc = $("#dat_nasc").val(); 
       this.Cpf = $("#cpf").val().replaceAll(/[^0-9]/gi,''); 
       this.Rg = $("#rg").val().replaceAll(/[^0-9]/gi,''); 
       this.Ativo = true; 
    }
}



let dialogCliente = $.confirm({
    title: `Novo cliente`, 
    content: $("#template_cad_cliente").html(),
    lazyOpen: true,
    closeIcon: true,  
    id_cliente: null, 
    type: 'green', 
    columnClass: 'col-12 col-md-7 col-lg-6', 
    draggable: false, 
    onOpen: () => {
        $('input').on('focus', function(){
            $(this).removeClass('is-invalid');
        });
    },
    onClose: function(){
      this.id_cliente = null; 
    },
    onOpenBefore: function(){

        $("#cpf").mask('000.000.000-00');
        $('#rg').mask('99.999.999-9'); 
        $('#telefone').mask('(00) 00000-0000'); 

        if(this.id_cliente){
            let cliente = clientes.find(user => user.Id == this.id_cliente);
    
            $("#nome").val(cliente.Nome); 
            $("#telefone").val(cliente.Telefone); 
            $("#email").val(cliente.Email); 
            $("#dat_nasc").val(cliente.DatNasc.replace("T00:00:00",'')); 
            $("#cpf").val(cliente.Cpf); 
            $("#rg").val(cliente.Rg); 
        }
    },
    onClose: function(){
        this.id_cliente = null; 
    }, 
    cadastrar: function(){
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Novo Cliente</span>`;
        this.buttons = {
            cadastrar: {
                text: 'Cadastrar',
                btnClass: 'btn btn-sm btn-success btCadastrar', 
                action: function(){
                    
                   let {campos_validos } = validarCampos(); 
                    
                   if(!campos_validos)
                     return false;
                   
                   let cliente = new Cliente(); 

                   $.ajax({
                       url: '/cliente/cadastrar', 
                       type: 'POST',
                       dataType: 'JSON',
                       data: cliente, 
                       success: data => {
                        ajaxResponse(data);

                        obterClientes(); 
                        if(data.error)
                            return false; 
                        else 
                            this.close(); 
                       }
                   });
                    
                   return false; 
                }
            }
        }
        this.open(); 
    },
    editar: function(id_cliente){
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Edição do Cliente</span>`;
        this.id_cliente = id_cliente; 
        this.buttons = {
            salvar: {
                text: 'Salvar',
                btnClass: 'btn btn-sm btn-success btAtualizar', 
                action: function(){
                    
                   let {campos_validos } = validarCampos(); 
                    
                   if(!campos_validos)
                     return false;
                   
                   let cliente = new Cliente(); 

                   cliente["id"] = id_cliente; 

                   $.ajax({
                       url: '/cliente/atualizar', 
                       type: 'POST',
                       dataType: 'JSON',
                       data: cliente, 
                       success: data => {
                          
                            ajaxResponse(data);

                            obterClientes(); 
                            if(data.error)
                                return false; 
                            else 
                                this.close(); 
                       }
                   });
                    
                   return false; 
                }
            }
        }
        this.open(); 
    }
});
$(document).ready(_=>{

    $('table').bootstrapTable({});

    $('table').bootstrapTable('load', {});

    $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});

    $('#btCadastrar').click(_=>dialogCliente.cadastrar());

    obterClientes(); 


    $("#btPesquisar").click(_=>obterClientes(true));
});


function buttons(id, rows) 
{

 const editar = `<button onclick="editar_cliente(this)" class="p-0 m-0 btn btn-sm shadow-none" data-idcliente="${id}">
                     <i class="fa-solid fa-square-pen text-secondary" 
                        data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Editar Cliente"></i>
                 </button>`;
 const inactive = `<button onclick="alterar_status(this)" id="alterar_status_${id}" class="p-0 m-0 btn btn-sm shadow-none" data-idcliente="${id}">
                     <i class="fa-solid fa-circle-minus text-secondary"
                     data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Inativar Cliente"></i>
                   </button>`;

  const active = `<button onclick="alterar_status(this)" id="alterar_status_${id}" class="p-0 m-0 btn btn-sm shadow-none" data-idcliente="${id}">
                      <i class="fa-solid fa-circle-plus  text-secondary"
                         data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Ativar Cliente"></i>
                  </button>`;



 return `<div class="d-flex justify-content-end gap-1 buttons-grid">${editar} ${rows.active ? inactive : active}</div>`;
}


function validarCampos(){
    let nome = $("#nome"); 
    let telefone = $("#telefone"); 
    let email = $("#email"); 
    let dat_nasc = $("#dat_nasc"); 
    let cpf = $("#cpf"); 
 
   

    let msg = ''; 
    
    let validacoes = [
        {
            campo: nome, 
            valido: nome.val() != '',
            msg: 'Nome inválido.'
        },
        {
            campo: telefone, 
            valido: telefone.val() != '' ? telefone.val().replaceAll(/[^0-9]/gi,'').length > 7 : true,
            msg: 'O telefone precisa ter no mínimo 8 digitos.'
        },
        {
            campo: email, 
            valido: true,
            msg: 'E-mail inválido.'
        },
        {
            campo: dat_nasc, 
            valido: dat_nasc.val() != '', 
            msg: 'Data de Nascimento inválida.'
        },
        {
            campo: cpf, 
            valido: cpf_valido(cpf.val()), 
            msg: 'CPF inválido.'
        }
    ];



    validacoes.map(field => {
        if(!field.valido)
        {
            msg += field.msg + '<br>'; 
            field.campo.addClass('is-invalid');
        }
    });

    if(msg != '')
       alert_error('Preencha corretamente os campos inválidos.');

    return {
        campos_validos: msg == ''
    }
}

function formataCPF(cpf){
    //retira os caracteres indesejados...
    cpf = cpf.replace(/[^\d]/g, "");
    
    //realizar a formatação...
      return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
}

function obterClientes(loadingButton = false){


    if(loadingButton){
        $("#btPesquisar").spinner();
    }

    let status = $("[name='flexRadioDefault']:checked").val();
    $.ajax({
         type: 'GET',
         url: '/cliente/todos',
         dataType: 'JSON',
         data: {
            nome: $("#filtro_nome").val(), 
            status: status == 'A' ? null : status 
         },
         success: data => {
         
             let users = [];
              clientes = []; 

             let clientes_cadastrados = data.item.clientes; 

       
             clientes_cadastrados.map(user => {
                let dat_cad = user.datCad.split('T')[0].split('-').reverse().join('/'); 
                users.push({
                        id: user.id, 
                        nome_cpf: `${user.nome} - <b>${formataCPF(user.cpf)}</b>`,
                        active: user.ativo, 
                        dat_cad, 
                        status: user.ativo ? `<span class="badge badge-success rounded-pill bg-success">ATIVO</span>` : `<span class="badge badge-success rounded-pill bg-danger">INATIVO</span>`
                    });
                

                   clientes.push({
                        Id: user.id,
                        Nome: user.nome,  
                        Telefone: user.telefone, 
                        Email: user.email, 
                        DatNasc: user.datNasc, 
                        Cpf: user.cpf, 
                        Rg: user.rg, 
                        Ativo: user.ativo 
                   });
             });

             $('table').bootstrapTable('load', users);

             $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});
            
             if(loadingButton)
                $("#btPesquisar").spinner({submete: false});
         }
     });
}

function editar_cliente(button){
    dialogCliente.editar(button.dataset.idcliente); 
}

function alterar_status(button){
    $(".popover").popover('dispose');

  let id_cliente =   button.dataset.idcliente; 

   $("button").attr('disabled', true);
   $("#"+button.id).spinner();

   let cliente = clientes.find(user=> user.Id == id_cliente);
  
   cliente.Ativo = !cliente.Ativo; 
   
   delete cliente.Id; 

   $.ajax({
    url: '/cliente/AlterarStatus', 
    type: 'POST',
    dataType: 'JSON',
    data: {
        id: id_cliente, 
        status: cliente.Ativo ? 1 : 0
    }, 
    success: data => {
       $("button").attr('disabled', false);
       
       ajaxResponse(data); 
       obterClientes();
    }
});
}