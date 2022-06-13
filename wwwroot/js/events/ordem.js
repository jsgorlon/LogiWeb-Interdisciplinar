let ordens = []; 
class Ordem {
     constructor(){ 
        this.IdCliente = $("#id_cliente").val(); 
        this.QtdItens = $("#qtd_itens").val(); 
        this.Volume = $("#volume").val(); 
        this.Peso = $("#peso").val(); 
        this.Observacao = $("#observacao").val(); 
        this.IdStatus = $("#status").val(); 
        this.IdEndereco = $("#endereco").val(); 
        this.Ativo = true; 
     }
}

let dialogOrdem = $.confirm({
    title: `Nova Ordem`, 
    content: $("#template_cad_ordem").html(),
    lazyOpen: true,
    closeIcon: true,  
    id_ordem: null, 
    type: 'green', 
    columnClass: 'col-12 col-md-7 col-lg-6', 
    draggable: false, 
    onClose: function(){
        this.id_ordem = null; 
    }, 
    onOpenBefore: function(){
        obterCliente('filter_cliente','selecione');
        obterEstado('filter_estado','selecione');
        obterCidade('filter_cidade','selecione');
        
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
                
                   $(".btCadastrar").spinner();

                   let ordem = new Ordem(); 

                   $.ajax({
                       url: '/ordem/cadastrar', 
                       type: 'POST',
                       dataType: 'JSON',
                       data: ordem, 
                       complete: data => {
                        $(".btCadastrar").spinner({submete: false});
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
      console.log(id_ordem, ordens)
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
    return $.ajax({
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

 function obterCidade(selectId, ig = 'selecione', id_select = 0){
    return $.ajax({
         type: 'GET',
         url: '/endereco/MostrarCidade',
         dataType: 'JSON',
         success: data => {
             let newOption = (label, value = '') =>  `<option ${value == id_select ? 'selected' : ''} value='${value}'>${label.toLocaleUpperCase()}</option>`;
             let html = newOption(ig);
 
             data.map(endereco => html += newOption(endereco.cidade, endereco.idcidade));
             $('#'+selectId).html(html);
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



