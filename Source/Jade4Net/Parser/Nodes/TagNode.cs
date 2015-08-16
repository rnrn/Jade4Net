using System;
using System.Collections.Generic;
using System.Text;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Expression;
using Jade.Model;
using Jade.Template;

namespace Jade.Parser.Nodes
{
    public class TagNode : AttributedNode
    {
        private bool textOnly;
        private Node textNode;
        private Node codeNode;
        private static readonly String[] selfClosingTags = { "meta", "img", "link", "input", "area", "base", "col", "br", "hr", "source"};
        private bool selfClosing = false;

        public Node TextNode { get { return textNode; } }
        public Node CodeNode { get { return codeNode; } }
        public bool TextOnly { get { return textOnly; } }

        public void setTextOnly(bool textOnly) {
            this.textOnly = textOnly;

        }

        public void setTextNode(Node textNode) {
            this.textNode = textNode;
        }

        public void setCodeNode(Node codeNode) {
            this.codeNode = codeNode;
        }

        public bool isTextOnly() {
            return this.textOnly;
        }

        public Node getTextNode() {
            return textNode;
        }

        public bool hasTextNode() {
            return textNode != null;
        }

        public bool hasCodeNode() {
            return codeNode != null;
        }

        public override void execute(IndentWriter writer, JadeModel model, JadeTemplate template) //throws JadeCompilerException
        {
            writer.newline();
            writer.append("<");
            writer.append(name);
            writer.append(Attributes(model, template));
            if (isTerse(template))
            {
                writer.append(">");
                return;
            }
            if (isSelfClosing(template) || (selfClosing && isEmpty()))
            {
                writer.append("/>");
                return;
            }
            writer.append(">");
            if (hasTextNode())
            {
                textNode.execute(writer, model, template);
            }
            if (hasBlock())
            {
                writer.increment();
                block.execute(writer, model, template);
                writer.decrement();
                writer.newline();
            }
            if (hasCodeNode())
            {
                codeNode.execute(writer, model, template);
            }
            writer.append("</");
            writer.append(name);
            writer.append(">");
        }

        private bool isEmpty() {
            return !hasBlock() && !hasTextNode() && !hasCodeNode();
        }

        public bool isTerse(JadeTemplate template) {
            return isSelfClosing(template) && template.isTerse();
        }

        public bool isSelfClosing(JadeTemplate template)
        {
            return !template.isXml()
                && Array.FindIndex(selfClosingTags, s => s == name) >= 0;
        }

        private String Attributes(JadeModel model, JadeTemplate template) {
            StringBuilder sb = new StringBuilder();

            Dictionary<String, Object> mergedAttributes = mergeInheritedAttributes(model);

            foreach (var entry in mergedAttributes)
            {
                try
                {
                    sb.Append(getAttributeString(entry.Key, entry.Value, model, template));
                }
                catch (ExpressionException e)
                {
                    throw new JadeCompilerException(this, template.getTemplateLoader(), e);
                }
            }

            return sb.ToString();
        }

        private String getAttributeString(String name, Object attribute, JadeModel model, JadeTemplate template)// throws ExpressionException
        {
            String value = null;
            if (attribute is String) {
                value = getInterpolatedAttributeValue(name, attribute, model, template);
            } else if (attribute is bool) {
                if ((bool)attribute)
                {
                    value = name;
                }
                else
                {
                    return "";
                }
                if (template.isTerse())
                {
                    value = null;
                }
            } else if (attribute is ExpressionString) {
                Object expressionValue = evaluateExpression((ExpressionString)attribute, model);
                if (expressionValue == null)
                {
                    return "";
                }
                // TODO: refactor
                if (expressionValue is bool) {
                    if ((bool)expressionValue)
                    {
                        value = name;
                    }
                    else
                    {
                        return "";
                    }
                    if (template.isTerse())
                    {
                        value = null;
                    }
                } else {
                    value = expressionValue.ToString();
                    value = Utils.escapeHTML(value);
                }
            } else {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            if (name != null)
            {
                sb.Append(" ").Append(name);
                if (value != null)
                {
                    sb.Append("=").Append('"');
                    sb.Append(value);
                    sb.Append('"');
                }
            }
            return sb.ToString();
        }

        private Object evaluateExpression(ExpressionString attribute, JadeModel model) //throws ExpressionException
        {
            String expression = ((ExpressionString)attribute).getValue();
            Object result = ExpressionHandler.evaluateExpression(expression, model);
            if (result is ExpressionString) {
                return evaluateExpression((ExpressionString)result, model);
            }
            return result;
        }

        private String getInterpolatedAttributeValue(String name, Object attribute, JadeModel model, JadeTemplate template)
            //throws JadeCompilerException
        {
            if (!preparedAttributeValues.ContainsKey(name))
            {
                preparedAttributeValues.Add(name, Utils.prepareInterpolate((String)attribute, true));
            }
            List<Object> prepared = preparedAttributeValues[name];
            try
            {
                return Utils.interpolate(prepared, model);
            }
            catch (ExpressionException e)
            {
                throw new JadeCompilerException(this, template.getTemplateLoader(), e);
            }
        }

        public void setSelfClosing(bool selfClosing) {
            this.selfClosing = selfClosing;
        }

        public bool isSelfClosing() {
            return selfClosing;
        }
    }

}
