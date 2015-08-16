using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Lexer;
using Jade.Lexer.Tokens;
using Jade.Parser.Nodes;
using Jade.Util;
using Template;
using Attribute = Jade.Lexer.Tokens.Attribute;

namespace Jade.Parser
{
    public class JadeParser
    {

        public static readonly Regex FILE_EXTENSION_PATTERN = new Regex(".*\\.\\w+$");
        private JadeLexer _jadeLexer;
        private Dictionary<String, Node> blocks = new Dictionary<String, Node>();
        private String[] textOnlyTags = {"script", "style"};
        private int? _spaces = null;
        private readonly TemplateLoader templateLoader;
        private JadeParser extending;
        private readonly String filename;
        private LinkedList<JadeParser> contexts = new LinkedList<JadeParser>();

        public JadeParser(String filename, TemplateLoader templateLoader) //throws IOException
        {
            this.filename = filename;
            this.templateLoader = templateLoader;
            _jadeLexer = new JadeLexer(filename, templateLoader);
            getContexts().AddLast(this);
        }

        public Node parse()
        {
            BlockNode block = new BlockNode();
            block.setLineNumber(_jadeLexer.getLineno());
            block.setFileName(filename);
            while (!(peek() is Eos))
            {
                if (peek() is Newline)
                {
                    nextToken();
                }
                else
                {
                    Node expr = parseExpr();
                    if (expr != null)
                    {
                        block.push(expr);
                    }
                }
            }
            if (extending != null)
            {
                getContexts().AddLast(extending);
                Node rootNode = extending.parse();
                getContexts().RemoveLast();
                return rootNode;
            }

            return block;
        }

        private Node parseExpr()
        {
            Token token = peek();
            if (token is Tag)
            {
                return parseTag();
            }
            if (token is Mixin)
            {
                return parseMixin();
            }
            if (token is MixinInject)
            {
                return parseMixinInject();
            }
            if (token is Block)
            {
                return parseBlock();
            }
            if (token is ExtendsToken)
            {
                return parseExtends();
            }
            if (token is Include)
            {
                return parseInclude();
            }
            if (token is Filter)
            {
                return parseFilter();
            }
            if (token is Comment)
            {
                return parseComment();
            }
            if (token is Text)
            {
                return parseText();
            }
            if (token is Each)
            {
                return parseEach();
            }
            if (token is While)
            {
                return parseWhile();
            }
            if (token is CssClass || token is CssId)
            {
                return parseCssClassOrId();
            }
            if (token is If)
            {
                return parseConditional();
            }
            if (token is CaseToken)
            {
                return parseCase();
            }
            if (token is Assignment)
            {
                return parseAssignment();
            }
            if (token is Doctype)
            {
                return parseDoctype();
            }
            if (token is Lexer.Tokens.Expression)
            {
                return parseCode();
            }
            if (token is Yield)
            {
                return parseYield();
            }
            throw new JadeParserException(filename, _jadeLexer.getLineno(), templateLoader, token);
        }

        private Node parseComment()
        {
            Token token = expect(typeof (Comment));

            CommentNode node;
            if (peek() is Indent)
            {
                node = new BlockCommentNode();
                node.setBlock(block());
            }
            else
            {
                node = new CommentNode();
            }
            node.setBuffered(token.isBuffer());
            node.setLineNumber(token.getLineNumber());
            node.setFileName(filename);
            node.setValue(token.getValue());

            return node;
        }

        private Node parseMixin()
        {
            Mixin mixinToken = (Mixin) expect(typeof (Mixin));
            MixinNode node = new MixinNode();
            node.setName(mixinToken.getValue());
            node.setLineNumber(mixinToken.getLineNumber());
            node.setFileName(filename);
            if (StringUtils.isNotBlank(mixinToken.getArguments()))
            {
                node.setArguments(mixinToken.getArguments());
            }
            if (peek() is Indent)
            {
                node.setBlock(block());
            }
            return node;
        }

