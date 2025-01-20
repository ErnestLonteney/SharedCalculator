using DevExpress.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;

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

        private double Add(double left, double right)
        {
            return left + right;
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
                    result = Add(left.Value, right.Value); ;
                    break;
                case '-':
                    result = Substract(left.Value, right.Value);
                    break;
                case '/':
                    {
                        double temp = DivideOperation(left, right, out bool DivideByZero);
                        
                        if (DivideByZero)
                        {
                            newInput = true;
                            CurrentValue = "Divide by zero!";
                            return Task.CompletedTask;
                        }
                        else
                        {
                            result = temp;
                        }
                    }
                    break;
                case '*':
                    result = Mulitiply((double)left, (double)right);
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
            result = Math.Pow(Convert.ToDouble(CurrentValue), 2);
            newInput = true;
            CurrentValue = result.ToString();

            return Task.CompletedTask;
        }

        Task SqrtCommandExecute()
        {
            // TODO Implement sqrt here
            left = Convert.ToDouble(CurrentValue);
            result = Math.Sqrt(left.Value);
            newInput = true;
            CurrentValue = result.ToString();

            return Task.CompletedTask;
        }

        Task AddMinusCommandExecute()
        {

            if (double.TryParse(currentValue, out double numValue))
            {
                numValue *= -1;
                currentValue = numValue.ToString();
            }
            else
            {
                currentValue = "0";
            }

            RaisePropertyChanged(nameof(CurrentValue));

            return Task.CompletedTask;
        }

        Task OneDivideCommandExecute()
        {
            // TODO Implement 1 / n 
            double right = Convert.ToDouble(CurrentValue);
            var res = CalculationDivided(1, right, out bool divideByZero); 
            newInput = true;
           


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
      
        double Mulitiply(double a, double b)
        { return a * b; }

        double DivideOperation(double? dividend, double? divisor, out bool DivideOnZero)
        {
            if (divisor == 0)
            { 
                DivideOnZero = true;
                return 0;
            }
            else 
            {  
                DivideOnZero = false;
                return Convert.ToDouble(dividend / divisor);
            }
        }

        bool UnaryCanExecute() => currentValue != "0" && !right.HasValue;

        double Substration(double a, double b)
        { 
            return a - b;
        }
        {
    class System
    {
        static void CalculateDescriminant (float a, float b, float c)
        {
            float descriminant = (float)Math.Pow(b,2) - 4 * a * c;
            
            Console.Write($"Дескримінант дорівнює: {descriminant}");

            float x1;
            float x2;

//Написати програму яка буде вираховувати квадратні корені рівняння. Нагадую що є три сценарія - коли Д <0 Д>0 і Д==0
//Якщо дискримінант менше нуля (D < 0), рівняння не має дійсних коренів.
            if (descriminant < '0')
            {
            Console.WriteLine(" рівняння не має дійсних коренів");
            }
//Якщо дискримінант дорівнює 0 (D = 0), рівняння має один дійсний корінь.
           if (descriminant == '0')
           {
           Console.WriteLine(" рівняння має один дійсний корінь");
           x1 = -b/(2*a);
           Console.WriteLine($"корінь дорівнює:{x1}");
           }

//Якщо дискримінант більший за 0 (D > 0), рівняння має два різних дійсних коренів.
           if (descriminant > '0')
           {
           Console.WriteLine(" рівняння має два різних дійсних корені.");
           x1 = -b+(float)Math.Sqrt(descriminant)/(2*a);
           x2 = -b-(float)Math.Sqrt(descriminant)/(2*a);
           Console.WriteLine($"перший корінь дорівнює:{x1}, другий корінь дорівнює:{x2}");
           }
        }

        static void Main()
        {
            Console.WriteLine("Input a please");
            float a = float.Parse(Console.ReadLine());
            Console.WriteLine("Input b please");
            float b = float.Parse(Console.ReadLine());
            Console.WriteLine("Input c please");
            float c = float.Parse(Console.ReadLine());

            CalculateDescriminant (a, b, c);
        }

    }
}
        #endregion
        //new commit
    }
