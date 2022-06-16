let ordens = []; 

class Ordem {
     constructor(){ 
        this.IdCliente = $("#cliente").val(); 
        this.Qtd_itens = $("#qtd_itens").val(); 
        this.Volume = $("#volume").val(); 
        this.Peso = $("#peso").val(); 
        this.Observacao = $("#obs_ordem").val(); 
       
        this.Ativo = true; 
     }
}

class Endereco {
 
   constructor(){
    this.IdEstado = $("#estado").val(); 
    this.IdCidade = $("#cidade").val(); 
    this.Bairro = $("#bairro").val(); 
    this.Logradouro = $("#logradouro").val(); 
    this.Nr_casa = $("#nr_casa").val(); 
    this.Cep = $("#cep").val().replaceAll(/[^0-9]/gi, ''); 
    this.Complemento = $("#complemento").val(); 
   }
}

let dialogOrdem = $.confirm({
    title: `Nova Ordem`, 
    content: $("#template_cad_ordem").html(),
    lazyOpen: true,
    closeIcon: true,  
    id_ordem: null, 
    edicao: false, 
    type: 'green', 
    columnClass: 'col-12 col-md-11 col-lg-10', 
    draggable: false, 
    onClose: function(){
        this.id_ordem = null; 
        this.edicao = false; 
    }, 
    onOpenBefore: function(){
        this.showLoading();

        obterCliente('filter_cliente','selecione');
       
        
        $("#cep").mask("99999-999");
        $("#volume, #qtd_itens").mask("99");
        $("#peso").mask("99");
    
        if(this.edicao)
        {
            $("#col_pesquisar_cliente").fadeOut();
            $("#cliente").attr('disabled', true);
        }

        obterEstado('estado', 'SELECIONE');

        this.hideLoading(); 
    },
    onOpen: () => {
        $("#btPesquisarCliente").click(function(){
            obterClientes();
        });

      
        $('input, select').on('focus', function(){
            $(this).removeClass('is-invalid');
        });

        $("#cliente").on('change', ev => {
            if(ev.target.value == '')
                $("#form_ordem").fadeOut(); 
            else 
                $("#form_ordem").fadeIn(); 
        });

        $("#estado").change(function(){
            let id_estado = $(this).val();

            if(!id_estado){
                $("#cidade").html("<option value=''>SELECIONE UM ESTADO</option>");
            }

            else {
                obterCidade(id_estado);
            }

        });
    }, 
    cadastrar: function(){
        
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Nova Ordem</span>`;
        
        this.buttons = {
            cadastrar: {
                text: 'Cadastrar',
                btnClass: 'btn btn-sm btn-success btCadastrar', 
                action: function(){
                    let dialog = this; 
                   let {campos_validos } = validarCampos(); 
                    
                   if(!campos_validos)
                     return false;
                
                   // $(".btCadastrar").spinner();

                   let ordem = new Ordem(); 
                    console.log(ordem);
                   $.ajax({
                       url: '/ordem/cadastrar', 
                       type: 'POST',
                       dataType: 'JSON',
                       data: {
                        ordem
                       }, 
                       complete: data => {
                        // $(".btCadastrar").spinner({submete: false});
                           console.log(data);
                          eval(data.responseText);
                          obterOrdens(); 
                          
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
    $('#btCadastrar').click(_=>dialogOrdem.cadastrar());

    obterOrdens(); 


    $("#btPesquisar").click(_=>obterOrdens(true));
});

function buttons(id, rows) 
{

 const inactive = `<button data-bs-content="Excluir Ordem" id="delete_${id}" onclick="excluir(this);" data-bs-toggle="popover"  data-active="${rows.active}" class="p-0 m-0 btn btn-sm shadow-none" data-idordem="${id}">
                     <i class="fa-solid fa-circle-minus"></i>
                   </button>`;
 return `<div class="d-flex justify-content-end gap-1 buttons-grid">${inactive}</div>`;
}
function excluir(button){

    $(".popover").popover('dispose');
   
     let id_ordem =   button.dataset.idordem; 
   
      $("button").attr('disabled', true);
      $("#"+button.id).spinner();
   
      let ordem = ordens.find(item=> item.id == id_ordem);
    
      delete ordem.Id; 
   
       $.ajax({
       url: '/ordem/excluir', 
       type: 'POST',
       dataType: 'JSON',
       data: {
           id: id_ordem
       }, 
       complete: data => {
          $("button").attr('disabled', false);
          obterOrdens();
          alert_success(`Ordem excluída com sucesso!`);
       }
    });
   }

function obterEstado(selectId, ig = 'selecione', id_select = 0){
     $.ajax({
         type: 'GET',
         url: '/endereco/MostrarEstado',
         dataType: 'JSON',
         success: data => {
             let newOption = (label, value = '') =>  `<option ${value == id_select ? 'selected' : ''} value='${value}'>${label.toLocaleUpperCase()}</option>`;
             let html = newOption(ig);
 
             data.map(endereco => html += newOption(endereco.estado, endereco.idEstado));
             $('#'+selectId).html(html);
         }
     });
 }

 function obterCidade(id_estado, ig = 'selecione', id_select = 0){
    $.ajax({
         type: 'GET',
         url: '/endereco/MostrarCidade',
         dataType: 'JSON',
         data:{
            id_estado
         },
         success: data => {
             let newOption = (label, value = '') =>  `<option ${value == id_select ? 'selected' : ''} value='${value}'>${label.toLocaleUpperCase()}</option>`;
             let html = newOption(ig);
 
             data.map(cidade => html += newOption(cidade.cidade, cidade.id));
             $('#cidade').html(`<option value=''>SELECIONE</option>${html}`);
         }
     });
 }

 function obterCliente(selectId, ig = 'selecione', id_select = 0){
    return $.ajax({
         type: 'GET',
         url: '/cliente/Clientes',
         dataType: 'JSON',
         success: data => {
            let newOption = (label, value = '') =>  `<option ${value == id_select ? 'selected' : ''} value='${value}'>${label.toLocaleUpperCase()}</option>`;
            let html = newOption(ig);

            data.map(item => html += newOption(item.nome, item.id));

            $('#'+selectId).html(html);
        }
     });
 }

 function obterOrdens(loadingButton = false){
    if(loadingButton){
        $("#btPesquisar").spinner();
    }

    let status = $("[name='flexRadioDefault']:checked").val();
    $.ajax({
         type: 'GET',
         url: '/ordem/ordem',
         dataType: 'JSON',
         data: {
            nome: "%"+$("#filter_cliente").val()+"%"
         },
         success: data => {
            ordens = []; 
             let orders = [];
             data.map(order => {
                orders.push({
                    id: order.id,
                    peso: order.peso ,
                    observacao: order.observacao,
                    qtd_itens: order.qtd_itens, 
                    cliente_nome: order.cliente.nome,
                    funcionario_nome: order.funcionario.nome,
                    logradouro: order.endereco.logradouro,
                    nr_casa: order.endereco.nr_casa, 
                    complemento: order.endereco.complemento,
                    bairro: order.endereco.bairro, 
                    cep: order.endereco.cep, 
                    cidade: order.endereco.cidade,
                    endereco: order.endereco.uf 
                   });
                
                   ordens.push({
                    id: order.id,
                    cliente_id: order.cliente.id ,
                    funcionario_id: order.funcionario.id, 
                    endereco_id: order.endereco.id ,
                    peso: order.peso ,
                    observacao: order.observacao,
                    qtd_itens: order.qtd_itens, 
                    cliente_nome: order.cliente.nome,
                    funcionario_nome: order.funcionario.nome,
                    logradouro: order.endereco.logradouro,
                    nr_casa: order.endereco.nr_casa, 
                    complemento: order.endereco.complemento,
                    bairro: order.endereco.bairro, 
                    cep: order.endereco.cep, 
                    cidade: order.endereco.cidade,
                    endereco: order.endereco.uf 
                   });
                

             });

             $('table').bootstrapTable('load', orders);

             $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});
            
             if(loadingButton)
                $("#btPesquisar").spinner({submete: false});
         }
     });
}

function obterClientes(){

    let nome_cliente = $("#nome_cliente");

    if(nome_cliente.val().length < 3){
        nome_cliente.addClass('is-invalid');
        alert_error('Atenção:', "O nome deve ter no <b>mínimo 3</b> caracteres.");
        return;
    }

    $.ajax({
         type: 'GET',
         url: '/cliente/todos',
         dataType: 'JSON',
         data: {
            nome: nome_cliente.val(),
            status: 1
         },
         success: data => {
            let clientes = data.item.clientes; 

            if(clientes.length == 0){
                alert_warning("Atenção", "Nenhum cliente foi encontrado!");
                $("#col_cliente, #form_ordem").fadeOut();
                $("#cliente").html(` <option value="">SELECIONE</option>`);
            }
            else {
                let items_select = "<option value='' selected>SELECIONE</option>";

                clientes.map(cliente => {
                    items_select += `<option value='${cliente.id}' ${clientes.length == 1 ? 'selected' : ''}>${cliente.nome}</option>`;
                });

                $("#cliente").html(items_select);
                $("#col_cliente").fadeIn();

                if(clientes.length == 1){
                    $("#form_ordem").fadeIn(); 
                }
            }

         }
     });
}


function validarCampos(){
    let qtd_itens = $("#qtd_itens"); 
    let volume = $("#volume"); 
    let peso = $("#peso"); 
    let obs_ordem = $("#obs_ordem"); 
    let estado = $("#estado"); 
    let cidade = $("#cidade");
    let bairro = $("#bairro"); 
    let logradouro = $("#logradouro"); 
    let nr_casa = $("#nr_casa"); 
    let cep = $("#cep"); 
    let complemento = $("#complemento"); 
   

    let msg = ''; 

    let isNotEmptyNum = (field) => field.val() != '' && field.val() > 0; 

    let validacoes = [
        {
            campo: qtd_itens, 
            valido: isNotEmptyNum(qtd_itens),
            msg: 'qtd_itens inválido.'
        },
        {
            campo: volume, 
            valido: isNotEmptyNum(volume),
            msg:'volume inválido.'
        },
        {
            campo: peso, 
            valido: isNotEmptyNum(peso),
            msg: 'Peso inválido.'
        },
        {
            campo: estado, 
            valido: estado.val() != '', 
            msg: 'Estado inválida.'
        },
        {
            campo: cidade, 
            valido: cidade.val() != '', 
            msg: 'Cidade inválida.'
        },
        {
            campo: bairro, 
            valido: bairro.val() != '', 
            msg: 'Bairro inválido.'
        },
        {
            campo: nr_casa, 
            valido: nr_casa.val() != '', 
            msg: 'Nr. Casa inválido'
        },
        {
            campo: cep, 
            valido: cep.val().replaceAll(/[^0-9]/gi, '').length == 8, 
            msg: 'CEP inválido'
        },
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
