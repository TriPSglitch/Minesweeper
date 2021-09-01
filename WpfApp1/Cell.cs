namespace CellSpace
{
    /// <summary>
    /// Класс для описания клеток
    /// </summary>
    class Cell
    {
        public bool IsMine { get; set; } = false;              // Ячейка имеет состояние мины и нет
        public int MineAround { get; set; } = 0;               // количество мин вокруг
        public bool IsFlagged { get; set; } = false;           // Для проверки установлен ли флаг на клетку
        public bool IsOpen { get; set; } = false;              // Для того, чтобы показывать количество мин вокруг при снятии флага, если клетка уже была открыта
    }
}