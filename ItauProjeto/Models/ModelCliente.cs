using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace ItauProjeto.Models
{
    public class ModelCliente : ModelClienteDados
    {

        [Required(ErrorMessage = "Email deve ser preenchido")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Email deve conter de 5 a 50 caracteres")]
        [EmailAddress(ErrorMessage = "O email inserido não é válido")]
        [Remote("validarDisponibilidadeEmail", "Validacoes", AdditionalFields = "id", ErrorMessage = "Email não disponível")]
        [DisplayName("Endereço de email:")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Senha deve ser preenchido")]
        [DataType(DataType.Password, ErrorMessage ="A data informada não é válida")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Senha deve conter de 5 a 30 caracteres")]
        [DisplayName("Senha:")]
        public string Senha { get; set; }

        //[Required(ErrorMessage = "Confirmar senha deve ser preenchido")]
        [System.ComponentModel.DataAnnotations.CompareAttribute("Senha", ErrorMessage = "As senha não conferem")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Senha deve conter de 5 a 30 caracteres")]
        [DataType(DataType.Password)]
        [DisplayName("Confirmar Senha:")]
        public string ConfirmarSenha { get; set; }

        [Remote("validarSenhaAtual", "Validacoes", AdditionalFields = "id", ErrorMessage = "Senha incorreta")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Senha deve conter de 5 a 30 caracteres")]
        [DataType(DataType.Password)]
        [DisplayName("Senha atual:")]
        public string SenhaAtual { get; set; }

    }
}
