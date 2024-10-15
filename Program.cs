/*
Разработать программу, которая выполняет: 
1. Генерацию массива числовых данных размера N со случайным
распределением значений элементов массива, подчиняющихся вероятностному
закону распределения. 
2. Сортировку исходного массива простыми и сложными методами (по 3 
каждого вида). 
3. Провести сравнительный анализ простых и сложных методов
сортировки элементов массива, рассчитав показатели производительности
(время сортировки и соотношение методов производительности (относительное
время сортировки)). Результаты анализа представить в табличной форме
записи. 
4. Определить оценку качества, реализованных в программе простых
методов сортировки (выбором, вставками, обменом) по двум показателям: 
C / N и  M / N, 
где С – количество операций сравнения элементов массива; 
М – количество перестановок элементов массива, потребовавшихся для
сортировки массива. 
Результаты представить в табличной форме записи. 

Количество элементов в массиве N: 3000
Вероятностный закон распределения: Burr
*/

//yield генераторы чё-то генерят

using System;
using System.Collections.Concurrent;
using System.Numerics;
using System.Transactions;
using System.Diagnostics;
using System.Security.AccessControl;

class Program
{
    static void Main()
    {
        //Заголовок программы
        Console.WriteLine("Алгоритмы сортировки данных в оперативной памяти\n");

        int arrSize = 3000;
        //Параметры вероятностного закона Burr
        double a, b, c, d;

        //Ввод данных для вероятностного закона Burr
        Console.WriteLine("Ввод параметров вероятностного закона Burr:");
        Console.WriteLine("Введите параметр А: ");
        a = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Введите параметр B: ");
        b = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Введите параметр C: ");
        c = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine("Введите параметр D: ");
        d = Convert.ToDouble(Console.ReadLine());

        //Генерация массива данных
        double[] numbers = GenerateBurrArray(arrSize, a, b, c, d);

        //Сортировка исходного массива простыми методами
        double[] arr1 = (double[])numbers.Clone();
        double[] arr2 = (double[])numbers.Clone();
        double[] arr3 = (double[])numbers.Clone();

        Stopwatch sw = new();

        //Сортировка пузырьком
        sw.Start();
        BubbleSort(arr1);
        sw.Stop();
        TimeSpan bubleSortTime = sw.Elapsed;

        //Сортировка простым выбором
        sw.Restart();
        SelectionSort(arr2);
        sw.Stop();
        TimeSpan selectionSortTime = sw.Elapsed;

        //Сортировка вставками
        sw.Restart();
        InsertionSort(arr3);
        sw.Stop();
        TimeSpan insertionSortTime = sw.Elapsed;

        //Сортировка исходного массива непростыми методами
        double[] arr4 = (double[])numbers.Clone();
        double[] arr5 = (double[])numbers.Clone();
        double[] arr6 = (double[])numbers.Clone();

        //Сортировка Хоара (Быстрая сортировка)
        sw.Restart();
        HoareSort(arr4, 0, arr4.Length - 1);
        sw.Stop();
        TimeSpan hoareSortTime = sw.Elapsed;

        //Сортировка Шелла
        sw.Restart();
        ShellSort(arr5);
        sw.Stop();
        TimeSpan shellSortTime = sw.Elapsed;

        //Сортировка кучей
        sw.Restart();
        HeapSort(arr6);
        sw.Stop();
        TimeSpan heapSortTime = sw.Elapsed;

        //Оценка качества сортировок
        Console.WriteLine("\n\"Оценка качества сортировок\"");
        Console.WriteLine("Метод сортировки\t\tВремя сортировки(мс)\tC/N\t\tM/N");
        Console.WriteLine("Сортировка пузырьком\t\t{0}\t\t\t{1}\t{2}", bubleSortTime.Milliseconds.ToString(),
            ((double)BubbleSortComparisons / 3000).ToString("F3"), ((double)BubbleSortSwaps / 3000).ToString("F3"));
        Console.WriteLine("Сортировка вставками\t\t{0}\t\t\t{1}\t\t{2}", insertionSortTime.Milliseconds.ToString(),
            ((double)InsertionSortComparisons / 3000).ToString("F3"), ((double)InsertionSortSwaps / 3000).ToString("F3"));
        Console.WriteLine("Сортировка выбором\t\t{0}\t\t\t{1}\t{2}", selectionSortTime.Milliseconds.ToString(),
            ((double)SelectionSortComparisons / 3000).ToString("F3"), ((double)SelectionSortSwaps / 3000).ToString("F3"));
        Console.WriteLine("Сортировка Шелла\t\t{0}\t\t\t{1}\t\t{2}", shellSortTime.Milliseconds.ToString(),
            ((double)ShellSortComparisons / 3000).ToString("F3"), ((double)ShellSortSwaps / 3000).ToString("F3"));
        Console.WriteLine("Сортировка Хоара\t\t{0}\t\t\t{1}\t\t{2}", hoareSortTime.Milliseconds.ToString(),
            ((double)HoareSortComparisons / 3000).ToString("F3"), ((double)HoareSortSwaps / 3000).ToString("F3"));
        Console.WriteLine("Сортировка кучей\t\t{0}\t\t\t{1}\t\t{2}", heapSortTime.Milliseconds.ToString(),
            ((double)HeapSortComparisons / 3000).ToString("F3"), ((double)HeapSortSwaps / 3000).ToString("F3"));
    }

    //Генерация чисел массива с помощью вероятностного закона Burr
    //Аргументы: размерность массива, параметры вероятностного закона
    static double[] GenerateBurrArray(int size, double a, double b, double d, double c)
    {
        double[] numbers = new double[size];
        Random random = new();

        for (int i = 0; i < size; i++)
        {
            double u = random.NextDouble();

            numbers[i] = a + b * Math.Pow((Math.Pow(u, -1 / (d + 1)) - 1), -1 / c);
        }

        return numbers;
    }

