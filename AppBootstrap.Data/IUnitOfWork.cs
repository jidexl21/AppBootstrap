using System.Transactions;

namespace AppBootstrap.Data
{
    public interface IUnitOfWork
    {
        ICaptainsDAO CaptainsDAO { get; }

        void Commit();
    }
}