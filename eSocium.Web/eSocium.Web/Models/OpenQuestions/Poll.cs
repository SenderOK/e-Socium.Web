using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Linq;

namespace eSocium.Web.Models.OpenQuestions
{
    /// <summary>
    /// Описание класса опроса и связанных с ним классов вопроса, ответа и респондента
    /// </summary>
    public class Poll
    {
        [Key]
        [Required]
        public int PollId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Date")]
        public string Date { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        [Display(Name = "Questions")]
        public virtual ICollection<Question> Questions { get; set; }
    }

    public class Question
    {
        [Key]
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public virtual Poll Poll { get; set; }
        [Required]
        [Display(Name = "Wording")]
        public string Wording { get; set; }
        [Display(Name = "Label")]
        public string Label { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }

    public class Answer
    {
        [Key]
        [Required]
        public int AnswerId { get; set; }
        [Required]
        public virtual Question Question { get; set; }
        [Required]
        public int RespondentId { get; set; }
        public string Text { get; set; }
    }

    public static class ModelStateExtensions
    {
        public static IEnumerable<string> GetErrorsFromModelState(this ModelStateDictionary msd)
        {
            return msd.SelectMany(x => x.Value.Errors.Select(
                error =>
                    error.ErrorMessage
                    +(error.Exception != null ?
                        ("("+error.Exception.Message+")")
                        : "")
                    )
                );
        }
    }
}