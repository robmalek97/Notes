using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.IO.Packaging;
using System.Windows.Input;
using System.Xml.Linq;
using System.Windows.Media;

namespace Notes
{
    public class Folder
    {
        public string Name { get; set; }
        public List<Note> Notes { get; set; }

        public Folder(string name)
        {
            Name = name;
            Notes = new List<Note>();
        }
        public static void NewFolder(WrapPanel wp, string name)
        {
            Grid grid = new Grid();
            grid.Width = 130;
            grid.Height = 40;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;

            Label label = new Label();
            label.Content = name;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Width = 110;
            label.FontFamily = new FontFamily("Segoe UI Variable Display Semibold");

            grid.PreviewMouseDown += WrapPanelFolderItemClicked;

            grid.Children.Add(label);

            wp.Children.Add(grid);
        }
        public static void LoadFolders(WrapPanel wp)
        {
            string[] folders = Directory.GetDirectories(System.IO.Path.Combine(Environment.CurrentDirectory, "Note"));

            foreach (string folder in folders)
            {
                NewFolder(wp, Path.GetFileName(folder));
                Folder fol = new Folder(Path.GetFileName(folder));
            }

            if (folders.Length > 0)
            {
                AcessStuff.activeFolder = Path.GetFileName(folders[0]);
                if (wp.Children.Count > 0 && wp.Children[0] is Grid)
                {
                    Grid firstGrid = wp.Children[0] as Grid;
                    firstGrid.Background = Brushes.LightGray;
                }
            }

            foreach (UIElement element in wp.Children)
            {
                element.PreviewMouseDown += WrapPanelFolderItemClicked;
            }
        }

        public static void WrapPanelFolderItemClicked(object sender, MouseButtonEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            WrapPanel wrapPanel = mainWindow.wrapPanelFolders;
            WrapPanel wrapPanel1 = mainWindow.wrapPanelNotes;
            TextBlock textBlock = mainWindow.noteText;
            TextBlock textBlockTitle = mainWindow.noteTitle;

            wrapPanel1.Children.Clear();

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
                    AcessStuff.activeFolder = label.Content.ToString();
                    string[] notes = Directory.GetFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", AcessStuff.activeFolder));
                    if (notes.Length > 0)
                    {
                        AcessStuff.activeNote = System.IO.Path.GetFileName(notes[0]);
                    }
                }
            }

            Note.LoadNotes(wrapPanel1, textBlock, textBlockTitle);
        }
    }
}
