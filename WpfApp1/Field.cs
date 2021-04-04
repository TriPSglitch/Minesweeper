using System;

namespace FieldSpace
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
                    int tempI = i;                                                          // Сохраняем начальную строку
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
}
