let ordId =[];
let entregas = []; 
let ordens = [];
let id_entrega;
class Ordem {
     constructor(){ 
        let peso = parseInt( $("#peso").val()); 

        this.IdFuncionario = _ID_FUNCIONARIO;
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

    dialogEntrega = $.confirm({
        title: `<span style="font-size:18px !important;" class="fw-bold">Nova entrega</span>`,
        content: $("#template_cad_entrega").html(), 
        type: 'green', 
        columnClass: 'col-12 col-md-7 col-lg-6', 
        draggable: false, 
        closeIcon: true, 
        lazyOpen: true, 
        buttons: {
            cadastrar:  {
                btnText: `<i class="fa-solid fa-circle-plus"></i> Cadastrar Entrega`,
                btnClass: 'btn btn-sm btn-success btCadEntrega', 
                action: function(){
                    gerarEntrega(this);
                    return false; 
                }
            }
        },
        onOpen: function(){
            obterFuncionarios();
            obterMotorista();
            $("#btPesquisarOrdemId").click(function(){
                let ordem = ordId.find(a => a.id_ordem == $("#id_ordem").val());

                if(!ordem)
                    ordemId($("#id_ordem").val());
                else 
                  alert_error('ATENÇÃO', "Esta ordem já foi adicionada a está entrega."); 
            });
            
            $("#id_ordem").mask("99999999");
        }, 
        onOpenBefore: function(){
            $("table").bootstrapTable();
            
        },
        criar: function() {
       
            this.open(); 
        }
    });

    $("#btNovaEntrega").click(_=>dialogEntrega.criar());
});

dialogStatus = $.dialog({
    title: `<span style="font-size:18px !important;" class="fw-bold">Detalhes entrega</span>`,
    content: $("#template_detalhe_entrega").html(), 
    type: 'green', 
    columnClass: 'col-12 col-md-7 col-lg-10', 
    draggable: false, 
    lazyOpen: true, 
    closeIcon: true, 
    onOpen: function(){
        let dialog = this; 
        $("#id_ordem").change(ev=>{
            if(!ev.target.value){
                $("input, select").val('').attr("disabled", false);
            }
        });

        $("#btPesquisarOrdem").click(function(){
          
            obterOrdem($("#id_ordem").val());
        });
        $("#btAlterarStatus").click(function(){
            alterarStatus($("#id_ordem").val(), $("#id_status").val(), dialog);
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
    }, 
    onClose: function(){
        $("input, select").attr("disabled", false);
    }
});

function buttons(id, rows) 
{

 const editar = `<button onclick="visualizar_entrega(this)" class="p-0 m-0 btn btn-sm shadow-none" data-identrega="${id}">
                     <i class="fa-solid fa-eye text-secondary" 
                        data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Visualizar detalhes entrega"></i>
                 </button>`;
 const excluir = `<button data-bs-content="Excluir Entrega" id="delete_${id}" onclick="excluir(this);" data-bs-toggle="popover"  data-active="${rows}" class="p-0 m-0 btn btn-sm shadow-none" data-identrega="${id}">
                 <i class="fa-solid fa-circle-minus"></i>
               </button>`;




 return `<div class="d-flex justify-content-end gap-1 buttons-grid">${editar} ${excluir}</div>`;
}

function visualizar_entrega(button){
    $(".popover").popover('dispose');
    id_entrega =   button.dataset.identrega; 
    obterOrdensEntrega(id_entrega);
    
    dialogStatus.visualizar(id_entrega);
}


function excluir(button){

    $(".popover").popover('dispose');
   
     let id_entreg =   button.dataset.identrega; 
   
      $("button").attr('disabled', true);
      $("#"+button.id).spinner();
      let entrega = entregas[0].find(item=> item.id == id_entreg);
      delete entrega.Id; 
     console.log(id_entreg)
      $.ajax({
        url: '/entrega/excluir', 
        type: 'POST',
        dataType: 'JSON',
        data: {
         id: id_entreg
        }, 
        success: data => {
        
         ajaxResponse(data);
         obterEntregas();

           
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
            let html = option("","SELECIONE");
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
                   html += option(order.id, `#${order.id} - ${order.cliente.nome.toLocaleUpperCase()}`); 
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
             let htmlMoto = option();

            data.item.funcionarios.map(funcionario => {


                if(funcionario.cargo.id != 3)
                    html += option(funcionario.id, funcionario.nome); 
                else 
                    htmlMoto += option(funcionario.id, funcionario.nome); 
            });

            $("#id_funcionario").html(html);
            $("#id_motorista").html(htmlMoto);

        }

        
     });
}
function obterMotorista(){
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
             let option = (value = "", text = "TODOS") => `<option value="${value}"}>${text}</option>`; 
             let html = option("","SELECIONE");
            data.item.funcionarios.map(funcionario => {
                html += option(funcionario.id, funcionario.nome); 
            });

            $("#id_motorista").html(html);
            $("#id_motoristas").html(html);
        }

        
     });
}
function obterOrdem(id){

    if(id)
    {
        console.log(ordens);
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
        //$("#id_status").val( ordem.status.id); 
        $("#form_ordem").fadeIn(); 

        $("input, select").attr("disabled", true); 
        $("#id_status, #id_ordem").attr("disabled", false); 

  
        obterStatus(ordem.status);
    }
}
function obterStatus(id_select = null){
    $.ajax({
         type: 'GET',
         url: '/entrega/status',
         dataType: 'JSON',
         success: data => {
             let option = (value = "", text = "SELECIONE") => `<option value="${value}"} ${text == id_select ? 'selected' : ''}>${text}</option>`; 
             let html = option();
            
            console.log(id_select);

            data.item.entregas.map(stat => {
                
                html += option(stat.status.id, stat.status.nome); 
            });
            $("#id_status").html(html);
        }    
     });
}

