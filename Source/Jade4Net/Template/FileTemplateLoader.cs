using System;
using System.IO;
using Template;

namespace Jade.Template
{
    public class FileTemplateLoader : TemplateLoader
    {

        private String encoding = "UTF-8";
        private String suffix = ".jade";
        private String basePath = "";

        public FileTemplateLoader(String basePath, String encoding) {
            this.basePath = basePath;
            this.encoding = encoding;
        }

        public long getLastModified(String name) {
            string filePath = FilePath(name);
            return new FileInfo(filePath).LastWriteTime.Ticks;
        }

        public TextReader getReader(String name)
        {
            TextReader templateSource = getFile(name);
            return templateSource;
        }

        public string GetPath()
        {
            return basePath;
        }

        private TextReader getFile(String name)
        {
            // TODO Security
            string filePath = FilePath(name);
            return File.OpenText(filePath);
        }

        private string FilePath(string name)
        {
            return Path.Combine(basePath, name);
        }
    }
}
