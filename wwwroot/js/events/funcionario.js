
let funcionarios = []; 
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
    onOpenBefore: function(){
        obterCargos('cargo','selecione');
        $("#cpf").mask('000.000.000-00');
        $('#rg').mask('99.999.999-9'); 
        $('#telefone').mask('(00) 00000-0000');  
    },
    onOpen: function(){
        let id_funcionario = this.id_funcionario; 

        if(id_funcionario)
        {
           let funcionario = funcionarios.find(user => user.Id == id_funcionario);
    
            $("#nome").val(funcionario.Nome); 
            $("#telefone").val(funcionario.Telefone); 
            $("#email").val(funcionario.Email); 
            $("#dat_nasc").val(funcionario.DatNasc.replace("T00:00:00",'')); 
            $("#cpf").val(funcionario.Cpf); 
            $("#rg").val(funcionario.Rg); 
            $("#login").val(funcionario.Login); 
            $("#cargo").val(funcionario.IdCargo); 
        }
    },
    cadastrar: function(){
        
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Novo Funcionario</span>`;
        
        this.buttons = {
            cadastrar: {
                text: 'Cadastrar',
                btnClass: 'btn btn-sm btn-success btCadastrar', 
                action: function(){
                    let dialog = this; 
                   let {campos_validos } = validarCampos(); 
                    
                   if(!campos_validos)
                     return false;
                
                   $(".btCadastrar").spinner();

                   let funcionario = new Funcionario(); 

                   $.ajax({
                       url: '/funcionario/cadastrar', 
                       type: 'POST',
                       dataType: 'JSON',
                       data: funcionario, 
                       complete: data => {
                        $(".btCadastrar").spinner({submete: false});
                           console.log(data);
                          eval(data.responseText);
                          obterFuncionarios(); 
                          
                       }
                   });
                    
                   return false; 
                }
            }
        }
        
        this.open(); 
    },
    editar: function(id_funcionario){
        
        console.log(id_funcionario);
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Edição do Funcionario</span>`;
        this.id_funcionario = id_funcionario; 
        this.buttons = {
            salvar: {
                text: 'Salvar',
                btnClass: 'btn btn-sm btn-success btAtualizar', 
                action: () => {
                    
                   let {campos_validos } = validarCampos(); 
                    
                   if(!campos_validos)
                     return false;
                   
                   let funcionario = new Funcionario(); 

                   funcionario["id"] = id_funcionario; 

                   $.ajax({
                       url: '/funcionario/atualizar', 
                       type: 'POST',
                       dataType: 'JSON',
                       data: funcionario, 
                       complete: data => {
                          
                          eval(data.responseText);
                          obterFuncionarios(); 
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

    $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});

    $('#btCadastrar').click(_=>dialogFuncionario.cadastrar());

    obterCargos('filter_cargo', 'todos');
    obterFuncionarios(); 


    $("#btPesquisar").click(_=>obterFuncionarios(true));
});


function buttons(id, rows) 
{

 const editar = `<button onclick="editar(this)" data-bs-content="Editar Funcionário" data-bs-toggle="popover" class="p-0 m-0 btn btn-sm shadow-none" data-idfuncionario="${id}">
                     <i class="fa-solid fa-square-pen"></i>
                 </button>`;
 const inactive = `<button data-bs-content="Inativar Funcionário" id="active_inactive_${id}" onclick="inativarAtivar(this);" data-bs-toggle="popover"  data-active="${rows.active}" class="p-0 m-0 btn btn-sm shadow-none" data-idfuncionario="${id}">
                     <i class="fa-solid fa-circle-minus"></i>
                   </button>`;

  const active = `<button data-bs-content="Ativar Funcionário" id="active_inactive_${id}" onclick="inativarAtivar(this);" data-bs-toggle="popover" data-active="${rows.active}" class="p-0 m-0 btn btn-sm shadow-none" data-idfuncionario="${id}">
                      <i class="fa-solid fa-circle-plus "></i>
                  </button>`;



 return `<div class="d-flex justify-content-end gap-1 buttons-grid">${editar} ${rows.active ? inactive : active}</div>`;
}

function editar(button){
    dialogFuncionario.editar(button.dataset.idfuncionario); 
}

function inativarAtivar(button){

 $(".popover").popover('dispose');

  let id_funcionario =   button.dataset.idfuncionario; 

   $("button").attr('disabled', true);
   $("#"+button.id).spinner();

   let funcionario = funcionarios.find(user=> user.Id == id_funcionario);
  
   funcionario.Ativo = !funcionario.Ativo; 
   
   delete funcionario.Id; 

   $.ajax({
    url: '/funcionario/AlterarStatus', 
    type: 'POST',
    dataType: 'JSON',
    data: {
        id: id_funcionario, 
        status: funcionario.Ativo ? 1 : 0
    }, 
    complete: data => {
       $("button").attr('disabled', false);
       obterFuncionarios();
       alert_success(`Usuário ${funcionario.Ativo ? 'ativado' : 'inativado'} com sucesso!`);
    }
});
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

function obterCargos(selectId, ig = 'selecione', id_select = 0){
   return $.ajax({
        type: 'GET',
        url: '/cargo',
        dataType: 'JSON',
        success: data => {
            let newOption = (label, value = '') =>  `<option ${value == id_select ? 'selected' : ''} value='${value}'>${label.toLocaleUpperCase()}</option>`;
            let html = newOption(ig);

            data.map(cargo => html += newOption(cargo.nome, cargo.id));
            $('#'+selectId).html(html);
        }
    });
}


function obterFuncionarios(loadingButton = false){


    if(loadingButton){
        $("#btPesquisar").spinner();
    }

    let status = $("[name='flexRadioDefault']:checked").val();
    $.ajax({
         type: 'GET',
         url: '/funcionario/todos',
         dataType: 'JSON',
         data: {
            nome: $("#filtro_nome").val() || null, 
            id_cargo: $("#filter_cargo").val() || null,   
            status: status == 'A' ? null : status 
         },
         success: data => {
            funcionarios = []; 
             let users = [];

             data.item.funcionarios.map(user => {
                users.push({
                    id: user.id, 
                    nome_cpf: `${user.nome} - <b>${formataCPF(user.cpf)}</b> ${user.ativo ? '' : "<span class='text-white rounded-pill bg-danger fw-bold px-2' style='font-size:13px;'>INATIVO</span>"}`,
                    cargo: user.cargo.nome,
                    active: user.ativo, 
                    status: user.ativo ? `<span class="badge badge-success rounded-pill bg-success">ATIVO</span>` : `<span class="badge badge-success rounded-pill bg-danger">INATIVO</span>`
                   });
                

                funcionarios.push({
                    Id: user.id,
                    Nome: user.nome,  
                    Telefone: user.telefone, 
                    Email: user.email, 
                    DatNasc: user.datNasc, 
                    Cpf: user.cpf, 
                    Rg: user.rg, 
                    IdCargo: user.cargo.id, 
                    cargo: user.cargo, 
                    Senha: user.senha,  
                    Login: user.login, 
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
        /*{
            campo: email, 
            valido: email.val().trim().length > 0 ?  : true,
            msg: 'E-mail inválido.'
        }, */
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


function formataCPF(cpf){
    //retira os caracteres indesejados...
    cpf = cpf.replace(/[^\d]/g, "");
    
    //realizar a formatação...
      return cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
  }