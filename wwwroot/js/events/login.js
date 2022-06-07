(_=>{
  
  $('[data-invalid]').addClass('text-danger'); 

  const btAcessar  = $("#btAcessar");
  const inputLogin = $("#login");
  const inputSenha = $("#senha");


  btAcessar.click(_=>{

    let [login, senha] = [inputLogin.val(), inputSenha.val()];


    if(!login)
      inputLogin.addClass('is-invalid');
    
    if(!senha)
      inputSenha.addClass('is-invalid');


    if(!login && !senha)
      return; 
    
    $.ajax({
      url: '/login/autenticar',
      dataType: 'JSON',
      type: 'POST',
      data: {
        Login: login, 
        Senha: senha, 
      },
      complete: data => {
        eval(data.responseText);
      }
    });

  });


  $('input').on('focus', function(){
    $(this).removeClass('is-invalid');
    $('#msg_usuario_senha_invalidos').css('visibility','hidden');
  });

})();


function showPass(el){

  let showPassword = el.dataset.visible == 'on';
  let icon = $("#eye_icon");     
  if(icon.hasClass('fa-eye-slash')){
      icon.removeClass('fa-eye-slash').addClass('fa-eye');
      $("#senha").attr('type','text');
  }
  else {
      $("#senha").attr('type','password');
      icon.addClass('fa-eye-slash').removeClass('fa-eye');
  }
}