using System;
using Jade.Compiler;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class IfConditionNode : Node
    {

        private bool defaultNode = false;
        private bool _isInverse = false;

        public IfConditionNode(String condition, int lineNumber) {
            this.value = condition;
            this.lineNumber = lineNumber;
        }

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

        public bool isInverse() {
            return _isInverse;
        }

        public void setInverse(bool isInverse) {
            _isInverse = isInverse;
        }
    }

}
