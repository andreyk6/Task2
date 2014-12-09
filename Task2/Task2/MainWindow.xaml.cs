using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Task2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContinueHereTest();
        }

        public static void ContinueHereTest()
        {
            Console.WriteLine("UI ThreadID - " + Thread.CurrentThread.ManagedThreadId);

            Action mainTaskAction = () => Console.WriteLine("MainTask ThreadID - " + Thread.CurrentThread.ManagedThreadId);

            Action mainContinuationAction = new Action(() => Console.WriteLine("MainAction ThreadID - " + Thread.CurrentThread.ManagedThreadId));

            Action<Task> mainContinuationActionWithTask = new Action<Task>((t) => {
                Console.WriteLine("MainAction<Task> ThreadID - " + Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("MainAction<Task> Task.IsCompleted - " + t.IsCompleted);
                });

            #region Action
            Task task1 = new Task(mainTaskAction);

            task1.ContinueHereWith(mainContinuationAction);

            task1.Start();
            task1.Wait();
            #endregion

            #region Action<Task>
            Task task2 = new Task(mainTaskAction);

            task2.ContinueWith((t) => Console.WriteLine("Task ContinueWith() action...."));

            task2.ContinueHereWith(mainContinuationActionWithTask);

            task2.Start();
            task2.Wait();
            #endregion

            #region Func<Task,T>
            Func<int> continuationFunc = new Func<int>(() => {
                Console.WriteLine("continationFunction ThreadID - " + Thread.CurrentThread.ManagedThreadId);
                return 10;
                });


            Task<int> task3 = new Task<int>(()=> {
                mainTaskAction();
                return 0;
                });

            task3.ContinueWith((t) => Console.WriteLine("Task ContinueWith() action...."));
            task3.ContinueHereWith(continuationFunc);
            task3.Start();
            task3.Wait();
            #endregion

            Console.WriteLine(task3.Result);
    }
    }
}
