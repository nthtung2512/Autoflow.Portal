namespace Autoflow.Portal.Base.EFCore
{
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; }
    }
}
