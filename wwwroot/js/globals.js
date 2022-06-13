

function alert_success(title = '', body = ''){
    return new Notify ({
        status: 'success',
        title: title,
        text: body,
        autoclose: true
      })
}

function alert_error(title = '', body = ''){
    return new Notify ({
        status: 'error',
        title: title,
        text: body,
        autoclose: true
      })
}

function alert_warning(title = '', body = ''){
    return new Notify ({
        status: 'warning',
        title: title,
        text: body,
        autoclose: true
      })
}

function ajaxResponse(data){

  let messages = "";

  if(data.message)
    data.message.map(m => messages += m+"<br>"); 

  if(data.error)
    alert_error('Atenção:', messages);
  else 
    alert_success('', messages);

}


function cpf_valido(cpf){
  cpf = cpf.replaceAll(/[^0-9]/gi,'');
  
  var numeros, digitos, soma, i, resultado, digitos_iguais;
  digitos_iguais = 1;
  if (cpf.length < 11)
        return false;
  for (i = 0; i < cpf.length - 1; i++)
        if (cpf.charAt(i) != cpf.charAt(i + 1))
              {
              digitos_iguais = 0;
              break;
              }
  if (!digitos_iguais)
        {
        numeros = cpf.substring(0,9);
        digitos = cpf.substring(9);
        soma = 0;
        for (i = 10; i > 1; i--)
              soma += numeros.charAt(10 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0))
              return false;
        numeros = cpf.substring(0,10);
        soma = 0;
        for (i = 11; i > 1; i--)
              soma += numeros.charAt(11 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1))
              return false;
        return true;
        }
  else
        return false;
}

jQuery.fn.spinner = function (options = {}) {

      if (!this.length)
        return undefined;
    
      if (typeof options == 'boolean')
        options = { submit: options };
    
      let _options = {
        submit: options.submit,
        spinnerSize: options.spinnerSize || '1rem',
        selector: options.selector || null,
      };
    
      this.each(function () {
    
        if (this.nodeName != 'BUTTON')
          return;
    
        if (!$(this).data('tt-submete') && _options.submit !== false) {
          const width = $(this).width();
          const height = $(this).height();
          $(this).data('tt-submete', $(this).html());
          $(this).attr('disabled', true);
          $(this).width(width);
          $(this).height(height);
          $(this).html(`<div class="d-flex mx-auto" style="width: ${_options.spinnerSize}; height: ${_options.spinnerSize};"><div class="spinner-border spinner-border-sm w-100 h-100"></div></div>`);
        }
        else if ($(this).data('tt-submete') && _options.submit !== true) {
          $(this).html($(this).data('tt-submete'));
          $(this).css('width', '');
          $(this).css('height', '');
          $(this).attr('disabled', false);
          $(this).removeData('tt-submete');
        }
      });
    
      if (_options.selector)
        $(_options.selector).prop('disabled', _options.submit);
    
      return this;
    }


    function validateEmail(email) 
    {
        var re = /\S+@\S+\.\S+/;
        return re.test(email);
    }


    $(document).ready(_=>{
      $('input, select').on('blur focus', function(){
        $(this).removeClass('is-invalid');
        console.log($(this));
      });
    });