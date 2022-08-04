//mapeando modelo de dados em arquivos separados evitando auto acoplamento de codigo
// fazendo com que a manutencao se torne mais facil
// cria arquivos com suas respectivas responsabilidades

//esse arquivo mapeia o modelo de dados para cliente
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaPedidoEFCore.Domain;

namespace SistemaPedidoEFCore.Data.Configurations
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
            //o "p" é uma action, ou seja, o TypeBuilder se tem metodos para fazer o mapeamento do modelo de dados
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Clientes"); //nome da tabela na base de dados
            builder.HasKey(p => p.Id); //informa a chave primaria
            builder.Property(p => p.Nome).HasColumnType("VARCHAR(80)").IsRequired(); //define a propriedade de um campo/ Configura o tipo do dado/informa que o campo é obrigatorio
            builder.Property(p => p.Telefone).HasColumnType("CHAR(11)"); //CHAR possui tamanho fixo e alocacao estatica na memoria
            builder.Property(p => p.CEP).HasColumnType("CHAR(8)").IsRequired();
            builder.Property(p => p.Estado).HasColumnType("CHAR(2)").IsRequired();
            builder.Property(p => p.Cidade).HasMaxLength(60).IsRequired();

            builder.HasIndex(i => i.Telefone).HasName("idx_cliente_telefone"); //cria um indice para um campo que possui uma grande quantidade de consultas, dando mais perfomance na aplicacao, com o Fluent API
        }
    }
}