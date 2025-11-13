using System;
using System.Collections.Generic; // Для List<T>
using System.Text;              // Для Console.OutputEncoding
using System.Globalization;     // Для CultureInfo (щоб крапка була десятковим роздільником)

// Простір імен для нашої логіки
namespace FunctionHierarchy
{
    /**
     * 3. Описати клас "дробово-лінійна функція"
     * (a1*x + a0) / (b1*x + b0)
     */
    public class FractionalLinearFunction
    {
        // Константа для порівняння з нулем (як ви радили)
        protected const double Epsilon = 1e-9;

        // Властивості
        public double A1 { get; protected set; }
        public double A0 { get; protected set; }
        public double B1 { get; protected set; }
        public double B0 { get; protected set; }

        /// <summary>
        /// Конструктор за замовчуванням.
        /// </summary>
        public FractionalLinearFunction()
        {
            SetCoefficients(0, 0, 0, 0);
        }

        /// <summary>
        /// Конструктор для ініціалізації коефіцієнтів.
        /// </summary>
        public FractionalLinearFunction(double a1, double a0, double b1, double b0)
        {
            SetCoefficients(a1, a0, b1, b0);
        }

        /// <summary>
        /// Встановлює коефіцієнти функції.
        /// </summary>
        public void SetCoefficients(double a1, double a0, double b1, double b0)
        {
            A1 = a1;
            A0 = a0;
            B1 = b1;
            B0 = b0;
            // ПОКРАЩЕННЯ: Тут можна додати валідацію,
            // наприклад, if (Math.Abs(b1) < Epsilon && Math.Abs(b0) < Epsilon)
            // throw new ArgumentException("Знаменник не може бути тотожним нулем.");
        }

        /// <summary>
        /// Обчислює значення функції в заданій точці x.
        /// </summary>
        /// <returns>Результат функції f(x).</returns>
        /// <exception cref="System.DivideByZeroException">Кидається, якщо знаменник дорівнює 0.</exception>
        public virtual double Evaluate(double x)
        {
            double denominator = B1 * x + B0;
            if (Math.Abs(denominator) < Epsilon)
            {
                throw new DivideByZeroException($"Помилка: Знаменник (B1*x + B0) дорівнює 0 при x = {x}");
            }
            return (A1 * x + A0) / denominator;
        }

        /// <summary>
        /// Спроба обчислити значення функції (безпечний API, без винятків).
        /// </summary>
        /// <param name="x">Точка для обчислення.</param>
        /// <param name="result">Вихідний параметр для результату.</param>
        /// <returns>true, якщо обчислення успішне; false, якщо ділення на нуль.</returns>
        public bool TryEvaluate(double x, out double result)
        {
            // Цей метод НЕ віртуальний. Він викликає віртуальний метод Evaluate().
            // Завдяки поліморфізму буде викликано правильну реалізацію Evaluate()
            // (з базового або похідного класу).
            try
            {
                result = Evaluate(x);
                return true;
            }
            catch (DivideByZeroException)
            {
                result = double.NaN; // або 0, або інше значення за замовчуванням
                return false;
            }
        }

        /// <summary>
        /// Повертає рядкове представлення функції (ідіоматичний C#).
        /// </summary>
        public override string ToString()
        {
            // Використовуємо CultureInfo.InvariantCulture для гарантованого
            // відображення крапки як десяткового роздільника
            return string.Format(CultureInfo.InvariantCulture,
                "Дробово-лінійна функція: f(x) = ({0}*x + {1}) / ({2}*x + {3})",
                A1, A0, B1, B0);
        }
    }

    /**
     * Похідний клас "дробова квадратична функція"
     * (a2*x^2 + a1*x + a0) / (b2*x^2 + b1*x + b0)
     */
    public class FractionalQuadraticFunction : FractionalLinearFunction
    {
        // Додаткові властивості
        public double A2 { get; protected set; }
        public double B2 { get; protected set; }

