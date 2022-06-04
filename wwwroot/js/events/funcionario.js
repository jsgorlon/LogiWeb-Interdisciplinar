

class Funcionario {
     constructor(){
        this.Nome = $("#nome").val(); 
        this.Telefone = $("#telefone").val().replaceAll(/[^0-9]/gi,''); 
        this.Email = $("#email").val().trim(); 
        this.DatNasc = $("#dat_nasc").val(); 
        this.Cpf = $("#cpf").val().replaceAll(/[^0-9]/gi,''); 
        this.Rg = $("#rg").val().replaceAll(/[^0-9]/gi,''); 
        this.IdCargo = $("#cargo").val(); 
        this.Senha = $("#senha").val(); 
        this.Login = $("#login").val(); 
        this.Ativo = true; 
     }
}

let dialogFuncionario = $.confirm({
    title: `Novo Funcionario`, 
    content: $("#template_cad_funcionario").html(),
    lazyOpen: true,
    closeIcon: true,  
    id_funcionario: null, 
    type: 'green', 
    columnClass: 'col-12 col-md-7 col-lg-6', 
    draggable: false, 
    onClose: function(){
        this.id_funcionario = null; 
    }, 
    onOpenBefore: () => {
        obterCargos('cargo');
        $("#cpf").mask('000.000.000-00');
        $('#rg').mask('99.999.999-9'); 
        $('#telefone').mask('(00) 00000-0000'); 
    },
    cadastrar: function(){
        
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Novo Funcionario</span>`;
        
        this.buttons = {
            cadastrar: {
                text: 'Cadastrar',
                btnClass: 'btn btn-sm btn-success btCadastrar', 
                action: () => {
                    
                   let {campos_validos } = validarCampos(); 
                    
                   if(!campos_validos)
                     return false;
                   
                   let funcionario = new Funcionario(); 

                   $.ajax({
                       url: '/funcionario/cadastrar', 
                       type: 'POST',
                       dataType: 'JSON',
                       data: funcionario, 
                       success: data => {
                        //obterFuncionarios(); 

                        alert_success("Funcionário cadastrado com sucesso!");
                       }
                   });
                    
                   return false; 
                }
            }
        }
        
        this.open(); 
    },
    editar: function(id_funcionario){
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Edição do Funcionario</span>`;
        this.id_funcionario = id_funcionario; 
        this.open(); 
    }
});
$(document).ready(_=>{

    $('table').bootstrapTable({});

    $('table').bootstrapTable('load', {});

    $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});

    $('#btCadastrar').click(_=>dialogFuncionario.cadastrar());

    obterCargos('filter_cargo', 'todos');
});


function buttons(id, rows) 
{

 const editar = `<button data-bs-toggle="Editar Cliente" data-bs-toggle="popover" class="p-0 m-0 btn btn-sm shadow-none" data-idcliente="${id}">
                     <i class="fa-solid fa-square-pen"></i>
                 </button>`;
 const inactive = `<button data-bs-toggle="Inativar Cliente" data-bs-toggle="popover" class="p-0 m-0 btn btn-sm shadow-none" data-idcliente="${id}">
                     <i class="fa-solid fa-circle-minus"></i>
                   </button>`;

  const active = `<button data-bs-toggle="Inativar Cliente" data-bs-toggle="popover" class="p-0 m-0 btn btn-sm shadow-none" data-idcliente="${id}">
                      <i class="fa-solid fa-circle-plus "></i>
                  </button>`;



 return `<div class="d-flex justify-content-end gap-1 buttons-grid">${editar} ${rows.active ? inactive : active}</div>`;
}


function generateUsers(){

    let users = [];
    for(let i = 0; i <= 300; i++){
        var active = Math.random() < 0.5;
       users.push({
        id: i, 
        nome_cpf: `Nome completo da pessoa ${1} -  <b>111.111.111-1${i}</b>`,
        active: active, 
        status: active ? `<span class="badge badge-success rounded-pill bg-success">ATIVO</span>` : `<span class="badge badge-success rounded-pill bg-danger">INATIVO</span>`
       });
    }

    return users; 
}

function showPass(el){

    let showPassword = el.dataset.visible == 'on';
    let icon = $("#eye_icon");     
    if(icon.hasClass('fa-eye-slash')){
        icon.removeClass('fa-eye-slash').addClass('fa-eye');
        $("#senha").attr('type','text');
    }
    else {
        $("#senha").attr('type','password');
        icon.addClass('fa-eye-slash').removeClass('fa-eye');
    }
}

function obterCargos(selectId, ig = 'selecione'){
   return $.ajax({
        type: 'GET',
        url: '/cargo',
        dataType: 'JSON',
        success: data => {
            let newOption = (label, value = '') =>  `<option value='${value}'>${label.toLocaleUpperCase()}</option>`;
            let html = newOption(ig);

            data.map(cargo => html += newOption(cargo.nome, cargo.id));

            console.log(html);
            $('#'+selectId).html(html);
        }
    });
}


function obterFuncionarios(){
    return $.ajax({
         type: 'GET',
         url: '/funcionario/todos',
         dataType: 'JSON',
         success: data => {
             console.log(data);
         }
     });
}

function validarCampos(){
    let nome = $("#nome"); 
    let telefone = $("#telefone"); 
    let email = $("#email"); 
    let dat_nasc = $("#dat_nasc"); 
    let cpf = $("#cpf"); 
    let rg = $("#rg"); 
    let cargo = $("#cargo"); 
    let senha = $("#senha"); 
    let login = $("#login"); 

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
        },
        {
            campo: cargo, 
            valido: cargo.val() != '', 
            msg: 'Cargo inválido.'
        },
        {
            campo: login, 
            valido: login.val() != '', 
            msg: 'Login inválido.'
        },
        {
            campo: senha, 
            valido: senha.val() != '', 
            msg: 'Senha inválido.'
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