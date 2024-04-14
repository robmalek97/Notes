using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Notes
{
    public class Note
    {
        public string Name { get; set; }

        public Note(string name)
        {
            Name = name;
        }

        public static void NewNote(WrapPanel wp, string name)
        {
            Grid grid = new Grid();
            grid.Width = 130;
            grid.Height = 40;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;

            Label label = new Label();
            label.Content = name.Substring(0, name.Length - 4);
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Width = 110;
            label.FontFamily = new FontFamily("Segoe UI Variable Display Semibold");

            grid.PreviewMouseDown += WrapPanelNoteItemClicked;

            grid.Children.Add(label);

            wp.Children.Add(grid);
        }

        public static void LoadNotes(WrapPanel wp, TextBlock textBlock, TextBlock textBlockTitle)
        {
            string[] notes = Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder));

            if (notes.Length > 0)
            {
                foreach (string note in notes)
                {
                    NewNote(wp, Path.GetFileName(note));
                    Note not = new Note(Path.GetFileName(note));
                }

                if (notes.Length > 0)
                {
                    foreach (var child in wp.Children)
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

                foreach (UIElement element in wp.Children)
                {
                    element.PreviewMouseDown += WrapPanelNoteItemClicked;
                }

            }

            LoadText(textBlock, textBlockTitle);


        }

        public static void LoadText(TextBlock txt, TextBlock txtTitle)
        {
            if (Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder)).Length > 0)
            {
                using (StreamReader sr = new StreamReader(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder, AcessStuff.activeNote)))
                {
                    txt.Text = sr.ReadToEnd();
                    txtTitle.Text = AcessStuff.activeNote.Substring(0, AcessStuff.activeNote.Length - 4);
                }
            }
            else
            {
                txt.Text = "";
                txtTitle.Text = "";
            }
        }

        public static void WrapPanelNoteItemClicked(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            WrapPanel wrapPanel = mainWindow.wrapPanelNotes;
            TextBlock textBlock = mainWindow.noteText;
            TextBlock textBlockTitle = mainWindow.noteTitle;

            foreach (UIElement element in wrapPanel.Children)
            {
                if (element is Grid)
                {
                    Grid grid = element as Grid;
                    grid.Background = Brushes.White;
                }
            }

            if (sender is Grid)
            {
                string labelContent = "";
                Grid grid = sender as Grid;
                grid.Background = Brushes.LightGray;
                if (grid.Children[0] is Label)
                {
                    Label label = grid.Children[0] as Label;
                    AcessStuff.activeNote = $"{label.Content.ToString()}.txt";
                }
            }

            LoadText(textBlock, textBlockTitle);
        }
    }
}
