using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Template;

namespace Jade.Exceptions
{
    public abstract class JadeException : ApplicationException
    {

        private static readonly long serialVersionUID = -8189536050437574552L;
        private String filename;
        private int lineNumber;
        private TemplateLoader templateLoader;

        public JadeException(String message, String filename, int lineNumber, TemplateLoader templateLoader, Exception e)
            : base(message, e)
        {
            ;
            this.filename = filename;
            this.lineNumber = lineNumber;
            this.templateLoader = templateLoader;
        }

        public JadeException(String message)
            : base(message)
        {
            ;
        }

        public String getFilename()
        {
            return filename;
        }

        public int getLineNumber()
        {
            return lineNumber;
        }

        public List<String> getTemplateLines()
        {
            try
            {
                List<String> result = new List<String>();
                var reader = templateLoader.getReader(filename);
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    result.Add(line);
                }
                return result;
            }
            catch (IOException)
            {
                return null;
            }
        }

        public override String ToString()
        {
            return this.GetType().FullName + " " + getFilename() + ":" + getLineNumber() + "\n" + Message;
        }

        public String toHtmlString()
        {
            return toHtmlString(null);
        }

        public String toHtmlString(String generatedHtml)
        {
            Dictionary<String, Object> model = new Dictionary<string, object>();
            model.Add("filename", filename);
            model.Add("linenumber", lineNumber);
            model.Add("message", Message);
            model.Add("lines", getTemplateLines());
            model.Add("exception", getName());
            if (generatedHtml != null)
            {
                model.Add("html", generatedHtml);
            }

            try
            {
                string content;
                using (var res = typeof(JadeException).Assembly.GetManifestResourceStream("/error.jade"))
                using (var stream = new StreamReader(res))
                    content = stream.ReadToEnd();
                return Jade4Net.render(content, model, true);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
                return null;
            }
        }

        private String getName()
        {
            return this.GetType().FullName.Replace("([A-Z])", " $1").Trim();
        }
    }
}