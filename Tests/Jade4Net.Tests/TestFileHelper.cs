using System;
using System.Diagnostics;
using System.IO;

namespace Jade.Tests
{
    public class TestFileHelper
    {
        public static readonly String TESTFILE_LEXER_FOLDER = "Resources\\lexer\\";
        public static readonly String TESTFILE_PARSER_FOLDER = "Resources\\parser\\";
        public static readonly String TESTFILE_COMPILER_FOLDER = "Resources\\compiler\\";
        public static readonly String TESTFILE_ORIGINAL_FOLDER = "Resources\\originalTests\\";
        public static readonly String TESTFILE_COMPILER_ERROR_FOLDER = "Resources\\compiler\\errors\\";

        public static String getResourcePath(String fileName) //throws FileNotFoundException
        {
            try
            {
                string path;
                var codeBase = Path.GetDirectoryName(typeof (TestFileHelper).Assembly.Location);
                path = Path.Combine(codeBase, fileName);
                return path;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static String getRootResourcePath() //throws FileNotFoundException
        {
            return getResourcePath("/");
        }

        public static String getLexerResourcePath(String fileName) //throws FileNotFoundException
        {
            return getResourcePath(TESTFILE_LEXER_FOLDER + fileName);
        }

        public static String getParserResourcePath(String fileName)
        {
            try
            {
                return getResourcePath(TESTFILE_PARSER_FOLDER + fileName);
            }
            catch (FileNotFoundException e)
            {
                Trace.WriteLine(e);
            }
            return null;
        }

        public static String getCompilerResourcePath(String fileName)
        {
            try
            {
                return getResourcePath(TESTFILE_COMPILER_FOLDER + fileName);
            }
            catch (FileNotFoundException e)
            {
                Trace.WriteLine(e);
            }
            return null;
        }

        public static String getOriginalResourcePath(String fileName)
        {
            try
            {
                return getResourcePath(TESTFILE_ORIGINAL_FOLDER + fileName);
            }
            catch (FileNotFoundException e)
            {
                Trace.WriteLine(e);
            }
            return null;
        }

        public static String getCompilerErrorsResourcePath(String fileName)
        {
            try
            {
                return getResourcePath(TESTFILE_COMPILER_ERROR_FOLDER + fileName);
            }
            catch (FileNotFoundException e)
            {
                Trace.WriteLine(e);
            }
            return null;
        }
    }
}