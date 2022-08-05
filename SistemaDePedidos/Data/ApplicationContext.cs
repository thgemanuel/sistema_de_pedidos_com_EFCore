using System.Linq;
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
            //o metodo EnableRetryOnFailure deixa a aplicacao mais resiliente em caso de falha na conexao
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging() // exibe os valores de cada parametro no console
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=SistemaDePedidosEFCoreConsole;Integrated Security=true",
                p=>p.EnableRetryOnFailure(maxRetryCount: 2, //numero maximo de tentativas
                                        maxRetryDelay: TimeSpan.FromSeconds(5), //intervalo de tempo entre tentativas
                                        errorNumbersToAdd: null) //codigos de erros adicionas que o EFCore interprete a mais
                                        .MigrationsHistoryTable("cursoEFCore")//alterando o nome da tabela de migrações gerada, nao criando mais a tabela EFMigrationHistory, usando o nome da tablea informada
                                        ); //habilitando funcionalidade de Retry em caso de falha de conexao, sem argumentos ele tenta se conectar 6x até completar um minuto
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
            MapearPropriedadeEsquecida(modelBuilder);
        }

        //mapeando propriedades que foram esquecidas de configurar no modelo de dados
        protected void MapearPropriedadeEsquecida(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties().Where(p => p.ClrType == typeof(string));

                foreach (var property in properties)
                {
                    //regra para verificar se propriedade ja foi configurada ou nao
                    //verificando se o tipo da coluna esta vazia, ou seja, se foi configurada ou nao, e tambem se nao foi definido um tamnho maximo para essa propriedade
                    if (string.IsNullOrEmpty(property.GetColumnType())
                        && !property.GetMaxLength().HasValue)
                    {
                        // property.SetMaxLength(100);
                        //toda vez que encontrar uma propriedade que nao esta configurada do tipo string, ira criar essa propriedade na base de dados por padrao com VARCHAR(100)
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}