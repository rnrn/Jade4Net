using System;
using System.Collections.Generic;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Expression;
using Jade.Model;
using Jade.Template;
using Jade.Util;

namespace Jade.Parser.Nodes
{
    public class MixinInjectNode : AttributedNode
    {

        protected List<String> arguments = new List<String>();

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)// throws JadeCompilerException
        {
            MixinNode mixin = model.getMixin(getName());
            if (mixin == null)
            {
                throw new JadeCompilerException(this, template.getTemplateLoader(), "mixin " + getName() + " is not defined");
            }

            // Clone mixin
            try
            {
                mixin = (MixinNode)mixin.Clone();
            }
            catch (Exception e)
            {
                // Can't happen
                throw;
            }

            if (hasBlock())
            {
                List<BlockNode> injectionPoints = getInjectionPoints(mixin.getBlock());
                foreach (BlockNode point in injectionPoints)
                {
                    point.getNodes().AddLast(block);
                }
            }

            model.pushScope();
            model.put("block", hasBlock());
            writeVariables(model, mixin, template);
            writeAttributes(model, mixin, template);
            mixin.getBlock().execute(writer, model, template);
            model.popScope();

        }

        private List<BlockNode> getInjectionPoints(Node block) {
            List<BlockNode> result = new List<BlockNode>();
            foreach (Node node in block.getNodes())
            {
                if (node is BlockNode && !node.hasNodes()) {
                result.Add((BlockNode)node);
            } else if (node is ConditionalNode){
                foreach (IfConditionNode condition in ((ConditionalNode)node).getConditions())
                {
                    result.AddRange(getInjectionPoints(condition.getBlock()));
                }
            } else if (node is CaseNode){
                foreach (CaseConditionNode condition in ((CaseNode)node).getCaseConditionNodes())
                {
                    result.AddRange(getInjectionPoints(condition.getBlock()));
                }
            } else if (node.hasBlock())
            {
                result.AddRange(getInjectionPoints(node.getBlock()));
            }
        }
		return result;
    }

    private void writeVariables(JadeModel model, MixinNode mixin, JadeTemplate template)
    {
        List<String> names = mixin.getArguments();
        List<String> values = arguments;
        if (names == null)
        {
            return;
        }
        for (int i = 0; i < names.Count; i++)
        {
            String key = names[i];
            Object value = null;
            if (i < values.Count)
            {
                value = values[i];
            }
            if (value != null)
            {
                try
                {
                    value = ExpressionHandler.evaluateExpression(values[i], model);
                }
                catch (Exception e)
                {
                    throw new JadeCompilerException(this, template.getTemplateLoader(), e);
                }
            }
            if (key != null)
            {
                model.put(key, value);
            }
        }
    }

    private void writeAttributes(JadeModel model, MixinNode mixin, JadeTemplate template)
    {
        model.put("attributes", mergeInheritedAttributes(model));
    }

    public List<String> getArguments()
    {
        return arguments;
    }

    public void setArguments(List<String> arguments)
    {
        this.arguments = arguments;
    }

    public void setArguments(String arguments)
    {
        this.arguments.Clear();
        this.arguments = ArgumentSplitter.split(arguments);
    }
}

}
