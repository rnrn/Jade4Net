using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Jade.Expression;
using Jade.Filters;
using Jade.Model;
using Jade.Parser;
using Jade.Parser.Nodes;
using Jade.Template;
using Template;

namespace Jade
{
    public class JadeConfiguration
    {

        private static readonly String FILTER_CDATA = "cdata";
        private static readonly String FILTER_STYLE = "css";
        private static readonly String FILTER_SCRIPT = "js";

        private bool prettyPrint = false;
        private bool caching = true;
        private Jade4Net.Mode mode = Jade4Net.Mode.HTML;

        private Dictionary<String, Filter> filters = new Dictionary<String, Filter>();
        private Dictionary<String, Object> sharedVariables = new Dictionary<String, Object>();
        private TemplateLoader templateLoader = new FileTemplateLoader("", "UTF-8");
        protected static readonly int MAX_ENTRIES = 1000;

        public JadeConfiguration()
        {
            setFilter(FILTER_CDATA, new CDATAFilter());
            setFilter(FILTER_SCRIPT, new JsFilter());
            setFilter(FILTER_STYLE, new CssFilter());
        }

        private ConcurrentDictionary<String, JadeTemplate> cache = new ConcurrentDictionary<String, JadeTemplate>();

        public JadeTemplate getTemplate(String name) //throws IOException, JadeException
        {
            if (caching)
            {
                long lastModified = templateLoader.getLastModified(name);
                String key = name + "-" + lastModified;

                JadeTemplate template = cache[key];

                if (template != null)
                {
                    return template;
                }
                else
                {
                    JadeTemplate newTemplate = createTemplate(name);
                    cache.AddOrUpdate(key, newTemplate, (s, jadeTemplate) => newTemplate);
                    return newTemplate;
                }
            }

            return createTemplate(name);
        }

        public void renderTemplate(JadeTemplate template, Dictionary<String, Object> model, TextWriter writer)
            //throws JadeCompilerException
        {
            JadeModel jadeModel = new JadeModel(sharedVariables);
            foreach (String filterName in filters.Keys)
            {
                jadeModel.addFilter(filterName, filters[filterName]);
            }
            jadeModel.putAll(model);
            template.process(jadeModel, writer);
        }

        public String renderTemplate(JadeTemplate template, Dictionary<String, Object> model)
            //throws JadeCompilerException
        {
            StringWriter writer = new StringWriter();
            renderTemplate(template, model, writer);
            return writer.ToString();
        }

        private JadeTemplate createTemplate(String name)
            //throws JadeException, IOException
        {
            JadeTemplate template = new JadeTemplate();

            JadeParser parser = new JadeParser(name, templateLoader);
            Node root = parser.parse();
            template.setTemplateLoader(templateLoader);
            template.setRootNode(root);
            template.setPrettyPrint(prettyPrint);
            template.setMode(getMode());
            return template;
        }

        public bool isPrettyPrint()
        {
            return prettyPrint;
        }

        public void setPrettyPrint(bool prettyPrint)
        {
            this.prettyPrint = prettyPrint;
        }

        public void setFilter(String name, Filter filter)
        {
            filters.Add(name, filter);
        }

        public void removeFilter(String name)
        {
            filters.Remove(name);
        }

        public Dictionary<String, Object> getSharedVariables()
        {
            return sharedVariables;
        }

        public void setSharedVariables(Dictionary<String, Object> sharedVariables)
        {
            this.sharedVariables = sharedVariables;
        }

        public TemplateLoader getTemplateLoader()
        {
            return templateLoader;
        }

        public void setTemplateLoader(TemplateLoader templateLoader)
        {
            this.templateLoader = templateLoader;
        }

        public Jade4Net.Mode getMode()
        {
            return mode;
        }

        public void setMode(Jade4Net.Mode mode)
        {
            this.mode = mode;
        }

        public bool templateExists(String url)
        {
            try
            {
                return templateLoader.getReader(url) != null;
            }
            catch (IOException e)
            {
                return false;
            }
        }

        public bool isCaching()
        {
            return caching;
        }

        public void setCaching(bool cache)
        {
            if (cache != this.caching)
            {
                ExpressionHandler.setCache(cache);
                this.caching = cache;
            }
        }

        public void clearCache()
        {
            ExpressionHandler.clearCache();
            cache.Clear();
        }
    }

}