    //Сортировка пузырьком
    //Аргументы: ссылка на сортируемый массив
    static int BubbleSortComparisons = 0;
    static int BubbleSortSwaps = 0;
    static void BubbleSort(double[] arr)
    {
        int arrSize = arr.Length;
        bool swapped;
        do
        {
            swapped = false;
            for (int i = 0; i < arrSize - 1; i++)
            {
                if (arr[i] > arr[i + 1])
                {
                    (arr[i], arr[i + 1]) = (arr[i + 1], arr[i]);

                    swapped = true;

                    BubbleSortSwaps++;
                }
                BubbleSortComparisons++;
            }
            arrSize--;
        } while (swapped);
    }

    //Сортировка простым выбором
    //Аргументы: ссылка на сортируемый массив
    static int SelectionSortComparisons = 0;
    static int SelectionSortSwaps = 0;
    static void SelectionSort(double[] arr)
    {
        int arrSize = arr.Length;
        for (int i = 0; i < arrSize - 1; i++)
        {
            int maxIndex = i; //Элемент с наибольшим значением ключа (в дальнейшем)
            for (int j = i + 1; j < arrSize; j++)
            {
                if (arr[j] < arr[maxIndex])
                {
                    maxIndex = j;
                }
                SelectionSortComparisons++;
            }
            if (maxIndex != i) //Замена элементов
            {
                (arr[maxIndex], arr[i]) = (arr[i], arr[maxIndex]);

                SelectionSortSwaps++;
            }
        }
    }

    //Сортировка вставками
    //Аргументы: ссылка на сортируемый массив
    static int InsertionSortComparisons = 0;
    static int InsertionSortSwaps = 0;
    static void InsertionSort(double[] arr)
    {
        for(int i = 1; i < arr.Length; i++)
        {
            int j = i;
            while(j > 0 && arr[j - 1] > arr[j])
            {
                (arr[j], arr[j - 1]) = (arr[j - 1], arr[j]);
                j--;
                InsertionSortComparisons++;
                InsertionSortSwaps++;
            }
            InsertionSortComparisons++;
        }
    }

    //Сортировка Шелла
    //Аргументы: ссылка на сортируемый массив
    static int ShellSortComparisons = 0;
    static int ShellSortSwaps = 0;
    static void ShellSort(double[] arr)
    {
        int arrSize = arr.Length;
        int step = arrSize / 2; //Шаг (расстояние между элементами)
        while (step > 0)
        {
            for (int i = step; i < arrSize; i++) //Проходка с позиции до конца
            {
                int j = i;
                double temp = arr[i];
                //Проходка по элементам, отстоющим на d = d/2 позиций
                while(j >= step && arr[j - step] > temp)
                {
                    arr[j] = arr[j - step];
                    j -= step;

                    ShellSortSwaps++;
                }
                arr[j] = temp;

                ShellSortComparisons++;
            }
            step /= 2;
        }
    }

    //Сортировка Хоара (быстрая сортировка)
    //Аргументы: ссылка на сортируемый массив, начальная и конечная позиции сортировки
    static int HoareSortComparisons = 0;
    static int HoareSortSwaps = 0;
    static void HoareSort(double[] arr, int start, int end)
    {
        if (start < end)
        {
            //Индекс опорного элемента
            int pivot = Partition(arr, start, end);

            HoareSort(arr, start, pivot - 1);
            HoareSort(arr, pivot + 1, end);
        }
    }

    static int Partition(double[] arr, int start, int end)
    {
        double pivot = arr[end]; //Последний элемент в качестве опорного
        int i = start - 1;

        for (int j = start; j < end; j++)
        {
            HoareSortComparisons++;

            if (arr[j] < pivot)
            {
                i++;

                HoareSortSwaps++;

                (arr[j], arr[i]) = (arr[i], arr[j]);
            }
        }
        //Перемещаем опорный элемент на свое место
        double temp2 = arr[i + 1];
        arr[i + 1] = pivot;
        arr[end] = temp2;

        HoareSortSwaps++;

        return i + 1;
    }

    //Сортировка кучей
    //Аргументы: ссылка на сортируемый массив
    static int HeapSortComparisons = 0;
    static int HeapSortSwaps = 0;
    static void HeapSort(double[] arr)
    {
        int arrSize = arr.Length;

        //Построение кучи
        for(int i = arrSize / 2 - 1; i >= 0; i--)
        {
            Heapify(arr, arrSize, i);
        }

        //Извлечение элементов из кучи
        for(int i = arrSize - 1; i >= 0; i--)
        {
            //Текущий корень - в конец
            (arr[i], arr[0]) = (arr[0], arr[i]);

            HeapSortSwaps++;

            Heapify(arr, i, 0);
        }
    }

    static void Heapify(double[] arr, int arrSize, int i)
    {
        int largest = i; //Наибольший элемент - корень
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        //Если левый потомок больше корня
        if(left < arrSize)
        {
            HeapSortComparisons++;
            if (arr[left] > arr[largest])
            {
                largest = left;
            }
        }

        //Если правый потомок больше наибольшего элемента
        if(right < arrSize)
        {
            HeapSortComparisons++;
            if (arr[right] > arr[largest])
            {
                largest = right;
            }
        }

        //Если наибольший элемент не корень
        if(largest != i)
        {
            (arr[largest], arr[i]) = (arr[i], arr[largest]);

            HeapSortSwaps++;

            Heapify(arr, arrSize, largest);
        }
    }
}
