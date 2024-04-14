using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

namespace Notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnNewFolder_Click(object sender, RoutedEventArgs e)
        {
            WindowNewFolder wndNewFolder = new WindowNewFolder();
            wndNewFolder.ShowDialog();

            Folder.NewFolder(wrapPanelFolders, AcessStuff.folderName);
        }

        private void btnNewNote_Click(object sender, RoutedEventArgs e)
        {
            WindowNewNote wndNewNote = new WindowNewNote();
            wndNewNote.ShowDialog();

            Note.NewNote(wrapPanelNotes, AcessStuff.noteName);
            wrapPanelNotes.Children.Clear();
            Note.LoadNotes(wrapPanelNotes, noteText, noteTitle);

            foreach (var child in wrapPanelNotes.Children)
            {
                if (child is Grid)
                {
                    Grid grid = child as Grid;
                    Label label = grid.Children.OfType<Label>().FirstOrDefault();
                    if (label != null && label.Content.ToString() == AcessStuff.activeNote.Substring(0, AcessStuff.activeNote.Length - 4))
                    {
                        grid.Background = Brushes.LightGray;
                        break;
                    }
                }
            }
        }

        private void binFolder_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.GetDirectories(System.IO.Path.Combine(Environment.CurrentDirectory, "Note")).Count() > 0)
            {
                Directory.Delete(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder), true);

                wrapPanelFolders.Children.Clear();

                wrapPanelNotes.Children.Clear();

                string[] folders = Directory.GetDirectories(System.IO.Path.Combine(Environment.CurrentDirectory, "Note"));
                AcessStuff.activeFolder = System.IO.Path.GetFileName(folders[0]);
                string[] notes = Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder));
                if (notes.Length > 0)
                {
                    AcessStuff.activeNote = System.IO.Path.GetFileName(notes[0]);
                }

                Folder.LoadFolders(wrapPanelFolders);

                Note.LoadNotes(wrapPanelNotes, noteText, noteTitle);



                if (folders.Length > 0)
                {
                    if (wrapPanelFolders.Children.Count > 0 && wrapPanelFolders.Children[0] is Grid)
                    {
                        Grid firstGrid = wrapPanelFolders.Children[0] as Grid;
                        firstGrid.Background = Brushes.LightGray;
                    }
                }
            }
        }

        private void binNote_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder)).Count() > 0)
            {
                File.Delete(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder, AcessStuff.activeNote));

                wrapPanelNotes.Children.Clear();

                string[] notes = Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder));
                if (notes.Length > 0)
                {
                    AcessStuff.activeNote = System.IO.Path.GetFileName(notes[0]);
                }

                Note.LoadNotes(wrapPanelNotes, noteText, noteTitle);

                foreach (var child in wrapPanelNotes.Children)
                {
                    if (child is Grid)
                    {
                        Grid grid = child as Grid;
                        Label label = grid.Children.OfType<Label>().FirstOrDefault();
                        if (label != null && label.Content.ToString() == AcessStuff.activeNote.Substring(0, AcessStuff.activeNote.Length - 4))
                        {
                            grid.Background = Brushes.LightGray;
                            break;
                        }
                    }
                }
            }
        }

        private void editNote_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder)).Count() > 0)
            {
                WindowEditNote wndEditNote = new WindowEditNote();
                wndEditNote.txtNoteName.Text = AcessStuff.activeNote.Substring(0, AcessStuff.activeNote.Length - 4);

                using (StreamReader sr = new StreamReader(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder, AcessStuff.activeNote)))
                {
                    wndEditNote.txtNoteText.Text = sr.ReadToEnd();
                }

                File.Delete(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder, AcessStuff.activeNote));

                wndEditNote.ShowDialog();

                Note.NewNote(wrapPanelNotes, AcessStuff.noteName);
                wrapPanelNotes.Children.Clear();
                Note.LoadNotes(wrapPanelNotes, noteText, noteTitle);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] folders = Directory.GetDirectories(System.IO.Path.Combine(Environment.CurrentDirectory, "Note"));

            foreach (string folder in folders)
            {
                Folder.NewFolder(wrapPanelFolders, System.IO.Path.GetFileName(folder));
                Folder fol = new Folder(System.IO.Path.GetFileName(folder));
            }

            if (folders.Length > 0)
            {
                AcessStuff.activeFolder = System.IO.Path.GetFileName(folders[0]);
                if (wrapPanelFolders.Children.Count > 0 && wrapPanelFolders.Children[0] is Grid)
                {
                    Grid firstGrid = wrapPanelFolders.Children[0] as Grid;
                    firstGrid.Background = Brushes.LightGray;
                }
            }

            foreach (UIElement element in wrapPanelFolders.Children)
            {
                element.PreviewMouseDown += Folder.WrapPanelFolderItemClicked;
            }

            string[] notes = Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder));

            foreach (string note in notes)
            {
                Note.NewNote(wrapPanelNotes, System.IO.Path.GetFileName(note));
                Note not = new Note(System.IO.Path.GetFileName(note));
            }

            if (notes.Length > 0)
            {
                AcessStuff.activeNote = System.IO.Path.GetFileName(notes[0]);
                if (wrapPanelNotes.Children.Count > 0 && wrapPanelNotes.Children[0] is Grid)
                {
                    Grid firstGrid = wrapPanelNotes.Children[0] as Grid;
                    firstGrid.Background = Brushes.LightGray;
                }
            }

            foreach (UIElement element in wrapPanelNotes.Children)
            {
                element.PreviewMouseDown += Note.WrapPanelNoteItemClicked;
            }

            Note.LoadText(noteText, noteTitle);
        }
    }
}
