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
                //Obetener el archivo y listarlo
                var dataFile = file;                
                var reader = new StreamReader(dataFile.OpenReadStream());
                var readFile = reader.ReadToEnd();
                readFile = readFile.Replace("\n", "");
                var dataList = readFile.Split('\r').ToList();
                dataList.Remove(""); 
                
                //procesar los datos y retornar la lista organizada
                var rawData = GetWorkDays(dataList, document);
                string cleanString = rawData.Replace("\r", "" );
                var cleanedList = cleanString.Split('\n').ToList();
                cleanedList.Remove("");
                List<string> finalList = new List<string>();

                foreach (var item in cleanedList)
                {
                    finalList.Add(item);                 
                }
                return Ok(finalList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //obtener los dias trabajados
        private string GetWorkDays(List<string> txt, int id)
        {
            List<int> rawList = new List<int>();

            rawList = txt.Select(x => Convert.ToInt32(x)).ToList();
            int nDay = 0;
            int count;
            string result = "";

            // listar viajes resultado por dia trabajado
            for (int i = 1; i < rawList.Count; i++)
            {
                nDay++;
                var data = rawList[i];
                List<int> listElemenstByDay = new List<int>();

                for (count = i + 1; count <= (i + data); count++)
                {
                    listElemenstByDay.Add(rawList[count]);
                }

                //obtener el # de viajes por dia
                var getWorkDay = "Case #" + nDay + ": " + GetWorkTrips(listElemenstByDay);
                result = string.Concat(result, getWorkDay, Environment.NewLine);
                //SaveData(id, resultadoxDia);
                i = count - 1;
            }

            return result;
        }

        //obtener el mejor # de viajes por dia trabajado
        // 1 ≤ T ≤ 500
        //1 ≤ N ≤ 100
        //1 ≤ Wi ≤ 100
        public static int GetWorkTrips(List<int> elements)
        {
            var coltrolStack = elements.Max();
            elements.Remove(coltrolStack);

            var Wi = 0; //peso de cada elemento
            var K = 1; //# de elementos en la bolsa
            var trips = 0; // # de viajes

            //logica para hallar optimo # de viajes
            while (Wi < 50 && coltrolStack < 50)
            {
                if (elements.Count == 0) 
                    return 0;

                var masBajo = elements.Min();
                elements.Remove(masBajo);
                K++;
                Wi = coltrolStack * K;
            }

            trips++;
            if (elements.Count > 0)
                trips += GetWorkTrips(elements);

            return trips;
        }

        //Guardar registro en BD
        private void SaveData(int document, string tripsNumber)
        {
            var data = new Ejecutor()
            {
                Document = document, 
                TransacDate = DateTime.Now.ToString(),
                NWorkTrips = tripsNumber
            };
            _context.Ejecutors.Add(data);
            _context.SaveChanges();
        }        
    }
}
