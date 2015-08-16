using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Attribute= Jade.Lexer.Tokens.Attribute;

namespace Jade.Lexer
{
    public class AttributeLexer
    {

        public enum State
        {
            KEY, KEY_CHAR, VALUE, EXPRESSION, ARRAY, STRING, OBJECT
        }

        /*
         * len = str.length , colons = this.colons , states = ['key'] , key = '' ,
         * val = '' , quote , c;
         */

        private String key = "";
        private String value = "";
        private Tokens.Attribute token;
        private LinkedList<State> states = new LinkedList<State>();
        private char quote = ' ';

        public AttributeLexer()
        {
            states.AddLast(State.KEY);
        }

        public Tokens.Attribute getToken(String input, int lineno)
        {
            token = new Tokens.Attribute(input, lineno);
            for (int i = 0; i < input.Length; i++)
            {
                parse(input[i]);
            }
            parse(',');
            return token;
        }

        private State state()
        {
            return states.First();
        }

        private void parse(char c)
        {
            char real = c;
            switch (c)
            {
                case ',':
                case '\n':
                    switch (state())
                    {
                        case State.EXPRESSION:
                        case State.ARRAY:
                        case State.STRING:
                        case State.OBJECT:
                            value += c;
                            break;
                        default:
                            states.AddLast(State.KEY);
                            value = value.Trim();
                            key = key.Trim();
                            if ("".Equals(key))
                            {
                                return;
                            }
                            String name = replaceAll(key, "^['\"]|['\"]$", "");
                            String cleanValue = replaceAll(value, "^['\"]|['\"]$", "");

                            if ("".Equals(cleanValue) && quote == ' ')
                            {
                                token.addBooleanAttribute(name, true);
                            }
                            else if (matches(value, "^\"[^\"]*\"$") || matches(value, "^'[^']*'$"))
                            {
                                token.addAttribute(name, cleanValue);
                            }
                            else
                            {
                                token.addExpressionAttribute(name, value);
                            }
                            key = "";
                            value = "";
                            quote = ' ';
                            break;
                    }
                    break;
                case '=':
                    parseAssign(real);
                    break;
                case '(':
                    parseExpressionStart(c);
                    break;
                case ')':
                    parseExpressionEnd(c);
                    break;
                case '{':
                    parseObjectStart(c);
                    break;
                case '}':
                    parseObjectEnd(c);
                    break;
                case '[':
                    parseArrayStart(c);
                    break;
                case ']':
                    parseArrayEnd(c);
                    break;
                case '"':
                case '\'':
                    parseQuotes(c);
                    break;
                default:
                    parseDefaults(c);
                    break;
            }
        }

        private bool matches(string input, string pattern)
        {
            var regex = new Regex(pattern);
            return regex.Matches(input).Count > 0;
        }

        private string replaceAll(string input, string pattern, string sample)
        {
            var regex = new Regex(pattern);
        input = regex.Replace(input, sample);
            return input;
        }

        private void parseAssign(char real)
        {
            switch (state())
            {
                case State.KEY_CHAR:
                    key += real;
                    break;
                case State.VALUE:
                case State.EXPRESSION:
                case State.ARRAY:
                case State.STRING:
                case State.OBJECT:
                    value += real;
                    break;
                default:
                    states.AddLast(State.VALUE);
                    break;
            }
        }

        private void parseExpressionStart(char c)
        {
            if (state() == State.VALUE || state() == State.EXPRESSION)
            {
                states.AddLast(State.EXPRESSION);
            }
            value += c;
        }

        private void parseExpressionEnd(char c)
        {
            if (state() == State.VALUE || state() == State.EXPRESSION)
            {
                states.RemoveLast();
            }
            value += c;
        }

        private void parseObjectStart(char c)
        {
            if (state() == State.VALUE)
            {
                states.AddLast(State.OBJECT);
            }
            value += c;
        }

        private void parseObjectEnd(char c)
        {
            if (state() == State.OBJECT)
            {
                states.RemoveLast();
            }
            value += c;
        }

        private void parseArrayStart(char c)
        {
            if (state() == State.VALUE)
            {
                states.AddLast(State.ARRAY);
            }
            value += c;
        }

        private void parseArrayEnd(char c)
        {
            if (state() == State.ARRAY)
            {
                states.RemoveLast();
            }
            value += c;
        }

        private void parseQuotes(char c)
        {
            switch (state())
            {
                case State.KEY:
                    states.AddLast(State.KEY_CHAR);
                    break;
                case State.KEY_CHAR:
                    states.RemoveLast();
                    break;
                case State.STRING:
                    if (c == quote)
                    {
                        states.RemoveLast();
                    }
                    value += c;
                    break;
                default:
                    states.AddLast(State.STRING);
                    value += c;
                    quote = c;
                    break;
            }
        }

        private void parseDefaults(char c)
        {
            switch (state())
            {
                case State.KEY:
                case State.KEY_CHAR:
                    key += c;
                    break;
                default:
                    value += c;
                    break;
            }
        }
    }

}
