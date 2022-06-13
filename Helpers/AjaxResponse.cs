
namespace logiWeb.Helpers; 


public class AjaxResponse {

    public bool Error { get; set; } = false; 

    public string Message {get; set;} = string.Empty; 

    public object Items { get; set;}
}