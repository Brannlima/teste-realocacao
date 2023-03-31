using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace TestApi.Models
{
    public class Empresarial : Fornecedor
    {
        public string CNPJ { get; set; }
    }
}
