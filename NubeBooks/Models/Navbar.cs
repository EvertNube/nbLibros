using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NubeBooks.Models
{
    public class Option
    {
        public string cadena { get; set; }
        public List<Option> lstOptions { get; set; }
    }
    public class Navbar
    {
        public List<Option> lstOptions { get; set; }
        
        public Navbar()
        {
            lstOptions = new List<Option>();
            for (int i = 0; i < 13; i++)
            {
                lstOptions.Add(new Option() { cadena = "", lstOptions = new List<Option>() });
            }

            //1 - Reportes
            lstOptions[1].lstOptions.Add(new Option() { cadena = "" });
            lstOptions[1].lstOptions.Add(new Option() { cadena = "" });
            lstOptions[1].lstOptions.Add(new Option() { cadena = "" });
            //2 - Libros
            lstOptions[2].lstOptions.Add(new Option() { cadena = "" });
            lstOptions[2].lstOptions.Add(new Option() { cadena = "" });
            //3 - Comprobantes
            lstOptions[3].lstOptions.Add(new Option() { cadena = "" });
            lstOptions[3].lstOptions.Add(new Option() { cadena = "" });
            lstOptions[3].lstOptions.Add(new Option() { cadena = "" });
            //6 - Entidades
            lstOptions[6].lstOptions.Add(new Option() { cadena = "" });
            lstOptions[6].lstOptions.Add(new Option() { cadena = "" });
            //7 - Pagos
            //lstOptions[7].lstOptions.Add(new Option() { cadena = "" });
            //8 - Presupuesto
            lstOptions[8].lstOptions.Add(new Option() { cadena = "" });
            lstOptions[8].lstOptions.Add(new Option() { cadena = "" });
            //9 - Inventarios
            lstOptions[9].lstOptions.Add(new Option() { cadena = "" });
            lstOptions[9].lstOptions.Add(new Option() { cadena = "" });
            //10- Items
            lstOptions[10].lstOptions.Add(new Option() { cadena = "" }); //10.0- Items
            lstOptions[10].lstOptions.Add(new Option() { cadena = "" }); //10.1- Ubicacion Items
            lstOptions[10].lstOptions.Add(new Option() { cadena = "" }); //10.2- Categoria Items 
            //11- Servicios
            //12- Proformas
            //13- Ordenes
        }

        public void clearAll()
        {
            foreach (var item in lstOptions)
            {
                item.cadena = "";
                foreach (var item2 in item.lstOptions)
                {
                    item.cadena = "";
                }
            }
        }

        public void activeAll()
        {
            foreach (var item in lstOptions)
            {
                item.cadena = "active";
                foreach (var item2 in item.lstOptions)
                {
                    item.cadena = "active";
                }
            }
        }
    }
}