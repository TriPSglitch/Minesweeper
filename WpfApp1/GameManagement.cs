using CellSpace;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameManagement
{
    /// <summary>
    /// Класс для управления игрой
    /// </summary>
    class GameManager
    {
        static Random rnd = new Random();
        public static int ClickCount { get; set; } = 0;                                 // Количество нажатий на поле
        public static int FlagCount { get; set; } = 0;                                  // Количесвто поставленных флагов
        public static int MineFlagged { get; set; } = 0;                                // Количество мин с флагами
        public static int OtherCellsFlagged { get; set; } = 0;                          // Количество других клеток с флагами
        public static double MineScale { get; set; } = 0.15;                            // Значение, на которое умножается количество клеток, для получения количества мин
        public static int ColumnCount { get; set; } = 10;                               // Количество столбцов
        public static int RowCount { get; set; } = 10;                                  // Количество строк
        public static bool IsSettingFlags { get; set; } = false;                        // Режим установки флага
        public static int MineCount { get; set; } = (int)Math.Ceiling(ColumnCount * RowCount * MineScale);       // Количсетво мин на поле
        private static int MineSettingsCount { get; set; } = MineCount;                 // Количество неустановленных мин
        public static void MineCountUpdate()                                            // Метод для обновления количества мин
        {
            #region Обновление количества мин
            MineCount = (int)Math.Ceiling(ColumnCount * RowCount * MineScale);
            #endregion
        }
        public static void Generate(Cell[,] cell, int m, int k)                         // Метод для генерации поля
        {
            #region Генерация поля
            for (int i = 0; i < cell.GetLength(1); i++)
            {
                int tempI = i;                                                          // Сохраняем начальную строку
                int temp = rnd.Next(0, cell.GetLength(0));                              // Получаем положение мины


                /*Сранная хрень, я с тетрадкой сидел и проверял, в итоге
                не должны быть ближайшие клетки минами, потому что я всё предусмотрел,
                но нет, мало того, что всё равно вокруг могут быть мины,
                так ещё бывает так, что клетка на которую ты нажал и есть мина, 
                с чего это так я хз, потому что у меня первым условием идёт пропуск той клетки, на которую ты нажал, 
                слава богу хоть, что это происходит редко, и только если ты нажал в угол*/
                #region Делаем вокруг клетки 0 мин    
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


                if (cell[i, temp].IsMine)                                               // Если клетка уже занята миной, то
                {
                    temp = 0;                                                           // Проходим строку с начала и ищем место для мины
                    while (cell[i, temp].IsMine && temp <= cell.GetLength(0))
                    {
                        if (temp == cell.GetLength(0) - 1 && i == m)
                        {
                            i++;                                                        // Если строка закончилась, то переходим на следующую
                            temp = 0;
                        }
                        temp++;
                    }
                }
                cell[i, temp].IsMine = true;                                            // Когда найдено место, устанавливаем мину
                MineSettingsCount--;                                                            // уменьшаем кол-во мин
                i = tempI;                                                              // Возвращаемся на начальную строку
                if (MineSettingsCount == 0)                                                         // если все мины поставлены, то завершаем генерацию
                    break;
            }
            if (MineSettingsCount > 0)                                                              // если остались невыставленные мины, то вызываем метод снова
                Generate(cell, m, k);
            #endregion
        }

        public static void MineAroundCounter(Cell[,] cell, int i, int j)                // Метод, который считает количество мин вокруг
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
                            if (cell[m, k].IsMine)
                                cell[i, j].MineAround++;
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (cell[m, k].IsMine)
                                cell[i, j].MineAround++;
                        }
                    }
                }
                else
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (cell[m, k].IsMine)
                                cell[i, j].MineAround++;
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
                            if (cell[m, k].IsMine)
                                cell[i, j].MineAround++;
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (cell[m, k].IsMine)
                                cell[i, j].MineAround++;
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (cell[m, k].IsMine)
                                cell[i, j].MineAround++;
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
                            if (cell[m, k].IsMine)
                                cell[i, j].MineAround++;
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (cell[m, k].IsMine)
                                cell[i, j].MineAround++;
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (cell[m, k].IsMine)
                                cell[i, j].MineAround++;
                        }
                    }
                }
            }
            #endregion
        }

        public static void OpenZero(Cell[,] cell, Button[,] buttons, int i, int j)      // Открытие клеток вокруг нуля
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
                            if (cell[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = cell[m, k].MineAround;             // Открываем другие клетки
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (cell[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (cell[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
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
                            if (cell[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (cell[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (cell[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
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
                            if (cell[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (cell[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (cell[m, k].IsOpen)
                                continue;
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
            }
            #endregion
        }

        public static void Around(Cell[,] cell, Button[,] buttons, int i, int j)        // Открытие клеток вокруг
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
                            if (cell[m, k].IsOpen || cell[m, k].IsFlagged)
                                continue;
                            if (cell[m, k].IsMine)
                            {
                                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                                Reload(ref cell, ref buttons);
                                return;
                            }
                            buttons[m, k].Content = cell[m, k].MineAround;              // Открываем другие клетки
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (cell[m, k].IsOpen || cell[m, k].IsFlagged)
                                continue;
                            if (cell[m, k].IsMine)
                            {
                                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                                Reload(ref cell, ref buttons);
                                return;
                            }
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else
                {
                    for (int m = i; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (cell[m, k].IsOpen || cell[m, k].IsFlagged)
                                continue;
                            if (cell[m, k].IsMine)
                            {
                                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                                Reload(ref cell, ref buttons);
                                return;
                            }
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
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
                            if (cell[m, k].IsOpen || cell[m, k].IsFlagged)
                                continue;
                            if (cell[m, k].IsMine)
                            {
                                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                                Reload(ref cell, ref buttons);
                                return;
                            }
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (cell[m, k].IsOpen || cell[m, k].IsFlagged)
                                continue;
                            if (cell[m, k].IsMine)
                            {
                                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                                Reload(ref cell, ref buttons);
                                return;
                            }
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (cell[m, k].IsOpen || cell[m, k].IsFlagged)
                                continue;
                            if (cell[m, k].IsMine)
                            {
                                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                                Reload(ref cell, ref buttons);
                                return;
                            }
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
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
                            if (cell[m, k].IsOpen || cell[m, k].IsFlagged)
                                continue;
                            if (cell[m, k].IsMine)
                            {
                                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                                Reload(ref cell, ref buttons);
                                return;
                            }
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else if (j == 9)
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j; k++)
                        {
                            if (cell[m, k].IsOpen || cell[m, k].IsFlagged)
                                continue;
                            if (cell[m, k].IsMine)
                            {
                                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                                Reload(ref cell, ref buttons);
                                return;
                            }
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
                else
                {
                    for (int m = i - 1; m <= i + 1; m++)
                    {
                        for (int k = j - 1; k <= j + 1; k++)
                        {
                            if (cell[m, k].IsOpen || cell[m, k].IsFlagged)
                                continue;
                            if (cell[m, k].IsMine)
                            {
                                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                                Reload(ref cell, ref buttons);
                                return;
                            }
                            buttons[m, k].Content = cell[m, k].MineAround;
                            ColorChanger(cell, buttons, m, k);
                            cell[m, k].IsOpen = true;
                            if (cell[m, k].MineAround == 0)
                                OpenZero(cell, buttons, m, k);
                        }
                    }
                }
            }
            #endregion
        }

        public static void Reload(ref Cell[,] cell, ref Button[,] buttons)              // Созданное с нуля колесо - перезагрузка
        {
            #region Перезагрузка 
            for (int i = 0; i < cell.GetLength(0); i++)
            {
                for (int j = 0; j < cell.GetLength(1); j++)
                {
                    cell[i, j].IsMine = false;
                    cell[i, j].IsFlagged = false;
                    cell[i, j].IsOpen = false;
                    cell[i, j].MineAround = 0;
                    buttons[i, j].Content = "";
                    buttons[i, j].Foreground = Brushes.Black;
                    buttons[i, j].Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
                }
            }
            ClickCount = 0;
            FlagCount = 0;
            MineFlagged = 0;
            OtherCellsFlagged = 0;
            MineSettingsCount = MineCount;
            #endregion
        }

        public static void ColorChanger(Cell[,] cell, Button[,] buttons, int i, int j)  // Метод, который меняет цвет текста кнопки
        {
            #region Установление цвета текста
            if (cell[i, j].MineAround == 1)
                buttons[i, j].Foreground = Brushes.Blue;
            else if (cell[i, j].MineAround == 2)
                buttons[i, j].Foreground = Brushes.Green;
            else if (cell[i, j].MineAround == 3)
                buttons[i, j].Foreground = Brushes.Red;
            else if (cell[i, j].MineAround == 4)
                buttons[i, j].Foreground = Brushes.DarkBlue;
            else if (cell[i, j].MineAround == 5)
                buttons[i, j].Foreground = Brushes.DarkRed;
            else if (cell[i, j].MineAround == 6)
                buttons[i, j].Foreground = Brushes.DarkGreen;
            #endregion
        }
    }
}