        private Node parseMixinInject()
        {
            Token token = expect(typeof (MixinInject));
            MixinInject mixinInjectToken = (MixinInject) token;
            MixinInjectNode node = new MixinInjectNode();
            node.setName(mixinInjectToken.getValue());
            node.setLineNumber(mixinInjectToken.getLineNumber());
            node.setFileName(filename);

            if (StringUtils.isNotBlank(mixinInjectToken.getArguments()))
            {
                node.setArguments(mixinInjectToken.getArguments());
            }

            while (true)
            {
                Token incomingToken = peek();
                if (incomingToken is CssId)
                {
                    Token tok = nextToken();
                    node.addAttribute("id", tok.getValue());
                }
                else if (incomingToken is CssClass)
                {
                    Token tok = nextToken();
                    node.addAttribute("class", tok.getValue());
                }
                else if (incomingToken is Attribute)
                {
                    Attribute tok = (Attribute) nextToken();
                    node.addAttributes(tok.getAttributes());
                }
                else
                {
                    break;
                }
            }

            if (peek() is Text)
            {
                node.setBlock(parseText());
            }
            else if (peek() is Indent)
            {
                node.setBlock(block());
            }
            return node;
        }

        private Node parseCssClassOrId()
        {
            Token tok = nextToken();
            Tag div = new Tag("div", line());
            _jadeLexer.defer(div);
            _jadeLexer.defer(tok);
            return parseExpr();
        }

        private Node parseBlock()
        {
            Token token = expect(typeof (Block));
            Block blockToken = (Block) token;
            String mode = blockToken.getMode();
            String name = blockToken.getValue().Trim();

            Node blockNode;
            if (peek() is Indent)
            {
                blockNode = block();
            }
            else
            {
                blockNode = new BlockNode();
                blockNode.setLineNumber(blockToken.getLineNumber());
                blockNode.setFileName(filename);
            }

            ((BlockNode) blockNode).setMode(mode);

            if (blocks.ContainsKey(name))
            {
                BlockNode prev = (BlockNode) blocks[name];
                if ("append".Equals(prev.getMode()))
                {
                    blockNode.getNodes().AppendRange(prev.getNodes());
                }
                if ("prepend".Equals(prev.getMode()))
                {
                    blockNode.getNodes().PrependRange(prev.getNodes());
                }
                if ("replace".Equals(prev.getMode()))
                {
                    blockNode = prev;
                }
            }

            blocks.Add(name, blockNode);
            return blockNode;
        }

        private Node parseInclude()
        {
            Token token = expect(typeof (Include));
            Include includeToken = (Include) token;
            String templateName = includeToken.getValue().Trim();

            String extension = Path.GetExtension(templateName);
            if (!"".Equals(extension) && !"jade".Equals(extension))
            {
                FilterNode node = new FilterNode();
                node.setLineNumber(_jadeLexer.getLineno());
                node.setFileName(filename);
                node.setValue(extension);
                try
                {
                    TextReader reader = templateLoader.getReader(resolvePath(templateName));
                    Node textNode = new TextNode();
                    textNode.setFileName(filename);
                    textNode.setLineNumber(_jadeLexer.getLineno());
                    textNode.setValue(reader.ReadToEnd());
                    node.setTextBlock(textNode);
                }
                catch (IOException e)
                {
                    throw new JadeParserException(filename, _jadeLexer.getLineno(), templateLoader,
                        "the included file [" + templateName + "] could not be opened\n" + e.Message);
                }
                return node;
            }

            JadeParser jadeParser = createParser(templateName);
            jadeParser.setBlocks(blocks);
            contexts.AddLast(jadeParser);
            Node ast = jadeParser.parse();
            contexts.RemoveLast();

            if (peek() is Indent && ast is BlockNode)
            {
                ((BlockNode) ast).getIncludeBlock().push(block());
            }

            return ast;
        }

        private Node parseExtends()
        {
            Token token = expect(typeof (ExtendsToken))
            ;
            ExtendsToken extendsToken = (ExtendsToken) token;
            String templateName = extendsToken.getValue().Trim();

            JadeParser jadeParser = createParser(templateName);

            jadeParser.setBlocks(blocks);
            jadeParser.setContexts(contexts);
            extending = jadeParser;

            LiteralNode node = new LiteralNode();
            node.setValue("");
            return node;
        }

        private JadeParser createParser(String templateName)
        {
            templateName = ensureJadeExtension(templateName);
            try
            {
                return new JadeParser(resolvePath(templateName), templateLoader);
            }
            catch (IOException e)
            {
                throw new JadeParserException(filename, _jadeLexer.getLineno(), templateLoader,
                    "the template [" + templateName
                    + "] could not be opened\n" + e.Message);
            }
        }

        private String ensureJadeExtension(String templateName)
        {
            if (StringUtils.isBlank(Path.GetExtension(templateName)))
            {
                return templateName + ".jade";
            }
            return templateName;
        }

        private String resolvePath(String templateName)
        {
            var fullPath = templateLoader.GetPath();
            templateName = templateName.Replace("/", "\\");
            var templatePath = (fullPath + templateName).Replace("\\\\", "\\");
            return templatePath;
        }

