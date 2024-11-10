using PRG282Project.DataAccessLayer;
using System;
using System.Data;

namespace PRG282Project.BusinessLogicLayer
{
    public class SummaryService
    {
        private SummaryRepository summaryRepository = new SummaryRepository();

        public (int totalStudents, double averageAge) GetSummaryFromDatabase()
        {
            return summaryRepository.GetSummaryFromDatabase();
        }

        public (int totalStudents, double averageAge) GetSummaryFromTextFile()
        {
            return summaryRepository.GetSummaryFromTextFile();
        }

        public void SaveSummaryToFile(int totalStudents, double averageAge)
        {
            summaryRepository.SaveSummaryToFile(totalStudents, averageAge);
        }

    }
}
