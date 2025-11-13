using System;
using System.Collections.Generic; // Для List<T>
using System.Text;              // Для Console.OutputEncoding

// Простір імен для нашої логіки (відповідає C# Code Conventions)
namespace FunctionHierarchy
{
    /**
     * 3. Описати клас "дробово-лінійна функція"
     * (a1*x + a0) / (b1*x + b0)
     */
    public class FractionalLinearFunction
    {
        // Властивості (Properties) згідно C# conventions (PascalCase)
        // 'protected set' дозволяє похідним класам змінювати ці значення
        public double A1 { get; protected set; }
        public double A0 { get; protected set; }
        public double B1 { get; protected set; }
        public double B0 { get; protected set; }

        /// <summary>
        /// Конструктор за замовчуванням.
        /// </summary>
        public FractionalLinearFunction()
        {
            // Ініціалізуємо нулями, щоб уникнути невизначеного стану
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
        /// Метод: завдання коефіцієнтів чисельника та знаменника.
        /// </summary>
        public void SetCoefficients(double a1, double a0, double b1, double b0)
        {
            A1 = a1;
            A0 = a0;
            B1 = b1;
            B0 = b0;
        }

        /// <summary>
        /// Метод: виведення коефіцієнтів на екран.
        /// (virtual - дозволяє перевизначення в похідних класах)
        /// </summary>
        public virtual void PrintCoefficients()
        {
            Console.WriteLine("--- Дробово-лінійна функція ---");
            Console.WriteLine("Формула: (A1*x + A0) / (B1*x + B0)");
            Console.WriteLine($"Чисельник (A1, A0): {A1}, {A0}");
            Console.WriteLine($"Знаменник (B1, B0): {B1}, {B0}");
        }

        /// <summary>
        /// Метод: знаходження значення в заданій точці x.
        /// (virtual - дозволяє перевизначення в похідних класах)
        /// </summary>
        /// <param name="x">Точка x0 для обчислення.</param>
        /// <returns>Результат функції f(x).</returns>
        /// <exception cref="System.DivideByZeroException">Кидається, якщо знаменник дорівнює 0.</exception>
        public virtual double Evaluate(double x)
        {
            double denominator = B1 * x + B0;

            // Перевірка ділення на нуль з похибкою для double
            if (Math.Abs(denominator) < 1e-9)
            {
                // Кидаємо інформативний виняток, як рекомендовано
                throw new DivideByZeroException($"Помилка обчислення: Знаменник (B1*x + B0) дорівнює 0 при x = {x}");
            }

            double numerator = A1 * x + A0;
            return numerator / denominator;
        }
    }

    /**
     * Створити похідний від нього клас "дробова функція"
     * (a2*x^2 + a1*x + a0) / (b2*x^2 + b1*x + b0)
     */
    public class FractionalFunction : FractionalLinearFunction
    {
        // Додаткові властивості
        public double A2 { get; protected set; }
        public double B2 { get; protected set; }

        /// <summary>
        /// Конструктор за замовчуванням.
        /// Викликає базовий конструктор.
        /// </summary>
        public FractionalFunction() : base() // base() викликає FractionalLinearFunction()
        {
            A2 = 0;
            B2 = 0;
        }

        /// <summary>
        /// Конструктор для ініціалізації всіх коефіцієнтів.
        /// </summary>
        public FractionalFunction(double a2, double a1, double a0, double b2, double b1, double b0)
            : base(a1, a0, b1, b0) // Викликаємо базовий конструктор для A1, A0, B1, B0
        {
            A2 = a2;
            B2 = b2;
        }

        /// <summary>
        /// Метод: завдання коефіцієнтів (Перевантажений метод).
        /// </summary>
        public void SetCoefficients(double a2, double a1, double a0, double b2, double b1, double b0)
        {
            // Встановлюємо власні коефіцієнти
            A2 = a2;
            B2 = b2;
            // Викликаємо метод базового класу для встановлення успадкованих
            base.SetCoefficients(a1, a0, b1, b0);
        }

