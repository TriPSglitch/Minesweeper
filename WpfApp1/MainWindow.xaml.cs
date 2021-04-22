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

        public MainWindow()
        {
            InitializeComponent();

            #region Инициализация поля
            for (int i = 0; i < field.GetLength(0); i++)
            {                                                           //
                for (int j = 0; j < field.GetLength(1); j++)            //
                {                                                       // Заполняем его экземплярами класса
                    field[i, j] = new Field();                          //
                }                                                       //
            }


            Field fieldConstructor = new Field();                           // Делаем ещё экземпляр класса для взаимодействия с классом


            int MineCount = fieldConstructor.MineCount;                 // Получаем количство мин
            fieldConstructor.Generate(field);                               // Вызываем генерацию поля


            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    fieldConstructor.MineAroundCounter(field, i, j);            // Прохожу по полю и получаю количество мин вокруг
                }
            }
            #endregion


            Image image = new Image();
            image.Source = new BitmapImage(new Uri(@"C:\Users\gr692_pdo\source\repos\Minesweeper\WpfApp1\Icons\Mine.jpg"));    // Выбираю изображение для мины
            MineCounter.Text = Convert.ToString(MineCount - fieldConstructor.MineCount);                                    // Вывожу кол-во мин


            #region Жопа
            foreach (UIElement item in FieldGrid.Children)                                              // Прохожу по матрице кнопок на форме
            {
                int i = Grid.GetRow(item), j = Grid.GetColumn(item);
                buttons[i, j] = (Button)item;
                /*if (field[i, j].IsMine)
                    buttons[i, j].Content = image;                                          // Если кнопка это мина, то вывожу на кнопку картинку
                else
                    buttons[i, j].Content = field[i, j].MineAround;                         // Иначе вывожу на кнопку количество мин вокруг*/

            }
            #endregion
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
        }
    }
}
