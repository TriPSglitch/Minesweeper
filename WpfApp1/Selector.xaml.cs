using System.Windows;
using System.Windows.Controls;
using GameManagement;
using WpfApp1;

namespace DifficultSelector
{
    /// <summary>
    /// Логика для выбора уровня сложности
    /// </summary>
    public partial class Selector : Window
    {
        MainWindow reloader = new MainWindow();
        public Selector()
        {
            InitializeComponent();
            Mines.Content = GameManager.MineCount;
        }

        private void LevelChanger(object sender, RoutedEventArgs e)
        {
            #region Смена уровня сложности
            if ((string)((Button)sender).Content == "Низкий")
            {
                GameManager.MineScale = 0.15;
            }
            else if ((string)((Button)sender).Content == "Средний")
            {
                GameManager.MineScale = 0.23;
            }
            else if ((string)((Button)sender).Content == "Высокий")
            {
                GameManager.MineScale = 0.30;
            }
            GameManager.MineCountUpdate();
            Mines.Content = GameManager.MineCount;
            reloader.Reload();
            #endregion
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            Close();
            reloader.Reload();
        }
    }
}