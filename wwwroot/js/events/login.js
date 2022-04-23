(_=>{
  
  $('[data-invalid]').addClass('text-danger'); 

  const btAcessar  = $("#btAcessar");
  const inputLogin = $("#login");
  const inputSenha = $("#senha");


  btAcessar.click(_=>{

    let [login, senha] = [inputLogin.val(), inputSenha.val()];
    
  });
})();