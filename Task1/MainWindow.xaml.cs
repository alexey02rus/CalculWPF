using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using parser = org.mariuszgromada.math.mxparser;

namespace Task1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            CalculatorViewModel cvm = new CalculatorViewModel();
            this.calculatorViewModel = cvm;
            DataContext = cvm;
            InitializeComponent();
        }
        public CalculatorViewModel calculatorViewModel { get; set; }       
        
        private void Enter_Click(object sender, RoutedEventArgs e)
        {            
            if (calculatorViewModel.OperationStatus == OperationStatus.Calculated && 
                string.IsNullOrEmpty(calculatorViewModel.InProgressString) == false)
            {
                InProgressNumber.Text = CalculatorModel.ParserCalculate(InProgressString.Text, InProgressNumber.Text);
                string s = InProgressString.Text + " = " + calculatorViewModel.InProgressNumber;                
                TextBlock tb = new TextBlock();
                tb.Text = s;
                tb.MouseUp += History_Click;
                tb.Foreground = new SolidColorBrush(Color.FromRgb(253,253,251));                
                tb.Visibility = Visibility.Visible;
                tb.TextWrapping = TextWrapping.Wrap;                
                tb.HorizontalAlignment = HorizontalAlignment.Stretch;                
                HistoryPanel.Items.Add(tb);
                CalculatorViewModel.ClearCommand.Execute(s);
            }
        }
        private void History_Click(object sender, RoutedEventArgs e)
        {
            string s = (sender as TextBlock).Text;
            s=s.Remove(s.IndexOf(" ="));
            InProgressString.Text = s;
            InProgressNumber.Text = CalculatorModel.ParserCalculate(InProgressString.Text, InProgressNumber.Text);
        }

        private void HistoryClear_Click(object sender, RoutedEventArgs e)
        {
            HistoryPanel.Items.Clear();
        }
    }
}