        private BlockNode parseYield()
        {
            nextToken();
            BlockNode block = (BlockNode) new BlockNode();
            block.setLineNumber(_jadeLexer.getLineno());
            block.setFileName(filename);
            block.setYield(true);
            return block;
        }

        private Node blockExpansion()
        {
            if (peek() is Colon)
            {
                Token token = expect(typeof (Colon))
                ;
                Colon colon = (Colon) token;
                BlockNode blockNode = new BlockNode();
                blockNode.setLineNumber(colon.getLineNumber());
                blockNode.setFileName(filename);
                blockNode.getNodes().AddLast(parseExpr());
                return blockNode;
            }
            return block();
        }

        private Node block()
        {
            BlockNode block = new BlockNode();
            block.setLineNumber(_jadeLexer.getLineno());
            block.setFileName(filename);
            expect(typeof (Indent))
            ;
            while (!(peek() is Outdent) && !(peek() is Eos))
            {
                if (peek() is Newline)
                {
                    nextToken();
                }
                else
                {
                    Node parseExpr = this.parseExpr();
                    if (parseExpr != null)
                    {
                        block.push(parseExpr);
                    }
                }
            }
            if (peek() is Outdent)
            {
                expect(typeof (Outdent));
            }
            return block;
        }

        private List<CaseConditionNode> whenBlock()
        {
            expect(typeof (Indent));
            List<CaseConditionNode> caseConditionalNodes = new List<CaseConditionNode>();
            while (!(peek() is Outdent) && !(peek() is Eos))
            {
                if (peek() is Newline)
                {
                    nextToken();
                }
                else
                {
                    caseConditionalNodes.Add(this.parseCaseCondition());
                }
            }
            if (peek() is Outdent)
            {
                expect(typeof (Outdent));
            }
            return caseConditionalNodes;
        }

        private Node parseText()
        {
            Token token = expect(typeof (Text))
            ;
            Node node = new TextNode();
            node.setValue(token.getValue());
            node.setLineNumber(token.getLineNumber());
            node.setFileName(filename);
            return node;
        }

        private Node parseEach()
        {
            Token token = expect(typeof (Each))
            ;
            Each eachToken = (Each) token;
            EachNode node = new EachNode();
            node.setValue(eachToken.getValue());
            node.setKey(eachToken.getKey());
            node.setCode(eachToken.getCode());
            node.setLineNumber(eachToken.getLineNumber());
            node.setFileName(filename);
            node.setBlock(block());
            if (peek() is Else)
            {
                nextToken();
                node.setElseNode(block());
            }
            return node;
        }

        private Node parseWhile()
        {
            Token token = expect(typeof (While))
            ;
            While whileToken = (While) token;
            WhileNode node = new WhileNode();
            node.setValue(whileToken.getValue());
            node.setLineNumber(whileToken.getLineNumber());
            node.setFileName(filename);
            node.setBlock(block());
            return node;
        }

        private Node parseAssignment()
        {
            Token token = expect(typeof (Assignment));
            Token assignmentToken = (Assignment) token;
            Node node = new AssigmentNode();
            node.setName(assignmentToken.getName());
            node.setValue(assignmentToken.getValue());
            node.setLineNumber(assignmentToken.getLineNumber());
            node.setFileName(filename);
            return node;
        }

