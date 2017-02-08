using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using HtmlAgilityPack;
using LinkedInSearchUi.DataTypes;
using LinkedInSearchUi.Indexing;
using System;

namespace LinkedInSearchUi.Model
{
    public class Model
    {
        private HtmlParser _parser;
        private LuceneService _luceneService;
        private CustomXmlWriter _customXmlWriter;

        public Model()
        {
            _parser = new HtmlParser();
            _customXmlWriter = new CustomXmlWriter();
        }

        public List<Person> ParseRawHtmlFilesFromDirectory()
        {
            List<Person> people = new List<Person>();
            var document = new HtmlDocument();
            foreach (var file in Directory.EnumerateFiles(@"C:\Users\Niall\Documents\Visual Studio 2015\Projects\LinkedInSearchUi\LinkedIn Dataset\SmallLinkedInDataSet", "*.html"))
            {
                try
                {
                    document.Load(file);
                    people.Add(_parser.GeneratePersonFromHtmlDocument(document));
                    if (people.Count % 1000 == 0)
                        Console.WriteLine(people.Count);
                }
                catch (Exception e) { }
            }
            _customXmlWriter.WriteToFile(people);
            _luceneService = new LuceneService(people);
            return people;
        }

        public List<Person> LuceneSearch(string textSearch)
        {
            return _luceneService.SearchIndex(textSearch);
        }
    }
}