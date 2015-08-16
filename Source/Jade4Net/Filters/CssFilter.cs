using System;
using System.Collections.Generic;

namespace Jade.Filters
{
    public class CssFilter : CachingFilter
    {
        public override String convert(String source, Dictionary<String, Object> attributes)
        {
            return "<style type=\"text/css\">" + source + "</style>";
        }
    }
}
