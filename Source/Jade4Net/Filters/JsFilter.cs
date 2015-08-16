using System;
using System.Collections.Generic;

namespace Jade.Filters
{
    public class JsFilter : CachingFilter
    {
        public override String convert(String source, Dictionary<String, Object> attributes)
        {
            return "<script type=\"text/javascript\">" + source + "</script>";
        }
    }
}
