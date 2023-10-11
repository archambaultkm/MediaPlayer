using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using TagLib;

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TagLib.File currentFile;
        private string currentFilePath;
        private bool userIsDragging;
        private bool isPlaying;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "MP3 files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg";


            //TODO wrap with try/catch
            if (fileDialog.ShowDialog() == true)
            {
                //set the media element source to the selected song
                mediaPlayer.Source = new Uri(fileDialog.FileName);

                //create a taglib file object for accessing mp3 metadata
                currentFile = TagLib.File.Create(fileDialog.FileName);

                //update the media information display
                initMediaDisplay(currentFile);

                //store the full path of the selected media for saving later
                currentFilePath = fileDialog.FileName;
            }
        }

        private void EditProperties_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void EditProperties_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (mediaPlayer != null) && (mediaPlayer.Source != null);
        }
        private void Play_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayer.Play();
            isPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //so long as media is already playing, pausing is valid
            e.CanExecute = isPlaying; 
        }
        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayer.Pause();
            isPlaying = false;
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //so long as media is already playing, stopping is valid
            e.CanExecute = isPlaying; 
        }
        private void Stop_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayer.Stop();
            isPlaying = false;
        }

        private void Slider_DragStarted(object sender, DragStartedEventArgs e)
        {
            userIsDragging = true;
        }

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            userIsDragging = false;
            mediaPlayer.Position = TimeSpan.FromSeconds(slider.Value);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //update the progress label to reflect where the user has dragged the progress bar to
            lblProgress.Text = TimeSpan.FromSeconds(slider.Value).ToString(@"hh\:mm\:ss"); 
        }

        private void initMediaDisplay(TagLib.File file)
        {
            //update the displayed media information

            mediaName.Text = file.Tag.Title;
            mediaArtist.Text = file.Tag.FirstAlbumArtist;
            mediaAlbum.Text = file.Tag.Album;

            //set image art if the mp3 file has album art attached
            if (file.Tag.Pictures.Length > 0)
            {
                var image = file.Tag.Pictures[0];
                albumArt.Source = getAlbumArtAsBitmap(image);
            }

            
        }

        private BitmapImage getAlbumArtAsBitmap (IPicture image)
        {
            //code for converting to ImageSource found here, a comination of both answers:
            //https://stackoverflow.com/questions/35786467/wpf-c-sharp-image-source 

            var imageBytes = image.Data.Data;

            Uri BitmapUri;
            StreamResourceInfo BitmapStreamSourceInfo;
            try
            {
                BitmapImage bi = new BitmapImage();

                bi.BeginInit();
                bi.StreamSource = new MemoryStream(imageBytes);
                bi.EndInit();

                return bi;
            }
            catch { throw; }
        }
    }
}
