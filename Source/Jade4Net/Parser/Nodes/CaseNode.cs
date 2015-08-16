using System;
using System.Collections.Generic;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Expression;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class CaseNode : Node
    {

        private List<CaseConditionNode> caseConditionNodes = new List<CaseConditionNode>();

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            try
            {
                foreach (CaseConditionNode caseConditionNode in caseConditionNodes)
                {
                    if (caseConditionNode.isDefault() || checkCondition(model, caseConditionNode))
                    {
                        caseConditionNode.execute(writer, model, template);
                        break;
                    }
                }
            }
            catch (ExpressionException e)
            {
                throw new JadeCompilerException(this, template.getTemplateLoader(), e);
            }
        }

        private bool checkCondition(JadeModel model, Node caseConditionNode) //throws ExpressionException
        {
            return ExpressionHandler.evaluateBooleanExpression(value + " == " + caseConditionNode.getValue(), model).Value;
        }

        public void setConditions(List<CaseConditionNode> caseConditionNodes) {
            this.caseConditionNodes = caseConditionNodes;
        }

        public List<CaseConditionNode> getCaseConditionNodes() {
            return caseConditionNodes;
        }

       
        public override object Clone() //throws CloneNotSupportedException
        {
            CaseNode clone = (CaseNode)base.Clone();

            clone.caseConditionNodes = new List<CaseConditionNode>();
            foreach (CaseConditionNode condition in caseConditionNodes)
            {
                clone.caseConditionNodes.Add((CaseConditionNode)condition.Clone());
            }

            return clone;
        }
    }

}
