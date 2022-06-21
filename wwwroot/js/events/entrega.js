
let entregas = []; 
let ordens = [];
let id_entrega
class Ordem {
     constructor(){ 
        let peso = parseInt( $("#peso").val()); 

       // this.IdFuncionario = $("#id_funcionario2").val(); 
        this.IdCliente = $("#cliente").val(); 
        this.Qtd_itens = $("#qtd_itens").val(); 
        this.Volume = $("#volume").val(); 
        this.Peso = peso.toFixed(2); 
        this.Observacao = $("#obs_ordem").val();
        this.Status =   $("#status").val(); 
        this.Estado = $("#estado").val(); 
        this.Cidade = $("#cidade").val(); 
        this.Bairro = $("#bairro").val(); 
        this.Logradouro = $("#logradouro").val(); 
        this.Nr_casa = $("#nr_casa").val(); 
        this.Cep = $("#cep").val().replaceAll(/[^0-9]/gi, ''); 
        this.Complemento = $("#complemento").val(); 

     }
}

class Entrega {
 
   constructor(){

    this.IdFuncionario = $("#id_funcionario").val();  
    this.IdMotorista = $("#id_motorista").val(); 
    this.DataCad = $("#data").val(); 
    this.Status =   $("#status").val();
   }
}
$(document).ready(_=>{

    $("table").bootstrapTable({}); 
    obterEntregas();
    obterFuncionarios(); 
    $("#btPesquisar").click(_=>obterEntregas(true));

    dialogEntrega = $.dialog({
        title: `<span style="font-size:18px !important;" class="fw-bold">Nova entrega</span>`,
        content: $("#template_cad_entrega").html(), 
        type: 'green', 
        columnClass: 'col-12 col-md-7 col-lg-6', 
        draggable: false, 
        closeIcon: true, 
        onOpen: function(){

        }, 
        onOpenBefore: function(){
            $("table").bootstrapTable();
            
        },
        criar: function() {
            this.buttons = {}; 
            this.open(); 
        }
    });

    $("#btNovaEntrega").click(_=>dialogEntrega.criar());
});

dialogStatus = $.dialog({
    title: `<span style="font-size:18px !important;" class="fw-bold">Detalhes entrega</span>`,
    content: $("#template_detalhe_entrega").html(), 
    type: 'green', 
    columnClass: 'col-12 col-md-7 col-lg-6', 
    draggable: false, 
    closeIcon: true, 
    onOpen: function(){
        $("#btPesquisarOrdem").click(function(){
            obterStatus();
            obterOrdem($("#id_ordem").val());
        });
        $("#btAlterarStatus").click(function(){
            alterarStatus($("#id_ordem").val(), $("#id_status").val());
        });
        
    }, 
    onOpenBefore: function(){
        
    },
    visualizar: function(id_entrega){
        this.id_entrega = id_entrega; 
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Visualização da entrega: #${id_entrega}</span>`;
        this.buttons ={
            fechar: {
                text: 'Fechar',
                btnClass: 'btn btn-sm btn-primary', 
            }
        }
        this.open();
    }
});

function buttons(id, rows) 
{

 const editar = `<button onclick="visualizar_entrega(this)" class="p-0 m-0 btn btn-sm shadow-none" data-identrega="${id}">
                     <i class="fa-solid fa-eye text-secondary" 
                        data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Visualizar detalhes entrega"></i>
                 </button>`;
 const inactive = `<button data-bs-content="Excluir Entrega" id="delete_${id}" onclick="excluir(this);" data-bs-toggle="popover"  data-active="${rows}" class="p-0 m-0 btn btn-sm shadow-none" data-identrega="${id}">
                 <i class="fa-solid fa-circle-minus"></i>
               </button>`;




 return `<div class="d-flex justify-content-end gap-1 buttons-grid">${editar}</div>`;
}

function visualizar_entrega(button){
    $(".popover").popover('dispose');
    id_entrega =   button.dataset.identrega; 
    obterOrdensEntrega(id_entrega);
    
    dialogStatus.visualizar(id_entrega);
}


function excluir(button){

    $(".popover").popover('dispose');
   
     let id_entrega =   button.dataset.identrega; 
   
      $("button").attr('disabled', true);
      $("#"+button.id).spinner();
   
      let entrega = entregas.find(item=> item.id == id_entrega);
      delete entrega.Id; 
   
       $.ajax({
       url: '/entrega/excluir', 
       type: 'POST',
       dataType: 'JSON',
       data: {
           id: id_ordem
       }, 
       complete: data => {
          $("button").attr('disabled', false);
          obterEntregas();
          alert_success(`Entrega excluída com sucesso!`);
       }
    });
   }

