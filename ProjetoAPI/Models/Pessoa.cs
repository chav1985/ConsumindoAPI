using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProjetoAPI.Models
{
    public class Pessoa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Campo Nome é obrigatório")]
        [MaxLength(60, ErrorMessage ="Esse campo deve conter entre 5 e 60 caracteres")]
        [MinLength(5, ErrorMessage ="Esse campo deve conter entre 5 e 60 caracteres")]
        public string Nome { get; set; }
    }
}
