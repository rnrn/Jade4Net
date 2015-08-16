using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Jade.Compiler;
using Jade.Exceptions;
using Jade.Lexer.Tokens;
using Template;
using Attribute = Jade.Lexer.Tokens.Attribute;

namespace Jade.Lexer
{
    public class JadeLexer
    {
        private LinkedList<String> options;
        JadeScanner _jadeScanner;
        private LinkedList<Token> deferredTokens;
        private int lastIndents = -1;
        private int lineno;
        private LinkedList<Token> stash;
        private LinkedList<int> indentStack;
        private String indentRe = null;
        private bool pipeless = false;
        //private bool attributeMode;
        private TextReader reader;
        private readonly String filename;
    private readonly TemplateLoader templateLoader;
    private String indentType;

        public JadeLexer(String filename, TemplateLoader templateLoader)// throws IOException
        {
        this.filename = ensureJadeExtension(filename);
        this.templateLoader = templateLoader;
            reader = templateLoader.getReader(this.filename);
            options = new LinkedList<String>();
            _jadeScanner = new JadeScanner(reader);
            deferredTokens = new LinkedList<Token>();
            stash = new LinkedList<Token>();
            indentStack = new LinkedList<int>();
            lastIndents = 0;
            lineno = 1;
        }

        public Token next()
        {
            handleBlankLines();
            Token token = null;
            if ((token = deferred()) != null)
            {
                return token;
            }

            if ((token = eos()) != null)
            {
                return token;
            }

            if ((token = pipelessText()) != null)
            {
                return token;
            }

            if ((token = yield()) != null)
            {
                return token;
            }

            if ((token = doctype()) != null)
            {
                return token;
            }

            if ((token = caseToken()) != null)
            {
                return token;
            }

            if ((token = when()) != null)
            {
                return token;
            }

            if ((token = defaultToken()) != null)
            {
                return token;
            }

            if ((token = extendsToken()) != null)
            {
                return token;
            }

            if ((token = append()) != null)
            {
                return token;
            }

            if ((token = prepend()) != null)
            {
                return token;
            }

            if ((token = block()) != null)
            {
                return token;
            }

            if ((token = include()) != null)
            {
                return token;
            }

            if ((token = mixin()) != null)
            {
                return token;
            }

            if ((token = mixinInject()) != null)
            {
                return token;
            }

            if ((token = conditional()) != null)
            {
                return token;
            }

            if ((token = each()) != null)
            {
                return token;
            }

            if ((token = whileToken()) != null)
            {
                return token;
            }

            if ((token = assignment()) != null)
            {
                return token;
            }

            if ((token = tag()) != null)
            {
                return token;
            }

            if ((token = filter()) != null)
            {
                return token;
            }

            if ((token = code()) != null)
            {
                return token;
            }

            if ((token = id()) != null)
            {
                return token;
            }

            if ((token = className()) != null)
            {
                return token;
            }

            if ((token = attributes()) != null)
            {
                return token;
            }

            if ((token = indent()) != null)
            {
                return token;
            }

            if ((token = comment()) != null)
            {
                return token;
            }

            if ((token = colon()) != null)
            {
                return token;
            }

            if ((token = dot()) != null)
            {
                return token;
            }

            if ((token = text()) != null)
            {
                return token;
            }

            throw new JadeLexerException("token not recognized " + _jadeScanner.getInput().Substring(0, 5), filename, getLineno(),
                        templateLoader);
        }


        public void handleBlankLines()
        {
            while (_jadeScanner.isAdditionalBlankline())
            {
                consume(1);
                lineno++;
            }
        }

        public void consume(int len)
        {
            _jadeScanner.consume(len);
        }

        public void defer(Token tok)
        {
            deferredTokens.AddLast(tok);
        }

        public Token lookahead(int n)
        {
            int fetch = n - stash.Count;
            while (fetch > 0)
            {
                stash.AddLast(next());
                fetch = fetch - 1;
            }
            n = n - 1;
            return this.stash.ElementAt(n);
        }

        public int getLineno()
        {
            return lineno;
        }

