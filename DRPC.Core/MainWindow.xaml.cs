using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRPC.Message;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace CustomRPCMaker.DRPC.Core
{
    public class Config
    {
        public string ConfigAppID { get; set; }
        public string ConfigAppName { get; set; }
        public string ConfigDetails { get; set; }
        public string ConfigState { get; set; }
        public string ConfigBigImage { get; set; }
        public string ConfigSmallImage { get; set; }
        public string ConfigBigImageText { get; set; }
        public string ConfigSmallImageText { get; set; }
        public int ConfigPartySizeMin { get; set; }
        public int ConfigPartySizeMax { get; set; }

        public bool ConfigIsDetails = false;
        public bool ConfigIsState = false;
        public bool ConfigIsTimestamp = false;
        public bool ConfigIsBigImageName = false;
        public bool ConfigIsSmallImageName = false;
        public bool ConfigIsBigImageText = false;
        public bool ConfigIsSmallImageText = false;
        public bool ConfigIsParty = false;
        public bool ConfigIsAppCheck = false;
    }

    public class Settings
    {
        public bool ConfigIsMinimizeCheck = false;
        public bool ConfigIsAutoLoadCheck = false;
        public bool ConfigEnableAppCheck = false;
        public bool ConfigIsElapsedTimeCheck = false;
        public bool ConfigIsCurrentTimeCheck = false;
        public string ConfigPath { get; set; }
    }

    public partial class MainWindow : Window
    {
        public DiscordRpcClient Client { get; private set; }

        public string AppID { get; set; }
        public string AppName { get; set; }
        public string Details { get; set; }
        public string State { get; set; }
        public string BigImage { get; set; }
        public string SmallImage { get; set; }
        public string BigImageText { get; set; }
        public string SmallImageText { get; set; }
        public int PartySizeMax { get; set; }
        public int PartySizeMin { get; set; }

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
        public bool IsElapsedTimeCheck = false;
        public bool IsCurrentTimeCheck = false;
        public bool IsAppCheck = false;

        public bool IsConfigLoaded { get; set;}

        public MainWindow()
        {
            InitializeComponent();

            /*Debug taskWindow = new Debug();
            taskWindow.Show();*/

            TaskbarIcon.Icon = new Icon(Environment.CurrentDirectory + @"/assets/ui_assets/discord-logo-Color.ico");
            TaskbarIcon.ToolTipText = "Discord RPC Maker";
            TaskbarIcon.Visibility = Visibility.Hidden;

            this.Dispatcher.Invoke(() =>
            {
                this.ConsoleTextBox.Text += $"[INFO] Welcome to Custom Discord RPC!\n";
                this.ConsoleTextBox.Text += $"[INFO] Change RPC settings in left box\n";
            });

            if (!File.Exists(Environment.CurrentDirectory + @"\AppConfig.json"))
            {
                Settings settingsConfig = new Settings
                {
                    ConfigIsMinimizeCheck = false,
                    ConfigIsAutoLoadCheck = false,
                    ConfigEnableAppCheck = false,
                    ConfigIsElapsedTimeCheck = false,
                    ConfigIsCurrentTimeCheck = false,
                    ConfigPath = String.Empty
                };

                File.WriteAllText(Environment.CurrentDirectory + @"/AppConfig.json", JsonConvert.SerializeObject(settingsConfig, Formatting.Indented));
            }
            else if (File.Exists(Environment.CurrentDirectory + @"/AppConfig.json"))
            {
                Settings loadConfig = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(Environment.CurrentDirectory + @"\AppConfig.json"));

                try
                {
                    if (loadConfig.ConfigIsAutoLoadCheck)
                    {
                        IsAutoLoadCheck = true;
                        AutoLoadConfigCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));

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
                            AppName = config.ConfigAppName;
                            IsDetails = config.ConfigIsDetails;
                            IsState = config.ConfigIsState;
                            IsTimestamp = config.ConfigIsTimestamp;
                            IsBigImageName = config.ConfigIsBigImageName;
                            IsSmallImageName = config.ConfigIsSmallImageName;
                            IsBigImageText = config.ConfigIsBigImageText;
                            IsSmallImageText = config.ConfigIsSmallImageText;
                            IsParty = config.ConfigIsParty;
                            IsAppCheck = config.ConfigIsAppCheck;

                            if (IsDetails)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.ConsoleTextBox.Text += $"[INFO] Details field ENABLED\n";
                                });
                                DetailsNameTextBox.IsEnabled = true;
                                DetailsButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                            }
                            if (IsState)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.ConsoleTextBox.Text += $"[INFO] State field ENABLED\n";
                                });
                                StateNameTextBox.IsEnabled = true;
                                StateButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                            }
                            if (IsTimestamp)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.ConsoleTextBox.Text += $"[INFO] Timestamp field ENABLED\n";
                                });
                                TimestampStartTextBox.IsEnabled = true;
                                TimestampEndTextBox.IsEnabled = true;
                                TimestampButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                            }
                            if (IsBigImageName)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.ConsoleTextBox.Text += $"[INFO] Large image field ENABLED\n";
                                });
                                BigImageNameTextBox.IsEnabled = true;
                                BigImageTextTextBox.IsEnabled = true;
                                BigImageButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                            }
                            if (IsSmallImageName)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.ConsoleTextBox.Text += $"[INFO] Large image field ENABLED\n";
                                });
                                SmallImageNameTextBox.IsEnabled = true;
                                SmallImageTextTextBox.IsEnabled = true;
                                IsSmallImageName = true;
                                SmallImageButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                            }
                            if (IsParty)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.ConsoleTextBox.Text += $"[INFO] Party size field ENABLED\n";
                                });
                                PartySizeMinTextBox.IsEnabled = true;
                                PartySizeMaxTextBox.IsEnabled = true;
                                PartyButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                            }
                            if (IsAppCheck)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.ConsoleTextBox.Text += $"[INFO] App check for RPC start ENABLED!\n";
                                });

                                EditAppNameTextBox.IsEnabled = true;
                                EnableAppCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));
                            }

                            LoadPathConfigTextBox.Text = loadConfig.ConfigPath;
                            EditPathConfigTextBox.Text = loadConfig.ConfigPath;

                            this.ConsoleTextBox.Text += $"[INFO] Config successfully loaded!\n";
                        });
                    }
                }
                catch
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[ERROR] Error loading Discord RPC config file, it may have been moved!\n";

                        AutoLoadConfigCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_outline_blank_white_36dp.png"));
                        IsAutoLoadCheck = false;
                    });
                }

                if (loadConfig.ConfigIsMinimizeCheck)
                {
                    IsMinimizeCheck = true;
                    MinimizeToTrayCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));
                }

                if (loadConfig.ConfigIsElapsedTimeCheck)
                {
                    IsElapsedTimeCheck = true;
                    DisplayElapsedTimeCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));
                }

                if (loadConfig.ConfigIsCurrentTimeCheck)
                {
                    IsCurrentTimeCheck = true;
                    DisplayCurrentTimeCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));
                }
            }
        }

        private void CloseApp_Click(object sender, MouseButtonEventArgs e)
        {
            if (IsMinimizeCheck)
            {
                this.Hide();
                TaskbarIcon.Visibility = Visibility.Visible;
            }
            else
            {
                Close();
            }
        }

        private void MinimizeApp_Click(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
            TaskbarIcon.Visibility = Visibility.Visible; 
        }

        private void TaskbarIcon_TrayMouseClick(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Visible;
            this.WindowState = WindowState.Normal;
            TaskbarIcon.Visibility = Visibility.Hidden;
        }

        private void CloseApp_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseApp.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_close_red_36dp.png"));
        }

        private void CloseApp_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseApp.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_close_white_36dp.png"));
        }

        private void ChooseSavePathButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Config config = new Config()
            {
                  ConfigAppID = AppID,
                  ConfigAppName = AppName,
                  ConfigDetails = Details,
                  ConfigState = State,
                  ConfigBigImage = BigImage,
                  ConfigSmallImage = SmallImage,
                  ConfigBigImageText = BigImageText,
                  ConfigSmallImageText = SmallImageText,
                  ConfigPartySizeMin = PartySizeMin,
                  ConfigPartySizeMax = PartySizeMax,
                  ConfigIsDetails = IsDetails,
                  ConfigIsState = IsState,
                  ConfigIsTimestamp = IsTimestamp,
                  ConfigIsBigImageName = IsBigImageName,
                  ConfigIsSmallImageName = IsSmallImageName,
                  ConfigIsBigImageText = IsBigImageText,
                  ConfigIsSmallImageText = IsSmallImageText,
                  ConfigIsParty = IsParty,
                  ConfigIsAppCheck = IsAppCheck
            };

            SaveFileDialog saveFileDiaINFO = new SaveFileDialog();
            saveFileDiaINFO.Filter = " Config files (*.json) | *.json";
            saveFileDiaINFO.FileName = "DiscordRPCConfig.json";
            if (saveFileDiaINFO.ShowDialog() == true)
                File.WriteAllText(saveFileDiaINFO.FileName, JsonConvert.SerializeObject(config, Formatting.Indented));

            SavePathConfigTextBox.Text = saveFileDiaINFO.FileName;
            EditPathConfigTextBox.Text = saveFileDiaINFO.FileName;
        }

        private void ChooseLoadPathButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ClearButton_MouseLeftButtonDown(sender, e);

                IsConfigLoaded = true;

                OpenFileDialog openFileDiaINFO = new OpenFileDialog();
                openFileDiaINFO.Filter = " Config files (*.json) | *.json";

                if (openFileDiaINFO.ShowDialog() == true)
                {
                    LoadPathConfigTextBox.Text = openFileDiaINFO.FileName;
                    EditPathConfigTextBox.Text = openFileDiaINFO.FileName;
                }

                Config loadConfig = JsonConvert.DeserializeObject<Config>(File.ReadAllText(openFileDiaINFO.FileName));

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
                    EditAppNameTextBox.Text = loadConfig.ConfigAppName;
                    IsDetails = loadConfig.ConfigIsDetails;
                    IsState = loadConfig.ConfigIsState;
                    IsTimestamp = loadConfig.ConfigIsTimestamp;
                    IsBigImageName = loadConfig.ConfigIsBigImageName;
                    IsSmallImageName = loadConfig.ConfigIsSmallImageName;
                    IsBigImageText = loadConfig.ConfigIsBigImageText;
                    IsSmallImageText = loadConfig.ConfigIsSmallImageText;
                    IsParty = loadConfig.ConfigIsParty;
                    IsAppCheck = loadConfig.ConfigIsAppCheck;

                    this.Dispatcher.Invoke(() =>
                    {
                        if (IsDetails)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[INFO] Details field ENABLED\n";
                            });
                            DetailsNameTextBox.IsEnabled = true;
                            DetailsButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }

                        if (IsState)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[INFO] State field ENABLED\n";
                            });
                            StateNameTextBox.IsEnabled = true;
                            StateButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }
                        else

                        if (IsTimestamp)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[INFO] Timestamp field ENABLED\n";
                            });
                            TimestampStartTextBox.IsEnabled = true;
                            TimestampEndTextBox.IsEnabled = true;
                            TimestampButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }
                        else

                        if (IsBigImageName)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[INFO] Large image field ENABLED\n";
                            });
                            BigImageNameTextBox.IsEnabled = true;
                            BigImageTextTextBox.IsEnabled = true;
                            BigImageButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }
                        else

                        if (IsSmallImageName)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[INFO] Large image field ENABLED\n";
                            });
                            SmallImageNameTextBox.IsEnabled = true;
                            SmallImageTextTextBox.IsEnabled = true;
                            IsSmallImageName = true;
                            SmallImageButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }

                        if (IsParty)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[INFO] Party size field ENABLED\n";
                            });
                            PartySizeMinTextBox.IsEnabled = true;
                            PartySizeMaxTextBox.IsEnabled = true;
                            PartyButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
                        }
                        if (IsAppCheck)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[INFO] App check for RPC start ENABLED!\n";
                            });

                            EditAppNameTextBox.IsEnabled = true;
                            EnableAppCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));
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

        public bool IsAppStarted()
        {
            Process[] localAll = Process.GetProcesses();

            if (localAll.Any(x => x.ProcessName == AppName))
            {
                return true;
            }
            return false;
        }

        private void StartRPCButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Thread thread = new Thread(() =>
            {
                if (IsAppCheck)
                {
                    if (AppName.Length > 1)
                    {
                        while (!IsAppStarted())
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[INFO] Run App to continue!\n";
                            });
                            Thread.Sleep(5000);
                        }

                        if (ClientIDBox.Password.Length > 1)
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

                                Timestamps timeChoose = null;

                                if (IsCurrentTimeCheck)
                                {
                                    timeChoose = new Timestamps()
                                    {
                                        StartUnixMilliseconds = (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1,
                                                                    DateTime.Now.Hour,
                                                                    DateTime.Now.Minute,
                                                                    DateTime.Now.Second))).TotalSeconds,
                                    };
                                }
                                else if (IsElapsedTimeCheck)
                                {
                                    timeChoose = Timestamps.Now;
                                }

                                Client.SetPresence(new RichPresence()
                                {
                                    Details = Details,
                                    State = State,
                                    Timestamps = timeChoose,
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

                                while (true)
                                {
                                    if (IsAppCheck)
                                    {
                                        if (!IsAppStarted())
                                        {
                                            Client.Dispose();
                                            this.Dispatcher.Invoke(() =>
                                            {
                                                this.ConsoleTextBox.Text += $"[INFO] Run App to continue!\n";
                                            });
                                            Thread.Sleep(5000);
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                Client.Dispose();
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.ConsoleTextBox.Text += $"[INFO] Discord RPC connection closed!\n";
                                });
                                IsStarted = false;
                            }
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.ConsoleTextBox.Text += $"[ERROR] Enter Client ID before starting!\n";
                            });
                        }
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.ConsoleTextBox.Text += $"[ERROR] Enter App name before starting!\n";
                        });
                    }
                }
                else
                {
                    if (ClientIDBox.Password.Length > 1)
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

                            Timestamps timeChoose = null;

                            if (IsCurrentTimeCheck)
                            {
                                timeChoose = new Timestamps()
                                {
                                    StartUnixMilliseconds = (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1,
                                                                DateTime.Now.Hour,
                                                                DateTime.Now.Minute,
                                                                DateTime.Now.Second))).TotalSeconds,
                                };
                            }
                            else if (IsElapsedTimeCheck)
                            {
                                timeChoose = Timestamps.Now;
                            }

                            Client.SetPresence(new RichPresence()
                            {
                                Details = Details,
                                State = State,
                                Timestamps = timeChoose,
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
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.ConsoleTextBox.Text += $"[ERROR] Enter Client ID before starting!\n";
                        });
                    }
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
                    this.ConsoleTextBox.Text += $"[INFO] Details field ENABLED\n";
                });
                DetailsNameTextBox.IsEnabled = true;
                IsDetails = true;
                DetailsButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] Details field DISABLED\n";
                });
                DetailsNameTextBox.IsEnabled = false;
                IsDetails = false;
                DetailsButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void StateButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsState == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] State field ENABLED\n";
                });
                StateNameTextBox.IsEnabled = true;
                IsState = true;
                StateButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));

                PartyButton.Opacity = 1;
                PartyButton.IsEnabled = true;
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] State field DISABLED\n";
                });
                StateNameTextBox.IsEnabled = false;
                IsState = false;
                StateButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_off_white_36dp.png"));

                PartyButton.Opacity = 0.5;
                PartyButton.IsEnabled = false;

                if (IsParty)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[INFO] Party size field DISABLED\n";
                    });
                    PartySizeMinTextBox.IsEnabled = false;
                    PartySizeMaxTextBox.IsEnabled = false;
                    IsParty = false;
                    PartyButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
                }
            }
        }

        private void TimestampButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsTimestamp == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] Timestamp field ENABLED\n";
                });
                // TimestampStartTextBox.IsEnabled = true;
                // TimestampEndTextBox.IsEnabled = true;
                IsTimestamp = true;
                TimestampButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] Timestamp field DISABLED\n";
                });
                // TimestampStartTextBox.IsEnabled = false;
                // TimestampEndTextBox.IsEnabled = false;
                IsTimestamp = false;
                TimestampButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void BigImageButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsBigImageName == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] Large image field ENABLED\n";
                });
                BigImageNameTextBox.IsEnabled = true;
                BigImageTextTextBox.IsEnabled = true;
                IsBigImageName = true;
                BigImageButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] Large image field DISABLED\n";
                });
                BigImageNameTextBox.IsEnabled = false;
                BigImageTextTextBox.IsEnabled = false;
                IsBigImageName = false;
                BigImageButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void SmallImageButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsSmallImageName == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] Small image field ENABLED\n";
                });
                SmallImageNameTextBox.IsEnabled = true;
                SmallImageTextTextBox.IsEnabled = true;
                IsSmallImageName = true;
                SmallImageButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] Small image field DISABLED\n";
                });
                SmallImageNameTextBox.IsEnabled = false;
                SmallImageTextTextBox.IsEnabled = false;
                IsSmallImageName = false;
                SmallImageButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void PartyButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsParty == false)
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] Party size field ENABLED\n";
                });
                PartySizeMinTextBox.IsEnabled = true;
                PartySizeMaxTextBox.IsEnabled = true;
                IsParty = true;
                PartyButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_on_white_36dp.png"));
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] Party size field DISABLED\n";
                });
                PartySizeMinTextBox.IsEnabled = false;
                PartySizeMaxTextBox.IsEnabled = false;
                IsParty = false;
                PartyButton.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_toggle_off_white_36dp.png"));
            }
        }

        private void SmallImageTextTexBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            SmallImageText = BigImageTextTextBox.Text;
            HiddenSmallImageText.ToolTip = SmallImageTextTextBox.Text;

            if (SmallImageTextTextBox.Text.Length < 1)
            {
                SmallImageTextTextBox.Text = null;
                SmallImageText = null;
            }
        }

        private void BigImageTextTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            BigImageText = BigImageTextTextBox.Text;
            HiddenBigImageText.ToolTip = BigImageNameTextBox.Text;

            if (BigImageNameTextBox.Text.Length < 1)
            {
                BigImageNameTextBox.Text = null;
                BigImageText = null;
            }
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

        private void EditAppNameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            AppName = EditAppNameTextBox.Text;
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
                    if (Convert.ToInt32(PartySizeMinTextBox.Text) < Convert.ToInt32(PartySizeMaxTextBox.Text))
                    {
                        PartySizeMin = Convert.ToInt32(PartySizeMinTextBox.Text);
                        PartySizePreview.Content = $"Party size ({PartySizeMin} of {PartySizeMax})";
                    }
                    else
                    {
                        PartySizeMinTextBox.Text = null;
                        this.Dispatcher.Invoke(() =>
                        {
                            this.ConsoleTextBox.Text += $"[ERROR] Party size value too big!\n";
                        });
                    }
                }
            }
            catch
            {
                PartySizeMin = 0;
                PartySizeMinTextBox.Text = null;

                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] Party size min value must be less than a max value!\n";
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

