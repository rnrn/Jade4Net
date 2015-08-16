using System;
using Jade.Compiler;
using Jade.Lexer.Tokens;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class DoctypeNode : Node
    {
        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template) 
            //throws JadeCompilerException
        {
            String name = getValue();
            if (name == null)
            {
                name = "5";
            }
            String doctypeLine = Doctypes.get(name);
            if (doctypeLine == null)
            {
                doctypeLine = "<!DOCTYPE " + name + ">";
            }

            if (doctypeLine.StartsWith("<?xml"))
            {
                template.setMode(Jade4Net.Mode.XML);
            }
            else if (doctypeLine.Equals("<!DOCTYPE html>"))
            {
                template.setMode(Jade4Net.Mode.HTML);
            }
            else
            {
                template.setMode(Jade4Net.Mode.XHTML);
            }

            writer.append(doctypeLine);
        }

    }

}
