using System;

namespace Jade.Parser
{

    public class FileNameBuilder
    {

        private String path;

        public FileNameBuilder(String path)
        {
            this.path = path;
        }

        public String build()
        {
            return path.EndsWith(".jade") ? path : path + ".jade";
        }

    }

}
