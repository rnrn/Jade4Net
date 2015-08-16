using System;
using System.IO;

namespace Template
{
    public class ReaderTemplateLoader : TemplateLoader
    {

        private readonly TextReader reader;
        private readonly String name;

        public ReaderTemplateLoader(TextReader reader, String name) {
            this.reader = reader;
            this.name = name;
        }

        public long getLastModified(String name)
        {
            checkName(name);
            return -1;
        }

        public TextReader getReader(String name)
        {
            checkName(name);
            return reader;
        }

        public string GetPath()
        {
            throw new NotImplementedException();
        }

        private void checkName(String name) {
            if (!name.Equals(this.name))
            {
                throw new ApplicationException("this reader only responds to [" + name + "] templates");
            }
        }



    }

}
