using System;
using System.Linq;

using GradeBook.Enums;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GradeBook.GradeBooks
{
    public class RankedGradeBook : BaseGradeBook
    {
        public RankedGradeBook(string name, bool isWeighted) : base(name, isWeighted)
        {
            Type = GradeBookType.Ranked;
        }

        public override char GetLetterGrade(double averageGrade)
        {
            // Refuse to use a ranked grade book for a class of fewer than five students.
            if (Students.Count < 5) throw new InvalidOperationException();

            // Resolve a count of students in each letter grade section.
            int letterGradeSectionCount = Students.Count / 5;

            // Resolve averages grades for all students, reversed sorted.
            List<double> studentsAverageGradesReverseSorted = Students.ConvertAll(s => s.AverageGrade);
            studentsAverageGradesReverseSorted.Sort();
            studentsAverageGradesReverseSorted.Reverse();

            // Calculate grade cutoffs.
            double gradeCutoffA = studentsAverageGradesReverseSorted[letterGradeSectionCount*1 - 1];
            double gradeCutoffB = studentsAverageGradesReverseSorted[letterGradeSectionCount*2 - 1];
            double gradeCutoffC = studentsAverageGradesReverseSorted[letterGradeSectionCount*3 - 1];
            double gradeCutoffD = studentsAverageGradesReverseSorted[letterGradeSectionCount*4 - 1];

            // Return grade letters.
            if (averageGrade >= gradeCutoffA) return 'A';
            else if (averageGrade >= gradeCutoffB) return 'B';
            else if (averageGrade >= gradeCutoffC) return 'C';
            else if (averageGrade >= gradeCutoffD) return 'D';
            else return 'F';
        }

        public override void CalculateStatistics()
        {
            // Refuse to use a ranked grade book for a class of fewer than five students.
            if (Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
                return;
            }

            // Calculate using the base method.
            base.CalculateStatistics();
        }

        public override void CalculateStudentStatistics(string name)
        {
            // Refuse to use a ranked grade book for a class of fewer than five students.
            if (Students.Count < 5)
            {
                Console.WriteLine("Ranked grading requires at least 5 students with grades in order to properly calculate a student's overall grade.");
                return;
            }

            // Calculate using the base method.
            base.CalculateStudentStatistics(name);
        }
    }
}
