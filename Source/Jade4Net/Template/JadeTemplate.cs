using System.IO;
using Jade.Model;
using Jade.Parser.Nodes;
using Template;

namespace Jade.Template
{
    public class JadeTemplate
    {

        private bool prettyPrint = false;
        private Node rootNode;
        private bool terse = true;
        private bool xml = false;
        private TemplateLoader templateLoader;

        public void process(JadeModel model, TextWriter writer)
        {
            Compiler.Compiler compiler = new Compiler.Compiler(rootNode);
            compiler.setPrettyPrint(prettyPrint);
            compiler.setTemplate(this);
            compiler.compile(model, writer);
        }

        public bool isPrettyPrint()
        {
            return prettyPrint;
        }

        public void setPrettyPrint(bool prettyPrint)
        {
            this.prettyPrint = prettyPrint;
        }

        public Node getRootNode()
        {
            return rootNode;
        }

        public void setRootNode(Node rootNode)
        {
            this.rootNode = rootNode;
        }

        public bool isTerse()
        {
            return terse;
        }

        public bool isXml()
        {
            return xml;
        }

        public void setTemplateLoader(TemplateLoader templateLoader)
        {
            this.templateLoader = templateLoader;
        }

        public TemplateLoader getTemplateLoader()
        {
            return templateLoader;
        }

        public void setMode(Jade4Net.Mode mode)
        {
            xml = false;
            terse = false;
            switch (mode)
            {
                case Jade4Net.Mode.HTML:
                    terse = true;
                    break;
                case Jade4Net.Mode.XML:
                    xml = true;
                    break;
            }
        }
    }

}