        /// <summary>
        /// Конструктор за замовчуванням.
        /// </summary>
        public FractionalQuadraticFunction() : base()
        {
            A2 = 0;
            B2 = 0;
        }

        /// <summary>
        /// Конструктор для ініціалізації всіх коефіцієнтів.
        /// </summary>
        public FractionalQuadraticFunction(double a2, double a1, double a0, double b2, double b1, double b0)
            : base(a1, a0, b1, b0) // Викликаємо базовий конструктор
        {
            A2 = a2;
            B2 = b2;
        }

        /// <summary>
        /// Встановлює коефіцієнти функції (Перевантажений метод).
        /// </summary>
        public void SetCoefficients(double a2, double a1, double a0, double b2, double b1, double b0)
        {
            A2 = a2;
            B2 = b2;
            base.SetCoefficients(a1, a0, b1, b0); // Встановлюємо успадковані
        }

        /// <summary>
        /// Обчислює значення квадратичної функції (Перевизначений метод).
        /// </summary>
        public override double Evaluate(double x)
        {
            double denominator = B2 * x * x + B1 * x + B0;
            if (Math.Abs(denominator) < Epsilon)
            {
                throw new DivideByZeroException($"Помилка: Знаменник (B2*x^2 + ...) дорівнює 0 при x = {x}");
            }
            double numerator = A2 * x * x + A1 * x + A0;
            return numerator / denominator;
        }

        /// <summary>
        /// Повертає рядкове представлення функції (Перевизначений метод).
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "Дробова квадратична функція: f(x) = ({0}*x^2 + {1}*x + {2}) / ({3}*x^2 + {4}*x + {5})",
                A2, A1, A0, B2, B1, B0);
        }
    }

    /**
     * Головний клас програми для тестування
     */
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            // 1. Створення об'єктів
            // f(x) = (2x + 4) / (1x - 3)
            var func1 = new FractionalLinearFunction(2.0, 4.0, 1.0, -3.0);

            // f(x) = (1x^2 + 2x + 1) / (1x^2 + 0x + 1)
            var func2 = new FractionalQuadraticFunction(1.0, 2.0, 1.0, 1.0, 0.0, 1.0);

            // 2. Демонстрація поліморфізму
            // Створюємо список посилань на БАЗОВИЙ тип.
            // Завдяки upcasting, ми можемо додати об'єкти похідних класів.
            // При виклику віртуальних методів (Evaluate, ToString) буде
            // викликано версію з реального, а не посилального типу.
            List<FractionalLinearFunction> functions = new List<FractionalLinearFunction>
            {
                func1,
                func2
            };

            Console.WriteLine("=== Демонстрація поліморфізму (виклики через список базового типу) ===");
            Console.WriteLine("====================================================================\n");

            double x_safe = 5.0;
            double x_danger = 3.0; // Спричинить ділення на 0 для func1

            foreach (var func in functions)
            {
                // 3. Демонстрація override ToString()
                // Console.WriteLine() автоматично викликає func.ToString()
                Console.WriteLine(func);

                // 4. Демонстрація Evaluate() з try-catch
                Console.WriteLine($"  (A) Тест Evaluate() при x = {x_danger}:");
                try
                {
                    double result = func.Evaluate(x_danger);
                    Console.WriteLine($"      -> Результат: {result:F2}");
                }
                catch (DivideByZeroException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"      -> Помилка: {ex.Message}");
                    Console.ResetColor();
                }

                // 5. Демонстрація TryEvaluate() (безпечний API)
                Console.WriteLine($"  (B) Тест TryEvaluate() при x = {x_danger}:");
                if (func.TryEvaluate(x_danger, out double tryResult))
                {
                    Console.WriteLine($"      -> Результат: {tryResult:F2}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("      -> Помилка: Не вдалося обчислити (ділення на нуль).");
                    Console.ResetColor();
                }

                Console.WriteLine("\n-------------------------------------------------\n");
            }
        }
    }
}