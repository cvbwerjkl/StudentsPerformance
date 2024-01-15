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
using WpfApp3.Service;

namespace WpfApp3.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        public BaseVM()
        {
            AddCommand = new RelayCommand(StuAdd, CanAdd);
            AvgCommand = new RelayCommand(StuAvg, CanAvg);
            DelCommand = new RelayCommand(StuDel, CanDel);
            NewSubCommand = new RelayCommand(NewSub, CanNewSub);
        }

        #region DATA

        private ObservableCollection<StudentScore> _studentsScoreList = JsonWorker.GetAllStudents();
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

        private ObservableCollection<SubjectScore> _subjectsScoreList = JsonWorker.GetAllSubjects();
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
                _firstName = value.Trim().ToUpper();
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
                _lastName = value.Trim().ToUpper();
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> _subjectsList = JsonWorker.GetSubjectsList();
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

        public IEnumerable<int> Score { get; private set; } = Enumerable.Range(1, 5);
        public static int SelectedScore { get; set; } = 0;

        private string _newSubject;
        public string NewSubject
        {
            get
            {
                return _newSubject;
            }
            set
            {
                _newSubject = value.Trim().ToUpper();
                OnPropertyChanged();
            }
        }

        public static StudentScore SelectedStudent { get; set; } = null;

        #endregion

        #region BUTTONS

        //Add button realization
        public ICommand AddCommand { get; set; }
        private void StuAdd(object paremter = null)
        {
            if (_firstName.All(char.IsLetter) && _lastName.All(char.IsLetter))
            {
                /*Adding and ordering students score list*/
                StudentsScoreList.Add(new StudentScore { FirstName = _firstName, LastName = _lastName, Subject = StudentSubject, Score = SelectedScore });
                StudentsScoreList = new ObservableCollection<StudentScore>(StudentsScoreList.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.Subject).ThenBy(x => x.Score));
                JsonWorker.UpdateStudentJson(StudentsScoreList);
                MessageBox.Show(FirstName + " " + LastName + " Score Added!");

                /*Updating subjects list*/
                ListWorker.SubjectAvgUp(SubjectsScoreList, StudentSubject, SelectedScore);
                SubjectsScoreList = new ObservableCollection<SubjectScore>(SubjectsScoreList.OrderBy(x => x.Subject));
                JsonWorker.UpdateSubjectJson(SubjectsScoreList);
            }
            else 
            {
                MessageBox.Show("Student first name and last name have to contain only letters!!!");
            }
            
        }

        private bool CanAdd(object parameter = null)
        {
            if (string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || StudentSubject == null || SelectedScore == 0)
            {
                return false;
            }
            return true;
        }

        //Averege button realization
        public ICommand AvgCommand { get; set; }
        private void StuAvg(object paremter = null)
        {

            double result = StudentsScoreList
                .Where(x => x.FirstName == SelectedStudent.FirstName && x.LastName == SelectedStudent.LastName && x.Subject == SelectedStudent.Subject)
                .Average(x => x.Score);
            MessageBox.Show(SelectedStudent.FirstName + " " + SelectedStudent.LastName + " average " + SelectedStudent.Subject + " score is " + result.ToString("N2"));
        }

        private bool CanAvg(object parameter = null)
        {
            if (SelectedStudent == null)
            {
                return false;
            }
            return true;
        }

        //Add subject button realization
        public ICommand NewSubCommand { get; set; }
        private void NewSub(object paremter = null)
        {
            if (SubjectsList.Where(x => x == NewSubject).Count() == 0 && NewSubject.All(char.IsLetter))
            {
                SubjectsScoreList.Add(new SubjectScore { Subject = NewSubject, ScoresNum = 0, ScoresSum = 0, AvgScore = 0 });
                SubjectsList.Add(NewSubject);
                SubjectsList = new ObservableCollection<string>(SubjectsList.OrderBy(x => x));
                SubjectsScoreList = new ObservableCollection<SubjectScore>(SubjectsScoreList.OrderBy(x => x.Subject));
                JsonWorker.UpdateSubjectJson(SubjectsScoreList);
                MessageBox.Show(NewSubject + " added!!!");
            }
            else if (!NewSubject.All(char.IsLetter))
            {
                MessageBox.Show("Subject name have to contain only letters!!!");
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


        //Delete button realization
        public ICommand DelCommand { get; set; }
        private void StuDel(object paremter = null)
        {
            if (MessageBox.Show("Do you want to delete " + SelectedStudent.FirstName + " " + SelectedStudent.LastName + " " + SelectedStudent.Subject + " score?",
                    "Delete score",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                /*Updating subjects list*/
                ListWorker.SubjectAvgDown(SubjectsScoreList, SelectedStudent.Subject, SelectedStudent.Score);
                SubjectsScoreList = new ObservableCollection<SubjectScore>(SubjectsScoreList.OrderBy(x => x.Subject));
                JsonWorker.UpdateSubjectJson(SubjectsScoreList);

                /*Removing score*/
                MessageBox.Show(SelectedStudent.FirstName + " " + SelectedStudent.LastName + " Score Deleted!");
                StudentsScoreList.Remove(StudentsScoreList.First(x => x.FirstName == SelectedStudent.FirstName && x.LastName == SelectedStudent.LastName && x.Subject == SelectedStudent.Subject && x.Score == SelectedStudent.Score));
                JsonWorker.UpdateStudentJson(StudentsScoreList);
            }
        }

        private bool CanDel(object parameter = null)
        {
            if (SelectedStudent == null)
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
