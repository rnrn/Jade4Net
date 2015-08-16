using System;
using System.Collections.Generic;

namespace Jade.Lexer.Tokens
{

    public class Doctypes
    {
        private static Dictionary<String, String> doctypes = new Dictionary<string, string>();

        static Doctypes()
        {
            doctypes.Add("5", "<!DOCTYPE html>");
            doctypes.Add("html", "<!DOCTYPE html>");
            doctypes.Add("xml", "<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            doctypes.Add("transitional",
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            doctypes.Add("strict",
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">");
            doctypes.Add("frameset",
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Frameset//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd\">");
            doctypes.Add("1.1",
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">");
            doctypes.Add("basic",
                "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML Basic 1.1//EN\" \"http://www.w3.org/TR/xhtml-basic/xhtml-basic11.dtd\">");
            doctypes.Add("mobile",
                "<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.2//EN\" \"http://www.openmobilealliance.org/tech/DTD/xhtml-mobile12.dtd\">");

        }

        public static String get(String jadeDoctype)
        {
            return doctypes[jadeDoctype];
        }
    }
}
