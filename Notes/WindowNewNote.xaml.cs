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
using System.Windows.Shapes;
using System.IO;

namespace Notes
{
    /// <summary>
    /// Interaction logic for WindowNewNote.xaml
    /// </summary>
    public partial class WindowNewNote : Window
    {
        public WindowNewNote()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (txtNoteName.Text != "" && !File.Exists(txtNoteName.Text) && txtNoteText.Text != "")
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder, $"{txtNoteName.Text}.txt")))
                    {
                        writer.Write(txtNoteText.Text);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Při vytváření poznámky došlo k chybě: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Note note = new Note(txtNoteName.Text);
                AcessStuff.noteName = $"{txtNoteName.Text}.txt";
                AcessStuff.activeNote = $"{txtNoteName.Text}.txt";
                Close();
            }
        }
    }
}
