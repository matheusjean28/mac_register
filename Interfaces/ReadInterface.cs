using MacToDatabaseModel;
using DeviceContext;

namespace Read.Interfaces
{
    public interface IRead
    {
         Task<IEnumerable<MacToDatabase>>ReadCsvItens(IFormFile file,DeviceDb db );
};


}