        public void setPipeless(bool pipeless)
        {
            this.pipeless = pipeless;
        }

        public Token advance()
        {
            Token t = this.stashed();
            return t != null ? t : next();
        }

        // TODO: use multiscan?!
        private String scan(String regexp)
        {
            String result = null;
            var matcher = _jadeScanner.getMatcherForPattern(regexp);
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 1)
            {
                int end = matcher[0].Length;
                consume(end);
                return matcher[0].Groups[1].Value;
            }
            return result;
        }

        // private int indexOfDelimiters(char start, char end) {
        // String str = JadeScanner.getInput();
        // int nstart = 0;
        // int nend = 0;
        // int pos = 0;
        // for (int i = 0, len = str.Length; i < len; ++i) {
        // if (start == str[i]) {
        // nstart++;
        // } else if (end == str[i]) {
        // nend = nend + 1;
        // if (nend == nstart) {
        // pos = i;
        // break;
        // }
        // }
        // }
        // return pos;
        // }

        private Token stashed()
        {
            if (stash.Count > 0)
            {
                return stash.poll();
            }
            return null;
        }

        private Token deferred()
        {
            if (deferredTokens.Count > 0)
            {
                return deferredTokens.poll();
            }
            return null;
        }

        private Token eos()
        {
            if (_jadeScanner.getInput().Length > 0)
            {
                return null;
            }
            if (indentStack.Count > 0)
            {
                indentStack.poll();
                return new Outdent("outdent", lineno);
            }
            else
            {
                return new Eos("eos", lineno);
            }
        }

