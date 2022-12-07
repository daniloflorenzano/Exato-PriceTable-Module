namespace Domain.Abstractions;

public interface IRepositoryFactory
{
    public IRepository Create();
}