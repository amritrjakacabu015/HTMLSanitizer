using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HtmlAgilityPack;

namespace APISanitizer
{
    static class Program
    {
        static void Main(string[] args)
        {
//            Console.WriteLine(Sanitizer.SanitizeHtml("<img alt=\"\" src=\"x\" onerror=\"alert(document.cookie)\" />"));
//            Console.WriteLine(Sanitizer.SanitizeHtml("<div><script>This is a paragraph</p></div>"));
//            Console.WriteLine(Sanitizer.SanitizeHtml("<div><p></p></div> This is a test"));
//            Console.WriteLine(Sanitizer.SanitizeHtml("<meta />"));
//            Console.WriteLine(Sanitizer.SanitizeHtml("This is just a regular string with a script <script> tag"));
            Console.WriteLine(Sanitizer.SanitizeHtml("<scri<script>pt>alert(1)</scri</script>pt>"));
//            Console.WriteLine(Sanitizer.SanitizeHtml("<tq-form-field><tq-form><div>Hello World!</p>"));


            Console.Read();
        }
    }
}
