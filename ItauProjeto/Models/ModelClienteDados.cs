using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace ItauProjeto.Models
{
    public class ModelClienteDados 
    {

        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Nome deve ser preenchido")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Nome deve conter de 5 a 50 caracteres")]
        [DisplayName("Nome:")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Data deve ser preenchida")]
        [DataType(DataType.Date, ErrorMessage = "Este formato de data não é válido")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DisplayName("Data de Nascimento:")]
        [ValidateDataRangeCliente]
        public DateTime DataNascimento { get; set; }
        //Install-Package Microsoft.AspNet.Mvc.pt-br -Version 4.0.30506

        [Required(ErrorMessage = "CEP deve ser preenchido")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "CEP inválido")]
        [DisplayName("CEP:")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "UF deve ser preenchido")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "UF inválido")]
        [DisplayName("Estado (UF):")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Cidade deve ser preenchida")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Cidade deve conter de 2 a 50 caracteres")]
        [DisplayName("Cidade:")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Bairro deve ser preenchido")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Bairro deve conter de 2 a 50 caracteres")]
        [DisplayName("Bairro:")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Endereço residencial deve ser preenchido")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Endereço residencial deve conter de 5 a 100 caracteres")]
        [DisplayName("Endereço residencial:")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "N° deve ser preenchido")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "N° deve conter de 1 a 10 caracteres")]
        [DisplayName("Número:")]
        public string Numero { get; set; }

        //[Required(ErrorMessage = "Faça o upload do comprovante de endereço")]
        [DisplayName("Comprovante de residência de até 10MB):")]
        [ValidateFileCliente(ErrorMessage = "Selecione um arquivo de até 10MB")]
        public HttpPostedFileBase ComprovanteEndereco { get; set; }

        [DisplayName("Comprovante cadastrado:")]
        public string NomeDoArquivo { get; set; }
    }
}
