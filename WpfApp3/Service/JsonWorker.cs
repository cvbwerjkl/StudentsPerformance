using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using WpfApp3.Models;


namespace WpfApp3.Service
{
    public static class JsonWorker
    {
        private const string STUDENTS_FILNAME = "studentsDB.json";
        private const string SUBJECTS_FILENAME = "subjectsDB.json";


        public static ObservableCollection<StudentScore> GetAllStudents()
        {
            try
            {
                return JsonConvert.DeserializeObject<ObservableCollection<StudentScore>>(File.ReadAllText(@STUDENTS_FILNAME));
            }
            catch
            {
                return new ObservableCollection<StudentScore>();
            }
        }

        public static ObservableCollection<SubjectScore> GetAllSubjects()
        {
            try
            {
                return JsonConvert.DeserializeObject<ObservableCollection<SubjectScore>>(File.ReadAllText(@SUBJECTS_FILENAME));
            }
            catch
            {
                return new ObservableCollection<SubjectScore>();
            }
        }

        public static void UpdateStudentJson(ObservableCollection<StudentScore> _studentsList)
        {
            File.WriteAllText(@STUDENTS_FILNAME, JsonConvert.SerializeObject(_studentsList));
        }

        public static void UpdateSubjectJson(ObservableCollection<SubjectScore> _subjectsList)
        {
            File.WriteAllText(@SUBJECTS_FILENAME, JsonConvert.SerializeObject(_subjectsList));
        }

        public static ObservableCollection<string> GetSubjectsList()
        {
            try
            {
                var content = File.ReadAllText(@SUBJECTS_FILENAME);
                var subjectScore = JsonConvert.DeserializeObject<ObservableCollection<SubjectScore>>(content);
                var ans = subjectScore.Select(x => x.Subject);
                return new ObservableCollection<string>(ans);
            }
            catch
            {
                return new ObservableCollection<string>();
            }
        }
    }
}
