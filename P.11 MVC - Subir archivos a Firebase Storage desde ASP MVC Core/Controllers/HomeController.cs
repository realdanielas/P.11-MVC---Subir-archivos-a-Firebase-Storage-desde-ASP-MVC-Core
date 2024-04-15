using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Mvc;
using P._11_MVC___Subir_archivos_a_Firebase_Storage_desde_ASP_MVC_Core.Models;
using System.Diagnostics;

namespace P._11_MVC___Subir_archivos_a_Firebase_Storage_desde_ASP_MVC_Core.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SubirArchivo(IFormFile archivo)
        {
            Stream archivoASubir = archivo.OpenReadStream();

            string email = "ana.saavedra@catolica.edu.sv";
            string clave = "";
            string ruta = "aspstoragefile.appspot.com";
            string api_key = "";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var autenticarFirebase = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();
            var tokenUser = autenticarFirebase.FirebaseToken;

            var tareaCargarArchivo = new FirebaseStorage(ruta,
                                                            new FirebaseStorageOptions
                                                            {
                                                                AuthTokenAsyncFactory = () => Task.FromResult(tokenUser),
                                                                ThrowOnCancel = true
                                                            }
                                                        ).Child("Archivos").Child(archivo.FileName).PutAsync(archivoASubir, cancellation.Token);
            var urlArchivoCargado = await tareaCargarArchivo;

            return RedirectToAction("VerImagen");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
