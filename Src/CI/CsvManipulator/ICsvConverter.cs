using System.Threading.Tasks;

namespace CsvManipulator
{
  public interface ICsvConverter
  {
    Task<string> GetFileStats();
    string CleanEmptyRowsColumns();
  }
}