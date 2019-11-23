using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderNameChanger
{
    public partial class Form1 : Form
    {
        //public static string basePath = @"C:\Users\sinan\source\TEST\";
        public static string basePath = @"C:\Users\yusi7001\Documents\";


        public Form1()
        {
            // 22 Kasım puhs test
            InitializeComponent();
            button2.Enabled = false;
            textBox1.Text = basePath;

            if (IsBasePathValid())
            {
                if (listBox1.Items.Count <= 0)
                {
                    FillListBoxWithItems();
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            button2.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (IsBasePathValid())
            //{
            //    if(listBox1.Items.Count<=0)
            //    {
            //        FillListBoxWithItems();
            //    }
            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (IsBasePathValid())
            {
                if (listBox1.SelectedItem == null)
                {
                    MessageBox.Show("bir firma seç");
                }
                else
                {
                    if (CreateTextFile())
                    {
                        MessageBox.Show("Mevcut dizinde yeni txt file yaratıldı. Butona tekrar basın.");
                    }
                    else
                    {
                        if (GetEditedFolderNames().Contains("My Nielsen Answers"))
                        {
                            MessageBox.Show("Başka bir firma için çalışacaksanız önce dosya isimlerini başlangıç haline getirin.");
                        }
                        else
                        {
                            string folderNameToBeUpdated = basePath + listBox1.SelectedItem;

                            Directory.Move(folderNameToBeUpdated, basePath + "My Nielsen Answers");

                            string originalText = listBox1.SelectedItem.ToString();
                            string editedText = originalText.Substring(originalText.IndexOf("_") + 1);
                            OverwriteTextFile(editedText);

                            listBox1.Items.Clear();
                            FillListBoxWithItems();

                            label3.Text = editedText + " firmasında çalışılıyor.";
                        }
                    }
                }
            }      
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (IsBasePathValid())
            {
                if (GetEditedFolderNames().Contains("My Nielsen Answers"))
                {
                    string line1 = File.ReadLines(basePath + "ActiveCompanyLogFile.txt").First();
                    string fileNameToBeReplaced = basePath + "My Nielsen Answers_" + line1;

                    Directory.Move(basePath + "My Nielsen Answers", fileNameToBeReplaced);

                    string path = basePath + "ActiveCompanyLogFile.txt";

                    File.WriteAllText(path, "");

                    listBox1.Items.Clear();

                    FillListBoxWithItems();

                    label3.Text = "Seçili Firma için uzantı geri eklendi.";
                }
                else
                {
                    MessageBox.Show("uzantısız dosya bulunamadı.");
                }
            }
        }

        public List<string> GetEditedFolderNames()
        {
            string[] folderNamesInPath = Directory.GetDirectories(basePath);

            int basePathLength = basePath.Length;

            List<string> justFolderNamesList = new List<string>();

            foreach (var item in folderNamesInPath)
            {
                justFolderNamesList.Add(item.Remove(0, basePathLength));
            }

            return justFolderNamesList;
        }

        public void FillListBoxWithItems()
        {
            listBox1.Items.AddRange(GetEditedFolderNames().ToArray());
        }

        public bool CreateTextFile()
        {
            string path = basePath + "ActiveCompanyLogFile.txt";
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();

                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine("");
                }

                return true;
            }

            return false;
        }

        public void OverwriteTextFile(string text)
        {
            string path = basePath + "ActiveCompanyLogFile.txt";

            if (File.Exists(path))
            {
                File.WriteAllText(path, "");

                using (TextWriter tw = new StreamWriter(path))
                {
                    tw.WriteLine(text);
                }
            }
        }

        public bool IsBasePathValid()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Path alanını boş.");
                return false;
            }
            else if (!Directory.Exists(textBox1.Text))
            {
                MessageBox.Show("Böyle bir klasör bulunmuyor.");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
