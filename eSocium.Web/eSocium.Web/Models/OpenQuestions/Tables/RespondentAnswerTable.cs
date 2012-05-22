using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eSocium.Web.Models.OpenQuestions.Tables
{
    public class RespondentAnswerTable
    {
        // Массив словарей (для каждого вопроса набор пар)
        public Dictionary<int, string>[] answers { get; private set; }
        // Метки столбцов листа
        public string[] questionLabels { get; private set; }
        public RespondentAnswerTable(IWorksheet worksheet, bool hasHeader)
        {
            int question_count = (hasHeader) ? (worksheet.ColumnCount() - 1) : 1;
            int first_row = (hasHeader) ? 1 : 0;
            questionLabels = (hasHeader) ? (worksheet.Header()) : (new string[0]);

            answers = new Dictionary<int, string>[question_count];
            for (int i = 0; i < answers.Length; ++i)
                answers[i] = new Dictionary<int, string>();

            //Конец инициализации и переход к копированию таблицы

            const int RespCol = 0;
            for (int row = first_row; worksheet[row, RespCol] != null; ++row)
            {
                try
                {
                    int resp_id = int.Parse(worksheet[row, RespCol]);
                    for (int question = 1; question <= question_count; ++question)
                    {
                        string resp_answer = worksheet[row, question];
                        if (!String.IsNullOrWhiteSpace(resp_answer))
                            answers[question - 1].Add(resp_id, resp_answer); // throws an exception when resp_id has duplicates
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Error parsing row {0} . Probably wrong file format.\nException is {1}", row.ToString(), e.ToString()));
                }
            }
        }
    }

}