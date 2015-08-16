using System;
using System.Collections.Generic;
using Jade.Compiler;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public abstract class Node : ICloneable
    {

        protected LinkedList<Node> nodes = new LinkedList<Node>();
        protected int lineNumber;
        protected String name;
        protected String value;
        protected Node block;
        protected String fileName;

        public Node Block { get { return block; } }
        public int LineNumber { get { return lineNumber; } }
        public String Name { get { return name; } }
        public String FileName { get { return fileName; } }
        public String Value { get { return value; } }
        public LinkedList<Node> Nodes { get { return nodes; } }

        public abstract void execute(IndentWriter writer, JadeModel model, JadeTemplate template);

        public void setLineNumber(int lineNumber) {
            this.lineNumber = lineNumber;
        }

        public int getLineNumber() {
            return lineNumber;
        }

        public void setValue(String value) {
            this.value = value;
        }

        public String getValue() {
            return value;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getName() {
            return name;
        }

        public void push(Node node) {
            if (node == null)
            {
                throw new ApplicationException();
            }
            nodes.AddLast(node);
        }

        public LinkedList<Node> getNodes() {
            return nodes;
        }

        public void setNodes(LinkedList<Node> nodes) {
            this.nodes = nodes;
        }

        public Node pollNode()
        {
            if (nodes.Count < 1) return null;
            else
            {
                var first = nodes.First.Value;
                nodes.RemoveFirst();
                return first;
            }
        }

        public bool hasNodes() {
            return nodes.Count >0;
        }

        public bool hasBlock() {
            return block != null;
        }

        public Node getBlock() {
            return block;
        }

        public void setBlock(Node block) {
            this.block = block;
        }

        public String getFileName() {
            return fileName;
        }

        public void setFileName(String fileName) {
            this.fileName = fileName;
        }

        public virtual object Clone()
        {
            Node clone = (Node) MemberwiseClone();

            // Deep copy block
            if (this.block != null)
            {
                clone.block = (Node) this.block.Clone();
            }

            clone.nodes = new LinkedList<Node>();
            foreach (Node node in this.nodes)
            {
                clone.nodes.AddLast((Node) node.Clone());
            }

            return clone;
        }
    }
}
