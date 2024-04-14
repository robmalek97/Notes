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
    /// Interaction logic for WindowNewFolder.xaml
    /// </summary>
    public partial class WindowNewFolder : Window
    {
        public WindowNewFolder()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (txtFolderName.Text != "" && !Directory.Exists(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", txtFolderName.Text)))
            {
                try
                {
                    Directory.CreateDirectory(System.IO.Path.Combine(Environment.CurrentDirectory, "Note", txtFolderName.Text));
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"Při vytváření složky došlo k chybě: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Folder folder = new Folder(txtFolderName.Text);
                AcessStuff.folderName = txtFolderName.Text;
                Close();
            }
            else
            {
                lblAlert.Content = "Špatně zadaný název složky";
            }
        }
    }
}
