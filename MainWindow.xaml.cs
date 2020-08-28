using DiscordRPC;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CustomRPCMaker
{
    public partial class MainWindow : Window
    {
        public DiscordRpcClient Client { get; private set; }

        public string AppID { get; set; }
        public string Details { get; set; }
        public string State { get; set; }
        public string BigImage { get; set; }
        public string SmallImage { get; set; }
        public string BigImageText { get; set; }
        public string SmallImageText { get; set; }
        public int PartySizeMin { get; set; }
        public int PartySizeMax { get; set; }

        public bool IsDetails = false;
        public bool IsState = false;
        public bool IsTimestamp = false;
        public bool IsBigImageName = false;
        public bool IsSmallImageName = false;
        public bool IsBigImageText = false;
        public bool IsSmallImageText = false;
        public bool IsParty = false;
        public bool IsStarted = false;

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
            this.Hide();
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
                TimestampStartTextBox.IsEnabled = true;
                TimestampEndTextBox.IsEnabled = true;
                IsTimestamp = true;
                TimestampButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                TimestampStartTextBox.IsEnabled = false;
                TimestampEndTextBox.IsEnabled = false;
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

        private void ChooseSavePathButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ChooseLoadPathButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = " Config files (*.json) | *.json";

            if (openFileDialog.ShowDialog() == true)
            {
                LoadPathConfigTextBox.Text = openFileDialog.FileName;
            }
        }

        private void Footer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void StartRPCButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsStarted && AppID != null)
            {
                Client = new DiscordRpcClient(AppID, autoEvents: false);
                Client.Initialize();

                Client.OnReady += (senderCtx, ctx) =>
                {
                };

                Client.SetPresence(new RichPresence()
                {
                    Details = Details,
                    State = State,
                    Timestamps = Timestamps.Now,
                    Party = new Party()
                    {
                        ID = "justTextForWorkAPrtySystem",
                        Size = PartySizeMin,
                        Max = PartySizeMax
                    },
                    Assets = new Assets()
                    {
                        LargeImageKey = BigImage,
                        LargeImageText = BigImageText,
                        SmallImageKey = SmallImage,
                        SmallImageText = SmallImageText
                    }
                });
            }
            else if (AppID == null)
            {

            }

        }

        void Update()
        {
            //Invoke the events once per-frame. The events will be executed on calling thread.
            Client.Invoke();
        }

        private void DetailsNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Details = DetailsNameTextBox.Text;
            if (Details.Length > 15)
            {
                DetailsTextPreview.Content = Details.Substring(0, 15) + "...";
            }
            else
            {
                DetailsTextPreview.Content = DetailsNameTextBox.Text;
            }
        }

        private void StateNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            State = StateNameTextBox.Text;
            if (State.Length > 15)
            {
                StateTextPreview.Content = State.Substring(0, 15) + "...";
            }
            else
            {
                StateTextPreview.Content = StateNameTextBox.Text;
            }
        }

        private void SmallImageTextTexBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SmallImageText = BigImageNameTextBox.Text;
            HiddenSmallImageText.ToolTip = SmallImageTextTextBox.Text;
        }

        private void BigImageTextTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            BigImageText = BigImageNameTextBox.Text;
            HiddenBigImageText.ToolTip = BigImageNameTextBox.Text;
        }

        private void ClientIDBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            AppID = ClientIDBox.Password;
        }

        private void BigImageNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            BigImage = BigImageNameTextBox.Text;
        }

        private void SmallImageNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SmallImage = BigImageNameTextBox.Text;
        }

        private void ConsoleTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ConsoleTextBox.ScrollToEnd();
        }

        private void TimestampStartTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TimestampEndTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PartySizeMinTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PartyButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsParty)
            {
                PartySizeMinTextBox.IsEnabled = true;
                PartySizeMaxTextBox.IsEnabled = true;
                IsParty = true;
                PartyButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                PartySizeMinTextBox.IsEnabled = false;
                PartySizeMaxTextBox.IsEnabled = false;
                IsParty = false;
                PartyButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void PartySizeMinTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (PartySizeMinTextBox.Text.Length < 1)
            {
                PartySizeMin = 0;
                PartySizePreview.Content = $"Party size ({PartySizeMin} of {PartySizeMax})";
            }
            else if (Convert.ToInt32(PartySizeMinTextBox.Text) > 999)
            {
                PartySizeMinTextBox.Text = null;
            }
            else
            {
                PartySizeMin = Convert.ToInt32(PartySizeMinTextBox.Text);
                PartySizePreview.Content = $"Party size ({PartySizeMin} of {PartySizeMax})";
            }
        }

        private void PartySizeMaxTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PartySizeMaxTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (PartySizeMaxTextBox.Text.Length < 1)
            {
                PartySizeMax = 0;
                PartySizePreview.Content = $"Party size ({PartySizeMin} of {PartySizeMax})";
            }
            else if (Convert.ToInt32(PartySizeMaxTextBox.Text) > 999)
            {
                PartySizeMaxTextBox.Text = null;
            }
            else
            {
                PartySizeMax = Convert.ToInt32(PartySizeMaxTextBox.Text);
                PartySizePreview.Content = $"Party size ({PartySizeMin} of {PartySizeMax})";
            }  
        }
    }
}
