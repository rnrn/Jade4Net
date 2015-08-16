using Jade.Compiler;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser
{
    public class BlockCommentNode : CommentNode
    {
        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)// throws JadeCompilerException
        {
            if (!isBuffered())
            {
                return;
            }
            writer.newline();
            if (value.StartsWith("if"))
            {
                writer.append("<!--[" + value + "]>");
                block.execute(writer, model, template);
                writer.append("<![endif]-->");
            }
            else
            {
                writer.append("<!--" + value);
                block.execute(writer, model, template);
                writer.append("-->");
            }
        }

    }

}
