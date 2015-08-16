using System;
using System.IO;

namespace Template
{
    public interface TemplateLoader
    {
        long getLastModified(String name);
        TextReader getReader(String name);
        string GetPath();
    }
}