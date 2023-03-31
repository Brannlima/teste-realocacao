using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace TestApi.Models
{
    public class Empresa
    {
        public Empresa()
        {
            this.Fornecedores = new HashSet<Fornecedor>();
        }

        public int EmpresaId { get; set; }

        public string CNPJ { get; set; }

        public string NomeFantasia { get; set; }

        public string CEP { get; set; }

        public virtual ICollection<Fornecedor> Fornecedores { get; set; }
    }
}
