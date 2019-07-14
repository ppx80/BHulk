using System.Threading.Tasks;

namespace BHulk
{
    public interface IBulkAction<T>
    {
        IBulkAction<T> InStepOf(int stepOf);
        int Execute();
        Task<int> ExecuteAsync();
    }
}