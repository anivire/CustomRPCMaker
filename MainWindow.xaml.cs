using DiscordRPC;
using DiscordRPC.Message;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CustomRPCMaker
{
    // please don't punch me for this code idk how make this easier
    public class Config
    {
        public string ConfigAppID { get; set; }
        public string ConfigDetails { get; set; }
        public string ConfigState { get; set; }
        public string ConfigBigImage { get; set; }
        public string ConfigSmallImage { get; set; }
        public string ConfigBigImageText { get; set; }
        public string ConfigSmallImageText { get; set; }
        public int ConfigPartySizeMin { get; set; }
        public int ConfigPartySizeMax { get; set; }
        public double ConfigTimestampStart { get; set; }
        public double ConfigTimestampEnd { get; set; }

        public bool ConfigIsDetails = false;
        public bool ConfigIsState = false;
        public bool ConfigIsTimestamp = false;
        public bool ConfigIsBigImageName = false;
        public bool ConfigIsSmallImageName = false;
        public bool ConfigIsBigImageText = false;
        public bool ConfigIsSmallImageText = false;
        public bool ConfigIsParty = false;
    }

    public class Settings
    {
        public bool ConfigIsMinimizeCheck = false;
        public bool ConfigIsAutoLoadCheck = false;
        public bool ConfigEnableAppCheck = false;
        public bool ConfigIsElapsedTimeOn = false;
        public bool ConfigIsCurrentTimeOn = false;
        public string ConfigPath { get; set; }
    }

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
        public double TimestampStart { get; set; }
        public double TimestampEnd { get; set; }

        public bool IsDetails = false;
        public bool IsState = false;
        public bool IsTimestamp = false;
        public bool IsBigImageName = false;
        public bool IsSmallImageName = false;
        public bool IsBigImageText = false;
        public bool IsSmallImageText = false;
        public bool IsParty = false;

        public bool IsStarted = false;
        public bool IsMinimizeCheck = false;
        public bool IsAutoLoadCheck = false;

        public bool IsConfigLoaded { get; set;}

        public MainWindow()
        {
            InitializeComponent();

            TaskbarIcon.Icon = new Icon(@"C:\Users\anivire\source\repos\CustomRPCMaker\ui_assets\Discord-Logo-Color.ico");
            TaskbarIcon.ToolTipText = "Discord RPC Maker";

            this.Dispatcher.Invoke(() =>
            {
                this.ConsoleTextBox.Text += $"[INFO] Welcome to Custom Discord RPC!\n";
                this.ConsoleTextBox.Text += $"[INFO] Change RPC settings in left box\n";
            });

            Settings loadConfig = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Environment.CurrentDirectory + @"\AppConfig.json"));

            if (!File.Exists(Environment.CurrentDirectory + @"\AppConfig.json"))
            {
                Settings settingsConfig = new Settings
                {
                    ConfigIsMinimizeCheck = false,
                    ConfigIsAutoLoadCheck = false,
                    ConfigEnableAppCheck = false,
                    ConfigIsElapsedTimeOn = false,
                    ConfigIsCurrentTimeOn = false,
                    ConfigPath = String.Empty
                };

                File.WriteAllText(Environment.CurrentDirectory + @"\AppConfig.json", JsonConvert.SerializeObject(settingsConfig, Formatting.Indented));
            }
            else
            {
                try
                {
                    if (loadConfig.ConfigIsAutoLoadCheck)
                    {
                        IsAutoLoadCheck = true;
                        AutoLoadConfigCheck.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_check_box_white_36dp.png"));

                        Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(loadConfig.ConfigPath));

                        this.Dispatcher.Invoke(() =>
                        {
                            ClientIDBox.Password = config.ConfigAppID;
                            DetailsNameTextBox.Text = config.ConfigDetails;
                            StateNameTextBox.Text = config.ConfigState;
                            BigImageNameTextBox.Text = config.ConfigBigImage;
                            SmallImageNameTextBox.Text = config.ConfigSmallImage;
                            BigImageTextTextBox.Text = config.ConfigBigImageText;
                            SmallImageTextTextBox.Text = config.ConfigSmallImageText;
                            PartySizeMinTextBox.Text = Convert.ToString(config.ConfigPartySizeMin);
                            PartySizeMaxTextBox.Text = Convert.ToString(config.ConfigPartySizeMax);
                            TimestampStartTextBox.Text = Convert.ToString(config.ConfigTimestampStart);
                            TimestampEndTextBox.Text = Convert.ToString(config.ConfigTimestampEnd);
                            IsDetails = config.ConfigIsDetails;
                            IsState = config.ConfigIsState;
                            IsTimestamp = config.ConfigIsTimestamp;
                            IsBigImageName = config.ConfigIsBigImageName;
                            IsSmallImageName = config.ConfigIsSmallImageName;
                            IsBigImageText = config.ConfigIsBigImageText;
                            IsSmallImageText = config.ConfigIsSmallImageText;
                            IsParty = config.ConfigIsParty;

                            this.Dispatcher.Invoke(() =>
                            {
                                if (IsDetails)
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.ConsoleTextBox.Text += $"[LOG] Details field ENABLED\n";
                                    });
                                    DetailsNameTextBox.IsEnabled = true;
                                    DetailsButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                                }
                                if (IsState)
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.ConsoleTextBox.Text += $"[LOG] State field ENABLED\n";
                                    });
                                    StateNameTextBox.IsEnabled = true;
                                    StateButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                                }
                                if (IsTimestamp)
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.ConsoleTextBox.Text += $"[LOG] Timestamp field ENABLED\n";
                                    });
                                    TimestampStartTextBox.IsEnabled = true;
                                    TimestampEndTextBox.IsEnabled = true;
                                    TimestampButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                                }
                                if (IsBigImageName)
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.ConsoleTextBox.Text += $"[LOG] Large image field ENABLED\n";
                                    });
                                    BigImageNameTextBox.IsEnabled = true;
                                    BigImageTextTextBox.IsEnabled = true;
                                    BigImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                                }
                                if (IsSmallImageName)
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.ConsoleTextBox.Text += $"[LOG] Large image field ENABLED\n";
                                    });
                                    SmallImageNameTextBox.IsEnabled = true;
                                    SmallImageTextTextBox.IsEnabled = true;
                                    IsSmallImageName = true;
                                    SmallImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                                }
                                if (IsParty)
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        this.ConsoleTextBox.Text += $"[LOG] Party size field ENABLED\n";
                                    });
                                    PartySizeMinTextBox.IsEnabled = true;
                                    PartySizeMaxTextBox.IsEnabled = true;
                                    PartyButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                                }

                                LoadPathConfigTextBox.Text = loadConfig.ConfigPath;
                                EditPathConfigTextBox.Text = loadConfig.ConfigPath;

                                this.ConsoleTextBox.Text += $"[INFO] Config successfully loaded!\n";
                            });
                        });
                    }
                }
                catch
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[ERROR] Error loading Discord RPC config file, it may have been moved!\n";

                        AutoLoadConfigCheck.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_check_box_outline_blank_white_36dp.png"));
                        IsAutoLoadCheck = false;
                    });
                }
            }

            if (loadConfig.ConfigIsMinimizeCheck)
            {
                IsMinimizeCheck = true;
                MinimizeToTrayCheck.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_check_box_white_36dp.png"));
            }
        }

        private void CloseApp_Click(object sender, MouseButtonEventArgs e)
        {
            if (IsMinimizeCheck)
            {
                this.Hide();
            }
            else
            {
                Close();
            }
        }

        private void MinimizeApp_Click(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
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

        private void ChooseSavePathButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Config config = new Config()
            {
                  ConfigAppID = AppID,
                  ConfigDetails = Details,
                  ConfigState = State,
                  ConfigBigImage = BigImage,
                  ConfigSmallImage = SmallImage,
                  ConfigBigImageText = BigImageText,
                  ConfigSmallImageText = SmallImageText,
                  ConfigPartySizeMin = PartySizeMin,
                  ConfigPartySizeMax = PartySizeMax,
                  ConfigTimestampStart = TimestampStart,
                  ConfigTimestampEnd = TimestampEnd,
                  ConfigIsDetails = IsDetails,
                  ConfigIsState = IsState,
                  ConfigIsTimestamp = IsTimestamp,
                  ConfigIsBigImageName = IsBigImageName,
                  ConfigIsSmallImageName = IsSmallImageName,
                  ConfigIsBigImageText = IsBigImageText,
                  ConfigIsSmallImageText = IsSmallImageText,
                  ConfigIsParty = IsParty
            };

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = " Config files (*.json) | *.json";
            saveFileDialog.FileName = "DiscordRPCConfig.json";
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(config, Formatting.Indented));

            SavePathConfigTextBox.Text = saveFileDialog.FileName;
            EditPathConfigTextBox.Text = saveFileDialog.FileName;
        }

        private void ChooseLoadPathButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ClearButton_MouseLeftButtonDown(sender, e);

                IsConfigLoaded = true;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = " Config files (*.json) | *.json";

                if (openFileDialog.ShowDialog() == true)
                {
                    LoadPathConfigTextBox.Text = openFileDialog.FileName;
                    EditPathConfigTextBox.Text = openFileDialog.FileName;
                }

                Config loadConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(openFileDialog.FileName));

                this.Dispatcher.Invoke(() =>
                {
                    ClientIDBox.Password = loadConfig.ConfigAppID;
                    DetailsNameTextBox.Text = loadConfig.ConfigDetails;
                    StateNameTextBox.Text = loadConfig.ConfigState;
                    BigImageNameTextBox.Text = loadConfig.ConfigBigImage;
                    SmallImageNameTextBox.Text = loadConfig.ConfigSmallImage;
                    BigImageTextTextBox.Text = loadConfig.ConfigBigImageText;
                    SmallImageTextTextBox.Text = loadConfig.ConfigSmallImageText;
                    PartySizeMinTextBox.Text = Convert.ToString(loadConfig.ConfigPartySizeMin);
                    PartySizeMaxTextBox.Text = Convert.ToString(loadConfig.ConfigPartySizeMax);
                    TimestampStartTextBox.Text = Convert.ToString(loadConfig.ConfigTimestampStart);
                    TimestampEndTextBox.Text = Convert.ToString(loadConfig.ConfigTimestampEnd);
                    IsDetails = loadConfig.ConfigIsDetails;
                    IsState = loadConfig.ConfigIsState;
                    IsTimestamp = loadConfig.ConfigIsTimestamp;
                    IsBigImageName = loadConfig.ConfigIsBigImageName;
                    IsSmallImageName = loadConfig.ConfigIsSmallImageName;
                    IsBigImageText = loadConfig.ConfigIsBigImageText;
                    IsSmallImageText = loadConfig.ConfigIsSmallImageText;
                    IsParty = loadConfig.ConfigIsParty;

                    this.Dispatcher.Invoke(() =>
                    {
                        if (IsDetails)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[LOG] Details field ENABLED\n";
                            });
                            DetailsNameTextBox.IsEnabled = true;
                            DetailsButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }

                        if (IsState)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[LOG] State field ENABLED\n";
                            });
                            StateNameTextBox.IsEnabled = true;
                            StateButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }
                        else

                        if (IsTimestamp)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[LOG] Timestamp field ENABLED\n";
                            });
                            TimestampStartTextBox.IsEnabled = true;
                            TimestampEndTextBox.IsEnabled = true;
                            TimestampButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }
                        else

                        if (IsBigImageName)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[LOG] Large image field ENABLED\n";
                            });
                            BigImageNameTextBox.IsEnabled = true;
                            BigImageTextTextBox.IsEnabled = true;
                            BigImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }
                        else

                        if (IsSmallImageName)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[LOG] Large image field ENABLED\n";
                            });
                            SmallImageNameTextBox.IsEnabled = true;
                            SmallImageTextTextBox.IsEnabled = true;
                            IsSmallImageName = true;
                            SmallImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }

                        if (IsParty)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[LOG] Party size field ENABLED\n";
                            });
                            PartySizeMinTextBox.IsEnabled = true;
                            PartySizeMaxTextBox.IsEnabled = true;
                            PartyButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }

                        this.ConsoleTextBox.Text += $"[INFO] Config successfully loaded!\n";
                    });
                });
            }
            catch
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] Error while config file loading!\n";
                });
            }
        }

        private void Footer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void StartRPCButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                try
                {
                    if (!IsStarted)
                    {
                        Client = new DiscordRpcClient(AppID);
                        Client.Initialize();

                        Client.OnReady += OnReady;
                        Client.OnPresenceUpdate += OnPresenceUpdate;
                        Client.OnConnectionFailed += OnConnectionFailed;
                        Client.OnConnectionEstablished += OnConnectionEstablished;
                        Client.OnClose += OnClose;

                        Client.SetPresence(new RichPresence()
                        {
                            Details = Details,
                            State = State,
                            Timestamps = new Timestamps()
                            {
                                StartUnixMilliseconds = (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1,
                                    DateTime.Now.Hour,
                                    DateTime.Now.Minute,
                                    DateTime.Now.Second))).TotalSeconds,
                            },
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
                        IsStarted = true;
                    }
                    else
                    {
                        Client.Dispose();
                        this.Dispatcher.Invoke(() =>
                        {
                            this.ConsoleTextBox.Text += $"[INFO] Discord RPC connection closed\n";
                        });
                        IsStarted = false;
                    }
                }
                catch
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[ERROR] Enter Client ID before starting!\n";
                    });
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }

        private void OnClose(object sender, CloseMessage args)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.ConsoleTextBox.Text += $"[INFO] Discord RPC connection closed\n";
            });
        }

        private void OnConnectionEstablished(object sender, ConnectionEstablishedMessage args)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.ConsoleTextBox.Text += $"[INFO] Discord connection successfully established!\n";
            });
        }

        private void OnConnectionFailed(object sender, ConnectionFailedMessage args)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.ConsoleTextBox.Text += $"[ERROR] Failed to connect Discord, run application and try again...\n";
            });
        }

        private void OnPresenceUpdate(object sender, PresenceMessage args)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.ConsoleTextBox.Text += $"[INFO] Discord RPC successfully updated!\n";
            });
        }

        private void OnReady(object sender, ReadyMessage args)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.ConsoleTextBox.Text += $"[INFO] RPC client ready to work with user {args.User.Username}!\n";
            });
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

        private void DetailsButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsDetails == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Details field ENABLED\n";
                });
                DetailsNameTextBox.IsEnabled = true;
                IsDetails = true;
                DetailsButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Details field DISABLED\n";
                });
                DetailsNameTextBox.IsEnabled = false;
                IsDetails = false;
                DetailsButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void StateButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsState == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] State field ENABLED\n";
                });
                StateNameTextBox.IsEnabled = true;
                IsState = true;
                StateButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] State field DISABLED\n";
                });
                StateNameTextBox.IsEnabled = false;
                IsState = false;
                StateButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void TimestampButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsTimestamp == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Timestamp field ENABLED\n";
                });
                TimestampStartTextBox.IsEnabled = true;
                TimestampEndTextBox.IsEnabled = true;
                IsTimestamp = true;
                TimestampButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Timestamp field DISABLED\n";
                });
                TimestampStartTextBox.IsEnabled = false;
                TimestampEndTextBox.IsEnabled = false;
                IsTimestamp = false;
                TimestampButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void BigImageButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsBigImageName == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Large image field ENABLED\n";
                });
                BigImageNameTextBox.IsEnabled = true;
                BigImageTextTextBox.IsEnabled = true;
                IsBigImageName = true;
                BigImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Large image field DISABLED\n";
                });
                BigImageNameTextBox.IsEnabled = false;
                BigImageTextTextBox.IsEnabled = false;
                IsBigImageName = false;
                BigImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void SmallImageButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsSmallImageName == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Small image field ENABLED\n";
                });
                SmallImageNameTextBox.IsEnabled = true;
                SmallImageTextTextBox.IsEnabled = true;
                IsSmallImageName = true;
                SmallImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Small image field DISABLED\n";
                });
                SmallImageNameTextBox.IsEnabled = false;
                SmallImageTextTextBox.IsEnabled = false;
                IsSmallImageName = false;
                SmallImageButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void PartyButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsParty == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Party size field ENABLED\n";
                });
                PartySizeMinTextBox.IsEnabled = true;
                PartySizeMaxTextBox.IsEnabled = true;
                IsParty = true;
                PartyButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[LOG] Party size field DISABLED\n";
                });
                PartySizeMinTextBox.IsEnabled = false;
                PartySizeMaxTextBox.IsEnabled = false;
                IsParty = false;
                PartyButton.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
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

        private void PartySizeMinTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (PartySizeMinTextBox.Text.Length < 1)
                {
                    PartySizeMin = 0;
                    PartySizePreview.Content = $"Party size ({PartySizeMin} of {PartySizeMax})";
                }
                else if (Convert.ToInt32(PartySizeMinTextBox.Text) > 999)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    PartySizeMin = Convert.ToInt32(PartySizeMinTextBox.Text);
                    PartySizePreview.Content = $"Party size ({PartySizeMin} of {PartySizeMax})";
                }
            }
            catch
            {
                PartySizeMinTextBox.Text = null;
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] Party size value too big!\n";
                });
            }
        }

        private void PartySizeMaxTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PartySizeMaxTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (PartySizeMaxTextBox.Text.Length < 1)
                {
                    PartySizeMax = 0;
                    PartySizePreview.Content = $"Party size ({PartySizeMin} of {PartySizeMax})";
                }
                else if (Convert.ToInt32(PartySizeMaxTextBox.Text) > 999)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    PartySizeMax = Convert.ToInt32(PartySizeMaxTextBox.Text);
                    PartySizePreview.Content = $"Party size ({PartySizeMin} of {PartySizeMax})";
                }
            }
            catch
            {
                PartySizeMaxTextBox.Text = null;
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] Party size value too big!\n";
                });
            }
        }

        private void TimestampStartTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (TimestampStartTextBox.Text.Length < 1)
                {
                    TimestampStart = 0;
                }
                else if (Convert.ToDouble(TimestampStartTextBox.Text) > 2147483648)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                   
                    TimestampStart = Convert.ToDouble(TimestampStartTextBox.Text);
                }
            }
            catch
            {
                TimestampStartTextBox.Text = null;
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] Timestamp value too big!\n";
                });
            }
        }

        private void TimestampEndTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (TimestampEndTextBox.Text.Length < 1)
                {
                    TimestampEnd = 0;
                }
                else if (Convert.ToDouble(TimestampEndTextBox.Text) > 2147483647)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {

                    TimestampEnd = Convert.ToDouble(TimestampEndTextBox.Text);
                }
            }
            catch
            {
                TimestampEndTextBox.Text = null;
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] Timestamp value too big!\n";
                });
            }
        }

        private void ReloadRPC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Client.SetPresence(new RichPresence()
                {
                    Details = Details,
                    State = State,
                    Timestamps = new Timestamps()
                    {
                        StartUnixMilliseconds = (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1,
                                DateTime.Now.Hour,
                                DateTime.Now.Minute,
                                DateTime.Now.Second))).TotalSeconds,
                    },
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
            catch
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] Enter Client ID before starting\n";
                });
            }
        }

        private void ClearButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DetailsNameTextBox.Text = String.Empty;
            StateNameTextBox.Text = String.Empty;
            TimestampStartTextBox.Text = String.Empty;
            TimestampEndTextBox.Text = String.Empty;
            BigImageNameTextBox.Text = String.Empty;
            SmallImageNameTextBox.Text = String.Empty;
            BigImageTextTextBox.Text = String.Empty;
            SmallImageTextTextBox.Text = String.Empty;
            PartySizeMinTextBox.Text = String.Empty;
            PartySizeMaxTextBox.Text = String.Empty;
            ClientIDBox.Password = String.Empty;

            AppID = String.Empty;
            Details = String.Empty;
            State = String.Empty;
            BigImage = String.Empty;
            SmallImage = String.Empty;
            BigImageText = String.Empty;
            SmallImageText = String.Empty;
            PartySizeMin = 0;
            PartySizeMax = 0;
            TimestampStart = 0;
            TimestampEnd = 0;

            IsDetails = false;
            IsState = false;
            IsTimestamp = false;
            IsBigImageName = false;
            IsSmallImageName = false;
            IsBigImageText = false;
            IsSmallImageText = false;
            IsParty = false;

            this.Dispatcher.Invoke(() =>
            {
                this.ConsoleTextBox.Text += $"[LOG] All fields cleared\n";
            });
        }

        private void OpenConfigButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SavePathConfigTextBox.Text != String.Empty)
                {
                    Process.Start("C:\\Windows\\System32\\notepad.exe", SavePathConfigTextBox.Text);
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[LOG] Open config file\n";
                    });
                }
                else if (LoadPathConfigTextBox.Text != String.Empty)
                {
                    Process.Start("C:\\Windows\\System32\\notepad.exe", LoadPathConfigTextBox.Text);
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[LOG] Open config file\n";
                    });
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] No config file selected\n";
                });
            }
        }

        private void MinimizeToTrayCheck_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsMinimizeCheck)
            {
                IsMinimizeCheck = false;
                MinimizeToTrayCheck.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_check_box_outline_blank_white_36dp.png"));
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[RPC Settings] Minimize to tray while closing program DISABLED\n";
                });

                Settings tempConfig = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Environment.CurrentDirectory + @"\AppConfig.json"));

                Settings settingsConfig = new Settings
                {
                    ConfigIsMinimizeCheck = false,
                    ConfigIsAutoLoadCheck = tempConfig.ConfigIsAutoLoadCheck,
                    ConfigEnableAppCheck = tempConfig.ConfigEnableAppCheck,
                    ConfigIsElapsedTimeOn = tempConfig.ConfigIsElapsedTimeOn,
                    ConfigIsCurrentTimeOn = tempConfig.ConfigIsCurrentTimeOn,
                    ConfigPath = tempConfig.ConfigPath
                };

                File.WriteAllText(Environment.CurrentDirectory + @"\AppConfig.json", JsonConvert.SerializeObject(settingsConfig, Formatting.Indented));
            }
            else
            {
                IsMinimizeCheck = true;
                MinimizeToTrayCheck.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_check_box_white_36dp.png"));
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[RPC Settings] Minimize to tray while closing program ENABLED\n";
                });

                Settings tempConfig = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Environment.CurrentDirectory + @"\AppConfig.json"));

                Settings settingsConfig = new Settings
                {
                    ConfigIsMinimizeCheck = true,
                    ConfigIsAutoLoadCheck = tempConfig.ConfigIsAutoLoadCheck,
                    ConfigEnableAppCheck = tempConfig.ConfigEnableAppCheck,
                    ConfigIsElapsedTimeOn = tempConfig.ConfigIsElapsedTimeOn,
                    ConfigIsCurrentTimeOn = tempConfig.ConfigIsCurrentTimeOn,
                    ConfigPath = tempConfig.ConfigPath
                };

                File.WriteAllText(Environment.CurrentDirectory + @"\AppConfig.json", JsonConvert.SerializeObject(settingsConfig, Formatting.Indented));
            }
        }

        private void AutoLoadConfigCheck_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Settings tempConfig = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Environment.CurrentDirectory + @"\AppConfig.json"));

                var newConfigPath = String.Empty;

                if (SavePathConfigTextBox.Text != String.Empty)
                    newConfigPath = SavePathConfigTextBox.Text;
                else if (LoadPathConfigTextBox.Text != String.Empty)
                    newConfigPath = LoadPathConfigTextBox.Text;
                else if (SavePathConfigTextBox.Text == String.Empty || LoadPathConfigTextBox.Text == String.Empty)
                    throw new Exception();

                if (IsAutoLoadCheck)
                {
                    IsAutoLoadCheck = false;
                    AutoLoadConfigCheck.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_check_box_outline_blank_white_36dp.png"));
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[RPC Settings] Auto-load config file DISABLED\n";
                    });

                    Settings settingsConfig = new Settings
                    {
                        ConfigIsMinimizeCheck = tempConfig.ConfigIsMinimizeCheck,
                        ConfigIsAutoLoadCheck = false,
                        ConfigEnableAppCheck = tempConfig.ConfigEnableAppCheck,
                        ConfigIsElapsedTimeOn = tempConfig.ConfigIsElapsedTimeOn,
                        ConfigIsCurrentTimeOn = tempConfig.ConfigIsCurrentTimeOn,
                        ConfigPath = null 
                    }; 

                    File.WriteAllText(Environment.CurrentDirectory + @"\AppConfig.json", JsonConvert.SerializeObject(settingsConfig, Formatting.Indented));
                }
                else
                {
                    IsAutoLoadCheck = true;
                    AutoLoadConfigCheck.Source = new BitmapImage(new Uri("C:/Users/anivire/source/repos/CustomRPCMaker/ui_assets/icons/baseline_check_box_white_36dp.png"));
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[RPC Settings] Auto-load config file ENABLED\n";
                    });

                    Settings settingsConfig = new Settings
                    {
                        ConfigIsMinimizeCheck = tempConfig.ConfigIsMinimizeCheck,
                        ConfigIsAutoLoadCheck = true,
                        ConfigEnableAppCheck = tempConfig.ConfigEnableAppCheck,
                        ConfigIsElapsedTimeOn = tempConfig.ConfigIsElapsedTimeOn,
                        ConfigIsCurrentTimeOn = tempConfig.ConfigIsCurrentTimeOn,
                        ConfigPath = newConfigPath
                    };

                    File.WriteAllText(Environment.CurrentDirectory + @"\AppConfig.json", JsonConvert.SerializeObject(settingsConfig, Formatting.Indented));
                }
            }
            catch
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] Save or load config for enable auto-load\n";
                });
            }
        }
    }
}
