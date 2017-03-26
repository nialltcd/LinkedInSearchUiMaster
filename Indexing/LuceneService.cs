using System.Collections.Generic;
using System.Data;
using LinkedInSearchUi.DataTypes;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

namespace LinkedInSearchUi.Indexing
{
    public class LuceneService : ILuceneService
    {
        // Note there are many different types of Analyzer that may be used with Lucene, the exact one you use
        // will depend on your requirements
        private Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        private Directory luceneIndexDirectory;
        private IndexWriter writer;
        private string indexPath = @"c:\temp\LuceneIndex";

        public LuceneService(List<Person> people)
        {
            luceneIndexDirectory = BuildIndex(people);
        }

        private Directory BuildIndex(IEnumerable<Person> people )
        {
            var directory = new RAMDirectory();

            using (Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
            using (var writer = new IndexWriter(directory, analyzer, new IndexWriter.MaxFieldLength(1000)))
            { // the writer and analyzer will popuplate the directory with documents

                foreach (Person person in people)
                {
                    var document = new Document();
                    document.Add(new Field("Id", person.Id.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("Name", person.Name, Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("NumberOfConnections", person.NumberOfConnections.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                    document.Add(new Field("WorkExperienceInMonths", person.WorkExperienceInMonths.ToString(), Field.Store.YES, Field.Index.ANALYZED));


                    string all = person.Name;
                    foreach (var experience in person.Experiences)
                    {
                        document.Add(new Field("Organisation", experience.Organisation, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Role", experience.Role, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Duration", experience.Duration, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("DurationInMonths", experience.DurationInMonths.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

                        all += " "+experience.Organisation +" "+ experience.Role+" ";
                    }
                    foreach (var education in person.Education)
                    {
                        document.Add(new Field("Institute", education.Institute, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        document.Add(new Field("Degree", education.Degree, Field.Store.YES, Field.Index.NOT_ANALYZED));
                        all += " " + education.Institute + " " + education.Degree + " ";
                    }
                    foreach (var skill in person.Skills)
                    {
                        document.Add(new Field("Skill",skill.Name,Field.Store.YES, Field.Index.NOT_ANALYZED));
                    }
                    document.Add(new Field("All", all, Field.Store.YES, Field.Index.ANALYZED));

                    writer.AddDocument(document);
                }

                writer.Optimize();
                writer.Flush(true, true, true);
            }
            return directory;
        }

        public List<Person> SearchIndex(string textSearch)
        {
            List<Person> searchResults = new List<Person>();
            using (var reader = IndexReader.Open(luceneIndexDirectory, true))
            using (var searcher = new IndexSearcher(reader))
            {
                using (Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30))
                {
                    var queryParser = new QueryParser(Version.LUCENE_30, "All", analyzer);
                    var query = queryParser.Parse(textSearch);
                    //queryParser.AllowLeadingWildcard = true;

                    //var query = queryParser.Parse(textSearch);

                    var collector = TopScoreDocCollector.Create(1000, true);

                    searcher.Search(query, collector);

                    var matches = collector.TopDocs().ScoreDocs;

                    foreach (var item in matches)
                    {

                        var id = item.Doc;
                        var doc = searcher.Doc(id);

                        //Create Person from returned information
                        var person = new Person();

                        person.Name = doc.GetField("Name").StringValue;
                        person.Id = int.Parse(doc.GetField("Id").StringValue);
                        person.NumberOfConnections = int.Parse(doc.GetField("NumberOfConnections").StringValue);
                        person.WorkExperienceInMonths = int.Parse(doc.GetField("WorkExperienceInMonths").StringValue);
                        person.Experiences = new List<Experience>();
                        var organisations = doc.GetFields("Organisation");
                        var durations = doc.GetFields("Duration");
                        var durationsInMonths = doc.GetFields("DurationInMonths");
                        var roles = doc.GetFields("Role");
                        for(int i=0;i<organisations.Length;i++)
                        {
                            person.Experiences.Add(new Experience()
                            {
                                Organisation = organisations[i].StringValue,
                                Role = roles[i].StringValue,
                                Duration = durations[i].StringValue,
                                DurationInMonths = int.Parse(durationsInMonths[i].StringValue)
                            });
                        }
                        person.Education = new List<Education>();
                        var institutes = doc.GetFields("Institute");
                        var degrees = doc.GetFields("Degree");
                        for (int i = 0; i < institutes.Length; i++)
                        {
                            person.Education.Add(new Education()
                            {
                                Institute = institutes[i].StringValue,
                                Degree = degrees[i].StringValue
                            });
                        }
                        person.Skills = new List<Skill>();
                        var skills = doc.GetFields("Skill");
                        foreach(var skill in skills)
                        {
                            person.Skills.Add(new Skill() { Name = skill.StringValue });
                        }
                        searchResults.Add(person);
                    }
                }
            }
            return searchResults;
        }
    }
}