function alterarStatus(id_ordem, id_status, dialog = null){
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
       
         dialog.close();
        }
    });
    this.close();
}

function ordemId(id){

    $.ajax({
         type: 'GET',
         url: '/ordem/Ordem',
         dataType: 'JSON',
         data: {
            id: id
         },
         success: data => {
            ajaxResponse(data);
            if(data.item.ordens.id != 0){
                ord ={
                        id_ordem:data.item.ordens.id,
                        endereco: data.item.ordens.endereco.logradouro +" "+ data.item.ordens.endereco.nr_casa +" "+ data.item.ordens.endereco.bairro +" "+ data.item.ordens.endereco.cidade +" "+ data.item.ordens.endereco.uf,
                        status: data.item.ordens.idStatus
                    };
                    console.log(ord)
                if (ord.status != 4) {
                    ordId.push(ord);            
                     $('#tblEntrega').bootstrapTable('load', ordId);
        
                     $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});                    
                }
            }else{
                
            }
            
         }
     });
}

function gerarEntrega(dialog){

    let idMoto = $("#id_motoristas").val();
    let messageError = ""; 

    if(!idMoto){
        $("#id_motoristas").addClass('is-invalid');
        messageError += "Selecione o motorista responsável pela entrega!<br>"; 
       
    }

    let idOrd = [];
    ordId.forEach(item => {
        idOrd.push(item.id_ordem);
    });

    if(idOrd.length == 0){
        messageError += "Atribua ao menos 1 ordem para esta entrega!"; 
    }

    if(messageError != "")
    {
        console.log(messageError);
        alert_error('ATENÇÃO',messageError);
        return 0;
    }

    $(".btCadEntrega").spinner();

    $.ajax({
        url: '/entrega/Cadastrar', 
        type: 'POST',
        dataType: 'JSON',
        data: {
         IdFuncionario: _ID_FUNCIONARIO,
         idMotorista: idMoto,
         idOrdem: idOrd
        }, 
        success: data => {
          $(".btCadEntrega").spinner({submete: false});
          ajaxResponse(data);
          obterEntregas();
       
          dialog.close(); 
        }
    });
}