using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TNSEjecutor.apiMudanza.Data;
using TNSEjecutor.Common.Entities;

namespace TNSEjecutor.apiMudanza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MudanzasController : ControllerBase
    {

        private readonly DataContext _context;

        public MudanzasController(DataContext context)
        {
            _context = context;
        }

        //GET api/<controller>
        [HttpGet]
        public ActionResult Get()
        {
            try
            {
                return Ok(_context.Ejecutors.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET api/<controller>
        [HttpPost]
        [Route("PostProcesar/{document}")]
        public  ActionResult PostProcesar(IFormFile file, int document)
        {
            try
            {
                var dataFile = HttpContext.Request.Form.Files[0];
                //List<IFormFile> file = new List<IFormFile>(archivoasd);
                var result = new StringBuilder();
                var reader = new StreamReader(dataFile.OpenReadStream());
                var readfile = reader.ReadToEnd();
                readfile = readfile.Replace("\n", "");
                var listado = readfile.Split('\r').ToList();
                listado.Remove("");                
                
                var resultadoFinal = ProcesarDiasDeTrabajo(listado, document);
                string cleanString = resultadoFinal.Replace("\n", "").Replace("\r", "");                
                //JObject jsonResult = JObject.Parse(cleanString);
                //string cleanString = resultadoFinal.Replace("\n", "");
                //string cleanString = resultadoFinal.Replace("\r", "" );
                var listadoFinal = cleanString.Split('\r').ToList();
                //listadoFinal.Remove("");

                return Ok(listadoFinal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string ProcesarDiasDeTrabajo(List<string> txt, int id)
        {
            List<int> listadoInicial = new List<int>();

            listadoInicial = txt.Select(x => Convert.ToInt32(x)).ToList();
            int diaNumero = 0;
            int c;

            string resultado = "";

            // construir un listado con el resultado que debe generar la salida
            for (int z = 1; z < listadoInicial.Count; z++)
            {
                diaNumero++;
                var numeroObjetos = listadoInicial[z];
                List<int> listaPesoObjetosPorDia = new List<int>();

                for (c = z + 1; c <= (z + numeroObjetos); c++)
                {
                    listaPesoObjetosPorDia.Add(listadoInicial[c]);
                }

                var resultadoxDia = "Case #" + diaNumero + ": " + CalcularViajes(listaPesoObjetosPorDia);

                resultado = string.Concat(resultado, resultadoxDia, Environment.NewLine);

                GuardarLog(id, resultadoxDia);

                z = c - 1;
            }

            return resultado;
        }

        private void GuardarLog(int id, string resultadoFinal)
        {
            var log = new Ejecutor()
            {
                //Id = 0,
                Document = id,//asignar el documento que viene del front
                TransacDate = DateTime.Now.ToString(),
                NWorkTrips = resultadoFinal
            };

            _context.Ejecutors.Add(log);
            _context.SaveChanges();
        }

        public static int CalcularViajes(List<int> elementos)
        {
            var pivot = elementos.Max();
            elementos.Remove(pivot);

            var peso = 0;
            var i = 1; //numero de elementos a mover en la bolsa
            var viajes = 0;

            while (peso < 50 && pivot < 50)
            {
                if (elementos.Count == 0) //si no hay mas elementos
                    return 0;

                var masBajo = elementos.Min();
                elementos.Remove(masBajo);
                i++;
                peso = pivot * i;
            }

            viajes++;

            if (elementos.Count > 0)
                viajes += CalcularViajes(elementos);// invoca de nuevo la funcion hasta que no existan elementos

            return viajes;
        }
    }
}
