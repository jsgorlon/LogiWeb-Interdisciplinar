

$(document).ready(_=>{

    $("table").bootstrapTable(); 

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