using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.ProblemTreatWrapper
{
    public class ProblemTreatWrapper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ProblemTreatWrapper() { }

        public ProblemTreatWrapper(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
