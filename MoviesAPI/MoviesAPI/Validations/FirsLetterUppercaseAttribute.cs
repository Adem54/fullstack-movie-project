
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Validations {
    public class FirstLetterUppercaseAttribute:ValidationAttribute
     {
        //Burda value nin nulll olma ihtimalinden dolayi her an hata alabiliyoruz ondan dolayi nullreference hatasini
        // bu sekilde nullable yaparak cozebiliyoruz 
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //value burda bize valu of the property i getiriyor
            //Diger tum value lere de validationContext araciligi ile erisebiliriz..
            // var genre=validationContext.ObjectInstance;
            
            //Eger client empty string veya null girerse o zaman bu firstLetter in buyuk mu kucuk mu oldugnun kontrol edilmesinin anlami yok
            //Ondan dolayi ilk once biz istiyoruz ki gelsin burda kullanicinin gonderidigi value dolu geliyor mu onu kontrol etsin isiyoruz
            if(value == null || string.IsNullOrEmpty(value.ToString()) ){
                  return ValidationResult.Success; 
            }
            //The thing is that we don't want to repeat the same validation in different validation rules
           var result=value.ToString();// Dereference of a possibly null reference  bu hatayi aliyoruz cunku null gelme durumunda hicbirsi gecerli olmyyor
           string firstLetter="";
           if(result is not null) {
             firstLetter=result.Substring(0,1);
           }
            if(firstLetter != firstLetter.ToUpper()){
                return new ValidationResult("FirstLetter should be uppercase");
            }
               return ValidationResult.Success;
        }
     }
}