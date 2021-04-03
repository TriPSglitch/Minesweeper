using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace WpfApp1
{
    /// <summary>
    /// Класс для реализация логики поля
    /// </summary>
    class Field
    {
        Random rnd = new Random();
        public bool IsMine { get; set; } = false;       // Ячейка имеет состояние мины и нет
        public int MineCount { get; set; } = 45;        // Количсетво мин на поле
        public void Generate(Field[,] field)            // Метод для генерации поля
        {
            #region Генерация поля
            for (int i = 0; i < field.GetLength(1); i++)
            {
                int count = rnd.Next(0, field.GetLength(0));                                // Получаем количество мин на строку
                for (int j = 0; j < count; j++)
                {
                    int tempI = i;
                    int temp = rnd.Next(0, field.GetLength(0));                             // Получаем положение мины
                    if (field[i, temp].IsMine)                                              // Если клетка уже занята миной, то
                    {
                        temp = 0;                                                           // Проходим строку с начала и ищем место для мины
                        while (field[i, temp].IsMine && temp <= field.GetLength(0))
                        {
                            if (temp == field.GetLength(0) - 1)
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
                    if (MineCount == 0)                                                     // если все мины поставлены, то завершаем генерацию
                        break;
                }
                if (MineCount == 0)                                                         // если все мины поставлены, то завершаем генерацию
                    break;
            }
            if (MineCount > 0)                                                              // если остались невыставленные мины, то вызываем метод снова
                Generate(field);
            #endregion
        }
    }
    /// <summary>
    /// Класс для для основного действа
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Field[,] field = new Field[10, 10];                             // Создаём массив, размерностью поля
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
            #region Жопа
            //int m = 0, k = 0;
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(@"C:\Users\Максим\source\repos\Minesweeper\WpfApp1\Icons\Mine.jpg"));    // Выбираю изображение для мины
            MineCounter.Text = Convert.ToString(MineCount - fieldConstructor.MineCount);                                    // Вывожу кол-во мин
            foreach (UIElement item in FieldGrid.Children)                                              // Прохожу по матрице кнопок
            {
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        if (Grid.GetRow(item) == i && Grid.GetColumn(item) == j)                        // Если Строка == i и столбец == j
                        {
                            if (field[i, j].IsMine)
                            {
                                ((Button)item).Content = image;
                            }
                            else
                                ((Button)item).Content = '0';
                        }
                    }
                }
            }
            /*if (field[0, 0].IsMine)
                C00.Content = '1';
            else
                C00.Content = '0';
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j].IsMine)
                    {

                    }
                    else
                    {

                    }
                }
            }*/
            #endregion
        }
    }
}
