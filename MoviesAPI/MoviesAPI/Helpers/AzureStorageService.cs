

using Azure.Storage.Blobs;

namespace MoviesAPI.Helpers {
    public class AzureStorageService : IFileStorageService
    {
        //constructor uzerinde azure storage in connection stringi uzerinden baglanti kuracagiz
        //Azure storage da deployment olusturduktan sonra go to research deyip access keys e giderz
        //Ordan connection string i kopyalayip kullanacagiz bu connection string bize Azure storage
        // hesabina dosya yukleyebilmemizi sagliyor
    //appsettings.Development.json da AzureStorageConnection key ismi ile connection string i kaydedip kullaniriz
    //Peki appsettings.Development taki bir value yi kulllanabilmek icin neye ihtiyacimz var tabi ki IConfiguration interface ine
    //Cunku biz appsettings,secret.json,environment gibi json string leri tuttugumuz data larimizin hepsine IConfiguration uzerinden
    //erisebiliriz
        private string connectionString;
        public AzureStorageService(IConfiguration configuration)
        {
            connectionString=configuration.GetConnectionString("AzureStorageConnection");
        }
        public async Task DeleteFile(string fileRoute, string containerName)
        {
            if(string.IsNullOrEmpty(fileRoute)){//Eger actor un resmi yok ise dogrudan fonksiyonu bitiriyoruz asagi inip islemlere devam etmek icin
                return;
            }

              var client=new BlobContainerClient(connectionString,containerName);
          await client.CreateIfNotExistsAsync();
          var fileName=Path.GetFileName(fileRoute);
          var blob=client.GetBlobClient(fileName);
          await blob.DeleteIfExistsAsync();
        }

//Edit islemi Azure de, dosyayi silip yeniden create etme islemi dir
        public async Task<string> EditFile(string containerName, IFormFile file, string fileRoute)
        {
           await DeleteFile(fileRoute,containerName);
           return await SaveFile(containerName,file); 
        }

        public async Task<string> SaveFile(string containerName, IFormFile file)
        {
          var client=new BlobContainerClient(connectionString,containerName);
          await client.CreateIfNotExistsAsync();//Bu container olusturuyor AzureStorage da, eger container yok ise daha onceden olusturulmamis ise
          //AzureStorage da container demek, bize dosyalarimzi group yaparak birlestirmeye izin veren bir dosya diye  dusunebiliriz
          //2 tane container imiz olacak 1 i actor picture leri icin 2. si  movies posterleri icin
          client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
          var extension=Path.GetExtension(file.FileName);
          //Resim isimlerini random yapiyoruz burda
          var fileName=$"{Guid.NewGuid()}{extension}";//string interpolation yapiyoruz
          var blob=client.GetBlobClient(fileName);
          await blob.UploadAsync(file.OpenReadStream());
          //Cloud icinde dosyanin url lini donduruyoruz burda da 
          return blob.Uri.ToString();

        }
    }
}
/*
Bu islemlerle biz, Azure servis implementasyonunu gerceklestirmis olduk
Bu islemden sonra nereye gidiyoruz tabi ki her zaman ki gibi, IFileStorageService ve AzureStorageServiceyi
Startup ta IServiceCollections da register etmeyi, yani IoC container imiza register etme islemine
*/