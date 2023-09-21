using MacToDatabaseModel;
using DeviceContext;

namespace Read.Interfaces
{
    public interface IRead
    {
         Task<IEnumerable<MacToDatabase>>ReadCsvItens(DeviceDb db );
};


}