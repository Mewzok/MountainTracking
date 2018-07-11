﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int totalNum = 0;

        public MainWindow()
        {
            InitializeComponent();
            SetTotalNum();
            DeserializeSettings();
        }

        // Keep track of how many buttons will have to be changed at startup
        private void SetTotalNum()
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\projectsaves\\totalnumber.txt");

            if(!file.Exists)
            {
                File.Create(file.ToString());

                TextWriter writer = new StreamWriter(file.ToString());
                writer.Write(0);
                writer.Close();
            }

            TextReader reader = new StreamReader(file.ToString());

            totalNum = Convert.ToInt32(reader.ReadToEnd());
            reader.Close();
        }

        // Button was clicked
        private void Numbers1_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as System.Windows.Controls.Button).Content.ToString();
            SaveFile(content);
        }

        // Open and use save file dialog
        private void SaveFile(String buttonName)
        {
            var result = System.Windows.MessageBox.Show($"Have you completed {buttonName}?", "", MessageBoxButton.YesNo);
            bool fileExists = false;
            int fileNum = 1;

            // Open a file dialog so user can choose the file to store
            if (result == MessageBoxResult.Yes)
            {
                var tempFile = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                tempFile = tempFile.Parent;
                tempFile = tempFile.Parent;
                tempFile = tempFile.Parent;
                tempFile = tempFile.Parent;

                // Opens the file dialog and sets default and filter
                OpenFileDialog ofd = new OpenFileDialog
                {
                    DefaultExt = ".txt",
                    Filter = "Text documents (.txt)|*.txt|C# File (.cs)|*.cs|All Files (.*)|*.*",
                    InitialDirectory = tempFile.ToString(),
                    RestoreDirectory = true
                };

                // Create the file to save data to
                try
                {
                    do
                    {
                        DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\projectsaves");
                        FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\projectsaves\\file" + fileNum + ".xml");
                        if (!File.Exists(file.ToString()))
                        {
                            // Serialize
                            SerializeFile(ofd, file, dir, buttonName);

                            fileExists = false;
                        }
                        else
                        {
                            fileNum++;
                            fileExists = true;
                        }

                    } while (fileExists == true);
                }
                catch
                {

                }
            }
        }

        // Serialize the file that contains the user's code
        private void SerializeFile(OpenFileDialog openFD, FileInfo file, DirectoryInfo dir, String buttonName)
        {
            try
            {
                if (openFD.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // Create file
                    Directory.CreateDirectory(dir.ToString());
                    var newFile = File.Create(file.ToString());
                    newFile.Close();

                    // Read and write every line of old file to new file
                    string pathFile = openFD.FileName;
                    XmlSerializer serializer = new XmlSerializer(typeof(String));
                    StreamReader sr = File.OpenText(pathFile);
                    String allText = "";


                    //while (!sr.EndOfStream)
                    //{
                    allText = sr.ReadToEnd();
                    sr.Close();

                    using (TextWriter tw = new StreamWriter(file.ToString()))
                    {
                        serializer.Serialize(tw, allText);
                    }

                    // Hide the button
                    Numbers1.Visibility = System.Windows.Visibility.Collapsed;

                    // Add it to Completed tab
                    CNumbers1.Visibility = System.Windows.Visibility.Visible;

                    SerializeSettings(Numbers1, CNumbers1, buttonName);

                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString());
            }
        }

        // Serialize the changes the user has made to the program
        private void SerializeSettings(System.Windows.Controls.Button hiddenB, System.Windows.Controls.Button viewB, String buttonName)
        {
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\savedsettings");
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\projectsaves\\buttoninfo\\button\\" + buttonName + ".xml");
            FileInfo file2 = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\projectsaves\\buttoninfo\\button" + "C" + buttonName + ".xml");

            if(!file.Exists)
            {
                if(!dir.Exists)
                {
                    Directory.CreateDirectory(dir.ToString());
                }
                var newFile = File.Create(file.ToString());
                newFile.Close();

                var newFile2 = File.Create(file2.ToString());
                newFile2.Close();

                XmlSerializer serializer = new XmlSerializer(typeof(System.Windows.Controls.Button));

                using (TextWriter tw = new StreamWriter(file.ToString()))
                {
                    serializer.Serialize(tw, buttonName);
                }

                using (TextWriter tw = new StreamWriter(file2.ToString()))
                {
                    serializer.Serialize(tw, "C" + buttonName);
                }
            }
        }

        // Deserialize the changes the user has made to the program
        private void DeserializeSettings()
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(System.Windows.Controls.Button));

            List<System.Windows.Controls.Button> btnList = new List<System.Windows.Controls.Button> { };

            // Run through every possible button, attempting to deserialize them all at startup
            while (totalNum > 0)
            {
                for (int i = 1; i <= 22; i++)
                {
                    TextReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\projectsaves\\buttoninfo\\button\\numbers" + i + ".xml");
                    object obj = deserializer.Deserialize(reader);

                    foreach(System.Windows.Controls.Button b in this.NumbersGrid.Children)
                        {
                            try
                            {
                                Numbers1 = (System.Windows.Controls.Button)obj;
                            }
                            catch
                            {

                            }
                        }


                }
            }
        }
    }
}