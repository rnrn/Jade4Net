using Jade.Compiler;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class LiteralNode : Node
    {
        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            writer.append(value);
        }

    }
}