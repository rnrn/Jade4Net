using System;
using System.Collections.Generic;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class TextNode : Node
    {
        private List<Object> preparedValue = new List<Object>();
        public List<Object> PreparedValue { get { return preparedValue; } }

        public void appendText(String txt)
        {
            value += txt;
            prepare();
        }

        public void setValue(String value)
        {
            this.value = value;
            prepare();
        }

        public String getValue()
        {
            return value;
        }

        private void prepare()
        {
            preparedValue = Utils.prepareInterpolate(value, false);
        }

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            try
            {
                String str = Utils.interpolate(preparedValue, model);
                writer.append(str);
            }
            catch (ExpressionException e)
            {
                throw new JadeCompilerException(this, template.getTemplateLoader(), e);
            }
        }

        public void addNode(Node node)
        {
            nodes.AddLast(node);
        }

        public LinkedList<Node> getNodes()
        {
            return nodes;
        }
    }

}