using System.ComponentModel.DataAnnotations;

namespace PictureLiker.DAL.Entities
{
    //TODO: May consider separate persistence models and domain models for all entities, and the reason for not doing it now => just too costly for a demo :P
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; private set; }
    }
}
