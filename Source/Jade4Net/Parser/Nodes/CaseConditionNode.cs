using Jade.Compiler;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class CaseConditionNode : Node
    {

        private bool defaultNode = false;

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            block.execute(writer, model, template);
        }

        public void setDefault(bool defaultNode) {
            this.defaultNode = defaultNode;
        }

        public bool isDefault() {
            return defaultNode;
        }
    }
}
