using System;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Expression;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class ExpressionNode : Node
    {

        private bool escape;
        private bool buffer;

        public void setEscape(bool escape) {
            this.escape = escape;
        }

        public void setBuffer(bool buffer) {
            this.buffer = buffer;
        }

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template) //throws JadeCompilerException
        {
            try
            {
                Object result = ExpressionHandler.evaluateStringExpression(getValue(), model);
                if (result == null || !buffer)
                {
                    return;
                }
                String str = result.ToString();
                if (escape)
                {
                    str = Utils.escapeHTML(str);
                }
                writer.append(str);

                if (hasBlock())
                {
                    writer.increment();
                    block.execute(writer, model, template);
                    writer.decrement();
                    writer.newline();
                }

            }
            catch (ExpressionException e)
            {
                throw new JadeCompilerException(this, template.getTemplateLoader(), e);
            }
        }

        public void setValue(String value) {
            base.setValue(value.Trim());
        }
    }
}
