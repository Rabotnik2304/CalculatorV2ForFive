using System.Text;

namespace CalculatorV2ForFive
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Эта программа сделана Кожурковым Георгием Львовичем из группы При-102");
                Console.WriteLine("Примечание: Данная программа не поддерживает операции с числами большими по модулю 2 147 483 647");

                Console.WriteLine();
                Console.WriteLine("Нажмите 1 чтобы перевести целое число по модулю меньшее 256 в дополнительный код");
                Console.WriteLine("Нажмите 2 чтобы сложить целые числа(положительные и отрицательные) с использованием дополнительного кода");
                Console.WriteLine("Нажмите 3 чтобы перевести любое вещественное число в формат с плавающей точкой");
                Console.WriteLine("Нажмите 4 чтобы проссумировать два вещественных числа в формате с плавающей точкой");
                Console.Write("Введите операцию:");

                string operation = Console.ReadLine().Trim();
                Console.WriteLine();

                try
                {
                    if (operation == "1")
                    {
                        ConverterToAdditionalStart();
                    }
                    else if (operation == "2")
                    {
                        AdditionalSumStart();
                    }
                    else if (operation == "3")
                    {
                        Console.WriteLine("Мы не можем сделать эту операцию");
                    }
                    else if (operation == "4")
                    {
                        Console.WriteLine("Мы не можем сделать эту операцию");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("Мы не можем сделать эту операцию");
                        Console.ResetColor();
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Ошибка:");
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }

                Console.WriteLine();
                Console.WriteLine("Вы хотите продолжить работу с Калькулятором, введите 1 если да, и любой другой символ если нет");
                string operationEnd = Console.ReadLine();
                if (operationEnd != "1")
                {
                    Console.Clear();
                    Console.WriteLine("Калькулятор закончил работу");
                    break;
                }
                Console.Clear();
            }
        }

        private static void AdditionalSumStart()
        {
            Console.WriteLine("Введите через пробел два целых числа, которые вы хотите сложить");
            Console.WriteLine("(Внимание, в случае если сумма чисел больше 127 калькулятор будет выводить неверные ответы)");
            string sumReadLine = Console.ReadLine().Trim();
            string[] sumSplit = sumReadLine.Split(" ");

            if (sumSplit.Length < 2)
            {
                throw new ArgumentException("Вы не ввели все числа");
            }
            if (sumSplit.Length > 2)
            {
                throw new ArgumentException("Вы ввели лишние числа");
            }

            if (int.TryParse(sumSplit[0], out int number1))
                number1 = number1;
            else
                throw new ArgumentException("Ваше первое число некоректно");

            if (int.TryParse(sumSplit[1], out int number2))
                number2 = number2;
            else
                throw new ArgumentException("Ваше второе число некоректно");

            Console.WriteLine();
            Console.WriteLine("Перед тем как производить сложение, нужно перевести оба числа в дополнительный код:");
            
            string additionalNumber1 = ConverterToAdditional(number1);
            string additionalNumber2 = ConverterToAdditional(number2);
            Console.WriteLine();
            Console.WriteLine("Теперь производим суммирование двух чисел в дополнительном коде:");
            Console.WriteLine();
            string sum = CorrectSum(additionalNumber1, additionalNumber2);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Итоговая сумма в дополнительном коде: {0}", sum);
            Console.ResetColor();

            if (sum.Substring(0, 1) == "1")
            {
                
            }

        }

        private static void ConverterToAdditionalStart()
        {
            Console.WriteLine("Введите целое число, которое вы хотите перевести в дополнительный код");

            string readLine = Console.ReadLine().Trim();

            if (int.TryParse(readLine, out int number1))
                number1 = number1;
            else
                throw new ArgumentException("Ваше число некорректно");
            
            ConverterToAdditional(number1); 
        }

        private static string ConverterToAdditional(int number1)
        {
            int startNumber1 = number1;
            string binNumber;
            Console.WriteLine();
            if (Math.Abs(number1) >= 256)
            {
                throw new ArgumentException("Ваше число по модулю больше либо равно 256");
            }

            if (number1 >= 0)
            {
                Console.WriteLine("Т.к. число {0} - не отрицательное, то, чтобы найти его представление в дополнительном коде нам нужно:", number1);
                Console.WriteLine("1. перевести это число в двоичную систему счисления ");
                Console.WriteLine("2. дописать к получившемуся числу нули слева, пока оно не станет длины 8 ");
                binNumber = RightFromDecToBinary(number1);

                Console.WriteLine();
                Console.WriteLine("Дописываем к числу {0} нули слева, пока оно не станет длины 8", binNumber);
                binNumber = binNumber.PadLeft(8, '0');
            }
            else
            {
                Console.WriteLine("Т.к. число {0} - отрицательное, то, чтобы найти его представление в дополнительном коде нам нужно:", number1);
                Console.WriteLine("1. перевести модуль этого числа в двоичную систему счисления");
                Console.WriteLine("2. дописать к получившемуся числу нули слева, пока оно не станет длины 8");
                Console.WriteLine("3. заменить в получившемся числе все 0 на 1 и все 1 на 0");
                Console.WriteLine("4. добавить к получившемуся числу 1");

                number1 = Math.Abs(number1);

                binNumber = RightFromDecToBinary(number1);
                Console.WriteLine();
                Console.WriteLine("Дописываем к числу {0} нули слева, пока оно не станет длины 8", binNumber);
                binNumber = binNumber.PadLeft(8, '0');
                Console.WriteLine("Итоговое число: {0}", binNumber);
                Console.WriteLine();

                Console.WriteLine("Теперь заменим в числе {0} все 0 на 1 и все 1 на 0", binNumber);

                StringBuilder reverseBinNumber = new StringBuilder();
                foreach (char c in binNumber)
                {
                    if (c == '0')
                    {
                        reverseBinNumber.Append(1);
                    }
                    else
                    {
                        reverseBinNumber.Append(0);
                    }
                }
                Console.WriteLine("У нас получилось число: {0}", reverseBinNumber);

                Console.WriteLine();
                Console.WriteLine("Теперь добавляем к числу {0} 1 и получаем итоговый ответ", reverseBinNumber);
                Console.WriteLine();
                binNumber = CorrectSum(reverseBinNumber.ToString(), "1");
  
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Представление числа {0} в дополнительном коде - {1}", startNumber1, binNumber);
            Console.ResetColor();
            return binNumber;
        }

        static int FromAnyToDec(string number, int baze)
        {

            long result = 0;
            int digitsCount = number.Length;
            int num;

            for (int i = 0; i < digitsCount; i++)
            {
                char c = number[i];
                num = c - '0';
                result *= baze;
                result += num;
            }

            return (int)result;
        }
        public static string CorrectSum(string number1,string number2)
        {

            StringBuilder sum = new StringBuilder();

            string num1 = number1;
            string num2 = number2;

            int base1 = 2;

            int NumD1 = FromAnyToDec(num1, 2);
            int NumD2 = FromAnyToDec(num2, 2);
            int len = 0;
            string maxNum = "";
            string minNum = "";

            if (NumD1 > NumD2)
            {
                len = num1.Length;
                maxNum = num1;
                minNum = num2;
            }
            else
            {
                len = num2.Length;
                maxNum = num2;
                minNum = num1;
            }
            Console.WriteLine("Cуммируем большее с меньшим");

            Console.Write(" ");
            Console.WriteLine(maxNum);
            Console.WriteLine("+");
            Console.Write(" ");

            Console.WriteLine(minNum.PadLeft(len));
            Console.Write(" ");
            for (int i = 0; i < len; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();

            maxNum = new string(maxNum.Reverse().ToArray());
            minNum = new string(minNum.Reverse().ToArray());

            Console.WriteLine("Производим посимвольное сложение");
            int des = 0;
            for (int i = 0; i < len; i++)
            {
                int res = 0;
                int digit1 = maxNum[i] - '0';
                int digit2 = 0;
                if (i < minNum.Length)
                    digit2 = minNum[i] - '0';

                res = des + digit1 + digit2;
                if (des == 0)
                {
                    Console.WriteLine("Складываем числа {0} и {1}", digit1, digit2);
                }
                else
                {
                    Console.WriteLine("Складываем числа {0} и {1}, и добавляем к ним 1 из прошлого разряда", digit1, digit2);
                }

                Console.WriteLine(res);

                if (res >= base1)
                {

                    sum.Append(res - base1);
                    if (i == len - 1)
                    {
                        Console.WriteLine("Т.к. у нас получилось число большее {0} и это последнее число в сложениии то мы пишем число {1} и дописываем слева 1", base1-1 , res - base1);
                        sum.Append("1");

                    }
                    else
                    {
                        Console.WriteLine("Т.к. у нас получилось число большее {0}, то мы пишем число {1} и добаляем 1 к следующему разряду", base1-1, res - base1);
                        des = 1;
                    }
                }
                else
                {
                    Console.WriteLine("Т.к. у нас получилось число меньшее чем 2, то мы пишем число {0}", res);
                    des = 0;
                    sum.Append(res);
                }
            }

            maxNum = new string(maxNum.Reverse().ToArray());
            minNum = new string(minNum.Reverse().ToArray());
            string res1 = sum.ToString();
            res1 = new string(res1.Reverse().ToArray());
            int lenRes = res1.Length;

            Console.Write(" ");
            Console.WriteLine(maxNum.PadLeft(lenRes));
            Console.WriteLine("+");
            Console.Write(" ");

            Console.WriteLine(minNum.PadLeft(lenRes));
            Console.Write(" ");
            for (int i = 0; i < lenRes; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            Console.Write(" ");
            Console.WriteLine(res1);

            return res1.ToString();
        }
        static string RightFromDecToBinary(int number)
        {
            int baze = 2;
            int numberStart = number;
            StringBuilder builder = new StringBuilder();

            Console.WriteLine();

            Console.WriteLine("Переводим число {0} из десятичной системы счисления в двоичную систему счисления", number);

            Console.WriteLine("Последовательно находим остатки от деления числа {0} на число 2, до тех пор пока деление возможно", number, baze);
            do
            {
                int mod = number % baze;
                char c = (char)('0' + mod);
                Console.WriteLine("Остаток от деления числа {0} на 2 равен {1}", number, mod);
                builder.Append(c);
                number /= baze;
            } while (number >= baze);

            Console.WriteLine("Т.к. деление числа {0} на число 2 больше невозможно, то последним остатком будет само число {0}", number);
            if (number != 0)
            {

                builder.Append((char)('0' + number));
            }
            Console.WriteLine();
            Console.WriteLine("Записываем получившиеся остатки от деления задом на перёд");
            string result = string.Join("", builder.ToString().Reverse());
            Console.WriteLine("Число {0} в двоичной системе счисления имеет вид: {1}", numberStart, result);

            return result;
        }
        
        
    }
}