function obterEntregas(loadingButton = false){
    if(loadingButton){
        $("#btPesquisar").spinner();
    }
    $.ajax({
         type: 'GET',
         url: '/entrega/Entregas',
         dataType: 'JSON',
         data: {
            id_funcionario: $("#id_funcionario").val(), 
            id_motorista: $("#id_motorista").val()
         },
         success: data => {
            entregas = []; 
             let entrega = [];
             data.item.entregas.map(entreg => {
                entrega.push({
                        id:entreg.id,
                        nome_motorista: entreg.motorista.nome.toLocaleUpperCase(),
                        nome_funcionario: entreg.funcionario.nome.toLocaleUpperCase(),
                        status: entreg.status.nome.toLocaleUpperCase()
                   });
                
                   entregas.push(entrega);

             });
             if (entregas[0]) {
                $('table').bootstrapTable('load', entregas[0]);
             }else{
                 $('table').bootstrapTable('load', entregas);
             }

             $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});
            
             if(loadingButton)
                $("#btPesquisar").spinner({submete: false});
         }
     });
}

function obterOrdensEntrega(id_entrega){

    $.ajax({
         type: 'GET',
         url: '/entrega/Detalhe',
         dataType: 'JSON',
         data: {
            id: id_entrega
         },
         success: data => {
            let option = (value = "", text = "TODOS") => `<option value="${value}"}>${text}</option>`; 
            let html = option();
            ordens = []; 
            let orders = [];
            data.item.ordens.map(order => {
                orders.push({
                        id:order.id,
                        id_num_pedido: order.id,
                        ativo: order.ativo, 
                        observacao: order.observacao,
                        peso: order.peso,
                        qtd_itens:order.qtd_itens,
                        nome_cliente: order.cliente.nome.toLocaleUpperCase(),
                        estado: order.endereco.estado,
                        cidade: order.endereco.cidade,
                        bairro: order.endereco.bairro,
                        logradouro: order.endereco.logradouro,
                        nr_casa: order.endereco.nr_casa,
                        cep: order.endereco.cep,
                        complemento: order.endereco.complemento,
                        status: order.status.nome
                   });
                   html += option(order.id, order.id); 
                   ordens.push(orders);
                 
             });

            
            $("#id_ordem").html(html);
         }
     });
}
function obterFuncionarios(){
    $.ajax({
         type: 'GET',
         url: '/funcionario/todos',
         dataType: 'JSON',
         data: {
            nome: null, 
            id_cargo: null,   
            status: 1
         },
         success: data => {
             let option = (value = "", text = "TODOS") => `<option value="${value}"}>${text}</option>`; 
             let html = option();
            data.item.funcionarios.map(funcionario => {
                html += option(funcionario.id, funcionario.nome); 
            });

            $("#id_funcionario").html(html);
            $("#id_motorista").html(html);
        }

        
     });
}
function obterOrdem(id){

    if(id)
    {
        let ordem = ordens[0].find(ord => ord.id == id); 
        $("#qtd_itens").val(ordem.qtd_itens); 
        $("#volume").val(ordem.volume); 
        $("#peso").val(ordem.peso);
        $("#obs_ordem").val(ordem.observacao); 
        $("#bairro").val(ordem.bairro); 
        $("#logradouro").val(ordem.logradouro); 
        $("#nr_casa").val(ordem.nr_casa); 
        $("#cep").val(ordem.cep); 
        $("#complemento").val(ordem.complemento); 
        $("#cidade").val(ordem.cidade); 
        $("#estado").val(ordem.estado); 
        $("#id_status").val( $('option:contains("'+ordem.status+'")').val() ); 
        $("#form_ordem").fadeIn(); 

    }
}
function obterStatus(){
    $.ajax({
         type: 'GET',
         url: '/entrega/status',
         dataType: 'JSON',
         success: data => {
             let option = (value = "", text = "SELECIONE") => `<option value="${value}"}>${text}</option>`; 
             let html = option();
             console.log(data);
            data.item.entregas.map(stat => {
                
                html += option(stat.status.id, stat.status.nome); 
            });
            $("#id_status").html(html);
        }    
     });
}

function alterarStatus(id_ordem, id_status){
    let ordem = ordens[0].find(ord => ord.id == id_ordem); 
    ordem.id_status = id_status; 


    $.ajax({
        url: '/entrega/AtualizarOrdens', 
        type: 'POST',
        dataType: 'JSON',
        data: {
         id_ordem,
         id_status,
         id_entrega
        }, 
        success: data => {
          $(".btCadastrar").spinner({submete: false});
         ajaxResponse(data);
         obterEntregas();
         this.close();
           
        }
    });
}