        private Node parseTag()
        {
            // ast-filter look-ahead
            int i = 2;
            if (lookahead(i) is Attribute)
            {
                i++;
            }
            if (lookahead(i) is Colon)
            {
                i++;
                if (lookahead(i) is Indent)
                {
                    return this.parseASTFilter();
                }
            }
            Token token = nextToken();
            String name = token.getValue();
            TagNode tagNode = new TagNode();
            tagNode.setLineNumber(_jadeLexer.getLineno());
            tagNode.setFileName(filename);
            tagNode.setName(name);
            tagNode.setValue(name);
            tagNode.setSelfClosing(token.isSelfClosing());

            while (true)
            {
                Token incomingToken = peek();
                if (incomingToken is CssId)
                {
                    Token tok = nextToken();
                    tagNode.addAttribute("id", tok.getValue());
                    continue;
                }
                else if (incomingToken is CssClass)
                {
                    Token tok = nextToken();
                    tagNode.addAttribute("class", tok.getValue());
                    continue;
                }
                else if (incomingToken is Attribute)
                {
                    Attribute tok = (Attribute) nextToken();
                    tagNode.addAttributes(tok.getAttributes());
                    tagNode.setSelfClosing(tok.isSelfClosing());
                    continue;
                }
                else
                {
                    break;
                }
            }

            // check immediate '.'
            bool dot = false;
            if (peek() is Dot)
            {
                dot = true;
                tagNode.setTextOnly(true);
                nextToken();
            }

            // (text | code | ':')?
            if (peek() is Text)
            {
                tagNode.setTextNode(parseText());
            }
            else if (peek() is Jade.Lexer.Tokens.Expression)
            {
                tagNode.setCodeNode(parseCode());
            }
            else if (peek() is Colon)
            {
                Token next = nextToken();
                BlockNode block = new BlockNode();
                block.setLineNumber(next.getLineNumber());
                block.setFileName(filename);
                tagNode.setBlock(block);
                block.push(parseExpr());
            }

            // newline*
            while (peek() is Newline)
            {
                nextToken();
            }

            if (!tagNode.isTextOnly())
            {
                if (Array.IndexOf(textOnlyTags, tagNode.getName()) >= 0)
                {
                    tagNode.setTextOnly(true);
                }
            }

            // script special-case
            if ("script".Equals(tagNode.getName()))
            {
                String type = tagNode.getAttribute("type");
                if (!dot && StringUtils.isNotBlank(type))
                {
                    String cleanType = type.replaceAll("^['\"]|['\"]$", "");
                    if (!"text/javascript".Equals(cleanType))
                    {
                        tagNode.setTextOnly(false);
                    }
                }
            }

            if (peek() is Indent)
            {
                if (tagNode.isTextOnly())
                {
                    _jadeLexer.setPipeless(true);
                    tagNode.setTextNode(parseTextBlock());
                    _jadeLexer.setPipeless(false);
                }
                else
                {
                    Node blockNode = block();
                    if (tagNode.hasBlock())
                    {
                        tagNode.getBlock().getNodes().AppendRange(blockNode.getNodes());
                    }
                    else
                    {
                        tagNode.setBlock(blockNode);
                    }
                }
            }

            return tagNode;
        }

        private Node parseTextBlock()
        {
            TextNode textNode = new TextNode();
            textNode.setLineNumber(line());
            textNode.setFileName(filename);
            Token token = expect(typeof (Indent));
            Indent indentToken = (Indent) token;
            int spaces = indentToken.getIndents();
            if (null == this._spaces)
                this._spaces = spaces;
            String indentStr = StringUtils.repeat(" ", spaces - this._spaces.Value);
            while (!(peek() is Outdent))
            {
                if (peek() is Eos)
                    break;
                
                if (peek() is Newline)
                {
                    textNode.appendText("\n");
                    this.nextToken();
                }
                else if (peek() is Indent)
                {
                    textNode.appendText("\n");
                    textNode.appendText(this.parseTextBlock().getValue());
                    textNode.appendText("\n");
                }
                else
                {
                    textNode.appendText(indentStr + this.nextToken().getValue());
                }
            }

            if (spaces == this._spaces)
                this._spaces = null;

            if (peek() is Eos)
                return textNode;
            token = expect(typeof(Outdent));
            return textNode;
        }

        private Node parseConditional()
        {
            If conditionalToken = (If) expect(typeof (If));
            ConditionalNode conditional = new ConditionalNode();
            conditional.setLineNumber(conditionalToken.getLineNumber());
            conditional.setFileName(filename);

            List<IfConditionNode> conditions = conditional.getConditions();

            IfConditionNode main = new IfConditionNode(conditionalToken.getValue(), conditionalToken.getLineNumber());
            main.setInverse(conditionalToken.isInverseCondition());
            main.setBlock(block());
            conditions.Add(main);

            while (peek() is ElseIf)
            {
                ElseIf token = (ElseIf) expect(typeof (ElseIf))
                ;
                IfConditionNode elseIf = new IfConditionNode(token.getValue(), token.getLineNumber());
                elseIf.setBlock(block());
                conditions.Add(elseIf);
            }

            if (peek() is Else)
            {
                Else token = (Else) expect(typeof (Else))
                ;
                IfConditionNode elseNode = new IfConditionNode(null, token.getLineNumber());
                elseNode.setDefault(true);
                elseNode.setBlock(block());
                conditions.Add(elseNode);
            }

            return conditional;
        }

        private Node parseCase()
        {
            Token token = expect(typeof (CaseToken))
            ;
            CaseToken caseToken = (CaseToken) token;
            CaseNode node = new CaseNode();
            node.setLineNumber(caseToken.getLineNumber());
            node.setFileName(filename);
            node.setValue(caseToken.getValue());
            node.setConditions(whenBlock());
            return node;
        }