/*        private void TimestampStartTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (TimestampStartTextBox.Text.Length < 1)
                {
                    TimestampStart = DateTime.Parse("0");
                }
                else if (Convert.ToDouble(TimestampStartTextBox.Text) > 2147483648)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                   
                    TimestampStart = DateTime.Parse(TimestampStartTextBox.Text);
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
        }*/

/*        private void TimestampEndTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                if (TimestampEndTextBox.Text.Length < 1)
                {
                    TimestampEnd = DateTime.Parse("0");
                }
                else if (Convert.ToDouble(TimestampEndTextBox.Text) > 2147483647)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {

                    TimestampEnd = DateTime.Parse(TimestampEndTextBox.Text);
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
        }*/

        private void ReloadRPC_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Timestamps timeChoose = null;

            if (IsStarted)
            {
                if (IsCurrentTimeCheck)
                {
                    timeChoose = new Timestamps()
                    {
                        StartUnixMilliseconds = (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1,
                                                    DateTime.Now.Hour,
                                                    DateTime.Now.Minute,
                                                    DateTime.Now.Second))).TotalSeconds,
                    };
                }
                else if (IsElapsedTimeCheck)
                {
                    timeChoose = Timestamps.Now;
                }

                if (IsTimestamp)
                {
                    Client.SetPresence(new RichPresence()
                    {
                        Details = Details,
                        State = State,
                        Timestamps = timeChoose,
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
                else
                {
                    Client.SetPresence(new RichPresence()
                    {
                        Details = Details,
                        State = State,
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
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[ERROR] Start App to change settings!\n";
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
            EditAppNameTextBox.Text = String.Empty;

            AppID = String.Empty;
            AppName = String.Empty;
            Details = String.Empty;
            State = String.Empty;
            BigImage = String.Empty;
            SmallImage = String.Empty;
            BigImageText = String.Empty;
            SmallImageText = String.Empty;
            PartySizeMin = 0;
            PartySizeMax = 0;

            IsDetails = false;
            IsState = false;
            IsTimestamp = false;
            IsBigImageName = false;
            IsSmallImageName = false;
            IsBigImageText = false;
            IsSmallImageText = false;
            IsParty = false;
            IsAppCheck = false;

            this.Dispatcher.Invoke(() =>
            {
                this.ConsoleTextBox.Text += $"[INFO] All fields cleared\n";
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
                        this.ConsoleTextBox.Text += $"[INFO] Open config file\n";
                    });
                }
                else if (LoadPathConfigTextBox.Text != String.Empty)
                {
                    Process.Start("C:\\Windows\\System32\\notepad.exe", LoadPathConfigTextBox.Text);
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[INFO] Open config file\n";
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
                MinimizeToTrayCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_outline_blank_white_36dp.png"));
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
                    ConfigIsElapsedTimeCheck = tempConfig.ConfigIsElapsedTimeCheck,
                    ConfigIsCurrentTimeCheck = tempConfig.ConfigIsCurrentTimeCheck,
                    ConfigPath = tempConfig.ConfigPath
                };

                File.WriteAllText(Environment.CurrentDirectory + @"\AppConfig.json", JsonConvert.SerializeObject(settingsConfig, Formatting.Indented));
            }
            else
            {
                IsMinimizeCheck = true;
                MinimizeToTrayCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));
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
                    ConfigIsElapsedTimeCheck = tempConfig.ConfigIsElapsedTimeCheck,
                    ConfigIsCurrentTimeCheck = tempConfig.ConfigIsCurrentTimeCheck,
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
                    AutoLoadConfigCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_outline_blank_white_36dp.png"));
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[RPC Settings] Auto-load config file DISABLED\n";
                    });

                    Settings settingsConfig = new Settings
                    {
                        ConfigIsMinimizeCheck = tempConfig.ConfigIsMinimizeCheck,
                        ConfigIsAutoLoadCheck = false,
                        ConfigEnableAppCheck = tempConfig.ConfigEnableAppCheck,
                        ConfigIsElapsedTimeCheck = tempConfig.ConfigIsElapsedTimeCheck,
                        ConfigIsCurrentTimeCheck = tempConfig.ConfigIsCurrentTimeCheck,
                        ConfigPath = null 
                    }; 

                    File.WriteAllText(Environment.CurrentDirectory + @"\AppConfig.json", JsonConvert.SerializeObject(settingsConfig, Formatting.Indented));
                }
                else
                {
                    IsAutoLoadCheck = true;
                    AutoLoadConfigCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[RPC Settings] Auto-load config file ENABLED\n";
                    });

                    Settings settingsConfig = new Settings
                    {
                        ConfigIsMinimizeCheck = tempConfig.ConfigIsMinimizeCheck,
                        ConfigIsAutoLoadCheck = true,
                        ConfigEnableAppCheck = tempConfig.ConfigEnableAppCheck,
                        ConfigIsElapsedTimeCheck = tempConfig.ConfigIsElapsedTimeCheck,
                        ConfigIsCurrentTimeCheck = tempConfig.ConfigIsCurrentTimeCheck,
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

        private void DisplayElapsedTimeCheck_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsTimestamp && !IsCurrentTimeCheck)
            {
                if (IsElapsedTimeCheck)
                {
                    IsElapsedTimeCheck = false;
                    DisplayElapsedTimeCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_outline_blank_white_36dp.png"));

                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[INFO] Display elapsed time DISABLED!\n";
                    });
                }
                else if (!IsElapsedTimeCheck)
                {
                    IsElapsedTimeCheck = true;
                    DisplayElapsedTimeCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));

                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[INFO] Display elapsed time ENABLED!\n";
                    });
                }
            }
            else
            {
                if (!IsTimestamp)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[ERROR] Enable Timestamp field!\n";
                    });
                }
                else if (IsCurrentTimeCheck)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[ERROR] Disable current time setting!\n";
                    });
                }
            }
        }

        private void DisplayCurrentTimeCheck_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsTimestamp && !IsElapsedTimeCheck)
            {
                if (IsCurrentTimeCheck)
                {
                    IsCurrentTimeCheck = false;
                    DisplayCurrentTimeCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_outline_blank_white_36dp.png"));

                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[INFO] Display current time DISABLED!\n";
                    });
                }
                else if (!IsCurrentTimeCheck)
                {
                    IsCurrentTimeCheck = true;
                    DisplayCurrentTimeCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));

                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[INFO] Display current time ENABLED!\n";
                    });
                }
            }
            else
            {
                if (!IsTimestamp)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[ERROR] Enable Timestamp field!\n";
                    });
                }
                else if (IsElapsedTimeCheck)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.ConsoleTextBox.Text += $"[ERROR] Disable eplapsed time setting!\n";
                    });
                }
            }
        }

        private void EnableAppCheck_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsAppCheck)
            {
                IsAppCheck = false;
                EnableAppCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_outline_blank_white_36dp.png"));
                EditAppNameTextBox.IsEnabled = false;

                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] App check for RPC start DISABLED!\n";
                });
            }
            else
            {
                IsAppCheck = true;
                EnableAppCheck.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/assets/ui_assets/icons/baseline_check_box_white_36dp.png"));
                EditAppNameTextBox.IsEnabled = true;

                this.Dispatcher.Invoke(() =>
                {
                    this.ConsoleTextBox.Text += $"[INFO] App check for RPC start ENABLED!\n";
                });
            }
        }

        private void HelpButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/aniv1re/CustomRPCMaker");
        }
    }
}
