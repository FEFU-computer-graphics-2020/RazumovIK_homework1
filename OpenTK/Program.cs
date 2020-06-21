using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTK
{
    public class Program
    {
        public static void Main()
        {
            var app = new App(1000, 1000, "OpenGL");
            app.Run(30);
        }
    }
}
