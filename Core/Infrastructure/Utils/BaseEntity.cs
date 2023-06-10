namespace Infrastructure.Utils;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreationDate { get; private set; }

    public BaseEntity()
    {
        CreationDate = DateTime.Now;
    }
}