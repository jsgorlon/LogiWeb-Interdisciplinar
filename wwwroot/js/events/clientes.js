
let dialogCadastrar = $.dialog({
    title: `Novo cliente`, 
    content: $("#template_cad_cliente").html(),
    lazyOpen: true,
    closeIcon: true,  
    id_cliente: null, 
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
    onClose: function(){
        this.id_cliente = null; 
    }, 
    cadastrar: function(){
        this.title = `<span style="font-size:18px !important;" class="fw-bold">Novo Cliente</span>`;

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

    $('table').bootstrapTable('load', [
        {
            id: 0, 
            nome_cpf: 'Allan 331.111.111-00',
        }
    ]);

    $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});

    $('#btCadastrar').click(_=>dialogCadastrar.cadastrar());

});


function buttons(id, rows) 
{

 const editar = `<button data-bs-toggle="Editar Cliente" data-bs-toggle="popover" class="btn btn-sm btn-warning" data-idcliente="${id}">
                     <i class="fa-solid fa-square-pen"></i>
                 </button>`;


 return `<div class="d-flex">${editar}</div>`;
}