using System.ComponentModel.DataAnnotations.Schema;

namespace Tests.EntityFramework.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Person")] public int PersonId { get; set; }

        public virtual Person Person { get; set; }
    }
}