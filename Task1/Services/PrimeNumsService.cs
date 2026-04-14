using System.Windows;
using System.Windows.Controls;

namespace Task1;

public class PrimeNumsService
{
    private TextBox TextBox { get; init; }
    
    // Повний стоп вирішив зробити через cancellationToken
    private CancellationTokenSource CtsSource { get; init; }
    
    // Паузу вирішив зробити через цю смішну штуку, працює класно
    // PauseEvent.Wait() - слухає, в залежності від стану або паузить або пропускається 
    // PauseEvent.Reset() - змінює стан на паузу
    // PauseEvent.Set() - змінює стан на продовження
    private ManualResetEventSlim PauseEvent { get; init; }

    public PrimeNumsService(TextBox textBox)
    {
        TextBox = textBox;
        CtsSource = new CancellationTokenSource();
        PauseEvent = new ManualResetEventSlim(true);
    }

    public void ExecuteComputing(long? lowerBoundary, long? upperBoundary)
    {
        var newThread = new Thread(() => { ComputePrimes(lowerBoundary, upperBoundary); });
        newThread.Start();
    }
    
    public void ComputePrimes(long? lowerBoundaryValue, long? upperBoundary)
    {
        try
        {
            var token = CtsSource.Token;

            long lowerBoundary = lowerBoundaryValue ?? 2;
            int counter = 0;

            for (long n = lowerBoundary; n <= (upperBoundary ?? long.MaxValue); n++)
            {
                token.ThrowIfCancellationRequested();
                PauseEvent.Wait(token);
                bool isPrime = true;
                for (long d = 2; d * d <= n; d++)
                {
                    if (n % d == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                {
                    counter++;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        TextBox.Text += $"{n} ";
                        if (counter % 8 == 0)
                            TextBox.Text += "\n";
                    });
                    // Невелика пауза для явної демонстрації багатопоточності
                    // Так краще видно що числа рахуються не всі одразу а послідовно, і ми можемо махати вікном у цей час
                    Thread.Sleep(100);
                }
            }
        }
        catch (OperationCanceledException)
        {
            Application.Current.Dispatcher.Invoke(() => TextBox.Text += "\nPrimes Computing Stopped.");
        }
    }

    public void StopComputing() => CtsSource.Cancel();

    public void PauseComputing() => PauseEvent.Reset();
    
    public void ResumeComputing() => PauseEvent.Set();
}