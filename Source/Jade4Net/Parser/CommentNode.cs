using Jade.Compiler;
using Jade.Model;
using Jade.Parser.Nodes;
using Jade.Template;

namespace Jade.Parser
{
    public class CommentNode : Node
    {
        private bool buffered;

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)// throws JadeCompilerException
        {
            if (!isBuffered())
            {
                return;
            }
            writer.newline();
            writer.append("<!--");
            writer.append(value);
            writer.append("-->");
        }

        public bool isBuffered() {
            return buffered;
        }

        public void setBuffered(bool buffered) {
            this.buffered = buffered;
        }
    }

}
