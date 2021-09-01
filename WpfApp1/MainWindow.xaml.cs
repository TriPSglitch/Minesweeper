using CellSpace;
using DifficultSelector;
using GameManagement;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Класс для для основного действа
    /// </summary>
    public partial class MainWindow : Window
    {
        Cell[,] cell = new Cell[10, 10];                                // Создаём массив, размерностью поля
        Button[,] buttons = new Button[10, 10];                         // Создаю матрицу кнопок
        /*Image FlagImage = new Image
        {
            Source = new BitmapImage(new Uri(@"C:\Users\Максим\Source\Repos\Minesweeper\WpfApp1\Icons\flag_icon.png", UriKind.RelativeOrAbsolute))    // Выбираю изображение для мины
        };*/


        public MainWindow()
        {
            InitializeComponent();
            MineCounter.Content = Convert.ToString(GameManager.MineCount);                      // Вывожу кол-во мин
            FlagsCounter.Content = Convert.ToString(GameManager.FlagCount);         // Вывожу количество поставленных влагов

            #region Инициализация поля
            for (int i = 0; i < cell.GetLength(0); i++)
            {                                                           //
                for (int j = 0; j < cell.GetLength(1); j++)             //
                {                                                       // Заполняем его экземплярами класса
                    cell[i, j] = new Cell();                            //
                }                                                       //
            }
            #endregion

            #region Заполнение кнопок
            foreach (UIElement item in FieldGrid.Children)                           // Прохожу по матрице кнопок на форме
            {
                buttons[Grid.GetRow(item), Grid.GetColumn(item)] = (Button)item;
            }
            #endregion
        }


        private void SettingFlags(object sender, RoutedEventArgs e)             // Включение и отключение режима флага
        {
            GameManager.IsSettingFlags = !GameManager.IsSettingFlags;

            FlagButton.Background = GameManager.IsSettingFlags ? Brushes.Red : new SolidColorBrush(Color.FromRgb(221, 221, 221));   // Смена цвета для кнопки флага
        }

        private void FlagAdd(int i, int j)
        {
            #region Пометка клетки
            if (!cell[i, j].IsFlagged && !cell[i, j].IsOpen)            // Если клетка не помечена и она не открыта
            {
                //buttons[i, j].Content = FlagImage;                    //
                //buttons[i, j].Content = 'X';                          // Пометка кнопки флагом
                buttons[i, j].Background = Brushes.Red;                 //


                GameManager.FlagCount++;
                FlagsCounter.Content = Convert.ToString(GameManager.FlagCount);
                if (cell[i, j].IsMine)                                  // Если это мина
                {
                    GameManager.MineFlagged++;                                      // То прибавляю к количеству помеченных мин
                }
                else
                {
                    GameManager.OtherCellsFlagged++;                                // Иначе прибавляю к количеству помеченных других клеток
                }
                cell[i, j].IsFlagged = true;                            // Помечаю клетку
            }
            #endregion

            #region Снятие пометки с клетки
            else if (cell[i, j].IsFlagged)                                                      // Если клетка помечена
            {
                //buttons[i, j].Content = "";                                                   //
                buttons[i, j].Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));   // Снятие флага с кнопки


                GameManager.FlagCount--;
                FlagsCounter.Content = Convert.ToString(GameManager.FlagCount);
                if (cell[i, j].IsMine)                                 // Если это мина
                {
                    GameManager.MineFlagged--;                                     // То убавляю количество помеченных мин
                }
                else
                {
                    GameManager.OtherCellsFlagged--;                               // Иначе убавляю количество помеченных других клеток
                }
                cell[i, j].IsFlagged = false;                          // Убираю пометку с клетки
            }
            #endregion
        }


        private void ClickOnCell(object sender, MouseButtonEventArgs e)
        {
            #region Генерация поля после первого нажатия на любую клетку
            GameManager.ClickCount++;
            if (GameManager.ClickCount == 1)
            {
                GameManager.Generate(cell, Grid.GetRow((Button)sender), Grid.GetColumn((Button)sender));      // Вызываем генерацию поля и передаём туда клетку, на которую нажали


                for (int m = 0; m < 10; m++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        GameManager.MineAroundCounter(cell, m, k);            // Прохожу по полю и получаю количество мин вокруг для каждой клетки
                    }
                }
            }
            #endregion


            int i = Grid.GetRow((Button)sender), j = Grid.GetColumn((Button)sender);


            // Если не выбран режим установки флагов и была нажата левая кнопа мыши
            #region Нажатие на кнопку в обычном режиме
            if (!GameManager.IsSettingFlags && e.LeftButton == MouseButtonState.Pressed)
            {
                if (cell[i, j].IsMine && !cell[i, j].IsFlagged)             // Если кнопка, на которую мы нажали - мина и она не помечена
                {
                    MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                    Reload();
                }
                else if (!cell[i, j].IsMine && !cell[i, j].IsFlagged)       // Если это не мина и клетка не помечена флагом
                {
                    // Если нажать на открытую клетку 
                    if (cell[i, j].IsOpen)
                    {
                        int flagArround = 0;


                        #region Подсчёт отмеченных клеток вокруг
                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                for (int m = i; m <= i + 1; m++)
                                {
                                    for (int k = j; k <= j + 1; k++)
                                    {
                                        if (cell[m, k].IsFlagged)
                                            flagArround++;
                                    }
                                }
                            }
                            else if (j == 9)
                            {
                                for (int m = i; m <= i + 1; m++)
                                {
                                    for (int k = j - 1; k <= j; k++)
                                    {
                                        if (cell[m, k].IsFlagged)
                                            flagArround++;
                                    }
                                }
                            }
                            else
                            {
                                for (int m = i; m <= i + 1; m++)
                                {
                                    for (int k = j - 1; k <= j + 1; k++)
                                    {
                                        if (cell[m, k].IsFlagged)
                                            flagArround++;
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
                                        if (cell[m, k].IsFlagged)
                                            flagArround++;
                                    }
                                }
                            }
                            else if (j == 9)
                            {
                                for (int m = i - 1; m <= i; m++)
                                {
                                    for (int k = j - 1; k <= j; k++)
                                    {
                                        if (cell[m, k].IsFlagged)
                                            flagArround++;
                                    }
                                }
                            }
                            else
                            {
                                for (int m = i - 1; m <= i; m++)
                                {
                                    for (int k = j - 1; k <= j + 1; k++)
                                    {
                                        if (cell[m, k].IsFlagged)
                                            flagArround++;
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
                                        if (cell[m, k].IsFlagged)
                                            flagArround++;
                                    }
                                }
                            }
                            else if (j == 9)
                            {
                                for (int m = i - 1; m <= i + 1; m++)
                                {
                                    for (int k = j - 1; k <= j; k++)
                                    {
                                        if (cell[m, k].IsFlagged)
                                            flagArround++;
                                    }
                                }
                            }
                            else
                            {
                                for (int m = i - 1; m <= i + 1; m++)
                                {
                                    for (int k = j - 1; k <= j + 1; k++)
                                    {
                                        if (cell[m, k].IsFlagged)
                                            flagArround++;
                                    }
                                }
                            }
                        }
                        #endregion


                        if (cell[i, j].MineAround == flagArround)               // И количество отмеченных клеток == количеству мин вокруг
                            GameManager.Around(cell, buttons, i, j);            // То открываем клетки вокруг
                    }


                    if (cell[i, j].MineAround == 0)                         // Если мин вокруг ноль, то открываем клетки вокруг
                    {
                        GameManager.OpenZero(cell, buttons, i, j);
                    }


                    buttons[i, j].Content = cell[i, j].MineAround;          // Открываем клетку, на которую мы нажали
                    GameManager.ColorChanger(cell, buttons, i, j);                 // Устанавливаем соответствующий цвет текста кнопки
                    cell[i, j].IsOpen = true;
                }
            }
            #endregion


            // Если выбран режим установки флагов или нажатие на правую кнопку, при этом клетка не открыта
            #region Нажатие на кнопку в режиме флага
            else if ((GameManager.IsSettingFlags || e.RightButton == MouseButtonState.Pressed) && !cell[i, j].IsOpen)
            {
                FlagAdd(i, j);

                #region Победа
                if (GameManager.MineFlagged == GameManager.MineCount && GameManager.OtherCellsFlagged == 0)     // Если количество помеченных мин == изначальному количеству мин и нет помеченных других клеток
                {
                    MessageBox.Show("Вы победили");                         // То игрок побеждает
                    Reload();
                }
                #endregion
            }
            #endregion
        }


        private void ClickReload(object sender, RoutedEventArgs e)
        {
            Reload();
        }


        public void Reload()
        {
            GameManager.Reload(ref cell, ref buttons);
            FlagsCounter.Content = Convert.ToString(GameManager.FlagCount);         // Вывожу количество поставленных влагов
            MineCounter.Content = Convert.ToString(GameManager.MineCount);          // Вывожу кол-во мин
        }


        private void DifficultyLevelSelector(object sender, RoutedEventArgs e)
        {
            Selector selector = new Selector();
            selector.Show();
        }
    }
}