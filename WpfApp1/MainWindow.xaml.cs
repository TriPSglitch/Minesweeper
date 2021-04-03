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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    class Field
    {
        Random rnd = new Random();
        public bool IsBomb { get; set; } = false;
        public int BombCount { get; set; } = 45;
        public void Generate(Field[,] field)
        {
            for (int i = 0; i < field.GetLength(1); i++)
            {
                int count = rnd.Next(0, field.GetLength(0));
                for (int j = 0; j < count; j++)
                {
                    int temp = rnd.Next(0, field.GetLength(0));
                    if (field[i, temp].IsBomb == true)
                    {
                        bool isEnter = false;
                        while (field[i, temp].IsBomb && temp < field.GetLength(0))
                        {
                            if (!isEnter && temp == field.GetLength(0) - 1)
                                temp = 0;
                            temp++;
                        }
                    }
                    field[i, temp].IsBomb = true;
                    BombCount--;
                    if (BombCount == 0)
                        break;
                }
                if (BombCount == 0)
                    break;
            }
            if (BombCount > 0)
                Generate(field);
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Button button = new Button();
            Field[,] field = new Field[10, 10];
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = new Field();
                }
            }
            Field fieldConstructor = new Field();
            int bombCount = fieldConstructor.BombCount;
            fieldConstructor.Generate(field);
            if (field[0, 0].IsBomb)
                C00.Content = '1';
            else
                C00.Content = '0';
            /*for (int i = 0; i < field.GetLength(0); i++)
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
            }*/
        }
    }
}
