using System;
using System.Collections;
using System.Collections.Generic;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Expression;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class EachNode : Node
    {

        private String key;
        private String code;
        private Node elseNode;

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            Object result;
            try
            {
                result = ExpressionHandler.evaluateExpression(getCode(), model);
            }
            catch (ExpressionException e)
            {
                throw new JadeCompilerException(this, template.getTemplateLoader(), e);
            }
            if (result == null)
            {
                throw new JadeCompilerException(this, template.getTemplateLoader(), "[" + code + "] has to be iterable but was null");
            }
            model.pushScope();
            run(writer, model, result, template);
            model.popScope();
        }

	private void run(IndentWriter writer, JadeModel model, Object result, JadeTemplate template) {
            if (result is IEnumerable) {
                runIterator(((IEnumerable) result).GetEnumerator(), model, writer, template);
            } else if (result.GetType().IsArray)
            {
                var iterator = ((IEnumerable)result).GetEnumerator();
                runIterator(iterator, model, writer, template);
            }
            else if (result is Dictionary<String, Object>) {
                runMap((Dictionary<String, Object>)result, model, writer, template);
            }
        }

        private void runIterator(IEnumerator iterator, JadeModel model, IndentWriter writer, JadeTemplate template) {
            int index = 0;

            if (!iterator.MoveNext())
            {
                executeElseNode(model, writer, template);
                return;
            }

            while (iterator.MoveNext())
            {
                model.put(getValue(), iterator.Current);
                model.put(getKey(), index);
                getBlock().execute(writer, model, template);
                index++;
            }
        }

        private void runMap(Dictionary<String, Object> result, JadeModel model, IndentWriter writer, JadeTemplate template)
        {
            var keys = result.Keys;
            if (keys.Count == 0)
            {
                executeElseNode(model, writer, template);
                return;
            }
            foreach (String key in keys)
            {
                model.put(getValue(), result[key]);
                model.put(getKey(), key);
                getBlock().execute(writer, model, template);
            }
        }

        private void executeElseNode(JadeModel model, IndentWriter writer, JadeTemplate template) {
            if (elseNode != null)
            {
                elseNode.execute(writer, model, template);
            }
        }

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public String getKey() {
            return key == null ? "$index" : key;
        }

        public void setKey(String key) {
            this.key = key;
        }

        public Node getElseNode() {
            return elseNode;
        }

        public void setElseNode(Node elseNode) {
            this.elseNode = elseNode;
        }

    }

}
