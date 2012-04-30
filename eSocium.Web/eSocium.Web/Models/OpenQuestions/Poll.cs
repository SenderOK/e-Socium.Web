using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using OfficeOpenXml;
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

    public class RespondentAnswerTable
    {
        // Массив словарей (для каждого вопроса набор пар)
        public Dictionary<int,string>[] answers { get; private set; }
        // Метки столбцов листа
        public List<string> questionLabels { get; private set; }
        public RespondentAnswerTable(HttpPostedFileBase xlsFile, bool hasHeader, int sheet_num) 
        {
            if (xlsFile == null)
            {
                throw new Exception("No file reached");
            }
            if (xlsFile.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                throw new Exception("Uploaded file is not an xlsx file");
            }

            ExcelPackage package = new ExcelPackage(xlsFile.InputStream);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[sheet_num];

            int question_count = 1;

            questionLabels = new List<string>();
            int first_row = 1;
            if (hasHeader)
            {
                ++first_row;
                for (; worksheet.Cells[1, question_count + 1].Value != null; ++question_count)
                    questionLabels.Add(worksheet.Cells[1, question_count + 1].Value.ToString());
                --question_count;
            }

            answers = new Dictionary<int, string>[question_count];
            for (int i = 0; i < answers.Length; ++i)
                answers[i] = new Dictionary<int, string>();

            const int RespCol = 1;
            for (int row = first_row; worksheet.Cells[row, RespCol].Value != null; ++row)
            {
                try {
                    int resp_id = int.Parse(worksheet.Cells[row, RespCol].Value.ToString());
                    for (int question = 0; question < question_count; ++question) 
                    {
                        string resp_answer = (worksheet.Cells[row, question + 2].Value ?? "").ToString();
                        if (!String.IsNullOrWhiteSpace(resp_answer))
                            answers[question].Add(resp_id, resp_answer); // throws an exception when resp_id has duplicates
                    }
                }
                catch (Exception e) {
                    throw new Exception("Error parsing row " + row.ToString() + ". Maybe wrong file format.\nException is " + e.ToString());
                }
            }
        }
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