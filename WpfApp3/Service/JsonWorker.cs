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
    public class JsonWorker
    {
        public static ObservableCollection<StudentScore> GetAllStudents()
        {
            try
            {
                return JsonConvert.DeserializeObject<ObservableCollection<StudentScore>>(File.ReadAllText(@"studentsDB.json"));
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
                return JsonConvert.DeserializeObject<ObservableCollection<SubjectScore>>(File.ReadAllText(@"subjectsDB.json"));
            }
            catch
            {
                return new ObservableCollection<SubjectScore>();
            }
        }

        public static void UpdateStudentJson(ObservableCollection<StudentScore> _studentsList)
        {
            File.WriteAllText(@"studentsDB.json", JsonConvert.SerializeObject(_studentsList));
        }

        public static void UpdateSubjectJson(ObservableCollection<SubjectScore> _subjectsList)
        {
            File.WriteAllText(@"subjectsDB.json", JsonConvert.SerializeObject(_subjectsList));
        }

        public static ObservableCollection<string> GetSubjectsList()
        {
            try
            {
                List<string> ans = (from SubjectScore in JsonConvert.DeserializeObject<ObservableCollection<SubjectScore>>(File.ReadAllText(@"subjectsDB.json")) select SubjectScore.Subject).ToList();
                return new ObservableCollection<string>(ans);
            }
            catch
            {
                return new ObservableCollection<string>();
            }
        }
    }
}
