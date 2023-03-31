using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace TestApi.Models
{
    public class Fornecedor
    {
        public Fornecedor()
        {
            this.Empresas = new HashSet<Empresa>();
        }

        public int FornecedorId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CEP { get; set; }
        public string TipoFornecedor { get; set; }

        [JsonIgnore]
        public virtual ICollection<Empresa> Empresas { get; set; }
    }
}
