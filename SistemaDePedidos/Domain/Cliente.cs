using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaPedidoEFCore.Domain
{
    //informa o nome da tabela que o EFCore ira utilizar
    [Table("Clientes")]
    public class Cliente
    {
        //utilizando o DataAnnotations
        //define uma unica propriedade como chave primaria de uma entidade
        [Key]
        public int Id{ get; set; }
        //informa que esse atributo define que essa propriedade é obrigatoria
        [Required]
        public string Nome{ get; set; }
        //ele mapeia a propriedade no qual o nome dessa propriedade nao é o mesmo nome do campo que esta na tabela no BD
        //como por exemplo na tabela ao inves de telefone, o atributo se chama Phone
        [Column("Phone")]
        public string Telefone{ get; set; }
        public string CEP{ get; set; }
        public string Estado{ get; set; }
        public string Cidade{ get; set; }
    }
}