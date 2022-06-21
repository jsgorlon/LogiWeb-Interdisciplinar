
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
function formatarCEP(str){
	var re = /^([\d]{2})\.*([\d]{3})-*([\d]{3})/; // Pode usar ? no lugar do *

	if(re.test(str)){
		return str.replace(re,"$1.$2-$3");
	}else{
		alert("CEP inválido!");
	}
	
	return "";
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
        lazyOpen: true, 
        closeIcon: true, 
        id_entrega: null, 
        onOpen: function(){ 
            let dialog = this; 
            $("#cod_ordem").mask("999999999");

            $("#pesquisar_pedido").click(_=>{
                $("#pesquisar_pedido").spinner();

                $.ajax({
                    url: "/ordem/getById",
                    type: "POST", 
                    dataType: "JSON", 
                    data: {
                        id: $("#cod_ordem").val()
                    },
                    success: (data) => {
                      if(data.item.ordens.length > 0){
                        let ordem = data.item.ordens[0];

                        $("#pedidos").attr('disabled', true).html(`<option selected value="${ordem.id}">#${ordem.id} - ${ordem.cliente.nome}</option>`);


                        $("#address").html(`
                            <div class="d-flex">${ordem.endereco.logradouro}</div>
                            <div class="d-flex">${ordem.endereco.bairro} - ${ordem.endereco.nr_casa}</div>
                            <div class="d-flex">${formatarCEP(ordem.endereco.cep)} ${ordem.endereco.cidade}, ${ordem.endereco.estado}</div>
                        `);
                      }
                      else {
                        $("#pedidos").attr('disabled', true).html(`<option selected>ORDEM NÃO ENCONTRADA</option>`);
                        $("#address").html("");
                      }

                      $("#pesquisar_pedido").spinner({submete: false});
                    }
                });
            });

            $("#gerar_entrega").click(function(){
               
                let id_motorista = $("#filter_motorista").val();

                if(!id_motorista){
                    $("#filter_motorista").addClass('is-invalid');
                    alert_error("Atenção", "Selecione um motorista para a entrega.");
                    return false; 
                }
                $("#gerar_entrega").spinner();

                $.ajax({
                    url: "/entrega/cadastrar",
                    type: "POST",
                    dataType: "JSON",
                    data: {
                        id_funcionario: _ID_FUNCIONARIO, 
                        id_motorista: id_motorista 
                    },
                    success: data => {
                        ajaxResponse(data); 
                        dialog.id_entrega = data.item.id_entrega;

                        $("#filter_motorista").attr("disabled", true); 
                        $("#gerar_entrega").fadeOut(_=>{
                            $("#form_add_ordem").fadeIn();
                        });
                    }
                });
            });

            $("#adicionar_ordem").click(function(){
                
               
                if($("#pedidos").val() == ""){
                    alert_warning("ATENÇÃO", "Não há uma ordem válida para adicionar a entrega.");
                    return false; 
                }

                $("#adicionar_ordem").spinner();

                $.ajax({
                    url: "/entrega/AdicionarOrdem", 
                    type: "POST",
                    dataType: "JSON", 
                    data: {
                        id_entrega: dialog.id_entrega, 
                        id_ordem: $("#pedidos").val()
                    },
                    success: data => {
                        ajaxResponse(data);
                        $("#adicionar_ordem").spinner({submete: false});

                        obterEntregaOrdem(dialog.id_entrega);
                    }
                });

            });
        }, 
        onOpenBefore: function(){
            $("table").bootstrapTable();
            
            let dialog = this; 

            $.ajax({
                type: 'GET',
                url: '/funcionario/todos',
                dataType: 'JSON',
                data: {
                   nome: null, 
                   id_cargo: 3,   
                   status: 1
                },
                success: data => {
                
                   
                   let option = (value = "", text = "TODOS", selected=false) => `<option value="${value}"} ${selected ? "selected" : ''}>${text}</option>`; 
                   let html = option('', 'SELECIONE', true);
                   
                   if(data.item.funcionarios.length == 0){
                    $.alert({
                        title: "<i class='fa-solid fa-triangle-exclamation mr-2 text-danger'></i> <small class='fw-bold'>ATENÇÃO</small>",
                        type: "red",
                        draggable: false,  
                        content: `<small>Não há motoristas cadastrados para gerar uma entrega.</small>`,
                        buttons: {
                            fechar: {
                                text: 'Fechar',
                                btnClass: "btn btn-sm btn-danger",
                                
                            }
                        },
                        onClose: () => {
                            dialog.close(); 
                        }
                    });
                   }
                   else {
                        data.item.funcionarios.map(funcionario => {
                            html += option(funcionario.id, funcionario.nome, data.item.funcionarios.length === 1); 
                        });
            
                        $("#filter_motorista").html(html);
                   }
               }
            });
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
         type: 'POST',
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
function obterFuncionarios(id_cargo=null){
    
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
            
            let motoristas = []; 
            let htmlMoto = option(); 
            data.item.funcionarios.map(funcionario => {
                if(funcionario.cargo.id == 3){
                    htmlMoto += option(funcionario.id, funcionario.nome); 
                }
                else {
                    html += option(funcionario.id, funcionario.nome); 
                }
            });

            $("#id_funcionario").html(html);
            $("#id_motorista").html(htmlMoto);
        }

        
     });
}

function obterEntregaOrdem(id_entrega){

    $.ajax({
        url: "/entrega/ObterEntregaOrdem", 
        type: "POST",
        dataType: "JSON",
        data: {
            id_entrega
        }, 
        success: data => {
            console.log(data);
        }
    })

}