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
            for (int i = 0; i < 4; i++)
            {
                lstOptions.Add(new Option() { cadena = "" });
            }
            /*foreach (var item in lstOptions)
            {
                item.cadena = "";
            }*/
            //Ultima seccion del menu tiene Mini-menus
            lstOptions.Last().lstOptions = new List<Option>();
            for (int i = 0; i < 7; i++)
            {
                lstOptions.Last().lstOptions.Add(new Option() { cadena = "" });
            }
            /*foreach (var item in lstOptions.Last().lstOptions)
            {
                item.cadena = "";
            }*/
        }

        public void clearAll()
        {
            foreach (var item in lstOptions)
            {
                item.cadena = "";
            }
            foreach (var item in lstOptions.Last().lstOptions)
            {
                item.cadena = "";
            }
        }

        public void activeAll()
        {
            foreach (var item in lstOptions)
            {
                item.cadena = "active";
            }
            foreach (var item in lstOptions.Last().lstOptions)
            {
                item.cadena = "active";
            }
        }
    }
}