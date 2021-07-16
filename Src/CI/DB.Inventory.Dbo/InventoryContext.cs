using Microsoft.EntityFrameworkCore;

namespace DB.Inventory.Dbo.Models
{
  public partial class InventoryContext : DbContext
  {
    readonly string _connectoinString;//todo: if not done: remove warnig and ... in protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)  #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
    public InventoryContext(string connectoinString) => _connectoinString = connectoinString;
  }
}