        private Token comment()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^ *\\/\\/(-)?([^\\n]*)");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 1)
            {
                bool buffer = !"-".Equals(matcher[0].Groups[1].Value);
                Comment comment = new Comment(matcher[0].Groups[2].Value.Trim(), lineno, buffer);
                consume(matcher[0].Length);
                return comment;
            }
            return null;
        }

        private Token code()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^(!?=|-)([^\\n]+)");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 1)
            {
                Tokens.Expression code = new Tokens.Expression(matcher[0].Groups[2].Value, lineno);
                String type = matcher[0].Groups[1].Value;
                code.setEscape(type.Equals("="));
                code.setBuffer(type.Equals("=") || type.Equals("!="));

                consume(matcher[0].Length);
                return code;
            }
            return null;
        }

        // code: function() {
        // var captures;
        // if (captures = /^(!?=|-)([^\n]+)/.exec(this.input)) {
        // this.consume(captures[0].length);
        // var flags = captures[1];
        // captures[1] = captures[2];
        // var tok = this.tok('code', captures[1]);
        // tok.escape = flags[0] === '=';
        // tok.buffer = flags[0] === '=' || flags[1] === '=';
        // return tok;
        // }
        // },

        private Token tag()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^(\\w[-:\\w]*)(\\/?)");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 0)
            {
                consume(matcher[0].Length);
                Tag tok;
                String name = matcher[0].Groups[1].Value;
                if (':' == name[name.Length - 1])
                {
                    name = name.Substring(0, name.Length - 1);
                    tok = new Tag(name, lineno);
                    this.defer(new Colon(":", lineno));
                    while (' ' == _jadeScanner.getInput()[0])
                        _jadeScanner.consume(1);
                }
                else
                {
                    tok = new Tag(name, lineno);
                }
                if (matcher[0].Groups.Count > 1 && matcher[0].Groups[2].Value.Length > 0)
                {
                    tok.setSelfClosing(true);
                }
                return tok;
            }
            return null;
        }

        private Token yield()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^yield *");
            if (matcher.Count > 0 && matcher[0].Success)
            {
                int end = matcher[0].Length;
                consume(end);
                return new Yield("yield", lineno);
            }
            return null;
        }

        private Token filter()
        {
            String val = scan("^:(\\w+)");
            if (StringUtils.isNotBlank(val))
            {
                return new Tokens.Filter(val, lineno);
            }
            return null;
        }

        private Token each()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^(?:- *)?(?:each|for) +(\\w+)(?: *, *(\\w+))? * in *([^\\n]+)");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 1)
            {
                consume(matcher[0].Length);
                String value = matcher[0].Groups[1].Value;
                String key = matcher[0].Groups[2].Value;
                String code = matcher[0].Groups[3].Value;
                Each each = new Each(value, lineno);
                each.setCode(code);
                each.setKey(key);
                return each;
            }
            return null;
            /*
             * if (captures = /^(?:- *)?(?:each|for) +(\w+)(?: *, *(\w+))? * in
             * *([^\n]+)/.exec(this.input)) { this.consume(captures[0].length); var
             * tok = this.tok('each', captures[1]); tok.key = captures[2] ||
             * '$index'; tok.code = captures[3]; return tok; }
             */
        }

        private Token whileToken()
        {
            String val = scan("^while +([^\\n]+)");
            if (StringUtils.isNotBlank(val))
            {
                return new While(val, lineno);
            }
            return null;
        }

        private Token conditional()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^(if|unless|else if|else)\\b([^\\n]*)");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 1)
            {
                String type = matcher[0].Groups[1].Value;
                String condition = matcher[0].Groups[2].Value;
                consume(matcher[0].Length);
                if ("else".Equals(type))
                {
                    return new Else(null, lineno);
                }
                else if ("else if".Equals(type))
                {
                    return new ElseIf(condition, lineno);
                }
                else
                {
                    If ifToken = new If(condition, lineno);
                    ifToken.setInverseCondition("unless".Equals(type));
                    return ifToken;
                }
            }
            return null;
        }

        /*
         * private Token conditionalElse() { String val = scan("^(else)"); if
         * (StringUtils.isNotBlank(val)) { return new Filter(val, lineno); } return
         * null; }
         */

        private Token doctype()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^(?:!!!|doctype) *([^\\n]+)?");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 0)
            {
                consume(matcher[0].Length);
                return new Doctype(matcher[0].Groups[1].Value, lineno);
            }
            return null;
        }

        private Token id()
        {
            String val = scan("^#([\\w-]+)");
            if (StringUtils.isNotBlank(val))
            {
                return new CssId(val, lineno);
            }
            return null;
        }

        private Token className()
        {
            String val = scan("^\\.([\\w-]+)");
            if (StringUtils.isNotBlank(val))
            {
                return new CssClass(val, lineno);
            }
            return null;
        }

        private Token text()
        {
            String val = scan("^(?:\\| ?| ?)?([^\\n]+)");
            if (StringUtils.isNotEmpty(val))
            {
                return new Text(val, lineno);
            }
            return null;
        }

        private Token extendsToken()
        {
            String val = scan("^extends? +([^\\n]+)");
            if (StringUtils.isNotBlank(val))
            {
                return new ExtendsToken(val, lineno);
            }
            return null;
        }

        private Token prepend()
        {
            String name = scan("^prepend +([^\\n]+)");
            if (StringUtils.isNotBlank(name))
            {
                Block tok = new Block(name, lineno);
                tok.setMode("prepend");
                return tok;
            }
            return null;
        }

        private Token append()
        {
            String name = scan("^append +([^\\n]+)");
            if (StringUtils.isNotBlank(name))
            {
                Block tok = new Block(name, lineno);
                tok.setMode("append");
                return tok;
            }
            return null;
        }

        private Token block()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^block\\b *(?:(prepend|append) +)?([^\\n]*)");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 1)
            {
                String val = matcher[0].Groups[1].Value;
                String mode = StringUtils.isNotBlank(val) ? val : "replace";
                String name = matcher[0].Groups[2].Value;
                Block tok = new Block(name, lineno);
                tok.setMode(mode);
                consume(matcher[0].Length);
                return tok;
            }
            return null;
        }

        private Token include()
        {
            String val = scan("^include +([^\\n]+)");
            if (StringUtils.isNotBlank(val))
            {
                return new Include(val, lineno);
            }
            return null;
        }

        private Token caseToken()
        {
            String val = scan("^case +([^\\n]+)");
            if (StringUtils.isNotBlank(val))
            {
                return new CaseToken(val, lineno);
            }
            return null;
        }

        private Token when()
        {
            String val = scan("^when +([^:\\n]+)");
            if (StringUtils.isNotBlank(val))
            {
                return new When(val, lineno);
            }
            return null;
        }

        private Token defaultToken()
        {
            String val = scan("^(default *)");
            if (StringUtils.isNotBlank(val))
            {
                return new Default(val, lineno);
            }
            return null;
        }

        private Token assignment()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^(\\w+) += *([^;\\n]+)( *;? *)");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 1)
            {
                String name = matcher[0].Groups[1].Value;
                String val = matcher[0].Groups[2].Value;
                consume(matcher[0].Length);
                Assignment assign = new Assignment(val, lineno);
                assign.setName(name);
                return assign;
            }
            return null;
        }

        private Token dot()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^\\.");
            if (matcher.Count > 0 && matcher[0].Length > 0)
            {
                Dot tok = new Dot(".", lineno);
                consume(matcher[0].Length);
                return tok;
            }
            return null;
        }

        private Token mixin()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^mixin +([-\\w]+)(?: *\\((.*)\\))?");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 1)
            {
                Mixin tok = new Mixin(matcher[0].Groups[1].Value, lineno);
                tok.setArguments(matcher[0].Groups[2].Value);
                consume(matcher[0].Length);
                return tok;
            }
            return null;
        }

        private Token mixinInject()
        {
            var matcher = _jadeScanner.getMatcherForPattern("^\\+([-\\w]+)");
            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 0)
            {
                MixinInject tok = new MixinInject(matcher[0].Groups[1].Value, lineno);
                consume(matcher[0].Length);

                matcher = _jadeScanner.getMatcherForPattern("^ *\\((.*?)\\)");

                if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 0)
                {
                    // verify group does not contain attributes
                    var attributeRegex = new Regex("^ *[-\\w]+ *=");
                    var attributeMatcher = attributeRegex.Matches(matcher[0].Groups[1].Value);
                    if (attributeMatcher.Count < 1)
                    {
                        tok.setArguments(matcher[0].Groups[1].Value);
                        consume(matcher[0].Length);
                    }
                }
                return tok;
            }
            return null;
        }

        private Token attributes()
        {
            if ('(' != _jadeScanner.charAt(0))
            {
                return null;
            }

            int index = indexOfDelimiters('(', ')');
            if (index == 0)
            {
                throw new JadeLexerException("invalid attribute definition; missing )", filename, getLineno(), templateLoader);
            }
            String str = _jadeScanner.getInput().Substring(1, index - 1);
            consume(index + 1);

            Tokens.Attribute attribute = new AttributeLexer().getToken(str, lineno);

            if (_jadeScanner.getInput()[0] == '/')
            {
                consume(1);
                attribute.setSelfClosing(true);
            }

            return attribute;
        }

        private int indexOfDelimiters(char start, char end)
        {
            String str = _jadeScanner.getInput();
            int nstart = 0;
            int nend = 0;
            int pos = 0;
            for (int i = 0, len = str.Length; i < len; i++)
            {
                if (start == str[i])
                {
                    nstart++;
                }
                else if (end == str[i])
                {
                    if (++nend == nstart)
                    {
                        pos = i;
                        break;
                    }
                }
            }
            return pos;
        }

        /*
         * private Token attributes() { Attribute tok = new Attribute(null, lineno);
         * var matcher = JadeScanner.getMatcherForPattern("^\\("); if
         * (matcher.find(0)) { consume(matcher[0].Length); attributeMode = true; } else
         * { return null; }
         * 
         * StringBuilder sb = new StringBuilder(); String regexp =
         * "^[, ]*?([-_\\w]+)? *?= *?(\"[^\"]*?\"|'[^']*?'|[.-_\\w]+)"; matcher =
         * JadeScanner.getMatcherForPattern(regexp); if (matcher.find(0)) { while
         * (matcher.find(0)) { String name = matcher[0].Groups[1].Value; String value =
         * matcher[0].Groups[2].Value; tok.addAttribute(name, value);
         * sb.append(matcher.group(0)); consume(matcher[0].Length); matcher =
         * JadeScanner.getMatcherForPattern(regexp); } tok.setValue(sb.toString()); }
         * else { return null; }
         * 
         * matcher = JadeScanner.getMatcherForPattern("^ *?\\)"); if (matcher.find(0)) {
         * consume(matcher[0].Length); attributeMode = false; } else { throw new
         * JadeLexerException
         * ("Error while parsing attribute. Missing closing bracket ", filename,
         * getLineno(), JadeScanner.getInput()); } return tok; }
         */

        private Token indent()
        {
            MatchCollection matcher;
            String re;

            if (indentRe != null)
            {
                matcher = _jadeScanner.getMatcherForPattern(indentRe);
            }
            else
            {
                // tabs
                re = "^\\n(\\t*) *";
                String indentType = "tabs";
                matcher = _jadeScanner.getMatcherForPattern(re);

                // spaces
                if (matcher.Count>0 && matcher[0].Groups[1].Value.Length == 0)
                {
                    re = "^\\n( *)";
                    indentType = "spaces";
                    matcher = _jadeScanner.getMatcherForPattern(re);
                }

                // established
                if (matcher.Count > 0 && matcher[0].Groups[1].Value.Length > 0)
                    this.indentRe = re;
                this.indentType = indentType;
            }

            if (matcher.Count > 0 && matcher[0].Success && matcher[0].Groups.Count > 0)
            {
                Token tok;
                int indents = matcher[0].Groups[1].Value.Length;
                if (lastIndents <= 0 && indents > 0)
                    lastIndents = indents;
                lineno++;
                consume(indents + 1);

                if ((indents > 0 && lastIndents > 0 && indents % lastIndents != 0) || _jadeScanner.isIntendantionViolated())
                {
                    throw new JadeLexerException("invalid indentation; expecting " + indents + " " + indentType, filename, getLineno(), templateLoader);
                }

                // blank line
                if (_jadeScanner.isBlankLine())
                {
                    return new Newline("newline", lineno);
                }

                // outdent
                if (indentStack.Count > 0 && indents < indentStack.First.Value)
                {
                    while (indentStack.Count > 0 && indentStack.First.Value > indents)
                    {
                        indentStack.poll();
                    }
                    stash.AddLast(new Outdent("outdent", lineno));
                    tok = this.stash.pollLast();
                    // indent
                }
                else if (indents > 0 && (indentStack.Count < 1 || indents != indentStack.First.Value))
                {
                    indentStack.AddLast(indents);
                    tok = new Indent("indent", lineno);
                    tok.setIndents(indents);
                    // newline
                }
                else
                {
                    tok = new Newline("newline", lineno);
                }

                return tok;
            }
            return null;
        }

        private Token pipelessText()
        {
            if (this.pipeless)
            {
                if ('\n' == _jadeScanner.getInput()[0])
                    return null;
                int i = _jadeScanner.getInput().IndexOf('\n');
                if (-1 == i)
                    i = _jadeScanner.getInput().Length;
                String str = _jadeScanner.getInput().Substring(0, i);
                consume(str.Length);
                return new Text(str, lineno);
            }
            return null;
        }

        private Token colon()
        {
            String val = scan("^(: *)");
            if (StringUtils.isNotBlank(val))
            {
                return new Colon(val, lineno);
            }
            return null;
        }

        private String ensureJadeExtension(String templateName)
        {
            if (StringUtils.isBlank(Path.GetExtension(templateName)))
            {
                return templateName + ".jade";
            }
            return templateName;
        }

        public bool getPipeless()
        {
            return pipeless;
        }
    }

    static class Extension
    {
        public static T poll<T>(this LinkedList<T> list)
        {
            var last = list.First.Value;
            list.RemoveFirst();
            return last;
        }

        public static T pollLast<T>(this LinkedList<T> list)
        {
            if (list.Count == 0)
                return default(T);

            var last = list.Last.Value;
            list.RemoveLast();
            return last;
        }
    }
}
