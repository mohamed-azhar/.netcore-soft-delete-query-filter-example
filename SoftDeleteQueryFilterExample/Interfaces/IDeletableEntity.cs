namespace SoftDeleteQueryFilterExample.Interfaces
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}
