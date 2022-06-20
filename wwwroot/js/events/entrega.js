
let entregas = []; 
let ordens = [];

/*class Ordem {
     constructor(){ 
        let peso = parseInt( $("#peso").val()); 

        this.IdFuncionario = _ID_FUNCIONARIO; 
        this.IdCliente = $("#cliente").val(); 
        this.Qtd_itens = $("#qtd_itens").val(); 
        this.Volume = $("#volume").val(); 
        this.Peso = peso.toFixed(2); 
        this.Observacao = $("#obs_ordem").val();
        this.Status =   $("#status").val();
       
        this.Ativo = true; 
     }
}*/

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

function obterOrdens(loadingButton = false){
    if(loadingButton){
        $("#btPesquisar").spinner();
    }

    let status = $("[name='status_ordem']:checked").val();
    $.ajax({
         type: 'GET',
         url: '/ordem/Ordem',
         dataType: 'JSON',
         data: {
            id_funcionario: $("#id_funcionario").val(), 
            nome: $("#filter_cliente").val().trim(), 
            status: status == 'A' ? null : status,
         },
         success: data => {
            ordens = []; 
             let orders = [];
             data.item.ordens.map(order => {
                orders.push({
                        id:order.id,
                        id_num_pedido: `<b class="${order.ativo ? '' : 'text-danger'}">#${order.id}</b>`,
                        ativo: order.ativo, 
                        observacao: order.observacao,
                        peso_qtd_itens: `Peso: ${ order.peso} KG <br>${order.qtd_itens} iten(s)`,
                        nome_cliente: order.cliente.nome.toLocaleUpperCase(),
                        nome_funcionario: order.funcionario.nome.toLocaleUpperCase(),
                   });
                
                   ordens.push(order);
             });

             $('table').bootstrapTable('load', orders);

             $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});
            
             if(loadingButton)
                $("#btPesquisar").spinner({submete: false});
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
            console.log(data);
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