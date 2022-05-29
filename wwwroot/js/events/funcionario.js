
let dialogCadastrar = $.dialog({
    title: `Novo cliente`, 
    content: $("#template_cad_cliente").html(),
    lazyOpen: true,
    closeIcon: true,  
    id_cliente: null, 
    type: 'green', 
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

    $('table').bootstrapTable('load', generateUsers());

    $("[data-bs-toggle='popover']").popover({content: 'body', trigger: 'hover'});

    $('#btCadastrar').click(_=>dialogCadastrar.cadastrar());

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