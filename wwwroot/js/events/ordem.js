let ordens = []; 

class Ordem {
     constructor(){ 
        let peso = parseInt( $("#peso").val()); 

        this.IdFuncionario = _ID_FUNCIONARIO; 
        this.IdCliente = $("#cliente").val(); 
        this.Qtd_itens = $("#qtd_itens").val(); 
        this.Volume = $("#volume").val(); 
        this.Peso = peso.toFixed(2); 
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
        $("input, select").attr('disabled', false);
    }, 
    onOpenBefore: function(){
        this.showLoading();

     /*    obterCliente('filter_cliente','selecione'); */
       
        
        $("#cep").mask("99999-999");
        $("#volume, #qtd_itens").mask("99");
        $("#peso").mask("99");
       
        let id_estado = 0;
        if(this.edicao)
        {
            let ordem = ordens.find(ord => ord.id == this.id_ordem); 

            $("#col_pesquisar_cliente").fadeOut();
            $("#cliente").html(`<option selected>${ordem.cliente.nome.toLocaleUpperCase()}</option>`).attr('disabled', true);

            $("input, select").attr('disabled', true);
            
            id_estado = ordem.endereco.idEstado;

            obterCidade(id_estado, 'SELECIONE', ordem.endereco.idCidade);

     
            $("#qtd_itens").val(ordem.qtd_itens); 
            $("#volume").val(ordem.volume); 
            $("#peso").val(ordem.peso) 
            $("#obs_ordem").val(ordem.observacao); 
            $("#bairro").val(ordem.endereco.bairro); 
            $("#logradouro").val(ordem.endereco.logradouro); 
            $("#nr_casa").val(ordem.endereco.nr_casa); 
            $("#cep").val(ordem.endereco.cep); 
            $("#complemento").val(ordem.endereco.complemento); 

            $("#form_ordem").fadeIn(); 
        }

        obterEstado('estado', 'SELECIONE', id_estado);

        this.hideLoading(); 
    },
    onOpen: function(){
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

        if(this.edicao)
            $("#col_cliente").fadeIn();
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
                   let endereco = new Endereco(); 
                    console.log(ordem);
                   $.ajax({
                       url: '/ordem/cadastrar', 
                       type: 'POST',
                       dataType: 'JSON',
                       data: {
                        ordem,
                        endereco
                       }, 
                       success: data => {
                         $(".btCadastrar").spinner({submete: false});
                        ajaxResponse(data);
                        obterOrdens();

                      //  obterClientes(); 
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
    visualizar: function(id_ordem, pedido_ativo = true){
        this.edicao = true; 
        let ordem = ordens.find(ord => ord.id == id_ordem);
        pedido_ativo = ordem.ativo; 

        this.id_ordem = id_ordem; 
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Visualização do pedido: #${id_ordem} ${!pedido_ativo ? "<small class='py-1 px-1 bg-danger rounded-pill text-white'>INATIVO</small>" : ''}</span>`;
        this.buttons ={
            fechar: {
                text: 'Fechar',
                btnClass: 'btn btn-sm btn-primary', 
            }
        }
        this.open();
    }
});
$(document).ready(_=>{
    $('table').bootstrapTable({});
    $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});
    $('#btCadastrar').click(_=>dialogOrdem.cadastrar());

    obterFuncionarios(); 
    obterOrdens(); 


    $("#btPesquisar").click(_=>obterOrdens(true));
});

function buttons(id, rows) 
{

 const editar = `<button onclick="visualizar_ordem(this)" class="p-0 m-0 btn btn-sm shadow-none" data-idordem="${id}">
                     <i class="fa-solid fa-eye text-secondary" 
                        data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Visualizar Ordem"></i>
                 </button>`;
 const inactive = `<button onclick="alterar_status(this)" id="alterar_status_${id}" class="p-0 m-0 btn btn-sm shadow-none" data-idordem="${id}">
                     <i class="fa-solid fa-circle-minus text-secondary"
                     data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Ao <b>Inativar ordem</b> você precisará criar outra." data-bs-html="true"></i>
                   </button>`;

  const active = `<button   class="p-0 m-0 btn btn-sm shadow-none" data-idordem="${id}" style="color:blue !important;">
                      <i class="fa-solid fa-circle-info text-info"
                         data-bs-toggle="popover" data-bs-placement="top" data-bs-content="Não é possível <b>REATIVAR</b> esta ordem." data-bs-html="true"></i>
                  </button>`;



 return `<div class="d-flex justify-content-end gap-1 buttons-grid">${editar} ${rows.ativo ? inactive : active}</div>`;
}


function alterar_status(button){

    $(".popover").popover('dispose');
   
     let id_ordem =   button.dataset.idordem; 
   
      $("button").attr('disabled', true);
      $("#"+button.id).spinner();
   
      let ordem = ordens.find(item=> item.id == id_ordem);
    
      delete ordem.id; 
      ordem.ativo = !ordem.ativo; 

       $.ajax({
       url: '/ordem/AlterarStatus', 
       type: 'POST',
       dataType: 'JSON',
       data: {
           id: id_ordem,
           status: ordem.ativo ? 1 : 0
       }, 
       success: data => {
          $("button").attr('disabled', false);
          obterOrdens();
         
          ajaxResponse(data);
       }
    });
}

function visualizar_ordem(button){
    $(".popover").popover('dispose');
   
    let id_ordem =   button.dataset.idordem; 

    dialogOrdem.visualizar(id_ordem);
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
 
             data.map(cidade => html += newOption(cidade.cidade, cidade.idCidade));
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

    let status = $("[name='status_ordem']:checked").val();
    $.ajax({
         type: 'GET',
         url: '/ordem/Todas',
         dataType: 'JSON',
         data: {
            id_funcionario: $("#id_funcionario").val(), 
            nome: $("#filter_cliente").val().trim(), 
            status: status == 'A' ? null : status,
         },
         success: data => {
            ordens = []; 
             let orders = [];
             console.log(data)
             data.item.ordens.map(order => {
                
                orders.push({
                        id:order.id,
                        id_num_pedido: `<b class="${order.ativo ? '' : 'text-danger'}">#${order.id}</b>`,
                        ativo: order.ativo, 
                        observacao: order.observacao,
                        peso_qtd_itens: `Peso: ${ order.peso} KG <br>${order.qtd_itens} iten(s)`,
                        nome_cliente: order.cliente.nome.toLocaleUpperCase(),
                        nome_funcionario: order.funcionario.nome.toLocaleUpperCase(),
                        status_descricao: order.status.nome
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
             let option = (value = "", text = "TODOS") => `<option value="${value}" ${(_ID_FUNCIONARIO == value) ? "selected" : ""}>${text}</option>`; 
             let html = option();
            data.item.funcionarios.map(funcionario => {
                html += option(funcionario.id, funcionario.nome); 
            });

            $("#id_funcionario").html(html);
        }
     });
}


