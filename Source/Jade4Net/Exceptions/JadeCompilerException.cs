using System;
using Jade.Parser.Nodes;
using Template;

namespace Jade.Exceptions
{
    public class JadeCompilerException : JadeException
    {

        //private static readonly long serialVersionUID = -126617495230190225L;

        public JadeCompilerException(Node node, TemplateLoader templateLoader, Exception e)
            :base(e.Message, node.getFileName(), node.getLineNumber(), templateLoader, e) {
           ;
        }

        public JadeCompilerException(Node node, TemplateLoader templateLoader, String message)
            : base(message, node.getFileName(), node.getLineNumber(), templateLoader, null){
            ;
        }

    }
}
