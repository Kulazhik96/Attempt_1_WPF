using System;
using System.Windows;
using System.IO;
using System.Text.RegularExpressions;

namespace Lab1_Kulazhin_WPF
{
    /// <summary>
    /// Code-behind for Lab1_Kulazhik.WPF (Convert Wizzard) application
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// This constructor is called only once, to combine code-behind with markup
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// If this button was pressed, you'll see your coordinates
        /// in output TextBox = "convertedText"
        /// </summary>
        private void Convert_Data(object sender, RoutedEventArgs e)
        {
            if (convertedText.Text != string.Empty)
                convertedText.Clear();

            GetConvertedCoordinates(coordinatesToConvert.Text);
        }

        /// <summary>
        /// Looks for matches with coordinate for (using Regex)
        /// and transoforms it into desired format.
        /// </summary>
        /// <param name="buffer">String with input data</param>
        private void GetConvertedCoordinates(string buffer)
        {
            Regex coordinate = new Regex(@"\d{1,}\.\d{1,}");
            MatchCollection coordinateMatches = coordinate.Matches(buffer);
            for (int i = 0; i < coordinateMatches.Count; i++)
            {
                if (i % 2 == 0)
                    convertedText.Text += "X: " + coordinateMatches[i];
                if (i % 2 != 0)
                    convertedText.Text += " Y: " + coordinateMatches[i] + '\n';
            }
            convertedText.Text = convertedText.Text.Replace('.', ',');
        }

        /// <summary>
        /// If this button is pressed, you'll get data in current directory
        /// and see the message about it
        /// </summary>
        /// <exception cref="IOException">Download_Data button may throw IOException
        /// if it's smth wrong with your current directory
        /// </exception>
        private void Download_Data(object sender, RoutedEventArgs e)
        {
            StreamWriter fout = null;
            try
            {
                string currentDirectory = Environment.CurrentDirectory + @"\ConvertedData_byKulazhik.txt";
                fout = new StreamWriter(new FileStream(currentDirectory, FileMode.Create, FileAccess.Write));
                fout.WriteLine(convertedText.Text);

                MessageBox.Show($"Downloaded successfully to {currentDirectory}");
            }

            catch (IOException exc)
            {
                MessageBox.Show(exc.Message);
            }

            finally
            {
                fout.Close();
            }
        }

        /// <summary>
        /// Get data from dropped file and convert it
        /// </summary>
        /// <exception cref="IOException">Drop_File event button may throw IOException
        /// if it's smth wrong with directory, form which you drag your .txt file
        /// </exception>
        private void CoordinatesToConvert_Drop(object sender, DragEventArgs e)
        {
            string[] file = null;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                file = (string[])e.Data.GetData(DataFormats.FileDrop);
            }

            StreamReader fin = null;
            string dataFromFile = string.Empty;
            try
            {
                fin = new StreamReader(new FileStream(file[0], FileMode.Open, FileAccess.Read));
                string buffer = fin.ReadToEnd();

                GetConvertedCoordinates(buffer);
            }

            catch(IOException exc)
            {
                MessageBox.Show(exc.Message);
            }

            finally
            {
                fin.Close();
            }
        }

        /// <summary>
        /// I took this part from StackOverflow.com and
        /// don't even know what is it doing,
        /// but I'll definitely find out!
        /// </summary>
        private void CoordinatesToConvert_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }
    }
}
