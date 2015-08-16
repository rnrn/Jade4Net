using System;
using System.IO;
using System.Reflection;
using Template;

namespace Jade.Template
{
    public class ClasspathTemplateLoader : TemplateLoader
    {
        private static readonly String suffix = ".jade";
        private String encoding = "UTF-8";

        public long getLastModified(String name) {
            return -1;
        }

        public TextReader getReader(String name)
        {
            if (!name.EndsWith(suffix)) name = name + suffix;
            var assembly = Assembly.GetExecutingAssembly();

            return new StreamReader(assembly.GetManifestResourceStream(name));
        }

        public string GetPath()
        {
            throw new NotImplementedException();
        }

        public String getEncoding() {
            return encoding;
        }

        public void setEncoding(String encoding) {
            this.encoding = encoding;
        }
    }
}
