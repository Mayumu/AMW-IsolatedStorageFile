using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using DoPliku2.Resources;
using System.IO.IsolatedStorage;
using System.IO;

namespace DoPliku2
{
    public partial class MainPage : PhoneApplicationPage
    {
        IsolatedStorageFile store;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            store = IsolatedStorageFile.GetUserStoreForApplication();
                //create a directory
                store.CreateDirectory("folder1");
                store.CreateDirectory("folder2");
                //create a file in root
                IsolatedStorageFileStream fileRoot = store.CreateFile("fileRoot.txt");
                fileRoot.Close();
                //create a file in folders
                IsolatedStorageFileStream fileFolder1 = store.CreateFile(Path.Combine("folder1", "fileFolder1.txt"));
                fileFolder1.Close();
                IsolatedStorageFileStream fileFolder2 = store.CreateFile(Path.Combine("folder2", "fileFolder2.txt"));
                fileFolder2.Close();
        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            console.IsReadOnly = true;
            console.Text = "";
            textBlock.Text = "root";
            string[] directoriesInTheRoot = store.GetDirectoryNames();
            string[] filesInTheRoot = store.GetFileNames();
            foreach (string dir in directoriesInTheRoot)
            {
                console.Text += " > " + dir + "\n";
                string searchpath = Path.Combine(dir, "*.*");
                string[] filesInSubDirs = store.GetFileNames(searchpath);
                foreach (string file in filesInSubDirs)
                {
                    console.Text += "     " + file + "\n";
                }
            }
            foreach (string file in filesInTheRoot)
            {
                console.Text += "" + file + "\n";
            }
        }

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            if(store.FileExists(textBoxAddress.Text))
            {
                textBlock.Text = textBoxAddress.Text;
                IsolatedStorageFileStream file = new IsolatedStorageFileStream(textBoxAddress.Text, FileMode.Open, store);
                StreamReader reader = new StreamReader(file);
                console.Text = reader.ReadToEnd();
                console.IsReadOnly = false;
                reader.Close();
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageFileStream file = new IsolatedStorageFileStream(textBoxAddress.Text, FileMode.Open, store);
            StreamWriter writer = new StreamWriter(file);
            writer.Write(console.Text);
            writer.Close();
        }
    }
}