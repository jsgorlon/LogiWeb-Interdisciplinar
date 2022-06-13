
namespace logiWeb.Helpers; 


public class AjaxResponse {

    public bool Error { get; set; } = false; 

    public List<string> Message {get; set;} = new List<string>(); 

    public Dictionary<string, object> Item {get; set;} = new Dictionary<string, object>(){};
}