using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using LinkedInSearchUi.DataTypes;
using System;

namespace LinkedInSearchUi.Indexing
{
    public class HtmlParser : IHtmlParser
    {
        public Person GeneratePersonFromHtmlDocument(HtmlDocument document)
        {

            //Retrieve the person's name from the document
            var personName = GetPersonsName(document);

            var experiences = new List<Experience>();
            var skills = new List<Skill>();
            try
            {
                //Retrieve the persons prior experience from the document
                experiences = GetPersonsExperience(document);
            }
            catch (Exception experienceException) { }
            try
            {
                //Retrieve the persons prior experience from the document
                skills = GetPersonsSkills(document);
            }
            catch (Exception skillException) { }
            return new Person() { Name = personName, Experiences = experiences, Skills= skills };

        }

        private string GetPersonsName(HtmlDocument document)
        {
            //Retrieve the person's name from the document
            var firstOrDefault = document.DocumentNode.SelectNodes("//span[@class='full-name']").FirstOrDefault();
            if (firstOrDefault != null)
                return firstOrDefault.InnerText;
            return null;
        }

        private List<Experience> GetPersonsExperience(HtmlDocument document)
        {
            List<Experience> experiences = new List<Experience>();
            //Retrieve the persons prior experience from the document
            var experienceNode = document.DocumentNode.SelectNodes("//div[@id='profile-experience'] //div[@class='content vcalendar']/div/div");
            if (experienceNode == null) return new List<Experience>();

            foreach (var node in experienceNode)
            {
                var experience = new Experience();
                experience.Role = node.SelectSingleNode(".  //div[@class='postitle'] //span[@class='title']").InnerText;
                experience.Organisation = node.SelectSingleNode(".//div[@class='postitle'] //span[@class='org summary']").InnerText;
                experience.Duration = node.SelectSingleNode(".//span[@class='duration']").InnerText;
                experiences.Add(experience);
            }
            return experiences;
        }

        private List<Skill> GetPersonsSkills(HtmlDocument document)
        {
            List<Skill> skills = new List<Skill>();
            var skillNode = document.DocumentNode.SelectNodes("//div[@id='profile-skills'] //div[@class='content'] //ol[@class='skills'] //li[@class='competency show-bean  ']");

            foreach (var node in skillNode)
            {
                var skill = new Skill();
                skill.Name = node.SelectSingleNode(".//span[@class='jellybean']").InnerText.Replace("\n",String.Empty);
                skills.Add(skill);
            }
            return skills;
        }
    }
}