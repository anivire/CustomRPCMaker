using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomRPCMaker
{
    public partial class MainWindow : Window
    {
        public bool IsDetails = false;
        public bool IsState = false;
        public bool IsTimestamp = false;
        public bool IsBigImageName = false;
        public bool IsSmallImageName = false;
        public bool IsBigImageText = false;
        public bool IsSmallImageText = false;

        public MainWindow()
        {
            InitializeComponent();

            TaskbarIcon.Icon = new Icon(@"C:\Users\anivire\source\repos\CustomRPCMaker\ui_assets\Discord-Logo-Color.ico");
            TaskbarIcon.ToolTipText = "Discord RPC Maker";
        }

        private void CloseApp_Click(object sender, MouseButtonEventArgs e)
        {
            Close(); 
        }

        private void MinimizeApp_Click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MainWorkingWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var move = sender as System.Windows.Controls.Grid;
            var win = Window.GetWindow(move);
            win.DragMove();
        }

        private void TaskbarIcon_TrayMouseClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
        }

        private void CloseApp_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseApp.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_close_red_36dp.png"));
        }

        private void CloseApp_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseApp.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_close_white_36dp.png"));
        }

        private void DetailsButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsDetails)
            {
                DetailsNameTextBox.IsEnabled = true;
                IsDetails = true;
                DetailsButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                DetailsNameTextBox.IsEnabled = false;
                IsDetails = false;
                DetailsButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void StateButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsState)
            {
                StateNameTextBox.IsEnabled = true;
                IsState = true;
                StateButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                StateNameTextBox.IsEnabled = false;
                IsState = false;
                StateButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void TimestampButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsTimestamp)
            {
                TimestampTextBox.IsEnabled = true;
                IsTimestamp = true;
                TimestampButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                TimestampTextBox.IsEnabled = false;
                IsTimestamp = false;
                TimestampButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void BigImageButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsBigImageName)
            {
                BigImageNameTextBox.IsEnabled = true;
                BigImageTextTextBox.IsEnabled = true;
                IsBigImageName = true;
                BigImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                BigImageNameTextBox.IsEnabled = false;
                BigImageTextTextBox.IsEnabled = false;
                IsBigImageName = false;
                BigImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void SmallImageButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsSmallImageName)
            {
                SmallImageNameTextBox.IsEnabled = true;
                SmallImageTextTextBox.IsEnabled = true;
                IsSmallImageName = true;
                SmallImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                SmallImageNameTextBox.IsEnabled = false;
                SmallImageTextTextBox.IsEnabled = false;
                IsSmallImageName = false;
                SmallImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }
    }
}
