using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FieldSpace;
using GameManagement;
namespace WpfApp1
{
    /// <summary>
    /// Класс для для основного действа
    /// </summary>
    public partial class MainWindow : Window
    {
        Field[,] field = new Field[10, 10];                             // Создаём массив, размерностью поля
        Button[,] buttons = new Button[10, 10];                         // Создаю матрицу кнопок
        Image FlagImage = new Image
        {
            Source = new BitmapImage(new Uri(@"C:\Users\Максим\Source\Repos\Minesweeper\WpfApp1\Icons\flag_icon.png", UriKind.RelativeOrAbsolute))    // Выбираю изображение для мины
        };
        int ClickCount = 0;
        Field fieldConstructor = new Field();                           // Делаем ещё экземпляр класса для взаимодействия с классом
        int MineCount = Field.MineCount;                                // Получаем количство мин
        int MineFlagged = 0, OtherCellsFlagged = 0;                     // Количество помеченных мин и помеченных других клеток
        int FlagCount = 0;                                              // Подсчёт количества поставленных флагов


        public MainWindow()
        {
            InitializeComponent();
            MineCounter.Content = Convert.ToString(MineCount);          // Вывожу кол-во мин
            FlagsCounter.Content = Convert.ToString(FlagCount);         // Вывожу количество поставленных влагов

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


        private void ClickOnField(object sender, RoutedEventArgs e)             // Нажатие на поле
        {
            #region Генерация поля после первого нажатия на любую клетку
            ClickCount++;
            if (ClickCount == 1)
            {
                fieldConstructor.Generate(field, Grid.GetRow((Button)sender), Grid.GetColumn((Button)sender));      // Вызываем генерацию поля и передаём туда клетку, на которую нажали


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
                    /*if (field[m, k].IsMine)
                        buttons[m, k].Content = "";                                      // Если кнопка это мина, то вывожу на кнопку картинку
                    else
                        buttons[m, k].Content = field[m, k].MineAround;                         // Иначе вывожу на кнопку количество мин вокруг*/

                }
                #endregion
            }
            #endregion


            int i = Grid.GetRow((Button)sender), j = Grid.GetColumn((Button)sender);


            #region Нажатие на кнопку в обычном режиме
            if (!GameManager.IsSettingFlags)                                // Если не выбран режим установки флагов
            {
                if (field[i, j].IsMine && !field[i, j].IsFlagged)           // Если кнопка, на которую мы нажали - мина и она не помечена
                {
                    MessageBox.Show("Вы проиграли");                        // То мы проигрываем
                    this.Close();
                }
                else if (!field[i, j].IsMine && !field[i, j].IsFlagged)     // Если это не мина и клетка не помечена флагом
                {
                    if (field[i, j].MineAround == 0)                        // И мин вокруг ноль, то открываем клетки вокруг
                    {
                        fieldConstructor.OpenZero(field, buttons, i, j);
                    }
                    buttons[i, j].Content = field[i, j].MineAround;         // Открываем клетку, на которую мы нажали
                    fieldConstructor.Color(field, buttons, i, j);           // Устанавливаем соответствующий цвет текста кнопки
                    field[i, j].IsOpen = true;
                }
            }
            #endregion

            #region Нажатие на кнопку в режиме флага
            else if (GameManager.IsSettingFlags && !field[i, j].IsOpen)      // Если выбран режим установки флагов
            {
                #region Пометка клетки
                if (!field[i, j].IsFlagged)                                 // Если клетка не помечена
                {
                    //buttons[i, j].Content = FlagImage;                    //
                    //buttons[i, j].Content = 'X';                          // Пометка кнопки флагом
                    buttons[i, j].Background = Brushes.Red;                 //


                    FlagCount++;
                    FlagsCounter.Content = Convert.ToString(FlagCount);
                    if (field[i, j].IsMine)                                 // Если это мина
                    {
                        MineFlagged++;                                      // То прибавляю к количеству помеченных мин
                        field[i, j].IsFlagged = true;                       // Помечаю клетку
                    }
                    else if (!field[i, j].IsMine)                           // Если это не мина
                    {
                        OtherCellsFlagged++;                                // То прибавляю к количеству помеченных других клеток
                        field[i, j].IsFlagged = true;                       // Помечаю клетку
                    }
                }
                #endregion

                #region Снятие пометки с клетки
                else                                                        // Если клетка помечена
                {
                    buttons[i, j].Content = "";                                                     // Убираю пометку с клетки
                    buttons[i, j].Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));   // Снятие флага с кнопки


                    FlagCount--;
                    FlagsCounter.Content = Convert.ToString(FlagCount);
                    if (field[i, j].IsMine)                                 // Если это мина
                    {
                        MineFlagged--;                                      // То убавляю количество помеченных мин
                        field[i, j].IsFlagged = false;                      // Убираю пометку с клетки
                    }
                    else if (!field[i, j].IsMine)                           // Если это не мина
                    {
                        OtherCellsFlagged--;                                // То убавляю количество помеченных других клеток
                        field[i, j].IsFlagged = false;                      // Убираю пометку с клетки
                        if (field[i, j].IsOpen)                             // Если клетка уже была открыта
                            buttons[i, j].Content = field[i, j].MineAround; // Вывести количество мин вокруг
                    }
                }
                #endregion


                #region Победа
                if (MineFlagged == MineCount && OtherCellsFlagged == 0)     // Если количество помеченных мин = изначальному количеству мин и нет помеченных других клеток
                {
                    MessageBox.Show("Вы победили");                         // То игрок побеждает
                    this.Close();
                }
                #endregion
            }
            #endregion
        }

        private void SettingFlags(object sender, RoutedEventArgs e)             // Включение и отключение режима флага
        {
            GameManager.IsSettingFlags = !GameManager.IsSettingFlags;

            #region Смена цвета для кнопки флага
            if (GameManager.IsSettingFlags)
                FlagButton.Background = Brushes.Red;
            else
                FlagButton.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
            #endregion
        }

        private void Reload(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }               // Перезагрузка
    }
}