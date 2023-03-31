using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace TestApi.Models
{
    public class PessoaFisica : Fornecedor
    {
        public string CPF { get; set; }
        public string RG { get; set; }

        public DateTime DataNascimento { get; set; }
    }
}
