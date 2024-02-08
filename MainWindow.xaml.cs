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

namespace Pract1wpf
{
   
    public partial class MainWindow : Window
    {
        private bool isPlayer1Turn = true; // Переменная для отслеживания очередности хода игроков
        private bool isGameEnded = false; // Флаг для проверки, завершена ли игра
        public MainWindow()
        {
            InitializeComponent();


        }

       
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!isGameEnded) // Проверяем, не завершилась ли игра
            {
                Button button = (Button)sender;
                if (button.Content.ToString() == "") // Проверяем, не занята ли уже кнопка
                {
                    if (isPlayer1Turn)
                        button.Content = "X";
                    else
                        button.Content = "O";

                    isPlayer1Turn = !isPlayer1Turn; // Переключаем очередность хода
                    button.IsEnabled = false; // Блокируем кнопку после хода

                    CheckForWinner();
                    if (!isPlayer1Turn) // Если сейчас ход робота, вызываем метод для его хода
                        RobotMove();
                }
            }
        }
        private void RestartClick(object sender, RoutedEventArgs e)
        {
            foreach (var control in grid.Children) // Очистка всех кнопок и сброс флагов
            {
                if (control is Button)
                {
                    var button = (Button)control;
                    button.Content = "";
                    button.IsEnabled = true;
                }
            }
            isPlayer1Turn = true;
            isGameEnded = false;
        }
        private void RobotMove()
        {
            // Реализуем простую логику хода робота через генерацию случайного числа для выбора пустой кнопки
            Random random = new Random();
            int index = random.Next(1, 10); // Генерируем случайное число от 1 до 9

            foreach (var control in grid.Children) // Проверяем доступные кнопки и выбираем первую пустую
            {
                if (control is Button)
                {
                    var button = (Button)control;
                    if (button.Name == "btn" + index && button.Content.ToString() == "") // Нашли пустую кнопку
                    {
                        button.Content = "O"; // Робот ставит нолик
                        button.IsEnabled = false; // Блокируем кнопку после хода
                        isPlayer1Turn = true; // После хода робота снова очередь игрока
                        CheckForWinner();
                        break;
                    }
                }
            }
            // Реализуем простую логику хода робота через генерацию случайного числа для выбора пустой кнопки
            Random random1 = new Random();
            int index1 = random.Next(1, 10); // Генерируем случайное число от 1 до 9

            foreach (var control in grid.Children) // Проверяем доступные кнопки и выбираем первую пустую
            {
                if (control is Button)
                {
                    var button = (Button)control;
                    if (button.Name == "btn" + index && button.Content.ToString() == "") // Нашли пустую кнопку
                    {
                        button.Content = "O"; // Робот ставит нолик
                        button.IsEnabled = false; // Блокируем кнопку после хода
                        isPlayer1Turn = true; // После хода робота снова очередь игрока
                        CheckForWinner();
                        break;
                    }
                }
            }
        }

        private bool CheckLineForWinner(Button btn1, Button btn2, Button btn3)
        {
            return (!string.IsNullOrEmpty(btn1.Content.ToString()) && btn1.Content == btn2.Content && btn2.Content == btn3.Content);
        }

        private void CheckForWinner()
        {
            // Проверяем все возможные комбинации выигрышных линий
            if (CheckLineForWinner(btn1, btn2, btn3) || CheckLineForWinner(btn4, btn5, btn6) || CheckLineForWinner(btn7, btn8, btn9) ||
                CheckLineForWinner(btn1, btn4, btn7) || CheckLineForWinner(btn2, btn5, btn8) || CheckLineForWinner(btn3, btn6, btn9) ||
                CheckLineForWinner(btn1, btn5, btn9) || CheckLineForWinner(btn3, btn5, btn7))
            {
                isGameEnded = true;
                MessageBox.Show((!isPlayer1Turn ? "Крестики" : "Нолики") + " выиграли!");
            }
            else // Проверяем наличие ничьей
            {
                bool isBoardFull = true;
                foreach (var control in grid.Children)
                {
                    if (control is Button)
                    {
                        var button = (Button)control;
                        if (button.Content.ToString() == "")
                        {
                            isBoardFull = false;
                            break;
                        }
                    }
                }
                if (isBoardFull)
                {
                    isGameEnded = true;
                    MessageBox.Show("Ничья!");
                }
            }
        }


    }
}
//XAML файлы(я не знаю как отдельно их добавить в гитхаб):
<Window x:Class="Pract1wpf.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height="512" Width="599">

    <Grid x:Name="grid">
        <Grid.RowDefinitions>
            <RowDefinition Height="111*" />
            <RowDefinition Height="111*" />
            <RowDefinition Height="111*" />
            <RowDefinition Height="29*"/>
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition />
            <ColumnDefinition />
            <ColumnDefinition />
        </Grid.ColumnDefinitions>

        <Button x:Name="btn1" Grid.Row="0" Grid.Column="0" Content="" Click="ButtonClick" IsEnabled="False"/>
        <Button x:Name="btn2" Grid.Row="0" Grid.Column="1" Content="" Click="ButtonClick" IsEnabled="False"/>
        <Button x:Name="btn3" Grid.Row="0" Grid.Column="2" Content="" Click="ButtonClick" IsEnabled="False"/>
        <Button x:Name="btn4" Grid.Column="0" Content="" Click="ButtonClick" IsEnabled="False" Margin="0,111,0,111" Grid.RowSpan="3"/>
        <Button x:Name="btn5" Grid.Column="1" Content="" Click="ButtonClick" IsEnabled="False" Margin="0,111,0,111" Grid.RowSpan="3"/>
        <Button x:Name="btn6" Grid.Column="2" Content="" Click="ButtonClick" IsEnabled="False" Margin="0,111,0,111" Grid.RowSpan="3"/>
        <Button x:Name="btn7" Grid.Row="2" Grid.Column="0" Content="" Click="ButtonClick" IsEnabled="False"/>
        <Button x:Name="btn8" Grid.Row="2" Grid.Column="1" Content="" Click="ButtonClick" IsEnabled="False"/>
        <Button x:Name="btn9" Grid.Row="2" Grid.Column="2" Content="" Click="ButtonClick" IsEnabled="False"/>

        <Button x:Name="btnRestart" Grid.Row="2" Grid.ColumnSpan="3" Content="Начать заново" Click="RestartClick" IsEnabled="True" Margin="0,152,0,0" Grid.RowSpan="2"/>
    </Grid>
</Window>
