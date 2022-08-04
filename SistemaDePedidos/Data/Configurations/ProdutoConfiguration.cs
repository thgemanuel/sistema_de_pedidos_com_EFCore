//mapeando modelo de dados em arquivos separados evitando auto acoplamento de codigo
// fazendo com que a manutencao se torne mais facil
// cria arquivos com suas respectivas responsabilidades

//esse arquivo mapeia o modelo de dados para produto
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPedidoEFCore.Domain;

namespace SistemaPedidoEFCore.Data.Configurations
{
    public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("Produtos");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CodigoBarras).HasColumnType("VARCHAR(14)").IsRequired();
            builder.Property(p => p.Descricao).HasColumnType("VARCHAR(60)");
            builder.Property(p => p.Valor).IsRequired();
            builder.Property(p => p.TipoProduto).HasConversion<string>(); //o tipo produto era enum, entao se quer armazenar no banco o dado com um determinado tipo utiliza o HasConversion 
        }
    }
}