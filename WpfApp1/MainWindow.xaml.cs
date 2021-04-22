using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FieldSpace;
namespace WpfApp1
{
    /// <summary>
    /// Класс для для основного действа
    /// </summary>
    public partial class MainWindow : Window
    {
        Field[,] field = new Field[10, 10];                             // Создаём массив, размерностью поля
        Button[,] buttons = new Button[10, 10];                         // Создаю матрицу кнопок
        Image BombImage = new Image
        {
            Source = new BitmapImage(new Uri(@"..\WpfApp1\Mine.jpg", UriKind.Relative))    // Выбираю изображение для мины
        };
        int ClickCount = 0;
        Field fieldConstructor = new Field();                           // Делаем ещё экземпляр класса для взаимодействия с классом


        public MainWindow()
        {
            InitializeComponent();
            int MineCount = fieldConstructor.MineCount;                 // Получаем количство мин
            MineCounter.Content = Convert.ToString(MineCount);    // Вывожу кол-во мин


            #region Инициализация поля
            for (int i = 0; i < field.GetLength(0); i++)
            {                                                           //
                for (int j = 0; j < field.GetLength(1); j++)            //
                {                                                       // Заполняем его экземплярами класса
                    field[i, j] = new Field();                          //
                }                                                       //
            }
            #endregion
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region Генерация поля после первого нажатия на любую клетку
            ClickCount++;
            if (ClickCount == 1)
            {
                fieldConstructor.Generate(field);                               // Вызываем генерацию поля


                for (int m = 0; m < 10; m++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        fieldConstructor.MineAroundCounter(field, m, k);            // Прохожу по полю и получаю количество мин вокруг
                    }
                }


                #region Заполнение кнопок
                foreach (UIElement item in FieldGrid.Children)                                              // Прохожу по матрице кнопок на форме
                {
                    int m = Grid.GetRow(item), k = Grid.GetColumn(item);
                    buttons[m, k] = (Button)item;
                    if (field[m, k].IsMine)
                        buttons[m, k].Content = BombImage;                                          // Если кнопка это мина, то вывожу на кнопку картинку
                    else
                        buttons[m, k].Content = field[m, k].MineAround;                         // Иначе вывожу на кнопку количество мин вокруг

                }
                #endregion
            }
            #endregion


            #region Нажатие на кнопку
            int i = Grid.GetRow((Button)sender), j = Grid.GetColumn((Button)sender);
            if (field[i, j].IsMine)                                     // Если кнопка, на которую мы нажали - мина
            {
                MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                this.Close();
            }
            else                                                        // Если это не мина
            {
                if (field[i, j].MineAround == 0)                        // И бомб вокруг ноль, то открываем клетки вокруг
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
                                    buttons[m, k].Content = field[m, k].MineAround;             // Открываем другие клетки
                                }
                            }
                        }
                        else if (j == 9)
                        {
                            for (int m = i; m <= i + 1; m++)
                            {
                                for (int k = j - 1; k <= j; k++)
                                {
                                    buttons[m, k].Content = field[m, k].MineAround;
                                }
                            }
                        }
                        else
                        {
                            for (int m = i; m <= i + 1; m++)
                            {
                                for (int k = j - 1; k <= j + 1; k++)
                                {
                                    buttons[m, k].Content = field[m, k].MineAround;
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
                                    buttons[m, k].Content = field[m, k].MineAround;
                                }
                            }
                        }
                        else if (j == 9)
                        {
                            for (int m = i - 1; m <= i; m++)
                            {
                                for (int k = j - 1; k <= j; k++)
                                {
                                    buttons[m, k].Content = field[m, k].MineAround;
                                }
                            }
                        }
                        else
                        {
                            for (int m = i - 1; m <= i; m++)
                            {
                                for (int k = j - 1; k <= j + 1; k++)
                                {
                                    buttons[m, k].Content = field[m, k].MineAround;
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
                                    buttons[m, k].Content = field[m, k].MineAround;
                                }
                            }
                        }
                        else if (j == 9)
                        {
                            for (int m = i - 1; m <= i + 1; m++)
                            {
                                for (int k = j - 1; k <= j; k++)
                                {
                                    buttons[m, k].Content = field[m, k].MineAround;
                                }
                            }
                        }
                        else
                        {
                            for (int m = i - 1; m <= i + 1; m++)
                            {
                                for (int k = j - 1; k <= j + 1; k++)
                                {
                                    buttons[m, k].Content = field[m, k].MineAround;
                                }
                            }
                        }
                    }
                    #endregion
                }
                buttons[i, j].Content = field[i, j].MineAround;         // Открываем клетку, на которую мы нажали
            }
            #endregion
        }
    }
}
