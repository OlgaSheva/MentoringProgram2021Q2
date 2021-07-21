using System.IO;
using System.Threading.Tasks;

namespace Catalog.Interfaces
{
    public interface ICachePictureService
    {
        Task Cache(int id, Stream stream);

        Task<MemoryStream> GetCachedData(int id);
    }
}
