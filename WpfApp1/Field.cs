﻿using System;

namespace FieldSpace
{
    /// <summary>
    /// Класс для реализация логики поля
    /// </summary>
    class Field
    {
        Random rnd = new Random();
        public bool IsMine { get; set; } = false;              // Ячейка имеет состояние мины и нет
        public static int MineCount { get; set; } = 10;        // Количсетво мин на поле
        public int MineAround { get; set; } = 0;               // количество мин вокруг
        public bool IsFlagged { get; set; } = false;           // Для проверки установлен ли флаг на клетку
        public bool IsOpen { get; set; } = false;              // Для того, чтобы показывать количество мин вокруг при снятии флага, если клетка уже была открыта
        public void Generate(Field[,] field)                   // Метод для генерации поля
        {
            #region Генерация поля
            for (int i = 0; i < field.GetLength(1); i++)
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
                if (MineCount == 0)                                                         // если все мины поставлены, то завершаем генерацию
                    break;
            }
            if (MineCount > 0)                                                              // если остались невыставленные мины, то вызываем метод снова
                Generate(field);
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
    }
}