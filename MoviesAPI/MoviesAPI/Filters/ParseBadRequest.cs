

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MoviesAPI.Filters
{
    public class ParseBadRequest : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            IStatusCodeActionResult?  result = context.Result as IStatusCodeActionResult;//using Microsoft.AspNetCore.Mvc.Infrastructure;
            //IStatusCodeActionResult bu Controller daki IActionResult u temsil ediyor context.
            //StatusCode erismek icin bizim result tun bu type da olmasi gerekiyor, ozellestirilmis bir resulttur
            if (result == null)
            {
                return;
            }
            //Artik statusCode lara IStatusCodeActionResult in properys i olan int? StatusCode { get; } uzerinden erisip hata durumlarinda
            //obje icinde gondeirlen mesajin dizi icinde gelmesini saglayacagiz
            var statusCode = result.StatusCode;
            if (statusCode == 400)//Eger ki BadRequest statusCode u alirsak
            {
                var response = new List<string>();//Bu string listesi icine atacagiz mesajlari
                //public virtual IActionResult? Result { get; set; }
                // public class BadRequestObjectResult : ObjectResult
                // public class ObjectResult : ActionResult, IStatusCodeActionResult, IActionResult
                //as () casting in alternatifidir ama farki, hata firlatmak yerine uygulamayi kirmadan null referans dondurur
                BadRequestObjectResult? badRequestObjectResult = context.Result as BadRequestObjectResult;//using Microsoft.AspNetCore.Mvc;
                if (badRequestObjectResult is not null && badRequestObjectResult.Value is string)
                {
                    //public object? Value { get; set; } bu ObjectResulta ait bir porperty
                    response.Add(badRequestObjectResult.Value.ToString());
                }
                else
                {
                    //Eger string degil ise  ozaamn bize gelen data objedir
                    //context burdan geliyor...public class ActionExecutingContext : FilterContext
                    //  public abstract class FilterContext : ActionContext
                    //public ModelStateDictionary ModelState { get; } ActionContext class i icinde bir Dictionay dir
                    //Yani aslinda context de bir ActionContext tir ve de ModelState i kullanabilir onu inherit ettigi icin
                    //  public ModelStateDictionary ModelState { get; }
                    //ModelStateDictionary class i icerisinde KeyEnumrable type li Keys public KeyEnumerable Keys { get; }
                    // public readonly struct KeyEnumerable : IEnumerable<string>, IEnumerable bir type in dizi olarak taninabilmesi
                    //icin Enumerable olmasi gerekir
                    //public ValueEnumerable Values { get; } da var KeyEnumerable yaninda
                    foreach (var key in context.ModelState.Keys)
                    {
                        //context.ModelState[key] bu sira ile keys lere karsilik gelen value leri verecek
                        // public readonly struct ValueEnumerable : IEnumerable<ModelStateEntry>, IEnumerable
                        //public abstract class ModelStateEntry icinde  public ModelErrorCollection Errors { get; }
                        //string Enumerable proerty si var...
                        //Yani her bir key e karsilik gelen valu elerde erros Enumerable itere edilebilen yapilardan olusuyor
                        //Ve iclerinde ki her bir error un da ErrorMessage property si var, bunlari bizim en basta olustrudgumuz
                        //string listemize atan islemi gerceklestiriyourz..
                        foreach (var error in context.ModelState[key].Errors)
                        {
                            //Bu yontemle artik tum farkli propertieslerle tanimlanmis errorlara erisebiliriz..
                            response.Add($"{key}: {error.ErrorMessage}");
                        }
                    }
                }
                context.Result = new BadRequestObjectResult(response);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
/*
Biz neden IActionFilter i inherit ederek bir islem yaptik cunku
IActionFilter lar request ile response

Ve bu islemi Startup.cs de global filtreleme olarak register etmemiz gerekiyor....cook onemli....
*/