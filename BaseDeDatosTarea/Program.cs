using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseDeDatosTarea
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //using (var db = new ToolBorrowingEntities())
            //{
            //    var tools = db.Tools.ToList(); 
            //    Console.WriteLine("Tools in the database:");
            //    foreach (var tool in tools)
            //    {
            //        Console.WriteLine($"ID: {tool.Id}, Nombre: {tool.Name}, Cantidad: {tool.Quantity}, Disponible: {tool.Status}");
            //    }

            //    Console.WriteLine("Inserting a new tool");
            //    var newTool = new Tool
            //    {
            //        Name = "Martillo",
            //        Description = "Martillo de carpintero",
            //        Quantity = 5,
            //        Status = true
            //    };
            //    db.Tools.Add(newTool);
            //    db.SaveChanges();
            //    Console.WriteLine("New tool inserted");

            //    Console.WriteLine("Tools in the database:");
            //    foreach (var tool in db.Tools.ToList())
            //    {
            //        Console.WriteLine($"ID: {tool.Id}, Nombre: {tool.Name}, Cantidad: {tool.Quantity}, Disponible: {tool.Status}");
            //    }

            //    Console.WriteLine("Press any key to exit...");
            //    Console.ReadKey();
            //}
        }
    }
}
