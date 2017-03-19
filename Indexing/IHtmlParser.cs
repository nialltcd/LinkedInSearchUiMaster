using HtmlAgilityPack;
using LinkedInSearchUi.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedInSearchUi.Indexing
{
    public interface IHtmlParser
    {
        Person GeneratePersonFromHtmlDocument(HtmlDocument document);
    }
}
