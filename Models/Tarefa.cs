using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace gerenciadorTarefas.Models
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha a descrição!")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Preencha a data de vencimento!")]
        public DateTime? DataDeVencimento { get; set; }

        [Required(ErrorMessage = "Selecione uma categoria!")]
        public string CategoriaId { get; set; }

        [ValidateNever]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "Selecione um status!")]
        public string StatusId { get; set; }

        [ValidateNever]
        public Status Status { get; set; }

        public bool Atrasado => StatusId == "aberto" && DataDeVencimento < DateTime.Today;

    }
}