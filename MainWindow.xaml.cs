using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
using CasparObjects;
using System.IO;
using Microsoft.Win32;
using System.Xml;
using System.Timers;




namespace Playout_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml 
    /// </summary>

    public partial class MainWindow : Window
    {
        //from now on, our caspar server is called _Caspar
        private CasparCG _Caspar = new CasparCG();

        public List<String> MediaList;
        public List<String> TemplateList;

        //This declares the class for a rundown item
        public class DataItem
        {
            public DateTime StartTime { get; set; }
            public String Name { get; set; }
            public int FrameIn { get; set; }
            public int Framerate { get; set; }
            public String EndAction { get; set; }
            public int Duration { get; set; }
            public String CG { get; set; }
            public int CGlayer { get; set; }
            public int CGdelay { get; set; }
            public String CGfield0 { get; set; }
            public String CGfield1 { get; set; }
            public String Command { get; set; }
        }

        //This code executes once the main window has loaded
        public MainWindow()
        {
            //This sets up the main Data Grid List
            InitializeComponent();
            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            DataGridTextColumn col3 = new DataGridTextColumn();
            DataGridTextColumn col4 = new DataGridTextColumn();
            DataGridTextColumn col5 = new DataGridTextColumn();
            DataGridTextColumn col6 = new DataGridTextColumn();
            DataGridTextColumn col7 = new DataGridTextColumn();
            DataGridTextColumn col8 = new DataGridTextColumn();
            DataGridTextColumn col9 = new DataGridTextColumn();
            DataGridTextColumn col10 = new DataGridTextColumn();
            DataGridTextColumn col11 = new DataGridTextColumn();
            DataGridTextColumn col12 = new DataGridTextColumn();
            MainGrid.Columns.Add(col1);
            MainGrid.Columns.Add(col2);
            MainGrid.Columns.Add(col3);
            MainGrid.Columns.Add(col4);
            MainGrid.Columns.Add(col5);
            MainGrid.Columns.Add(col6);
            MainGrid.Columns.Add(col7);
            MainGrid.Columns.Add(col8);
            MainGrid.Columns.Add(col9);
            MainGrid.Columns.Add(col10);
            MainGrid.Columns.Add(col11);
            MainGrid.Columns.Add(col12);
            col1.Binding = new Binding("StartTime");
            col2.Binding = new Binding("Name");
            col3.Binding = new Binding("FrameIn");
            col4.Binding = new Binding("Framerate");
            col5.Binding = new Binding("EndAction");
            col6.Binding = new Binding("Duration");
            col7.Binding = new Binding("CG");
            col8.Binding = new Binding("CGlayer");
            col9.Binding = new Binding("CGdelay");
            col10.Binding = new Binding("CGfield0");
            col11.Binding = new Binding("CGfield1");
            col12.Binding = new Binding("Command");
            col1.Header = "Start Time";
            col2.Header = "Clip Name";
            col3.Header = "In Frame";
            col4.Header = "Frame Rate";
            col5.Header = "End Action";
            col6.Header = "Total Frames";
            col7.Header = "CG";
            col8.Header = "CG Layer";
            col9.Header = "CG Delay";
            col10.Header = "CG Field 0";
            col11.Header = "CG Field 1";
            col12.Header = "Extra AMCP Command";

            //start the clock
            SetTimer();


        }

        public void AddItem(DateTime startTime, string Name, int frameIn, int framerate, string endaction, int duration, string CG, int cgLayer, int cgDelay, string CGf0, string CGf1, string Command)
        {
            //This is where we'll calculate the durations and add the item to our list
            MainGrid.Items.Add(new DataItem { StartTime = startTime, Name = Name, FrameIn = frameIn, Framerate = framerate, EndAction = endaction, Duration = duration, CG = CG, CGlayer = cgLayer, CGdelay = cgDelay, CGfield0 = CGf0, CGfield1 = CGf1, Command = Command });
        }

        public void GetChannels()
        {
            ReturnInfo inforeturn = _Caspar.Execute("INFO");
            string infomessage = inforeturn.Data;
            string[] channels = infomessage.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            playoutChannel.Items.Clear();
            cgChannel.Items.Clear();

            foreach (string i in channels)
            {
                try
                {
                    string[] channeldata = i.Split(' ');
                    string channelname = channeldata[0] + " " + channeldata[1];
                    playoutChannel.Items.Add(channelname);
                    cgChannel.Items.Add(channelname);
                    previewChannel.Items.Add(channelname);
                }
                catch { }
            }

            if (playoutChannel.Items != null)
            {
                playoutChannel.SelectedIndex = 0;
            }

            if (cgChannel.Items != null)
            {
                cgChannel.SelectedIndex = 0;
            }

            if (previewChannel.Items != null)
            {
                previewChannel.SelectedIndex = 0;
            }

        }

        //this method gets use the channel number for the specified channel type
        public int GetChannel(string channelType)
        {
            string currentChannelInfo;

            if (channelType == "playout")
            {
                currentChannelInfo = playoutChannel.SelectedItem.ToString();
            }
            if (channelType == "cg")
            {
                currentChannelInfo = cgChannel.SelectedItem.ToString();
            }
            if (channelType == "preview")
            {
                currentChannelInfo = previewChannel.SelectedItem.ToString();
            }
            else
            {
                currentChannelInfo = "1 1080p5000";
            }

            string[] currentChannel = currentChannelInfo.Split(' ');

            return Convert.ToInt32(currentChannel[0]);
        }

        public int GetPlayoutFramerate()
        {
            string currentChannelInfo = playoutChannel.SelectedItem.ToString();
            string[] currentChannel = currentChannelInfo.Split(' ');
            string videoMode = currentChannel[1];

            if (videoMode == "PAL") { return 25; }
            else if (videoMode == "NTSC") { return 30; }
            else
            {
                string[] videomodeparams = videoMode.Split('p');
                string videomodeframerate = videomodeparams[1];
                string framerate = videomodeframerate.Substring(0, 2);
                return Convert.ToInt32(framerate);
            }
        }



        private void ToggleFullscreen(object sender, RoutedEventArgs e)
        {
            if (this.WindowStyle != WindowStyle.SingleBorderWindow)
            {
                this.ResizeMode = ResizeMode.CanResize;
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                this.Topmost = false;
                Logo.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                this.ResizeMode = ResizeMode.NoResize;
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                this.Topmost = true;
                Logo.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public void Log(string message)
        {
            statusText.Content = message;
        }

        public void GetMediaList()
        {
            MediaList = _Caspar.GetMediaClipsNames();
            add_mediaSelector.Items.Clear();
            foreach (string i in MediaList)
            {
                add_mediaSelector.Items.Add(i);
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            _Caspar.Connect(ipbox.Text);

            //This sets up some colours
            Color sucessCol = (Color)ColorConverter.ConvertFromString("#FF8BC34A");
            Color errorCol = (Color)ColorConverter.ConvertFromString("#F44336");
            SolidColorBrush sucessBrush = new SolidColorBrush(sucessCol);
            SolidColorBrush errorBrush = new SolidColorBrush(errorCol);

            //Connect to the caspar server and log the result - change the UI
            if (_Caspar.Connected == true)
            {
                Connect.Foreground = sucessBrush;
                toolbar_addControls.Visibility = Visibility.Visible;
                toolbar_transportcontrols.Visibility = Visibility.Visible;
                toolbar_channels.Visibility = Visibility.Visible;

                try { GetChannels(); } catch (Exception errChannels) { Log("Couldn't get channel list because " + errChannels); }

                Log("Connected to CasparCG Server " + ipbox.Text);
            }
            else
            {
                Connect.Foreground = errorBrush;
                Log("Error connecting to server");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            GetMediaList();
        }

        private void RefreshMediaList(object sender, RoutedEventArgs e)
        {

            GetMediaList();
        }

        public void GetMediaProperties()
        {
            System.Threading.Thread.Sleep(250);

            //let's get the name of the selected clip
            string currentclip = add_mediaSelector.Text;

            //now lets send a CINF (clip info) command to caspar, and store the return data
            ReturnInfo clipInfoString = _Caspar.Execute("CINF \"" + currentclip + "\"");

            //if the clip info string isn't empty, lets parse it and figure out the duration in seconds.
            //Otherwise throw an error

            if (clipInfoString.Data != null)
            {

                if (add_EnablePreview.IsChecked == true)
                {
                    _Caspar.Execute("LOAD 1-10 \"" + add_mediaSelector.SelectedItem + "\"");
                }

                //this removes the file name at the beginning as it may contains spaces and mess things up
                int start = clipInfoString.Data.IndexOf("\"");
                int end = clipInfoString.Data.LastIndexOf("\"");
                clipInfoString.Data = clipInfoString.Data.Remove(start, end - start);

                //now lets remove any other unessesary characters at the end of the data string
                string input = clipInfoString.Data;
                int index = input.IndexOf("\r\n");
                clipInfoString.Data = input.Substring(0, index);

                try
                {
                    //here we can finally take the frame number and divide it by the frame rate to get the seconds
                    string[] clipData = clipInfoString.Data.Split(' ');
                    string[] frameRate = clipData[7].Split('/');
                    float clipDurationSeconds = float.Parse(clipData[6]) / Int32.Parse(frameRate[1]);
                    int durationInFrames = int.Parse(clipData[6]);


                    //Now let's convert that to something that looks nice
                    TimeSpan duration = TimeSpan.FromSeconds(clipDurationSeconds);
                    TimeSpan defaultIn = TimeSpan.FromSeconds(0);

                    //and let's populate our fields
                    add_Slider_In.Maximum = durationInFrames;
                    add_Slider_In.Value = 0;
                    add_Slider_Out.Maximum = durationInFrames;
                    add_Slider_Out.Value = Convert.ToDouble(durationInFrames);


                    add_f_in.Content = "0";
                    add_f_dur.Content = durationInFrames.ToString();
                    add_f_out.Content = durationInFrames.ToString();
                    add_f_framerate.Content = frameRate[1].ToString();


                    add_In.Text = defaultIn.ToString(@"hh\:mm\:ss");
                    add_Duration.Text = duration.ToString(@"hh\:mm\:ss");
                    add_Out.Text = duration.ToString(@"hh\:mm\:ss");

                    Log("Retrieved media duration for " + currentclip);
                }
                catch (Exception err)
                {
                    Log("Couldn't calculate duration because " + err.Message);
                }

                //also let's set the starting date and time to the correct value

                add_StartDate.SelectedDate = DateTime.Now;
                add_StartTime.SelectedTime = DateTime.Now;

            }
            else
            {
                Log("Failed to get media duration information.");
            }



        }

        private void InSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Int32 framerate = Convert.ToInt32(add_f_framerate.Content.ToString());

                //Lets figure out the new duration
                Int32 newDurationFrames = Convert.ToInt32(add_Slider_Out.Value) - Convert.ToInt32(add_Slider_In.Value);
                double newDurationSecs = newDurationFrames / framerate;

                //Lets save all the new frame values (yes i'm using labels to store it, it's a bit botched but it works)
                add_f_dur.Content = newDurationFrames.ToString();
                add_f_in.Content = add_Slider_In.Value.ToString();

                //And lets do that for the seconds too so it looks nice to the user
                TimeSpan currentIn = TimeSpan.FromSeconds(add_Slider_In.Value / framerate);
                add_In.Text = currentIn.ToString(@"hh\:mm\:ss");
                TimeSpan newDuration = TimeSpan.FromSeconds(newDurationSecs);
                add_Duration.Text = newDuration.ToString(@"hh\:mm\:ss");

                if (newDurationFrames < 0)
                {
                    add_Slider_In.Value = add_Slider_Out.Value;
                }



                //This calculates if the clip framerate is different to our caspar channel framerate.
                double playoutFramerate = GetPlayoutFramerate();
                double frameRatio = 1 / (Convert.ToDouble(add_f_framerate.Content) / playoutFramerate);

                //If we have preview enabled, we send the preview frame to Caspar
                if (add_EnablePreview.IsChecked == true)
                {
                    _Caspar.Execute("CALL 1-10  SEEK " + Convert.ToInt32(add_f_in.Content) * frameRatio);
                }
            }
            catch (Exception inExep)
            {
                Log("Couldn't update the in slider because " + inExep);
            }
        }

        private void OutValueSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                Int32 framerate = Convert.ToInt32(add_f_framerate.Content.ToString());


                //Lets figure out the new duration
                Int32 newDurationFrames = Convert.ToInt32(add_Slider_Out.Value) - Convert.ToInt32(add_Slider_In.Value);
                double newDurationSecs = newDurationFrames / framerate;

                //Lets save all the new frame values (yes i'm using labels to store it, it's a bit botched but it works)
                add_f_dur.Content = newDurationFrames.ToString();
                add_f_out.Content = add_Slider_Out.Value.ToString();

                //And lets do that for the seconds too so it looks nice to the user
                TimeSpan currentOut = TimeSpan.FromSeconds(add_Slider_Out.Value / framerate);
                add_Out.Text = currentOut.ToString(@"hh\:mm\:ss");
                TimeSpan newDuration = TimeSpan.FromSeconds(newDurationSecs);
                add_Duration.Text = newDuration.ToString(@"hh\:mm\:ss");
                if (newDurationFrames < 0)
                {
                    add_Slider_Out.Value = add_Slider_In.Value;
                }

                //This calculates if the clip framerate is different to our caspar channel framerate.
                double playoutFramerate = GetPlayoutFramerate();
                double frameRatio = 1 / (Convert.ToDouble(add_f_framerate.Content) / playoutFramerate);


                //If we have preview enabled, we send the preview frame to Caspar
                if (add_EnablePreview.IsChecked == true)
                {
                    _Caspar.Execute("CALL 1-10 SEEK " + Convert.ToInt32(add_f_out.Content) * frameRatio);
                }
            }
            catch (Exception inExep)
            {
                Log("Couldn't update the out slider because " + inExep);
            }
        }

        private void RefreshTemplatesList(object sender, RoutedEventArgs e)
        {
            TemplateList = _Caspar.GetTemplateNames();
            add_TemplateList.Items.Clear();
            foreach (string i in TemplateList)
            {
                add_TemplateList.Items.Add(i);
            }
        }

        private void SendPlayCommand(object sender, RoutedEventArgs e)
        {
            _Caspar.Execute("PLAY 1-10");
        }

        private void SendPauseCommand(object sender, RoutedEventArgs e)
        {
            _Caspar.Execute("PAUSE 1-10");
        }

        private void MediaListChanged(object sender, EventArgs e)
        {

            GetMediaProperties();
        }

        private void previewPlayCG(object sender, RoutedEventArgs e)
        {
            string templateName = add_TemplateList.SelectedItem.ToString();
            string f0 = add_CGf0.Text;
            string f1 = add_CGf1.Text;
            int cgLayer = Convert.ToInt32(add_CGlayer.Text);
            CasparCG.Retard cgDelay = new CasparCG.Retard(Convert.ToInt32(add_CGdelayInSeconds.Text));
            int cgChannel = GetChannel("cg");

            Template previewTemplate = new CasparObjects.Template();
            previewTemplate.AddField("f0", f0);
            previewTemplate.AddField("f1", f1);
            previewTemplate.UseJSON = true;

            _Caspar.CG_Add(cgChannel, cgLayer, templateName, previewTemplate, true, cgDelay);

        }

        private void previewUpdateCG(object sender, RoutedEventArgs e)
        {
            string f0 = add_CGf0.Text;
            string f1 = add_CGf1.Text;
            int cgLayer = Convert.ToInt32(add_CGlayer.Text);
            int cgChannel = GetChannel("cg");
            Template previewTemplate = new CasparObjects.Template();
            previewTemplate.AddField("f0", f0);
            previewTemplate.AddField("f1", f1);
            previewTemplate.UseJSON = true;
            _Caspar.CG_Update(cgChannel, cgLayer, previewTemplate);
        }

        private void previewStopCG(object sender, RoutedEventArgs e)
        {
            int cgLayer = Convert.ToInt32(add_CGlayer.Text);
            int cgChannel = GetChannel("cg");
            _Caspar.CG_Stop(cgChannel, cgLayer);
        }

        //this adds a rundown item from the add popup info
        private void AddItemToRundown(object sender, RoutedEventArgs e)
        {

            //get the date and time (still needs fixing to be able to auto calculate offset from previous clip)
            DateTime StartTime = add_StartTime.SelectedTime.Value;

            //declare all the clip variables and set them to something blank
            string ClipName = "";
            int FrameIn = 0;
            int FrameDur = 0;
            int Framerate = GetPlayoutFramerate();
            string EndAction = "hold";

            //if there's a clip, get all it's properties
            if (add_mediaSelector.SelectedItem != null)
            {
                ClipName = add_mediaSelector.SelectedItem.ToString();
                FrameIn = Convert.ToInt32(add_f_in.Content);
                FrameDur = Convert.ToInt32(add_f_dur.Content);
                Framerate = Convert.ToInt32(add_f_framerate.Content);
            }

            //figure out the end action and set the according variable
            if (add_Hold.IsChecked == true) { EndAction = "hold"; }
            else if (add_Black.IsChecked == true) { EndAction = "black"; }
            else if (add_Loop.IsChecked == true) { EndAction = "loop"; }
            else { }

            //declare cg variables and set default values
            string cgName = "0";
            string cgf0 = "";
            string cgf1 = "";
            int cgLayer = 20;
            int cgDelay = 0;

            //if a template is selected, get the template data
            if (add_TemplateList.SelectedItem != null)
            {
                cgName = add_TemplateList.SelectedItem.ToString();
                cgf0 = add_CGf0.Text;
                cgf1 = add_CGf1.Text;
                cgLayer = Convert.ToInt32(add_CGlayer.Text);
                cgDelay = Convert.ToInt32(add_CGdelayInSeconds.Text);
            }

            //copy the AMCP commands from the text box in to a variable
            string command = add_commands.Text;


            //get everything we collected and write it to the Datagrid
            AddItem(StartTime, ClipName, FrameIn, Framerate, EndAction, FrameDur, cgName, cgLayer, cgDelay, cgf0, cgf1, command);
        }

        private void SavePlaylist_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Playout Manager Rundown File (*.pmr)|*.pmr";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, DataGridToString());
            }
        }

        public string DataGridToString()
        {
            string stringBuilder = "";

            foreach (var item in MainGrid.Items.OfType<DataItem>())
            {
                string itemEncoded = item.StartTime + "," + item.Name + "," + item.FrameIn + "," + item.Framerate + "," + item.EndAction + "," + item.Duration + "," + item.CG + "," + item.CGlayer + "," + item.CGdelay + "," + item.CGfield0 + "," + item.CGfield1 + "," + item.Command;
                stringBuilder = stringBuilder + itemEncoded + "¬";
            }

            return stringBuilder;
        }


        public void StringToDataGrid(string encodedString)
        {
            MainGrid.Items.Clear();
            String[] line = encodedString.Split('¬');
            foreach (string item in line)
            {
                if (item != "")
                {
                    string[] property = item.Split(',');
                    AddItem(DateTime.Parse(property[0]), property[1], Convert.ToInt32(property[2]), Convert.ToInt32(property[3]), property[4], Convert.ToInt32(property[5]), property[6], Convert.ToInt32(property[7]), Convert.ToInt32(property[8]), property[9], property[10], property[11]);
                }
            }
        }


        private void LoadPlaylist_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Playout Manager Rundown File (*.pmr)|*.pmr";
            if (openFileDialog.ShowDialog() == true)
            {
                string loadString = File.ReadAllText(openFileDialog.FileName);
                StringToDataGrid(loadString);
            }




        }

        public void PlayItem(string Name, int frameIn, int framerate, string endaction, int duration, string CG, int cgLayer, int cgDelay, string CGf0, string CGf1, string Command)
        {
            string loopCommand = "";
            if (endaction == "loop") { loopCommand = " LOOP"; }

            //check if there's media
            if (Name != "")
            {

                //This calculates if the clip framerate is different to our caspar channel framerate.
                double playoutFramerate = GetPlayoutFramerate();
                double frameRatio = 1 / (Convert.ToDouble(framerate) / playoutFramerate);

                try
                {
                    //Sends all our commands to CasparCG to play the media
                    _Caspar.Execute("STOP 1-10");
                    _Caspar.Execute("LOAD 1-10 \"" + Name + "\"" + loopCommand + " SEEK " + Convert.ToInt32((frameIn * frameRatio)) + " LENGTH " + Convert.ToInt32((duration * frameRatio)));
                    _Caspar.Execute("PLAY 1-10");
                }
                catch (Exception playErr)
                { Log(playErr.Message); }

            }

            //check if there's a CG
            if (CG != "")
            {
                //Play the CG
                CasparCG.Retard cgDelayRetard = new CasparCG.Retard(Convert.ToInt32(cgDelay));
                int cgChannel = GetChannel("cg");

                Template previewTemplate = new CasparObjects.Template();
                previewTemplate.AddField("f0", CGf0);
                previewTemplate.AddField("f1", CGf1);
                previewTemplate.UseJSON = true;

                try
                {
                    _Caspar.CG_Add(cgChannel, cgLayer, CG, previewTemplate, true, cgDelayRetard);
                }
                catch (Exception cgErr)
                { Log(cgErr.Message); }

            }

            //lastly execute the Custom AMCP command
            if (Command != null)
            {
                if (Command == "STOP")
                {
                    _Caspar.CG_Stop();
                }
                else
                {
                    _Caspar.Execute(Command);
                }
            }
        }

        private void PlaySelected_Click(object sender, RoutedEventArgs e)
        {
            DataItem playItem = MainGrid.SelectedItem as DataItem;
            label_current.Content = "NOW PLAYING: " + playItem.Name;
            PlayItem(playItem.Name, playItem.FrameIn, playItem.Framerate, playItem.EndAction, playItem.Duration, playItem.CG, playItem.CGlayer, playItem.CGdelay, playItem.CGfield0, playItem.CGfield1, playItem.Command);
        }

        private void CommandCGStop_Click(object sender, RoutedEventArgs e)
        {
            add_commands.Text = "CG 1-20 STOP";
        }

        private void CommandCGClear_Click(object sender, RoutedEventArgs e)
        {
            add_commands.Text = "CG 1-20 CLEAR";
        }

        private void CommandCGUpdate_Click(object sender, RoutedEventArgs e)
        {
            add_commands.Text = "CG 1-20 UPDATE 1 ";
        }

        private void CommandMixerVolume_Click(object sender, RoutedEventArgs e)
        {
            add_commands.Text = "CG 1-20 UPDATE 1 ";
        }

        private static Timer aTimer;

        private void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        //takes a DataItem and returns the duration as a timespan
        public TimeSpan GetItemDuration(DataItem selectedItem)
        {
            int DurSeconds = selectedItem.Duration / selectedItem.Framerate;
            return TimeSpan.FromSeconds(DurSeconds);
        }

        //this method happens every second
        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            if (_Caspar.Connected)
            {

                this.Dispatcher.Invoke(() =>
                {
                //Check DataGrid for items
                //If item time matches event time, trigger it

                if (MainGrid.Items != null)
                    {
                        Log("Datagrid not empty, scanning items to find matching time at " + e.SignalTime);
                        foreach (var playItem in MainGrid.Items.OfType<DataItem>())
                        {
                            string start = playItem.StartTime.ToString(@"ddMMyyhhmmss");
                            string now = e.SignalTime.ToString(@"ddMMyyhhmmss");

                            if (start == now)
                            {
                                label_current.Content = "NOW PLAYING: " + playItem.Name;
                                PlayItem(playItem.Name, playItem.FrameIn, playItem.Framerate, playItem.EndAction, playItem.Duration, playItem.CG, playItem.CGlayer, playItem.CGdelay, playItem.CGfield0, playItem.CGfield1, playItem.Command);
                                Log(playItem.Name + " has started playing at " + e.SignalTime);
                            }
                        }
                    }
                    else { Log("Datagrid Empty, not scanning items at " + e.SignalTime); }

                //Get the current status of the caspar engine

                try
                    {
                        ReturnInfo info = _Caspar.Execute("INFO 1-10");
                        XmlDocument infoxml = new XmlDocument();
                        string xmlString = info.Data.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                        infoxml.RemoveAll();
                        infoxml.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><xmlroot>" + xmlString + "</xmlroot>");

                        XmlNodeList pathList = infoxml.GetElementsByTagName("path");
                        XmlNodeList timeList = infoxml.GetElementsByTagName("time");

                        string currentMediaTime = "";
                        string currentMediaPath = "";

                        for (int i = 0; i < pathList.Count; i++)
                        {
                            currentMediaPath = pathList[i].InnerXml;
                        }

                        for (int i = 0; i < timeList.Count; i++)
                        {
                            currentMediaTime = timeList[i].InnerXml;
                        }

                    //Update now playing and next time displays
                    label_current.Content = "NOW PLAYING: " + currentMediaPath;
                        timecode_current.Content = currentMediaTime;

                    }
                    catch (Exception xmlerr)
                    {
                        Log("Couldn't get server data because " + xmlerr.Message);
                    }




                });

            }

        }
    }
}
