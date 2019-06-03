using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace NotePad
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private static object lockObject = new object();

        public MainWindow()
        {
            InitializeComponent();
            Timer timer = new Timer(Save, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(10));
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Print(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.ShowDialog();
        }

        private void Save(object sender)
        {
            using (var writer = new StreamWriter("Безымянный.txt"))
            {
                writer.WriteLine(new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text);
            }
        }

        private void SaveAs(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовые документы (.txt)|*.txt";
            saveFileDialog.DefaultExt = ".txt";

            saveFileDialog.ShowDialog();

            using (var writer = new StreamWriter(saveFileDialog.FileName))
            {
                if (!File.Exists(saveFileDialog.FileName)) {
                    File.Create(saveFileDialog.FileName + ".txt");
                }
                writer.WriteLine(new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text);
            }
        }

        private void Open(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".txt";
            openFileDialog.Filter = "Текстовые документы (.txt)|*.txt";
            openFileDialog.ShowDialog();
            richTextBox.Document.Blocks.Clear();
            using (var reader = new StreamReader(openFileDialog.FileName))
            {
                richTextBox.Document.Blocks.Add(new Paragraph(new Run(reader.ReadToEnd())));
            }
            Title = openFileDialog.FileName;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            Save(new object());
        }
    }
}
