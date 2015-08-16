using Jade.Compiler;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class MixinNode : MixinInjectNode
    {
        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            if (hasBlock())
            {
                model.setMixin(getName(), this);
            }
            else
            {
                base.execute(writer, model, template);
            }
        }

    }
}

