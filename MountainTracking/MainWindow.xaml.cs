using System;
using System.Collections.Generic;
using System.Windows;
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
        List<System.Windows.Controls.Button> btnList = new List<System.Windows.Controls.Button> {};
        List<ProjectButton> numbersBL = new List<ProjectButton>( new ProjectButton[22] );

        public MainWindow()
        {
            MessageBox.Show(numbersBL[22].filePath);

            InitializeComponent();

            initButtonList();
            initButtonSettings();
            //SetTotalNum();
            //InitializeButtonList();
            //DeserializeSettings();
        }

        private void initButtonList()
        {
            string btnSett;
            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo btnFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\projectsaves");
            FileInfo file = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\projectsaves\\buttonsettings.txt");

            if (!file.Exists)
            {
                File.Create(file.ToString()).Close();

                TextWriter writer = new StreamWriter(file.ToString());
                
                for(int i = 0; i < 22; i++)
                {
                    writer.Write("Project" + (i + 1) + "\nFilepath: " + btnFile + (i + 1) + ".txt\nCompleted: false\n\n");
                }
                writer.Close();
            }

            TextReader reader = new StreamReader(file.ToString());
            btnSett = reader.ReadToEnd();
            reader.Close();

            int x = 1;

            string tests = "no";

            MessageBox.Show(numbersBL.Count.ToString());

            foreach(ProjectButton p in numbersBL)
            {
                p.filePath = btnSett.Substring(btnSett.IndexOf("Project" + x) + x + 17);
                MessageBox.Show(p.filePath);
            }

            MessageBox.Show(tests);
        }

        private void initButtonSettings()
        {

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
                writer.Write("0");
                writer.Close();
            }

            TextReader reader = new StreamReader(file.ToString());

            totalNum = Convert.ToInt32(reader.ReadToEnd());
            reader.Close();
        }

        private void InitializeButtonList()
        {
            #region ButtonAdding
                btnList.Add(Numbers1);
                btnList.Add(Numbers2);
                btnList.Add(Numbers3);
                btnList.Add(Numbers4);
                btnList.Add(Numbers5);
                btnList.Add(Numbers6);
                btnList.Add(Numbers7);
                btnList.Add(Numbers8);
                btnList.Add(Numbers9);
                btnList.Add(Numbers10);
                btnList.Add(Numbers11);
                btnList.Add(Numbers12);
                btnList.Add(Numbers13);
                btnList.Add(Numbers14);
                btnList.Add(Numbers15);
                btnList.Add(Numbers16);
                btnList.Add(Numbers17);
                btnList.Add(Numbers18);
                btnList.Add(Numbers19);
                btnList.Add(Numbers20);
                btnList.Add(Numbers21);
                btnList.Add(Numbers22);
            #endregion
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
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog
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
        private void SerializeFile(System.Windows.Forms.OpenFileDialog openFD, FileInfo file, DirectoryInfo dir, String buttonName)
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
                    try
                    {
                        TextReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\projectsaves\\buttoninfo\\button\\numbers" + i + ".xml");
                        object obj = deserializer.Deserialize(reader);
                        btnList[i] = (System.Windows.Controls.Button)obj;
                    }
                    catch{}
                }
            }
        }
    }
}