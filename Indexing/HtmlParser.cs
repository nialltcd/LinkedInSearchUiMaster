using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using LinkedInSearchUi.DataTypes;
using System;
using System.Text.RegularExpressions;

namespace LinkedInSearchUi.Indexing
{
    public class HtmlParser : IHtmlParser
    {
        public Person GeneratePersonFromHtmlDocument(HtmlDocument document)
        {

            //Retrieve the person's name from the document
            var personName = GetPersonsName(document);
            int numberOfConnections = 0;
            try
            {
                numberOfConnections = GetNumberOfConnections(document);
            }catch (Exception connectionException) { }

            var experiences = new List<Experience>();
            var skills = new List<Skill>();
            int workExperienceInMonths = 0;
            try
            {
                //Retrieve the persons prior experience from the document
                experiences = GetPersonsExperience(document);
                foreach (var experience in experiences)
                {
                    workExperienceInMonths += experience.DurationInMonths;
                }
            }
            catch (Exception experienceException) { }
            var education = new List<Education>();
            try
            {
                //Retrieve the persons prior experience from the document
                education = GetPersonsEducation(document);
            }
            catch(Exception educationException) { }
            try
            {
                //Retrieve the persons prior experience from the document
                skills = GetPersonsSkills(document);
            }
            catch (Exception skillException) { }
            return new Person() { Name = personName, NumberOfConnections=numberOfConnections, WorkExperienceInMonths = workExperienceInMonths, Experiences = experiences, Skills= skills, Education=education };

        }

        private int GetNumberOfConnections(HtmlDocument document)
        {
            var firstOrDefault = document.DocumentNode.SelectNodes("//dd[@class='overview-connections']/p/strong").FirstOrDefault();
            if (firstOrDefault != null)
                return int.Parse(new string(firstOrDefault.InnerText.Where(c => char.IsDigit(c)).ToArray()));
            return 0;
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
                experience.DurationInMonths = ConvertToMonths(experience.Duration);
                experiences.Add(experience);
            }
            return experiences;
        }

        private int ConvertToMonths(string duration)
        {
            var yearMonthRegex = new Regex(@"\((\d+)\syears?\s(\d+)\smonths?\)");
            var yearRegex = new Regex(@"\((\d+)\syears?\)");
            var monthRegex = new Regex(@"\((\d+)\smonths?\)");

            Match match = yearRegex.Match(duration);

            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value)*12;
            }
            match = monthRegex.Match(duration);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            match = yearMonthRegex.Match(duration);
            if (match.Success)
            {
                return (int.Parse(match.Groups[1].Value) * 12) + int.Parse(match.Groups[2].Value);
            }
            return 0;
        }

        private List<Education> GetPersonsEducation(HtmlDocument document)
        {
            List<Education> educationList = new List<Education>();
            var educationNode = document.DocumentNode.SelectNodes("//div[@id='profile-education'] //div[@class='content vcalendar']/div");
            if (educationNode == null) return new List<Education>();

            foreach (var node in educationNode)
            {
                var education = new Education();
                education.Institute = node.SelectSingleNode(".//*[@class='summary fn org']/a").InnerText;
                education.Degree = node.SelectSingleNode(".//*[@class='degree']").InnerText;
                educationList.Add(education);
            }
            return educationList;
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