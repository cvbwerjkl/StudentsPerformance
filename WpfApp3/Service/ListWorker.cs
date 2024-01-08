using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Models;

namespace WpfApp3.Service
{
    public static class ListWorker
    {
        /*change subject averege when score added*/
        public static void SubjectAvgUp(ObservableCollection<SubjectScore> SubjectsScoreList, string StudentSubject, int SelectedScore)
        {
            var found = SubjectsScoreList.First(x => x.Subject == StudentSubject);
            int tempScoreNum = found.ScoresNum + 1;
            int tempScoreSum = found.ScoresSum + SelectedScore;
            double tempAvgScore = (double)tempScoreSum / tempScoreNum;
            
            SubjectsScoreList.Remove(found);
            SubjectsScoreList.Add(new SubjectScore { Subject = StudentSubject, ScoresNum = tempScoreNum, ScoresSum = tempScoreSum, AvgScore = tempAvgScore });
        }

        /*change subject averege when score deleted*/
        public static void SubjectAvgDown(ObservableCollection<SubjectScore> SubjectsScoreList, string StudentSubject, int SelectedScore)
        {
            var found = SubjectsScoreList.First(x => x.Subject == StudentSubject);
            int tempScoreNum = found.ScoresNum - 1;
            int tempScoreSum = found.ScoresSum - SelectedScore;
            if (tempScoreNum != 0)
            {
                double tempAvgScore = (double)tempScoreSum / tempScoreNum;
                SubjectsScoreList.Remove(found);
                SubjectsScoreList.Add(new SubjectScore { Subject = StudentSubject, ScoresNum = tempScoreNum, ScoresSum = tempScoreSum, AvgScore = tempAvgScore });
            }
            else
            {
                SubjectsScoreList.Remove(found);
                SubjectsScoreList.Add(new SubjectScore { Subject = StudentSubject, ScoresNum = 0, ScoresSum = 0, AvgScore = 0 });
            }
        }

    }
}
