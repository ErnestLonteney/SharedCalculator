using DevExpress.Mvvm;
using System;
using System.Threading.Tasks;

namespace SharedCalculator
{
    public class MainWindowViewModel : ViewModelBase 
    {
        public IAsyncCommand InputDigitCommand { get; }
        public IAsyncCommand BackToZeroCommand { get; }   
        public IAsyncCommand SignCommand { get; }
        public IAsyncCommand ResultCommand { get; } 
        public IAsyncCommand BackspaceCommand { get; }
        public IAsyncCommand PercentCommand { get; }
        public IAsyncCommand PowCommand { get; }
        public AsyncCommand SqrtCommand { get; }
        public AsyncCommand AddMinusCommand { get; }   
        public AsyncCommand OneDivideCommand { get; }

        double result = 0;
        string currentValue = "0";
        char sign;
        double? left = null, right = null;
        bool newInput = true;

        public MainWindowViewModel()
        {
            InputDigitCommand = new AsyncCommand<string>(DigitCommandExecute);
            BackToZeroCommand = new AsyncCommand(BackToZeroExecute);
            SignCommand = new AsyncCommand<char>(SignCommandExecute);
            ResultCommand = new AsyncCommand(ResultCommandExecute, CanResultCalculate);
            BackspaceCommand = new AsyncCommand(BackspaceCommandExecute);
            PercentCommand = new AsyncCommand(PercentCommandExecute, CanResultCalculate);
            PowCommand = new AsyncCommand(PowCommandExecute, UnaryCanExecute);
            SqrtCommand = new AsyncCommand(SqrtCommandExecute, UnaryCanExecute);
            AddMinusCommand = new AsyncCommand(AddMinusCommandExecute, UnaryCanExecute);
            OneDivideCommand = new AsyncCommand(OneDivideCommandExecute, UnaryCanExecute);
        }

        private double Substract(double left, double right)
        {
            return left - right;
        }      

        public string CurrentValue
        {
            get => currentValue;
            set 
            {
                if (value == "CE")
                { 
                    currentValue = "0"; 
                }
                else if (CurrentValue == "0" || newInput)
                {
                    currentValue = value;
                }
                else
                {
                    currentValue += value;
                }

                RaisePropertyChanged(nameof(CurrentValue));
            }
        }

        #region Commands
        Task DigitCommandExecute(string parameter)
        {
            CurrentValue = parameter;
            newInput = false;

            return Task.CompletedTask;
        }

        Task BackToZeroExecute()
        {
            CurrentValue = "CE";
            result = 0;
            left = right = null;

            return Task.CompletedTask;
        }

        Task SignCommandExecute(char parameter)
        {
            left = Convert.ToDouble(CurrentValue);
            sign = parameter;
            newInput = true;

            return Task.CompletedTask;
        }

        Task ResultCommandExecute()
        {
            right = Convert.ToDouble(CurrentValue);
            switch (sign)
            {
                case '+':  // TODO Implement adding here
                    result = 0;
                    break;
                case '-':
                    result = Substract(left.Value, right.Value);
                    break;
                case '/':
                    {
                        bool divideByZero;
                        result = CalculationDivided(left.Value, right.Value, out divideByZero);
                        if (divideByZero) {
                            CurrentValue = "Divide by zero!";
                        } else 
                        {
                            CurrentValue = result.ToString();
                        }
                    }
                    break;
                case '*':
                    // TODO Implement mulitiply 
                    result = 0;
                    break;
            }

            newInput = true;
            left = right = null;
            CurrentValue = result.ToString();
        

            return Task.CompletedTask;
        }

        Task BackspaceCommandExecute()
        {
            if (currentValue == "0")
                return Task.CompletedTask;

            if (currentValue.Length == 1)
            {
                currentValue = "0";
            }
            else
            {
                currentValue = CurrentValue.Remove(CurrentValue.Length - 1);
            }

            RaisePropertyChanged(nameof(CurrentValue));

            return Task.CompletedTask;
        }


        Task PercentCommandExecute()
        {
            right = Convert.ToDouble(CurrentValue);
            var result = calculatePercents(left.Value, right.Value);
            newInput = true;
            CurrentValue = result.ToString();

            RaisePropertiesChanged(nameof(CurrentValue));

            newInput = true;
            return Task.CompletedTask;
        }

        Task PowCommandExecute()
        {
            left = Convert.ToDouble(CurrentValue);
            result = Pow(left.Value);
            newInput = true;
            CurrentValue = result.ToString();

            return Task.CompletedTask;
        }

        Task SqrtCommandExecute()
        {
            // TODO Implement sqrt here
            CurrentValue = "result here";

            return Task.CompletedTask;
        }

        Task AddMinusCommandExecute()
        {
            // TODO Implement adding minus here
            currentValue = "result here";

            RaisePropertyChanged(nameof(CurrentValue));

            return Task.CompletedTask;
        }

        Task OneDivideCommandExecute()
        {
            // TODO Implement 1 / n 
            left = 0;  // Get left side here
            var res = 0; // Call dividing method here
            newInput = true;
            bool divideByZero = false;


            if (divideByZero)
            {
                CurrentValue = "Divide by zero!";
                return Task.CompletedTask;
            }
            else
            {
                CurrentValue = res.ToString();
            }

            return Task.CompletedTask;
        }

        double CalculationDivided(double left, double right, out bool divedeByZero) 
        {
            if (right == 0) 
            {
                divedeByZero = true;
                return double.NaN;
            } 
            else 
            {
                divedeByZero = false;
                return (double)left / right;
            }
        }


        double calculatePercents(double value, double percents)
        {
            return value / 100 * percents;
        }

        bool CanResultCalculate() => left.HasValue && newInput == false;

        bool UnaryCanExecute() => currentValue != "0" && !right.HasValue;
        #endregion
        public static double Pow(double n) => Math.Pow(n, 2);
    }
}
