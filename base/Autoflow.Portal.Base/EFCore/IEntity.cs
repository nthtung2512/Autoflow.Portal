namespace Autoflow.Portal.Base.EFCore
{
    public interface IEntity
    {
        object?[] GetKeys();
    }
}