        /// <summary>
        /// Метод: виведення коефіцієнтів на екран (Перевизначений метод).
        /// 'override' вказує, що ми замінюємо 'virtual' метод бази.
        /// </summary>
        public override void PrintCoefficients()
        {
            Console.WriteLine("--- Дробова (квадратична) функція ---");
            Console.WriteLine("Формула: (A2*x^2 + A1*x + A0) / (B2*x^2 + B1*x + B0)");
            Console.WriteLine($"Чисельник (A2, A1, A0): {A2}, {A1}, {A0}");
            Console.WriteLine($"Знаменник (B2, B1, B0): {B2}, {B1}, {B0}");
        }

        /// <summary>
        /// Метод: знаходження значення в заданій точці x (Перевизначений метод).
        /// </summary>
        /// <param name="x">Точка x0 для обчислення.</param>
        /// <returns>Результат функції f(x).</returns>
        /// <exception cref="System.DivideByZeroException">Кидається, якщо знаменник дорівнює 0.</exception>
        public override double Evaluate(double x)
        {
            double denominator = B2 * x * x + B1 * x + B0;

            // Перевірка ділення на нуль
            if (Math.Abs(denominator) < 1e-9)
            {
                throw new DivideByZeroException($"Помилка обчислення: Знаменник (B2*x^2 + B1*x + B0) дорівнює 0 при x = {x}");
            }

            double numerator = A2 * x * x + A1 * x + A0;
            return numerator / denominator;
        }
    }

    /**
     * Головний клас програми для тестування
     */
    public class Program
    {
        /// <summary>
        /// Створити об’єкти класів та обчислити їх значення,
        /// демонструючи поліморфізм.
        /// </summary>
        public static void Main(string[] args)
        {
            // Встановлюємо кодування консолі для коректного відображення кирилиці
            Console.OutputEncoding = Encoding.UTF8;

            // 1. Створення об'єктів через конструктори

            // f(x) = (2x + 4) / (1x - 3)
            // Ця функція має розрив (ділення на 0) в точці x = 3
            FractionalLinearFunction func1 = new FractionalLinearFunction(2.0, 4.0, 1.0, -3.0);

            // f(x) = (1x^2 + 2x + 1) / (1x^2 + 0x + 1)
            FractionalFunction func2 = new FractionalFunction(1.0, 2.0, 1.0, 1.0, 0.0, 1.0);

            // 2. Демонстрація поліморфізму (як вимагалось у відгуку)
            // Створюємо список (або масив) посилань на БАЗОВИЙ тип
            List<FractionalLinearFunction> functions = new List<FractionalLinearFunction>();
            functions.Add(func1); // Додаємо об'єкт базового класу
            functions.Add(func2); // Додаємо об'єкт ПОХІДНОГО класу

            Console.WriteLine("=== Демонстрація поліморфізму (виклики через список базового типу) ===");
            Console.WriteLine("====================================================================\n");

            // Точки для тестування
            double x_safe = 5.0;
            double x_danger = 3.0; // Ця точка спричинить ділення на 0 для func1

            // 3. Перебираємо список і викликаємо методи
            foreach (var func in functions)
            {
                // Виклик PrintCoefficients()
                // Завдяки 'virtual'/'override' буде викликано правильну версію методу
                func.PrintCoefficients();

                // 4. Обробка помилок (ділення на нуль)

                // Тест 1: Безпечна точка
                try
                {
                    double result_safe = func.Evaluate(x_safe);
                    Console.WriteLine($"  -> Значення в точці x = {x_safe}: {result_safe:F2}"); // :F2 - форматування до 2 знаків
                }
                catch (DivideByZeroException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  -> Помилка при x = {x_safe}: {ex.Message}");
                    Console.ResetColor();
                }

                // Тест 2: Небезпечна точка (перевірка ділення на 0)
                try
                {
                    double result_danger = func.Evaluate(x_danger);
                    Console.WriteLine($"  -> Значення в точці x = {x_danger}: {result_danger:F2}");
                }
                catch (DivideByZeroException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"  -> Помилка при x = {x_danger}: {ex.Message}");
                    Console.ResetColor();
                }

                Console.WriteLine("\n-------------------------------------------------\n");
            }
        }
    }
}
