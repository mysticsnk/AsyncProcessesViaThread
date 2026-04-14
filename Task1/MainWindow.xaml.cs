using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public PrimeNumsService PrimeNumsService { get; set; }
    public FibonacciNumsService FibonacciNumsService { get; set; }
    public MainWindow()
    {
        InitializeComponent();
        PrimeNumsService = new PrimeNumsService(PrimeNumsTextBox);
        FibonacciNumsService = new FibonacciNumsService(FibonacciNumsTextBox);
    }

    private void PrimeNumsButton_OnClick(object sender, RoutedEventArgs e)
    {
        long? lowerBoundary;
        if (long.TryParse(LowerBoundaryTextBox.Text, out var lowerBoundaryValue))
            lowerBoundary = lowerBoundaryValue;
        else
            lowerBoundary = null;

        long? upperBoundary;
        if (long.TryParse(UpperBoundaryTextBox.Text, out long upperBoundaryValue))
            upperBoundary = upperBoundaryValue;
        else
            upperBoundary = null;
        
        PrimeNumsService.ExecuteComputing(lowerBoundary, upperBoundary);
    }

    

    private void FibonacciNumsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var fibonacciNumsThread = new Thread(() =>
        {
            FibonacciNumsService.ComputeFibonacci();
        });
        
        fibonacciNumsThread.Start();
    }

    

    private void StopPrimeNumsButton_OnClick(object sender, RoutedEventArgs e) => PrimeNumsService.StopComputing();
    private void PausePrimeNumsButton_OnClick(object sender, RoutedEventArgs e) => PrimeNumsService.PauseComputing();
    private void ResumePrimeNumsButton_OnClick(object sender, RoutedEventArgs e) => PrimeNumsService.ResumeComputing();

    private void StopFibonacciNumsButton_OnClick(object sender, RoutedEventArgs e) => FibonacciNumsService.StopComputing();
    private void PauseFibonacciNumsButton_OnClick(object sender, RoutedEventArgs e) => FibonacciNumsService.PauseComputing();
    private void ResumeFibonacciNumsButton_OnClick(object sender, RoutedEventArgs e) => FibonacciNumsService.ResumeComputing();

    private void ResetPrimeNumsButton_OnClick(object sender, RoutedEventArgs e)
    {
        PrimeNumsService.StopComputing();
        PrimeNumsService = new PrimeNumsService(PrimeNumsTextBox);
        PrimeNumsTextBox.Clear();
    }

    private void ResetFibonacciNumsButton_OnClick(object sender, RoutedEventArgs e)
    {
        FibonacciNumsService.StopComputing();
        FibonacciNumsService = new FibonacciNumsService(FibonacciNumsTextBox);
        FibonacciNumsTextBox.Clear();
    }
}