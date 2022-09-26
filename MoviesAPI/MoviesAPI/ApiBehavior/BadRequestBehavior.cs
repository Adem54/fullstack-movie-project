
using Microsoft.AspNetCore.Mvc;

namespace MoviesAPI.ApiBehavior {
    public class BadRequestBehavior {
        //Burda static void uniq bir method olusturacagiz
        public static void Parse(ApiBehaviorOptions options){
            //public class ApiBehaviorOptions : IEnumerable<ICompatibilitySwitch>, IEnumerable{ } clsasi
            //public Func<ActionContext, IActionResult> InvalidModelStateResponseFactory { get; set; } 
            //bu da hazir anonim fonksiyon tutabilen bir funct delegesidir, ActionContext parametre degeri iken
            //IActionResult ise donus type dir..
            //Yani context ActionContext typedir
              options.InvalidModelStateResponseFactory=context=>{
                var response=new List<string>();
                 foreach (var key in context.ModelState.Keys)
                    {
                        foreach (var error in context.ModelState[key].Errors)
                        {
                            //Bu yontemle artik tum farkli propertieslerle tanimlanmis errorlara erisebiliriz..
                            response.Add($"{key}: {error.ErrorMessage}");
                        }
                    }

                    return new BadRequestObjectResult(response);
              };  
        }
    }
}