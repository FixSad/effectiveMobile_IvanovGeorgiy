namespace DeliveryService.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        void Create(T viewModel);
        void Delete(char id);
        IQueryable<T> Get(); 
    }
}
