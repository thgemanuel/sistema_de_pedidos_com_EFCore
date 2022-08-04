using Microsoft.EntityFrameworkCore;

namespace SistemaPedidoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=SistemaDePedidosEFCoreConsole;Integrated Security=true");
        }
    }
}