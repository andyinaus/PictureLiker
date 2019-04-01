using System.ComponentModel.DataAnnotations;

namespace PictureLiker.DAL.Entities
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; private set; }
    }
}
