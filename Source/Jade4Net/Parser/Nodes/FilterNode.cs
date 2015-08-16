using System;
using System.Collections.Generic;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class FilterNode : Node
    {

        private Node textBlock;
        private Dictionary<String, Object> attributes = new Dictionary<String, Object>();

        public bool hasTextBlock()
        {
            return textBlock != null;
        }

        public void setTextBlock(Node textBlock)
        {
            this.textBlock = textBlock;
        }

        public Node getTextBlock()
        {
            return textBlock;
        }

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            var filter = model.getFilter(getValue());
            String result = textBlock.getValue();
            if (filter != null)
            {
                result = filter.convert(result, attributes, model);
            }
            try
            {
                result = Utils.interpolate(result, model, false);
            }
            catch (ExpressionException e)
            {
                throw new JadeCompilerException(this, template.getTemplateLoader(), e);
            }
            writer.append(result);
        }

        public void setAttributes(Dictionary<String, Object> attributes)
        {
            this.attributes = attributes;

        }

    }

}