        private CaseConditionNode parseCaseCondition()
        {
            CaseConditionNode node = new CaseConditionNode();
            Token token = null;
            if (peek() is When)
            {
                token = expect(typeof (When))
                ;
            }
            else
            {
                token = expect(typeof (Default))
                ;
                node.setDefault(true);
            }
            node.setLineNumber(token.getLineNumber());
            node.setFileName(filename);
            node.setValue(token.getValue());
            node.setBlock(blockExpansion());
            return node;
        }

        private Node parseCode()
        {
            Token token = expect(typeof (Lexer.Tokens.Expression));
            Lexer.Tokens.Expression expressionToken = (Lexer.Tokens.Expression) token;
            ExpressionNode codeNode = new ExpressionNode();
            codeNode.setValue(expressionToken.getValue());
            codeNode.setBuffer(expressionToken.isBuffer());
            codeNode.setEscape(expressionToken.isEscape());
            codeNode.setLineNumber(expressionToken.getLineNumber());
            codeNode.setFileName(filename);
            bool blockVal = false;
            int i = 1;
            while (lookahead(i) != null && lookahead(i) is Newline)
                ++i;
            blockVal = lookahead(i) is Indent;
            if (blockVal)
            {
                skip(i - 1);
                codeNode.setBlock((BlockNode) block());
            }
            return codeNode;
        }

        private Node parseDoctype()
        {
            Token token = expect(typeof (Doctype))
            ;
            Doctype doctype = (Doctype) token;
            DoctypeNode doctypeNode = new DoctypeNode();
            doctypeNode.setValue(doctype.getValue());
            return doctypeNode;
        }

        // var tok = this.expect('code')
        // , node = new nodes.Code(tok.val, tok.buffer, tok.escape)
        // , block
        // , i = 1;
        // node.line = this.line();
        // while (this.lookahead(i) && 'newline' == this.lookahead(i).type) ++i;
        // block = 'indent' == this.lookahead(i).type;
        // if (block) {
        // this.skip(i-1);
        // node.block = this.block();
        // }
        // return node;

        private Node parseFilter()
        {
            Token token = expect(GetType())
            ;
            Filter filterToken = (Filter) token;
            Attribute attr = (Attribute) accept(typeof (Attribute))
            ;
            _jadeLexer.setPipeless(true);
            Node tNode = parseTextBlock();
            _jadeLexer.setPipeless(false);

            FilterNode node = new FilterNode();
            node.setValue(filterToken.getValue());
            node.setLineNumber(line());
            node.setFileName(filename);
            node.setTextBlock(tNode);
            if (attr != null)
            {
                node.setAttributes(attr.getAttributes());
            }
            return node;
        }

        private Node parseASTFilter()
        {
            Token token = expect(GetType());
            Filter filterToken = (Filter) token;
            Attribute attr = (Attribute) accept(typeof (Attribute));

            token = expect(typeof (Colon));

            FilterNode node = new FilterNode();
            node.setValue(filterToken.getValue());
            node.setBlock(block());
            node.setLineNumber(line());
            node.setFileName(filename);
            node.setAttributes(attr.getAttributes());
            return node;
        }

        private Token lookahead(int i)
        {
            return _jadeLexer.lookahead(i);
        }

        private Token peek()
        {
            return lookahead(1);
        }

        private void skip(int n)
        {
            while (n > 0)
            {
                _jadeLexer.advance();
                n = n - 1;
            }
        }

        private Token nextToken()
        {
            return _jadeLexer.advance();
        }

        private Token accept(Type clazz)
        {
            if (this.peek().GetType().Equals(clazz))
            {
                return _jadeLexer.advance();
            }
            return null;
        }

        private int line()
        {
            return _jadeLexer.getLineno();
        }

        private Token expect(Type expectedTokenClass)
        {
            Token t = this.peek();
            if (t.GetType() == expectedTokenClass)
            {
                return nextToken();
            }
            else
            {
                throw new JadeParserException(filename, _jadeLexer.getLineno(), templateLoader, expectedTokenClass, t.GetType());
            }
        }

        public Dictionary<String, Node> getBlocks()
        {
            return blocks;
        }

        public void setBlocks(Dictionary<String, Node> blocks)
        {
            this.blocks = blocks;
        }

        public LinkedList<JadeParser> getContexts()
        {
            return contexts;
        }

        public void setContexts(LinkedList<JadeParser> contexts)
        {
            this.contexts = contexts;
        }
    }
}
