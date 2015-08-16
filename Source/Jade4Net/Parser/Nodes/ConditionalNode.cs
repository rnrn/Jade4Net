using System;
using System.Collections.Generic;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Expression;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class ConditionalNode : Node
    {

        private List<IfConditionNode> conditions = new List<IfConditionNode>();

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            foreach (IfConditionNode conditionNode in this.conditions)
            {
                try
                {
                    if (conditionNode.isDefault() || checkCondition(model, conditionNode.getValue()) ^ conditionNode.isInverse())
                    {
                        conditionNode.getBlock().execute(writer, model, template);
                        return;
                    }
                }
                catch (ExpressionException e)
                {
                    throw new JadeCompilerException(conditionNode, template.getTemplateLoader(), e);
                }
            }
        }

        private bool checkCondition(JadeModel model, String condition) //throws ExpressionException
        {
            Boolean? value = ExpressionHandler.evaluateBooleanExpression(condition, model);
            return (value == null) ? false : value.Value;
        }

        public List<IfConditionNode> getConditions() {
            return conditions;
        }

        public void setConditions(List<IfConditionNode> conditions) {
            this.conditions = conditions;
        }

        public override object Clone() //throws CloneNotSupportedException
        {
            ConditionalNode clone = (ConditionalNode)base.Clone();

            clone.conditions = new List<IfConditionNode>();
            foreach (IfConditionNode condition in conditions)
            {
                clone.conditions.Add((IfConditionNode)condition.Clone());
            }

            return clone;
        }
    }
}
