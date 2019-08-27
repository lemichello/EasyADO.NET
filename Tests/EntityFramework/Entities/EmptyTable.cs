using System.ComponentModel.DataAnnotations.Schema;

namespace Tests.EntityFramework.Entities
{
    [Table("EmptyTable")]
    public class EmptyTable
    {
        public int Id { get; set; }
        public string EmptyName { get; set; }
    }
}