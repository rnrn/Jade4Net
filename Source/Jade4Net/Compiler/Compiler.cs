using System;
using System.IO;
using Jade.Model;
using Jade.Parser.Nodes;
using Jade.Template;

namespace Jade.Compiler
{
    public class Compiler
    {

        private readonly Node rootNode;
        private bool prettyPrint;
        private JadeTemplate template = new JadeTemplate();

        public Compiler(Node rootNode)
        {
            this.rootNode = rootNode;
        }

        public String compileToString(JadeModel model)
        {
            StringWriter writer = new StringWriter();
            compile(model, writer);
		return writer.ToString();
        }

        public void compile(JadeModel model, TextWriter w)
        {
            IndentWriter writer = new IndentWriter(w);
            writer.setUseIndent(prettyPrint);
            rootNode.execute(writer, model, template);
        }

        public void setPrettyPrint(bool prettyPrint)
        {
            this.prettyPrint = prettyPrint;
        }

        public void setTemplate(JadeTemplate jadeTemplate)
        {
            this.template = jadeTemplate;
        }
    }
}
