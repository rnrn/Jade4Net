using System;
using Jade.Compiler;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class BlockNode : Node
    {

        private bool yield = false;
        private BlockNode yieldBlock;
        private String mode;

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            foreach (Node node in getNodes())
            {
                node.execute(writer, model, template);
            }
        }

        public void setYield(bool yield)
        {
            this.yield = yield;
        }

        public bool isYield()
        {
            return yield;
        }

        public void setYieldBlock(BlockNode yieldBlock)
        {
            this.yieldBlock = yieldBlock;
        }

        public BlockNode getYieldBlock()
        {
            return yieldBlock;
        }

        public BlockNode getIncludeBlock()
        {
            foreach (Node node in getNodes())
            {
                if (node is BlockNode && ((BlockNode) node).isYield())
                {
                    return (BlockNode) node;
                }
                if (node is TagNode && ((TagNode) node).isTextOnly())
                {
                    continue;
                }
                if (node is BlockNode && ((BlockNode) node).getIncludeBlock() != null)
                {
                    return ((BlockNode) node).getIncludeBlock();
                }
                if (node.hasBlock() && node.getBlock() is BlockNode)
                {
                    return ((BlockNode) node.getBlock()).getIncludeBlock();
                }
            }
            return this;
        }

        public String getMode()
        {
            return mode;
        }

        public void setMode(String mode)
        {
            this.mode = mode;
        }

    }

}