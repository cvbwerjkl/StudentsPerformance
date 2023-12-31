using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using WpfApp3.Commands;
using WpfApp3.Models;
using WpfApp3.Units;

namespace WpfApp3.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        public BaseVM()
        {
            AddCommand = new RelayCommand(StuAdd, CanAdd);
            AvgCommand = new RelayCommand(StuAvg, CanAvg);
            NewSubCommand = new RelayCommand(NewSub, CanNewSub);
        }

        #region DATA

        private ObservableCollection<StudentScore> _studentsScoreList = DataWorker.GetAllStudents();
        public ObservableCollection<StudentScore> StudentsScoreList
        {
            get
            {
                return _studentsScoreList;
            }
            set
            {
                _studentsScoreList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<SubjectScore> _subjectsScoreList = DataWorker.GetAllSubjects();
        public ObservableCollection<SubjectScore> SubjectsScoreList
        {
            get
            {
                return _subjectsScoreList;
            }
            set
            {
                _subjectsScoreList = value;
                OnPropertyChanged();
            }
        }

        private string _firstName;

        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _subjectsList = DataWorker.GetAllSubjects2();
        public ObservableCollection<string> SubjectsList
        {
            get
            {
                return _subjectsList;
            }
            set
            {
                _subjectsList = value;
                OnPropertyChanged();
            }
        }
        public static string StudentSubject { get; set; }

        private List<int> _score = new List<int> { 5, 4, 3, 2, 1 };
        public List<int> Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }
        public static int StudentScore { get; set; } = 0;

        private string _newSubject;
        public string NewSubject
        {
            get
            {
                return _newSubject;
            }
            set
            {
                _newSubject = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region BUTTONS

        //Add button realization
        public ICommand AddCommand { get; set; }
        private void StuAdd(object paremter = null)
        {
            StudentsScoreList.Add(new StudentScore { FirstName = _firstName, LastName = _lastName, Subject = StudentSubject, Score = StudentScore });
            DataWorker.UpdateStudentJson(StudentsScoreList);
            MessageBox.Show(FirstName + " " + LastName + " Score Added!");

            var found = SubjectsScoreList.FirstOrDefault(x => x.Subject == StudentSubject);
            int tempScoreNum = found.ScoresNum + 1;
            int tempScoreSum = found.ScoresSum + StudentScore;
            double tempAvgScore = (double)tempScoreSum / tempScoreNum;
            SubjectsScoreList.Remove(found);
            SubjectsScoreList.Add(new SubjectScore { Subject = StudentSubject, ScoresNum = tempScoreNum, ScoresSum = tempScoreSum, AvgScore = tempAvgScore });
            DataWorker.UpdateSubjectJson(SubjectsScoreList);
        }

        private bool CanAdd(object parameter = null)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || StudentSubject == null || StudentScore == 0)
            {
                return false;
            }
            return true;
        }

        //Avg button realization
        public ICommand AvgCommand { get; set; }
        private void StuAvg(object paremter = null)
        {
            double result = DataWorker.AvgScore(FirstName, LastName, StudentSubject, StudentsScoreList);
            if (result > 1)
            {
                MessageBox.Show(FirstName + " " + LastName + " averege score is " + result.ToString("N2"));
            }
            else
            {
                MessageBox.Show(FirstName + " " + LastName + " student or " + StudentSubject + "scores didn't found!!!");
            }
        }

        private bool CanAvg(object parameter = null)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || StudentSubject == null)
            {
                return false;
            }
            return true;
        }

        //NewSub button realization
        public ICommand NewSubCommand { get; set; }
        private void NewSub(object paremter = null)
        {
            if (SubjectsList.Where(x => x == NewSubject).Count() == 0)
            {
                SubjectsScoreList.Add(new SubjectScore { Subject = NewSubject, ScoresNum = 0, ScoresSum = 0, AvgScore = 0 });
                SubjectsList.Add(NewSubject);
                DataWorker.UpdateSubjectJson(SubjectsScoreList);
                MessageBox.Show(NewSubject + " added!!!");
            }
            else
            {
                MessageBox.Show(NewSubject + " existed!!!");
            }
        }

        private bool CanNewSub(object parameter = null)
        {
            if (string.IsNullOrEmpty(NewSubject))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region PROPERTY CHANGE EVENT
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
