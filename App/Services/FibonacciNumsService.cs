using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;

namespace Task1;

public class FibonacciNumsService
{
    private TextBox TextBox { get; init; }
    private CancellationTokenSource CtsSource { get; init; }
    private ManualResetEventSlim PauseEvent { get; init; }

    public FibonacciNumsService(TextBox textBox)
    {
        TextBox = textBox;
        CtsSource = new CancellationTokenSource();
        PauseEvent = new ManualResetEventSlim(true);
    }

    public void ExecuteComputing()
    {
        var newThread = new Thread(() =>
        {
            ComputeFibonacci();
        });
        newThread.Start();
    }
    
    public void ComputeFibonacci()
    {
        try
        {
            var token = CtsSource.Token;
            long num1 = 1;
            long num2 = 1;
            int counter = 0;
            while (true)
            {
                token.ThrowIfCancellationRequested();
                PauseEvent.Wait(token);
                counter++;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TextBox.Text += $"{num2} ";
                    if (counter % 5 == 0)
                        TextBox.Text += "\n";
                });
                // Невелика пауза для явної демонстрації багатопоточності
                // Так краще видно що числа рахуються не всі одразу а послідовно, і ми можемо махати вікном у цей час
                Thread.Sleep(200);

                long oldNum2 = num2;
                num2 = num2 + num1;
                num1 = oldNum2;
            }
        }
        catch (OperationCanceledException)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TextBox.Text += "\nComputing Fibonacci Stopped";
            });
        }
    }

    public void StopComputing() => CtsSource.Cancel();
    
    public void PauseComputing() => PauseEvent.Reset();
    
    public void ResumeComputing() => PauseEvent.Set();
}