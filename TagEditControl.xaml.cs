using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

namespace MediaPlayer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TagEditControl : UserControl
    {
        public TagLib.File currentFile;
        public event RoutedEventHandler ClickSave;

        public TagEditControl()
        {
            InitializeComponent();
        }

        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save_Tags();

        }

        public void Save_Tags()
        {
            currentFile.Tag.Title = tagTitle.Text;
            currentFile.Tag.Performers[0] = tagArtist.Text;
            currentFile.Tag.Album = tagAlbum.Text;
            currentFile.Tag.Year = Convert.ToUInt16(tagYear.Text);

            try
            {
                currentFile.Dispose();
                currentFile.Save();
                this.Visibility = System.Windows.Visibility.Hidden;
                
            }
            catch
            {
                throw;
            }
        }

        public void initTagEditor()
        {
            //update tag information on opening dialogue
            tagTitle.Text = currentFile.Tag.Title;
            tagArtist.Text = currentFile.Tag.Performers[0];
            tagAlbum.Text = currentFile.Tag.Album;
            tagYear.Text = currentFile.Tag.Year.ToString();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (ClickSave != null)
            {
                ClickSave(this, e);
            }
        }

    }
}
