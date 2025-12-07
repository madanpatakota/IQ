using Misard.IQs.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misard.IQs.Application.DTOs.Attempts
{
    public class AttemptListItemDto
    {
        public int SessionId { get; set; }
        public string TechnologyName { get; set; } = string.Empty;
        public int DifficultyLevel { get; set; }   // no nullable
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal ScorePercent { get; set; }
        public DateTime CreatedOn { get; set; }
        public QuizStatus Status { get; set; }
    }

}
