using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaPedidoEFCore.Data.Configurations;
using SistemaPedidoEFCore.Domain;

//mapeando dados atravez do Fluent API
namespace SistemaPedidoEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        //criando instancia do logger para escrever log da aplicacao
        public static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());
        //é possivel criar o modelo de dados a partir do DbSet<> quanto
        //  sobreescrevendo a funcao OnModelCreating
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging() // exibe os valores de cada parametro no console
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=SistemaDePedidosEFCoreConsole;Integrated Security=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //informar um modelo de dados que foi configurado em classe separada
            
            //primeira forma manual
            // modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            // modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            // modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());
            // modelBuilder.ApplyConfiguration(new PedidoConfiguration());

            //segunda forma automatica
            //aqui é passado o assembly do projeto para importar todas as configuracoes
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
        }
    }
}