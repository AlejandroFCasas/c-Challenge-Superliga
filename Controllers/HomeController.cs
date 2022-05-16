using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {
        List<Socio> valuesCollection = new List<Socio>();

        List<string> NameRiverList = new List<string>();

        List<string> listadoDePersonas = new List<string>();

        public ActionResult Index()
        {


            return View();
        }
        
        
    [HttpPost]
    public ActionResult Data(HttpPostedFileBase file)
        {
            if( file == null || !file.ContentType.Equals("text/csv"))
            {
                TempData["errorCsv"]= "Cargue solo archivos .csv";   
                return RedirectToAction("Index");
            }




            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/csv/socios.csv");

            //List<string> valuesCollection = new List<string>();

            using (var f = new StreamReader(path))
            {
                string line = string.Empty;

                while ((line = f.ReadLine()) != null)
                {
                    var parts = line.Split(';');
                    Socio aux = new Socio();
                    aux.name = parts[0];
                    aux.age = Int32.Parse(parts[1]);
                    aux.cuadro = parts[2];
                    aux.civil = parts[3];
                    aux.estudios = parts[4];
                    valuesCollection.Add(aux);
                }

                //Item Nro 1
                TempData["totalPersonas"] = valuesCollection.Count();

                //Item Nro 2
                TempData["RacingPromedioEdad"] = valuesCollection
                    .Where(x => x.cuadro.Equals("Racing"))
                    .Average(x => x.age);

                //Item Nro 3
                //var item3 = new List<String>();
                var item3 = valuesCollection
                    .OrderBy(x => x.age)
                    .Where(x => x.estudios.Equals("Universitario"))
                    .Where(x => x.civil.Equals("Casado"))
                    .Select(x => new Socio {
                    
                        name = x.name+" ",
                        age = x.age,
                        cuadro = " "+x.cuadro
                         } as Socio
                    ) 
                    .Take(100)
                    .ToList();


                TempData["100Personas"] = item3;


                //Item Nro 4
                var namePopulares = new List<String>();
                foreach (var grouping in valuesCollection.Where(t => t.cuadro.Equals("River")).GroupBy(t=>t.name).Take(5)) //GroupBy(t => t.name).

                //foreach (var grouping in valuesCollection.GroupBy(t => t.cuadro.Equals("River")).Take(5))
                {
                    namePopulares.Add("El nombre " + grouping.Key + " se repite " + grouping.Count() + " veces.");
                    //namePopulares2.Add (string.Format("'{0}' está repetido {1} veces.", grouping.Key, grouping.Count()));

                    //namePopulares.Add(new SelectListItem()
                    //{
                    //    Text = grouping.Key,
                    //    Value = grouping.Count().ToString()
                    //});
                }

                TempData["nombrePopularesRiver"] = namePopulares;


                //Item Nro 5
                List<string> result = new List<string>();

                foreach (var equipos in valuesCollection.GroupBy(t => t.cuadro).OrderByDescending(group => group.Count()))

                {
                    String s = string.Format("El equipo '{0}' tiene la cantidad de {1} socios, el promedio de sus socios es de {2} el socio con menor edad es la de {3} y la mayor edad es {4}", equipos.Key, equipos.Count(), equipos.Average(q => q.age), equipos.Min(z => z.age), equipos.Max(z => z.age));
                    result.Add(s);
                }

                TempData["Item5"] = result;


            }
            return View();

        }
    



    }


}
