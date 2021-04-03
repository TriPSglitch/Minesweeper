using System;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Класс для реализация логики поля
    /// </summary>
    class Field
    {
        Random rnd = new Random();
        public bool IsBomb { get; set; } = false;       // Ячейка имеет состояние бомбы и нет
        public int BombCount { get; set; } = 45;        // Количсетво бомб на поле
        public void Generate(Field[,] field)            // Метод для генерации поля
        {
            #region Генерация поля
            for (int i = 0; i < field.GetLength(1); i++)
            {
                int count = rnd.Next(0, field.GetLength(0));                                // Получаем количество бомб на строку
                for (int j = 0; j < count; j++)
                {
                    int tempI = i;
                    int temp = rnd.Next(0, field.GetLength(0));                             // Получаем положение бомбы
                    if (field[i, temp].IsBomb)                                              // Если клетка уже занята бомбой, то
                    {
                        temp = 0;                                                           // Проходим строку с начала и ищем место для бомбы
                        while (field[i, temp].IsBomb && temp < field.GetLength(0))          
                        {
                            if (temp == field.GetLength(0) - 1)
                            {
                                i++;                                                        // Если строка закончилась, то переходим на следующую
                            }
                            temp++;
                        }
                    }
                    field[i, temp].IsBomb = true;                                           // Когда найдено место, устанавливаем бомбу
                    BombCount--;                                                            // уменьшаем кол-во бомб
                    i = tempI;                                                              // Возвращаемся на начальную строку
                    if (BombCount == 0)                                                     // если все бомбы поставлены, то завершаем генерацию
                        break;
                }
                if (BombCount == 0)                                                         // если все бомбы поставлены, то завершаем генерацию
                    break;
            }
            if (BombCount > 0)                                                              // если остались невыставленные бомбы, то вызываем метод снова
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
            int bombCount = fieldConstructor.BombCount;                 // Получаем количство бомб
            fieldConstructor.Generate(field);                               // Вызываем генерацию поля
            #region Пиздец
            if (field[0, 0].IsBomb)
                C00.Content = '1';
            else
                C00.Content = '0';
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j].IsBomb)
                    {

                    }
                    else
                    {

                    }
                }
            }
            #endregion
        }
    }
}
