using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Jade.Model;
using Jade.Parser;
using Jade.Parser.Nodes;
using Jade.Template;
using Template;

namespace Jade
{
    public class Jade4Net
    {

        public enum Mode
        {
            HTML,
            XML,
            XHTML
        }

        public static String render(String filename, Dictionary<String, Object> model)
            // throws IOException, JadeCompilerException 
        {
            return render(filename, model, false);
        }

        public static String render(String filename, Dictionary<String, Object> model, bool pretty)
            // throws IOException, JadeCompilerException
        {
            JadeTemplate template = getTemplate(filename);
            template.setPrettyPrint(pretty);
            return templateToString(template, model);
        }

        public static void render(String filename, Dictionary<String, Object> model, TextWriter writer)
            // throws IOException, JadeCompilerException
        {
            render(filename, model, writer, false);
        }

        public static void render(String filename, Dictionary<String, Object> model, TextWriter writer, bool pretty)
            // throws IOException,JadeCompilerException
        {
            JadeTemplate template = getTemplate(filename);
            template.setPrettyPrint(pretty);
            template.process(new JadeModel(model), writer);
        }

        public static String render(JadeTemplate template, Dictionary<String, Object> model)
            // throws JadeCompilerException
        {
            return render(template, model, false);
        }

        public static String render(JadeTemplate template, Dictionary<String, Object> model, bool pretty)
            // throws JadeCompilerException
        {
            template.setPrettyPrint(pretty);
            return templateToString(template, model);
        }

        public static void render(JadeTemplate template, Dictionary<String, Object> model, TextWriter writer)
            // throws JadeCompilerException
        {
            render(template, model, writer, false);
        }

        public static void render(JadeTemplate template, Dictionary<String, Object> model, TextWriter writer, bool pretty)
            // throws JadeCompilerException
        {
            template.setPrettyPrint(pretty);
            template.process(new JadeModel(model), writer);
        }

        public static String render(Uri url, Dictionary<String, Object> model)
            // throws IOException, JadeCompilerException
        {
            return render(url, model, false);
        }

        public static String render(Uri url, Dictionary<String, Object> model, bool pretty)
            // throws IOException, JadeCompilerException
        {
            string contents;
            using (var wc = new WebClient())
                contents = wc.DownloadString(url);
            var reader = new StringReader(contents);
            JadeTemplate template = getTemplate(reader, url.AbsoluteUri);
            return render(template, model, pretty);
        }

        public static String render(TextReader reader, String filename, Dictionary<String, Object> model)
            // throws IOException, JadeCompilerException 
        {
            return render(reader, filename, model, false);
        }

        public static String render(TextReader reader, String filename, Dictionary<String, Object> model, bool pretty)
            //throws IOException, JadeCompilerException 
        {
            JadeTemplate template = getTemplate(reader, filename);
            return render(template, model, pretty);
        }

        public static JadeTemplate getTemplate(String filename) // throws IOException
        {
            return createTemplate(filename, new FileTemplateLoader("", "UTF-8"));
        }

        private static JadeTemplate getTemplate(TextReader reader, String name) // throws IOException
        {
            return createTemplate(name, new ReaderTemplateLoader(reader, name));
        }

        private static JadeTemplate createTemplate(String filename, TemplateLoader loader) // throws IOException
        {
            JadeParser jadeParser = new JadeParser(filename, loader);
            Node root = jadeParser.parse();
            JadeTemplate template = new JadeTemplate();
            template.setTemplateLoader(loader);
            template.setRootNode(root);
            return template;
        }

        private static String templateToString(JadeTemplate template, Dictionary<String, Object> model)
            // throws JadeCompilerException
        {
            JadeModel jadeModel = new JadeModel(model);
            StringWriter writer = new StringWriter();

            template.process(jadeModel, writer);
            return writer.ToString();
        }


    }
}