
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



let dialogCadastrar = $.confirm({
    title: `Novo cliente`, 
    content: $("#template_cad_cliente").html(),
    lazyOpen: true,
    closeIcon: true,  
    id_cliente: null, 
    type: 'green', 
    columnClass: 'col-12 col-md-7 col-lg-6', 
    draggable: false, 
    buttons: {
        cadastrar: {
            text: 'Cadastrar',
            btnClass: 'btn btn-sm btn-success', 
            action: () => {
                alert('Requisição realizada.');
            }
        }
    },
    onOpenBefore: () => {

        $("#cpf").mask('000.000.000-00');
        $('#rg').mask('99.999.999-9'); 
        $('#telefone').mask('(00) 00000-0000'); 
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
                action: () => {
                    
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
                        //obterFuncionarios(); 

                        alert_success("Cliente cadastrado com sucesso!");
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
        this.open(); 
    }
});
$(document).ready(_=>{

    $('table').bootstrapTable({});

    $('table').bootstrapTable('load', {});

    $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});

    $('#btCadastrar').click(_=>dialogCadastrar.cadastrar());

});


function buttons(id, rows) 
{

 const editar = `<button data-bs-toggle="Editar Cliente" data-bs-toggle="popover" class="p-0 m-0 btn btn-sm shadow-none" data-idcliente="${id}">
                     <i class="fa-solid fa-square-pen text-secondary" 
                        data-bs-toggle="tooltip" data-bs-placement="top" title="Editar"></i>
                 </button>`;
 const inactive = `<button data-bs-toggle="Inativar Cliente" data-bs-toggle="popover" class="p-0 m-0 btn btn-sm shadow-none" data-idcliente="${id}">
                     <i class="fa-solid fa-circle-minus text-secondary"
                     data-bs-toggle="tooltip" data-bs-placement="top" title="Inativar"></i>
                   </button>`;

  const active = `<button data-bs-toggle="Inativar Cliente" data-bs-toggle="popover" class="p-0 m-0 btn btn-sm shadow-none" data-idcliente="${id}">
                      <i class="fa-solid fa-circle-plus  text-secondary"
                         data-bs-toggle="tooltip" data-bs-placement="top" title="Ativar"></i>
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
            valido: email.val().trim().length > 0 ? validateEmail(email) : true,
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