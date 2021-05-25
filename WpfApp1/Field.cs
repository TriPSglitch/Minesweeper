using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace FieldSpace
{
    /// <summary>
    /// Класс для реализация логики поля
    /// </summary>
    class Field
    {
        Random rnd = new Random();
        public bool IsMine { get; set; } = false;              // Ячейка имеет состояние мины и нет
        public static int MineCount { get; set; } = 17;        // Количсетво мин на поле
        public int MineAround { get; set; } = 0;               // количество мин вокруг
        public bool IsFlagged { get; set; } = false;           // Для проверки установлен ли флаг на клетку
        public bool IsOpen { get; set; } = false;              // Для того, чтобы показывать количество мин вокруг при снятии флага, если клетка уже была открыта
        public void Generate(Field[,] field, int m, int k)                   // Метод для генерации поля
        {
            #region Генерация поля
            for (int i = 0; i < field.GetLength(1); i++)
            {
                int tempI = i;                                                          // Сохраняем начальную строку
                int temp = rnd.Next(0, field.GetLength(0));                             // Получаем положение мины


                /*Сранная хрень, я с тетрадкой сидел и проверял, в итоге
                не должны быть ближайшие клетки минами, потому что я всё предусмотрел,
                но нет, мало того, что всё равно вокруг могут быть мины,
                так ещё бывает так, что клетка на которую ты нажал и есть мина, 
                с чего это так я хз, потому что у меня первым условием идёт пропуск той клетки, на которую ты нажал, 
                слава богу хоть, что это происходит редко, и только если ты нажал в угол*/
                #region Делаем вокруг клетки 0 бомб    
                if (temp == k && i == m)                        // Пропускаем клетки вокруг той, на которую мы нажали первой
                {
                    continue;
                }
                else
                {
                    if (i == 0)
                    {
                        if (k == 0)
                        {
                            if ((temp == k && i == m + 1) || (temp == k + 1 && i == m + 1) || (temp == k + 1 && i == m))
                                continue;
                        }
                        else if (k == 9)
                        {
                            if ((temp == k && i == m + 1) || (temp == k - 1 && i == m + 1) || (temp == k - 1 && i == m))
                                continue;
                        }
                        else
                        {
                            if ((temp == k && i == m + 1) || (temp == k - 1 && i == m + 1) || (temp == k + 1 && i == m + 1) || (temp == k - 1 && i == m) || (temp == k + 1 && i == m))
                                continue;
                        }
                    }
                    else if (i == 9)
                    {
                        if (k == 0)
                        {
                            if ((temp == k && i == m - 1) || (temp == k + 1 && i == m - 1) || (temp == k + 1 && i == m))
                                continue;
                        }
                        else if (k == 9)
                        {
                            if ((temp == k && i == m - 1) || (temp == k - 1 && i == m - 1) || (temp == k - 1 && i == m))
                                continue;
                        }
                        else
                        {
                            if ((temp == k && i == m - 1) || (temp == k - 1 && i == m - 1) || (temp == k + 1 && i == m - 1) || (temp == k - 1 && i == m) || (temp == k + 1 && i == m))
                                continue;
                        }
                    }
                    else
                    {
                        if (k == 0)
                        {
                            if ((temp == k && i == m - 1) || (temp == k + 1 && i == m - 1) || (temp == k + 1 && i == m) || (temp == k && i == m + 1) || (temp == k + 1 && i == m + 1))
                                continue;
                        }
                        else if (k == 9)
                        {
                            if ((temp == k && i == m - 1) || (temp == k - 1 && i == m - 1) || (temp == k - 1 && i == m) || (temp == k && i == m + 1) || (temp == k - 1 && i == m + 1))
                                continue;
                        }
                        else
                        {
                            if ((temp == k && i == m - 1) || (temp == k - 1 && i == m - 1) || (temp == k + 1 && i == m - 1) || (temp == k - 1 && i == m) || (temp == k + 1 && i == m)
                                || (temp == k && i == m + 1) || (temp == k - 1 && i == m + 1) || (temp == k + 1 && i == m + 1))
                                continue;
                        }
                    }
                }
                #endregion


                if (field[i, temp].IsMine)                                              // Если клетка уже занята миной, то
                {
                    temp = 0;                                                           // Проходим строку с начала и ищем место для мины
                    while (field[i, temp].IsMine && temp <= field.GetLength(0))
                    {
                        if (temp == field.GetLength(0) - 1 && i == m)
                        {
                            i++;                                                        // Если строка закончилась, то переходим на следующую
                            temp = 0;
                        }
                        temp++;
                    }
                }
                field[i, temp].IsMine = true;                                           // Когда найдено место, устанавливаем мину
                MineCount--;                                                            // уменьшаем кол-во мин
                i = tempI;                                                              // Возвращаемся на начальную строку
                if (MineCount == 0)                                                         // если все мины поставлены, то завершаем генерацию
                    break;
            }
            if (MineCount > 0)                                                              // если остались невыставленные мины, то вызываем метод снова
                Generate(field, m, k);
            #endregion
        }

        public void MineAroundCounter(Field[,] field, int i, int j)             // Метод, который считает количество мин вокруг
        {
            #region Количество мин вокруг
            if (i == 0)
            {
                if (j == 0)
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j; k <= j + 1; k++)
                        {
                            if (field[m, k].IsMine)
                                field[i, j].MineAround++;
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (field[m, k].IsMine)
                                field[i, j].MineAround++;
                        }
                    }
                }
                else
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (field[m, k].IsMine)
                                field[i, j].MineAround++;
                        }
                    }
                }
            }
            else if (i == 9)
            {
                if (j == 0)
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j; k <= j + 1; k++)
                        {
                            if (field[m, k].IsMine)
                                field[i, j].MineAround++;
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (field[m, k].IsMine)
                                field[i, j].MineAround++;
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (field[m, k].IsMine)
                                field[i, j].MineAround++;
                        }
                    }
                }
            }
            else
            {
                if (j == 0)
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j; k <= j + 1; k++)
                        {
                            if (field[m, k].IsMine)
                                field[i, j].MineAround++;
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (field[m, k].IsMine)
                                field[i, j].MineAround++;
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (field[m, k].IsMine)
                                field[i, j].MineAround++;
                        }
                    }
                }
            }
            #endregion
        }

        public void Color(Field[,] field, Button[,] buttons, int i, int j)      // Метод, который меняет цвет текста кнопки
        {
            #region Установление цвета текста
            if (field[i, j].MineAround == 1)
                buttons[i, j].Foreground = Brushes.Blue;
            else if (field[i, j].MineAround == 2)
                buttons[i, j].Foreground = Brushes.Green;
            else if (field[i, j].MineAround == 3)
                buttons[i, j].Foreground = Brushes.Red;
            else if (field[i, j].MineAround == 4)
                buttons[i, j].Foreground = Brushes.DarkBlue;
            else if (field[i, j].MineAround == 5)
                buttons[i, j].Foreground = Brushes.DarkRed;
            else if (field[i, j].MineAround == 6)
                buttons[i, j].Foreground = Brushes.DarkGreen;
            #endregion
        }

        public void OpenZero(Field[,] field, Button[,] buttons, int i, int j)
        {
            #region Открытие других клеток
            if (i == 0)
            {
                if (j == 0)
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j; k <= j + 1; k++)
                        {
                            if (field[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = field[m, k].MineAround;             // Открываем другие клетки
                            field[m, k].Color(field, buttons, m, k);
                            field[m, k].IsOpen = true;
                            if (field[m, k].MineAround == 0)
                                field[m, k].OpenZero(field, buttons, m, k);
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (field[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = field[m, k].MineAround;
                            field[m, k].Color(field, buttons, m, k);
                            field[m, k].IsOpen = true;
                            if (field[m, k].MineAround == 0)
                                field[m, k].OpenZero(field, buttons, m, k);
                        }
                    }
                }
                else
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (field[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = field[m, k].MineAround;
                            field[m, k].Color(field, buttons, m, k);
                            field[m, k].IsOpen = true;
                            if (field[m, k].MineAround == 0)
                                field[m, k].OpenZero(field, buttons, m, k);
                        }
                    }
                }
            }
            else if (i == 9)
            {
                if (j == 0)
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j; k <= j + 1; k++)
                        {
                            if (field[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = field[m, k].MineAround;
                            field[m, k].Color(field, buttons, m, k);
                            field[m, k].IsOpen = true;
                            if (field[m, k].MineAround == 0)
                                field[m, k].OpenZero(field, buttons, m, k);
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (field[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = field[m, k].MineAround;
                            field[m, k].Color(field, buttons, m, k);
                            field[m, k].IsOpen = true;
                            if (field[m, k].MineAround == 0)
                                field[m, k].OpenZero(field, buttons, m, k);
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (field[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = field[m, k].MineAround;
                            field[m, k].Color(field, buttons, m, k);
                            field[m, k].IsOpen = true;
                            if (field[m, k].MineAround == 0)
                                field[m, k].OpenZero(field, buttons, m, k);
                        }
                    }
                }
            }
            else
            {
                if (j == 0)
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j; k <= j + 1; k++)
                        {
                            if (field[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = field[m, k].MineAround;
                            field[m, k].Color(field, buttons, m, k);
                            field[m, k].IsOpen = true;
                            if (field[m, k].MineAround == 0)
                                field[m, k].OpenZero(field, buttons, m, k);
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (field[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = field[m, k].MineAround;
                            field[m, k].Color(field, buttons, m, k);
                            field[m, k].IsOpen = true;
                            if (field[m, k].MineAround == 0)
                                field[m, k].OpenZero(field, buttons, m, k);
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (field[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = field[m, k].MineAround;
                            field[m, k].Color(field, buttons, m, k);
                            field[m, k].IsOpen = true;
                            if (field[m, k].MineAround == 0)
                                field[m, k].OpenZero(field, buttons, m, k);
                        }
                    }
                }
            }
            #endregion
        }
    }
}