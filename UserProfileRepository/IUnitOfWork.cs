namespace UserProfileRepository
{
    public interface IUnitOfWork
    {
        void Dispose();

        void SaveChanges();
    }
}
