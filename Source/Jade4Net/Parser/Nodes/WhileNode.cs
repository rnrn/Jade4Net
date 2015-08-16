using Jade.Compiler;
using Jade.Exceptions;
using Jade.Expression;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class WhileNode : Node
    {

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            try
            {
                model.pushScope();
                while (ExpressionHandler.evaluateBooleanExpression(value, model).Value)
                {
                    block.execute(writer, model, template);
                }
                model.popScope();
            }
            catch (ExpressionException e)
            {
                throw new JadeCompilerException(this, template.getTemplateLoader(), e);
            }
        }
    }

}
