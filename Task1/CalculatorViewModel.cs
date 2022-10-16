using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using parser = org.mariuszgromada.math.mxparser;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Task1
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        private string activeNumber = "";
        
        public string ActiveNumber
        {
            get=> activeNumber;
            set
            {
                activeNumber = value;
                OnPropertyChanged();
            }
        }
        private string inProgressString = "";
        public string InProgressString
        {
            get => inProgressString;
            set
            {
                inProgressString = value;
                OnPropertyChanged();
            }
        }
        private string inProgressNumber ="";
        public string InProgressNumber
        {
            get => inProgressNumber;
            set
            {
                inProgressNumber = value;
                OnPropertyChanged();
            }
        }        
        public OperationStatus OperationStatus { get; set; }

        private List<TextBlock> historyOperations = new List<TextBlock>(4);
        public List<TextBlock> HistoryOperations
        {
            get => historyOperations;
            set
            {
                historyOperations = value;
                OnPropertyChanged();
            }
        }
        private void ChangeStatus()
        {
            if (InProgressNumber == "Некорректный ввод")
            {
                OperationStatus = OperationStatus.Unsuccessful;
            }
            else
            {
                OperationStatus = OperationStatus.Calculated;
            }
            if (InProgressString.EndsWith("÷0"))
            {
                InProgressNumber = "Деление на ноль";
                OperationStatus = OperationStatus.Unsuccessful;
            }
        }
        public static ICommand AddNumberCommand { get; set; }
        private void OnAddNumberCommandExecute (object p)
        {
            string numbers = "0123456789";
            if (numbers.Contains(p.ToString()))
            {
                ActiveNumber += p.ToString();
            }
            InProgressString += p.ToString();
            InProgressNumber = CalculatorModel.ParserCalculate(InProgressString, InProgressNumber);
            ChangeStatus();
        }        
        private bool CanAddNumberCommandExecuted (object p)
        {
            return true;
        }
        public static ICommand AddPointCommand { get; set; }
        private void OnAddPointCommandExecute(object p)
        {
            string point = ",";
            if (InProgressString.Length > 0)
            {
                InProgressString += point;
                ActiveNumber += ".";
            }
            ChangeStatus();
        }
        private bool CanAddPointCommandExecuted(object p)
        {
            if (InProgressNumber.EndsWith(",") || ActiveNumber.Contains(".") || InProgressString.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static ICommand MakeOperationCommand { get; set; }
        private void OnMakeOperationCommandExecute(object p)
        {
            InProgressString += p.ToString();            
            ActiveNumber = "";
            OperationStatus = OperationStatus.InProgress;
        }
        private string forbiddens = "×^−+÷";
        private bool CanMakeOperationCommandExecuted(object p)
        {
            if (InProgressString.Length == 0 ||
                InProgressString.EndsWith(",") ||
                forbiddens.Contains(inProgressString[InProgressString.Length - 1]) ||
                OperationStatus == OperationStatus.InProgress)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static ICommand PercentCommand { get; set; }
        private void OnPercentCommandExecute(object p)
        {
            string s = ActiveNumber;
            string pointString = s.Replace(".", ",");            
            try
            {
                double number = double.Parse(pointString) / 100;
                ActiveNumber = number.ToString();
                int i = InProgressString.LastIndexOf(pointString);
                if (i != -1)
                {
                    InProgressString = InProgressString.Remove(i);
                    InProgressString += number.ToString();
                }
                InProgressNumber = CalculatorModel.ParserCalculate(InProgressString, InProgressNumber);
            }
            catch { }
            ChangeStatus();
        }
        private bool CanPercentCommandExecuted(object p)
        {
            if (ActiveNumber == "")
            {
                return false;
            }
            else
            {
                return true;
            }
                
        }
        public  static ICommand ClearCommand { get; set; }
        private void OnClearCommandExecute(object p)
        {
            InProgressString = "";
            InProgressNumber = "";
            ActiveNumber = "";
        }
        private bool CanClearCommandExecuted(object p)
        {
            return true;
        }
        public static ICommand ClearHistoryCommand { get; set; }
        private void OnClearHistoryCommandExecute(object p)
        { 
            HistoryOperations.Clear();
        }
        private bool CanClearHistoryCommandExecuted(object p)
        {
            return true;
        }
        public static ICommand RemoveCommand { get; set; }
        private void OnRemoveCommandExecute(object p)
        {
            InProgressString = InProgressString.Remove(InProgressString.Length - 1);
            if (ActiveNumber.Length > 0)
            {
                ActiveNumber = ActiveNumber.Remove(ActiveNumber.Length - 1);
            }
            InProgressNumber = CalculatorModel.ParserCalculate(InProgressString, InProgressNumber);
            ChangeStatus();
        }
        private bool CanRemoveCommandExecuted(object p)
        {
            if (InProgressString.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public CalculatorViewModel()
        {
            AddNumberCommand = new RelayCommand(OnAddNumberCommandExecute, CanAddNumberCommandExecuted);
            AddPointCommand = new RelayCommand(OnAddPointCommandExecute, CanAddPointCommandExecuted);
            MakeOperationCommand = new RelayCommand(OnMakeOperationCommandExecute, CanMakeOperationCommandExecuted);
            PercentCommand = new RelayCommand(OnPercentCommandExecute, CanPercentCommandExecuted);
            ClearCommand = new RelayCommand(OnClearCommandExecute, CanClearCommandExecuted);
            ClearHistoryCommand = new RelayCommand(OnClearHistoryCommandExecute, CanClearHistoryCommandExecuted);
            RemoveCommand = new RelayCommand(OnRemoveCommandExecute, CanRemoveCommandExecuted);
        }   
    }
}
