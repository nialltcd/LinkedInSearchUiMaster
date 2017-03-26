using System.Collections.Generic;

namespace LinkedInSearchUi.DataTypes
{
    public class Person
    {
        public string Name { get; set; }
        public List<Experience> Experiences { get; set; }
        public int WorkExperienceInMonths { get; set; }
        public int NumberOfConnections { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Education> Education { get; set; }
        public int Id { get; set; }
    }
}