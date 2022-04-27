using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace ExampleApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        AppContext db;

        public MainWindow()
        {
            InitializeComponent();

            db = new AppContext();

            loadAllUser();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void loadAllUser()
        {
            List<User> users = db.Users.ToList();
            listofUsers.ItemsSource = users;
        }

        private void exitProgram_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void saveNewFile_Click(object sender, RoutedEventArgs e)
        {
            saveFile();
        }

        private void openNewFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            bool? res = openFileDialog.ShowDialog();

            if (res != false)
            {
                Stream stream;
                if ((stream = openFileDialog.OpenFile()) != null)
                {
                    string fileName = openFileDialog.FileName;
                    string fileText = File.ReadAllText(fileName);
                    textBox.Text = fileText;
                }
            }
        }

        private void timeNewRomanFont_Click(object sender, RoutedEventArgs e)
        {
            textBox.FontFamily = new FontFamily("Times New Roman");
            verdanaFont.IsChecked = false;
        }

        private void verdanaFont_Click(object sender, RoutedEventArgs e)
        {
            textBox.FontFamily = new FontFamily("Verdana");
            timeNewRomanFont.IsChecked = false;
        }

        private void selectFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string fontSize = selectFontSize.SelectedItem.ToString();
            fontSize = fontSize.Substring(fontSize.Length - 2);
            switch (fontSize)
            {
                case "10":
                    textBox.FontSize = 10;
                    break;
                case "14":
                    textBox.FontSize = 14;
                    break;
                case "16":
                    textBox.FontSize = 16;
                    break;
                case "20":
                    textBox.FontSize = 20;
                    break;
                case "24":
                    textBox.FontSize = 24;
                    break;
                case "32":
                    textBox.FontSize = 32;
                    break;
                case "48":
                    textBox.FontSize = 48;
                    break;

            }
        }

        private void createNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "")
            {
                saveFile();
            }
            textBox.Text = "";
        }

        private void saveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.ShowDialog();
            bool? res = saveFileDialog.ShowDialog();

            if (res != false)
            {
                using (Stream stream = File.Open(saveFileDialog.FileName, FileMode.OpenOrCreate))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(textBox.Text);
                    }
                }
            }
        }

        private void regBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = loginField.Text;
            string password = paswField.Password;

            if (login.Length < 4)
            {
                loginField.Background = Brushes.Red;
                loginField.ToolTip = "Некорректно заполнено";
            }
            else if (password.Length < 4)
            {
                paswField.Background = Brushes.Red;
                paswField.ToolTip = "Некорректно заполнено";
            }
            else
            {
                loginField.Background = Brushes.Transparent;
                paswField.Background = Brushes.Transparent;

                User user = null;
                user = db.Users.Where(u => u.Login == login && u.Password == password).FirstOrDefault();

                if (user != null)
                {
                    MessageBox.Show("Пользователь авторизован");
                }
                else
                {
                    user = db.Users.Where(u => u.Login == login).FirstOrDefault();
                    if (user != null)
                    {
                        MessageBox.Show("Такой логин занят, введите другой");
                    }
                    else
                    {
                        user = new User(login, password);

                        db.Users.Add(user);
                        db.SaveChanges();

                        MessageBox.Show("Пользователь добален");

                        loadAllUser();
                    }
                }
            }
        }
    }
}
