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
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace M3UPlaylistCreatorWithGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FillCheckBoxOnInit();
        }

        private void OnCreate(object sender, RoutedEventArgs e)
        {
            if (!list_SongList.HasItems)
            {
                MsgUser("The Songlist is Empty", Brushes.Red);
                return;
            }
            if (txtBox_Playlistname.Text.Length <= 0)
            {
                MsgUser("Please give me a Playlist name. :)", Brushes.Red);
                return;
            }

            MsgUser("Playlist is creating. Please wait!", Brushes.Black);

            string filePath = txtBox_FolderPath.Text + "\\" + txtBox_Playlistname.Text + ".m3u";
            // Create new File if Playlist already exist.
            if (File.Exists(filePath))
                File.Create(filePath).Close();

            // Create Header
            if (!(bool)checkBox_SamsungPhone.IsChecked)
                File.AppendAllText(filePath, "#EXTM3U\n");
            // Create M3U Infortmation per Music File
            int countRounds = 1;
            foreach (FileInformation item in list_SongList.ItemsSource)
            {
                if ((bool)checkBox_SamsungPhone.IsChecked)
                    File.AppendAllText(filePath, "/storage/emulated/0/Music/Vocaloid Best Songs for me! Car/" + item.FileName + "\n");
                else
                    File.AppendAllText(filePath, "#EXTINF:" + item.SongLenghtSeconds + "," + item.SongTitle + "\n" + item.FileName + "\n");
                int percent = (int)((float)100 / (float)list_SongList.Items.Count * (float)countRounds);
                countRounds++;
                pb_Progress.Value = percent;
                MsgUser(percent + "% done!", Brushes.Black);
            }
            MsgUser("Playlist created!", Brushes.Black);
        }

        private void OnSelectFolder(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog openFileDlg = new FolderBrowserDialog();
            DialogResult result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
               txtBox_FolderPath.Text = openFileDlg.SelectedPath;
               if (Directory.Exists(openFileDlg.SelectedPath))
                    FillSongList(openFileDlg.SelectedPath, cbox_types.Text);
            }
        }

        private void OnSelectedSongList(object sender, RoutedEventArgs e)
        {
            list_SongList.SelectedItem = null;
        }

        private void FillSongList(string folder, string type)
        {
            FileInfo[] fileInfo = new DirectoryInfo(folder).GetFiles("*" + type);

            List<FileInformation> fileInfoList = new List<FileInformation>();
            foreach (var item in fileInfo)
            {
                TagLib.File file = TagLib.File.Create(item.FullName);
                fileInfoList.Add(new FileInformation(item.Name, file.Tag.Title, file.Properties.Duration.TotalSeconds.ToString().Split(',')[0]));
                Log(item.Name);
            }
            list_SongList.ItemsSource = fileInfoList;
            MsgUser(fileInfo.Length + " Songs found.", Brushes.Black);
        }

        private void FillCheckBoxOnInit()
        {
            List<string> checkBox = new List<string>();
            checkBox.Add(".mp3");
            cbox_types.ItemsSource = checkBox;
            cbox_types.SelectedIndex = 0;

            if (Directory.Exists(txtBox_FolderPath.Text))
                FillSongList(txtBox_FolderPath.Text, cbox_types.Text);
        }

        private static void Log(string msg)
        {
            File.AppendAllText("log.txt", DateTime.Now.ToString() + " [Debug] " + msg + "\n");
        }

        private void MsgUser(string msg, SolidColorBrush color)
        {
            lbl_InformUser.Content = msg;
            lbl_InformUser.Foreground = color;
        }
    }
}
