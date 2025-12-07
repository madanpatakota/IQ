using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misard.IQs.Application.DTOs.Attempts
{
    public class AttemptDetailDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public string OptionA { get; set; } = string.Empty;
        public string OptionB { get; set; } = string.Empty;
        public string OptionC { get; set; } = string.Empty;
        public string OptionD { get; set; } = string.Empty;

        public string SelectedOption { get; set; } = string.Empty;   // char? -> string
        public string CorrectOption { get; set; } = string.Empty;    // char -> string
        public bool IsCorrect { get; set; }                          // bool? → bool
    }

}
