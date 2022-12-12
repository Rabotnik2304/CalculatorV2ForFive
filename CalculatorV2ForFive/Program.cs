using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;
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
                Console.WriteLine("Нажмите 1 чтобы перевести целое число по модулю от -128 до 127 в дополнительный код");
                Console.WriteLine("Нажмите 2 чтобы сложить целые числа(положительные и отрицательные) с использованием дополнительного кода");
                Console.WriteLine("Нажмите 3 чтобы перевести любое вещественное число в формат нормализованной записи");
                Console.WriteLine("Нажмите 4 чтобы проссумировать два вещественных числа в формате нормализованной записи");
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
                        ConverterFromFloatToBinaryFloatStart();
                    }
                    else if (operation == "4")
                    {
                        SumFloatBinaryStart();
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
        private static void SumFloatBinaryStart()
        {
            Console.WriteLine("Введите через пробел два вещественных числа, которые вы хотите сложить");

            //Console.ForegroundColor = ConsoleColor.DarkRed;
            //Console.WriteLine("(Внимание, в случае если сумма чисел по модулю больше 127 калькулятор будет выводить неверные ответы)");
            //Console.ResetColor();

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

            if (double.TryParse(sumSplit[0], out double number1))
                number1 = number1;
            else
                throw new ArgumentException("Ваше первое число некоректно");

            if (double.TryParse(sumSplit[1], out double number2))
                number2 = number2;
            else
                throw new ArgumentException("Ваше второе число некоректно");
            
            Console.WriteLine();
            Console.WriteLine("Для начала определим знак итогового ответа");
            
            double number1Abs = Math.Abs(number1);
            double number2Abs = Math.Abs(number2);
            double maxNumberAbs = Math.Max(number1Abs, number2Abs);

            int resultSign;

            if (maxNumberAbs == number1Abs)
            {
                Console.WriteLine("Т.к. число {0} по модулю больше числа {1}, то знак итогового числа совпадает со знаком числа {0}", number1, number2);
                resultSign = Math.Sign(number1);
            }
            else
            {
                Console.WriteLine("Т.к. число {0} по модулю больше числа {1}, то знак итогового числа совпадает со знаком числа {0}", number2, number1);
                resultSign = Math.Sign(number2);
            }

            if (resultSign==-1)
            {
                resultSign = 1;
            }
            else
            {
                resultSign = 0;
            }

            Console.WriteLine("Следовательно, знак результата сложения в формате нормализованной записи: {0}", resultSign);

            Console.WriteLine();
            Console.WriteLine("Перед тем как производить сложение, нужно перевести оба числа в формат нормализованной записи:");

            string binaryFloatNumber1 = ConverterFromFloatToBinaryFloat(number1);
            string binaryFloatNumber2 = ConverterFromFloatToBinaryFloat(number2);

            Console.WriteLine("Теперь производим суммирование двух чисел в формате нормализованной записи:");
            Console.WriteLine();

            Console.WriteLine("Первым делом нам нужно добиться равенства смещенного порядка у двух чисел:");
            Console.WriteLine();

            string numberSign1 = binaryFloatNumber1.Substring(0, 1);
            string numberOrder1 = binaryFloatNumber1.Substring(2, 8);
            string mantissa1 = binaryFloatNumber1.Substring(11);

            string numberSign2 = binaryFloatNumber2.Substring(0, 1);
            string numberOrder2 = binaryFloatNumber2.Substring(2, 8);
            string mantissa2 = binaryFloatNumber2.Substring(11);
            
            if (FromAnyToDec(numberOrder1, 2) == FromAnyToDec(numberOrder2, 2))
            {
                Console.WriteLine("Т.к. у чисел порядки равны, то делать ничего не нужно");
            }
            
            else if (FromAnyToDec(numberOrder1,2)> FromAnyToDec(numberOrder2, 2))
            {
                Alignment(binaryFloatNumber1, ref binaryFloatNumber2, numberOrder1, numberSign2, ref numberOrder2, ref mantissa2);
            }
            else 
            {
                Alignment(binaryFloatNumber2, ref binaryFloatNumber1, numberOrder2, numberSign1, ref numberOrder1, ref mantissa1);
            }
            Console.WriteLine();
            Console.WriteLine("Числа после выравнивания порядка:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Первое число: {0}", binaryFloatNumber1);
            Console.WriteLine("Второе число: {0}", binaryFloatNumber2);
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Теперь осталось только сложить мантиссы и подставить полученный в самом начале знак итогового числа:");
            string resultOrder = numberOrder2;
            string resultMantissa = CorrectSum(mantissa1,mantissa2);

            if (resultMantissa.Length > 23)
            {
                Console.WriteLine();
                Console.WriteLine("Т.к. итоговое значение мантиссы имеет длину больше 23, то нам нужно");
                Console.WriteLine("Увеличить значение смещённого порядка на 1, \"отрезать\" от мантиссы первый элемент и сдвинуть её вправо");
                resultOrder = Sum(resultOrder, "1");
                resultMantissa = resultMantissa.Substring(1);
                resultMantissa = "0" + resultMantissa;
                resultMantissa = resultMantissa.Substring(0,23);
            }

            Console.WriteLine();
            
            string resultBinary=resultSign+"|"+ resultOrder + "|"+ resultMantissa;

            float resultDec = (float)number1 + (float)number2;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Итоговый ответ в формате нормализованной записи: {0}", resultBinary);
            Console.WriteLine("Итоговый ответ в десятичной системе счисления: {0}",resultDec);
            Console.ResetColor();

        }

        private static void Alignment(string binaryFloatNumber1, ref string binaryFloatNumber2, string numberOrder1, string numberSign2, ref string numberOrder2, ref string mantissa2)
        {
            Console.WriteLine("Т.к. у числа {0} порядок больше,\n" +
                                "то нам нужно увеличивать порядок числа {1}", binaryFloatNumber1, binaryFloatNumber2);
            Console.WriteLine();
            Console.WriteLine("Для этого будем сдвигать мантиссу вправо и увеличивать значение смещенного порядка на 1, пока порядки не сравняются: ");
            Console.WriteLine("(Примечание: после первого сдвига мантиссы, она будет начинаться не с 0, а с 1, т.к. эта 1 осталась между \n" +
                "смещенным порядком и мантиссой после представления числа в нормализованной экспоненциальной форме,\n" +
                "просто мы её не записывали для экономии памяти)");
            Console.WriteLine();
            bool flag = true;
            while (numberOrder2 != numberOrder1)
            {
                numberOrder2 = Sum(numberOrder2, "1");
                if (flag)
                {
                    mantissa2 = "1" + mantissa2;
                    flag = false;
                }
                else
                {
                    mantissa2 = "0" + mantissa2;
                }

                mantissa2 = mantissa2.Substring(0, 23);
                Console.WriteLine(numberSign2 + "|" + numberOrder2 + "|" + mantissa2);
            }

            binaryFloatNumber2 = numberSign2 + "|" + numberOrder2 + "|" + mantissa2;
        }

        private static string ConverterFromFloatToBinaryFloatStart()
        {
            Console.WriteLine("Введите вещественное число, которое вы хотите перевести в формат нормализованной записи");
            Console.WriteLine("(Целая часть отделяется от дробной \",\")");
            string readLine = Console.ReadLine().Trim();

            if (double.TryParse(readLine, out double doubleNumber1))
                doubleNumber1 = doubleNumber1;
            else
                throw new ArgumentException("Ваше число некорректно");

            string Result = ConverterFromFloatToBinaryFloat(doubleNumber1);

            return Result;
        }

        private static string ConverterFromFloatToBinaryFloat(double doubleNumber1)
        {
            string str = Convert.ToString(doubleNumber1);
            string[] parts = str.Split(',');

            int intIntegerPartNumber = int.Parse(parts[0]);
            string floatPartNumber;
            if (parts.Length == 1)
            {
                floatPartNumber = "0";
            }
            else
            {
                floatPartNumber = parts[1];
            }

            string stringDoubleNumber1 = intIntegerPartNumber.ToString() + "," + floatPartNumber;
            Console.WriteLine();
            Console.WriteLine("Переведём вещественное число {0} в двоичную систему счисления", stringDoubleNumber1);
            Console.WriteLine("Сначала переведём целую часть числа в двоичную систему счисления: ");

            string binaryIntegerPartNumber = IntegerPartNumberToBinary(stringDoubleNumber1, intIntegerPartNumber);
            int lenOfbinaryIntegerPartNumber;
            if (doubleNumber1 < 0 && intIntegerPartNumber == 0)
            {
                binaryIntegerPartNumber = "-" + binaryIntegerPartNumber;
            }
            if (binaryIntegerPartNumber.Substring(0, 1) == "-")
            {
                lenOfbinaryIntegerPartNumber = binaryIntegerPartNumber.Length - 1;
            }
            else
            {
                lenOfbinaryIntegerPartNumber = binaryIntegerPartNumber.Length;
            }
            Console.WriteLine();
            Console.WriteLine("Теперь переведём дробную часть числа в двоичную систему счисления: ");
            Console.WriteLine();
            string resultStringBinaryFloatPartNumber = FloatPartNumberToBinary(stringDoubleNumber1, ref floatPartNumber, lenOfbinaryIntegerPartNumber);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Итого, число {0} в двоичной системе имеет вид: {1},{2}", stringDoubleNumber1, binaryIntegerPartNumber, resultStringBinaryFloatPartNumber);
            Console.ResetColor();
            string resultBinaryNumber = binaryIntegerPartNumber + resultStringBinaryFloatPartNumber;
            Console.WriteLine();
            Console.WriteLine("Теперь осталось только перевести полученное число в формат со смещенным порядком и мантиссой:");
            Console.WriteLine();
            Console.WriteLine("Для начала представим двоичное число в нормализованной экспоненциальной форме:");

            string numberSign = "";
            string mantissa = "";
            int numberOrder = 0;

            if (intIntegerPartNumber != 0)
            {
                numberSign = resultBinaryNumber.Substring(0, 1 + binaryIntegerPartNumber.Length - lenOfbinaryIntegerPartNumber);
                mantissa = resultBinaryNumber.Substring(1 + binaryIntegerPartNumber.Length - lenOfbinaryIntegerPartNumber);
                numberOrder = lenOfbinaryIntegerPartNumber - 1;
            }
            else
            {

                numberSign = resultStringBinaryFloatPartNumber.TrimStart('0').Substring(0, 1);
                if (doubleNumber1 < 0)
                {
                    numberSign = "-" + numberSign;
                }
                mantissa = resultStringBinaryFloatPartNumber.TrimStart('0').Substring(1);

                numberOrder = -(resultStringBinaryFloatPartNumber.Length - mantissa.Length);

                if (mantissa.Length == 0)
                {
                    mantissa = "0";
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Это будет: {0},{1} * 2^{2}", numberSign, mantissa, numberOrder);
            Console.ResetColor();
            Console.WriteLine();

            Console.WriteLine("Теперь осталось только вычислить смещенный порядок, и перевести его в двоичную систему счисления");
            int shiftedNumberOrder = 127 + numberOrder;
            Console.WriteLine("Значение смещенного порядка в десятичной системе счисления: 127 + {0} = {1}", numberOrder, shiftedNumberOrder);

            Console.WriteLine("Теперь переведём значение смещенного порядка из десятичной системы счисления в двоичную:");

            string binaryShiftedNumberOrder = RightFromDecToBinary(shiftedNumberOrder);
            Console.WriteLine();
            Console.WriteLine("Теперь осталось только составить представление числа {0} в формате нормализованной записи", stringDoubleNumber1);
            Console.WriteLine();
            string Result = "";
            if (doubleNumber1 < 0)
            {
                Console.WriteLine("Т.к. наше число отрицательное, то первой цифрой в числе будет 1");
                Console.WriteLine("После 1 пишем значение смещенного порядка в двоичной системе счисления: {0}", binaryShiftedNumberOrder.TrimStart('-'));
                Console.WriteLine("Ну и после этого пишем мантису: {0}", mantissa.PadRight(23, '0'));
                Console.WriteLine("(в случае если значение мантисы по длине меньше 23, то добавляем справа 0, пока длина не станет равной 23)");
                
                Result = "1" + "|" + binaryShiftedNumberOrder.TrimStart('-').PadLeft(8, '0') + "|" + mantissa.PadRight(23, '0');
            }
            else
            {
                Console.WriteLine("Т.к. наше число положительное, то первой цифрой в числе будет 0");
                Console.WriteLine("После 0 пишем значение смещенного порядка в двоичной системе счисления: {0}", binaryShiftedNumberOrder.TrimStart('-'));
                Console.WriteLine("Ну и после этого пишем мантису: {0}", mantissa.PadRight(23, '0'));
                Console.WriteLine("(в случае если значение мантисы по длине меньше 23, то добавляем справа 0, пока длина не станет равной 23)");
                Result = "0" + "|" + binaryShiftedNumberOrder.TrimStart('-').PadLeft(8, '0') + "|" + mantissa.PadRight(23, '0');
            }

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Представление числа {0} в формате нормализованной записи имеет вид {1}", stringDoubleNumber1, Result);
            Console.ResetColor();
            Console.WriteLine();
            return Result;
        }

        private static string FloatPartNumberToBinary(string stringDoubleNumber1, ref string stringFloatPartNumber, int lenOfbinaryIntegerPartNumber)
        {
            Console.WriteLine("Чтобы перевести число 0,{0} из десятичной системы счисления в двоичную будем умножать число на 2 и", stringFloatPartNumber);
            Console.WriteLine("записывать получившуюся целую часть(Синие цифры), пока число 0,{0} не станет целым", stringFloatPartNumber);
            Console.WriteLine("Если представление числа 0,{0} в двоичной системе имеет бесконечный вид,", stringFloatPartNumber);
            Console.WriteLine("то мы запишем только первые {0} цифр(ы) этого представления(Чтобы не вылезти за переделы мантиссы)", 23 - lenOfbinaryIntegerPartNumber+1);
            Console.WriteLine();
            int lenOfFloatPartNumber = stringFloatPartNumber.Length;
            int floatPartNumber = int.Parse(stringFloatPartNumber);
            StringBuilder resultBinaryFloatPartNumber = new StringBuilder();
            for (int i = 0; i < 23 - lenOfbinaryIntegerPartNumber + 1; i++)
            {
                Console.WriteLine("0|{0}", floatPartNumber.ToString().PadLeft(lenOfFloatPartNumber, '0'));
                Console.WriteLine("*");
                Console.WriteLine(" |" + "2".PadLeft(lenOfFloatPartNumber, ' '));
                Console.WriteLine("".PadRight(lenOfFloatPartNumber + 2, '-'));
                floatPartNumber = floatPartNumber * 2;
                string floatStringPartNumber = floatPartNumber.ToString().PadLeft(lenOfFloatPartNumber + 1, '0');

                string stringPartBeforeI = floatStringPartNumber.Substring(0, 1);
                string stringPartAfterI = floatStringPartNumber.Substring(1);
                resultBinaryFloatPartNumber.Append(stringPartBeforeI);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(stringPartBeforeI);
                Console.ResetColor();
                Console.WriteLine("|" + stringPartAfterI);
                floatPartNumber = int.Parse(stringPartAfterI);
                Console.WriteLine();

                if (stringPartAfterI == "".PadLeft(lenOfFloatPartNumber, '0'))
                {
                    break;
                }
            }

            string resultStringBinaryFloatPartNumber = resultBinaryFloatPartNumber.ToString();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Дробная часть числа {0} в двоичной системе имеет вид: 0,{1}", stringDoubleNumber1, resultStringBinaryFloatPartNumber);
            Console.ResetColor();
            return resultStringBinaryFloatPartNumber;
        }

        private static string IntegerPartNumberToBinary(string stringDoubleNumber1, int intIntegerPartNumber)
        {
            bool ifLessThanZero = false;
            if (intIntegerPartNumber < 0)
            {
                Console.WriteLine();
                Console.WriteLine("Т.к. число {0} отрицательное, то переведем в двочиную систему модуль этого числа и допишем слева \"-\"", intIntegerPartNumber);

                ifLessThanZero = true;
            }

            intIntegerPartNumber = Math.Abs(intIntegerPartNumber);

            string binaryIntegerPartNumber = RightFromDecToBinary(intIntegerPartNumber);

            if (ifLessThanZero)
            {
                Console.WriteLine();
                Console.WriteLine("Т.к. число -{0} отрицательное допишем слева \"-\"", intIntegerPartNumber);
                binaryIntegerPartNumber = "-" + binaryIntegerPartNumber;
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Целая часть числа {0} в двоичной системе имеет вид: {1}", stringDoubleNumber1, binaryIntegerPartNumber);
            Console.ResetColor();
            return binaryIntegerPartNumber;
        }

        private static void AdditionalSumStart()
        {
            Console.WriteLine("Введите через пробел два целых числа, которые вы хотите сложить");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("(Внимание, в случае если сумма чисел меньше -128 или больше 127 калькулятор будет выводить неверные ответы)");
            Console.ResetColor();
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
            Console.WriteLine();

            if (sum.Length>8)
            {
                sum= sum.Substring(1,8);
                Console.WriteLine("Т.к. итоговаая сумма имеет длину больше 8, то нужно отрезать старший разряд - {0}",sum);
                Console.WriteLine();
            }

            if (sum.Substring(0, 1) == "1")
            {
                Console.WriteLine("Т.к. у получившегося числа в первом разряде стоит 1, то оно отрицательное");
                Console.WriteLine("Теперь, чтобы перевести его из доп.кода в десятичное число нужно:");
                Console.WriteLine("1. Отнять от получившегося числа 1");
                Console.WriteLine("2. заменить в получившемся числе все 0 на 1 и все 1 на 0");
                Console.WriteLine("3. перевести получившееся число из двочиной системы счисления в десятичную и дописать \"-\" слева");
                Console.WriteLine();
                string sum1 = CorrectVichet(sum, "1");

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Число, после того как мы отняли один : {0}", sum1);
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Теперь заменим в числе {0} все 0 на 1 и все 1 на 0", sum1);
                
                StringBuilder reverseSum1 = new StringBuilder();
                foreach (char c in sum1)
                {
                    if (c == '0')
                    {
                        reverseSum1.Append(1);
                    }
                    else
                    {
                        reverseSum1.Append(0);
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Число, после замены в нём всех 0 на 1 и всех 1 на 0: {0}", reverseSum1);
                Console.WriteLine();
                Console.ResetColor();

                Console.WriteLine("Теперь нужно перевести полученное число в десятичную систему счисления, дописать слева \"-\" и мы получим ответ");
                int res1 = RightFromAnyToDec(reverseSum1.ToString(), 2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Итоговая сумма: -{0}", res1);
                Console.WriteLine();
                Console.ResetColor();

            }
            else
            {
                Console.WriteLine("Т.к. у получившегося числа в первом разряде стоит 0, то оно полоительное");
                Console.WriteLine("Теперь, чтобы перевести его из доп.кода в десятичное число нужно:");
                Console.WriteLine("1. перевести получившееся число из двочиной системы счисления в десятичную");
                               
                int res1 = RightFromAnyToDec(sum, 2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Итоговая сумма: {0}", res1);
                Console.WriteLine();
                Console.ResetColor();
            }

        }

        private static string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static string CorrectVichet(string number1, string number2)
        {
            
            int baze = 2;
            int NumD1 = FromAnyToDec(number1, baze);
            int NumD2 = FromAnyToDec(number2, baze);

            string num1 = number1;
            string num2 = number2;
            if (NumD1 > NumD2)
            {
                number1 = num1;
                number2 = num2;
            }
            else
            {
                number1 = num2;
                number2 = num1;
            }

            number2 = number2.PadLeft(number1.Length,'0');
            List<char> charList1 = number1.ToCharArray().ToList();
            List<char> charList2 = number2.ToCharArray().ToList();
            Console.WriteLine("Вычитаем из большего меньшее");
            Console.Write(" ");
            Console.WriteLine(number1);
            Console.WriteLine("-");
            Console.Write(" ");
            for (int i = 0; i < charList1.Count - charList2.Count; i++)
            {
                Console.Write("0");
            }
            Console.WriteLine(number2);
            Console.Write(" ");
            for (int i = 0; i < charList1.Count; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();

            List<int> numberList1 = charList1.Select(c => (int)Alphabet.IndexOf(c)).ToList();
            List<int> numberList2 = charList2.Select(c => (int)Alphabet.IndexOf(c)).ToList();
            int j;

            Console.WriteLine("Производим поразрядное вычитание:");

            for (int i = numberList1.Count - 1; i >= 0; i--)
            {
                j = i - (numberList1.Count - numberList2.Count);

                if (j >= 0)
                {
                    Console.WriteLine("В разряде {0}, мы вычитаем из {1} {2} и получаем:", i + 1, numberList1[i], numberList2[j]);
                    numberList1[i] -= numberList2[j];
                }

                if (i >= 1 && j >= 0)
                {
                    Console.WriteLine(String.Join(" ", numberList1));
                }

                while (numberList1[i] < 0)
                {
                    Console.WriteLine("Т.к. до этого мы получили отрицательное число " +
                        "производим заём из следующего разряда и увеличиваем {0} на {1} и получаем {2}", numberList1[i], baze, numberList1[i] + baze);

                    numberList1[i] += baze;
                    numberList1[i - 1]--;
                    Console.WriteLine(String.Join(" ", numberList1));
                }
            }
            if (numberList1.Count == numberList2.Count)
            {
                Console.WriteLine(String.Join(" ", numberList1));
            }
            Console.WriteLine();
            Console.WriteLine("Выведем получившийся ответ в {0} системе счисления:",baze);
            Console.WriteLine();
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < numberList1.Count; i++)
            {
                result.Append(Alphabet[numberList1[i]]);
            }
            Console.Write(" ");
            Console.WriteLine(number1);
            Console.WriteLine("-");
            Console.Write(" ");

            for (int i = 0; i < charList1.Count - charList2.Count; i++)
            {
                Console.Write("0");
            }
            Console.WriteLine(number2);
            Console.Write(" ");
            for (int i = 0; i < charList1.Count; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            Console.Write(" ");
            Console.WriteLine(result.ToString());
            return(result.ToString());
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
            if (number1< -128)
            {
                throw new ArgumentException("Ваше число меньше -128");
            }

            if (number1 > 127)
            {
                throw new ArgumentException("Ваше число больше 127");
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Итоговое число: {0}", binNumber);
                Console.ResetColor();
                
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("У нас получилось число: {0}", reverseBinNumber);
                Console.ResetColor();
                
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

        static int RightFromAnyToDec(string number, int baze)
        {

            if (baze > 50)
                throw new ArgumentException("Система счисления должна быть меньше или равна 50");

            Console.WriteLine();

            Console.WriteLine("Перeводим число {0} из {1} системы счисления в десятичную систему счисления", number, baze);

            Console.WriteLine();

            Console.WriteLine("Последовательно проходимся по числу {0} с лева на право и строим его в десятичной системе счисления", number);

            long result = 0;
            int digitsCount = number.Length;
            int num;
            number = new string(number.Reverse().ToArray());
            for (int i = 0; i < digitsCount; i++)
            {
                char c = number[i];

                if (c >= '0' && c <= '9')
                    num = c - '0';
                else if (c >= 'A' && c <= 'Z')
                    num = c - 'A' + 10;
                else if (c >= 'a' && c <= 'z')
                    num = c - 'a' + (('Z' - 'A') + 1) + 10;
                else throw new ArgumentException("Строка содержит символ не корректный для данной системы счисления");

                if (num >= baze)
                    throw new ArgumentException("Строка содержит символ не корректный для данной системы счисления");

                Console.Write("Добавим к числу {0} {1}*{2}^{3} и получим ", result, num, baze, i);


                result += num * (long)Math.Pow(baze, i);

                Console.WriteLine(result);

                if (result > 2147483647)
                {
                    throw new ArgumentException("Ваше число слишком большое, см. примечание");
                }
            }

            Console.WriteLine("Мы получили наше число в десятичной системе счисления: {0}", result);
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Число {0} в двоичной системе счисления имеет вид: {1}", numberStart, result);
            Console.ResetColor();
            
            return result;
        }

        public static string Sum(string number1, string number2)
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

            maxNum = new string(maxNum.Reverse().ToArray());
            minNum = new string(minNum.Reverse().ToArray());

           
            int des = 0;
            for (int i = 0; i < len; i++)
            {
                int res = 0;
                int digit1 = maxNum[i] - '0';
                int digit2 = 0;
                if (i < minNum.Length)
                    digit2 = minNum[i] - '0';

                res = des + digit1 + digit2;

                if (res >= base1)
                {

                    sum.Append(res - base1);
                    if (i == len - 1)
                    {
                        sum.Append("1");

                    }
                    else
                    {
                        des = 1;
                    }
                }
                else
                {
                    des = 0;
                    sum.Append(res);
                }
            }

            maxNum = new string(maxNum.Reverse().ToArray());
            minNum = new string(minNum.Reverse().ToArray());
            string res1 = sum.ToString();
            res1 = new string(res1.Reverse().ToArray());
            int lenRes = res1.Length;

            return res1.ToString();
        }
    }
}