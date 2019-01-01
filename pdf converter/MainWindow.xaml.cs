using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pdf_converter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog op = new OpenFileDialog();
                op.Filter = "pdf file|*.pdf";
                op.ShowDialog();
                string tortf = ReadPdfFile(op.FileName);
                RichTextBox rtf = new RichTextBox();
                rtf.Document.Blocks.Clear();
                rtf.Document.Blocks.Add(new Paragraph(new Run(tortf)));
                SaveFileDialog sv = new SaveFileDialog();
                sv.Filter = "txt file|*txt";
                sv.ShowDialog();
                var write = new StreamWriter(new FileStream(sv.FileName+".txt", FileMode.Create, FileAccess.ReadWrite), Encoding.UTF8);
                write.Write(tortf);
                write.Close();
                string path = System.IO.Path.GetDirectoryName(sv.FileName)+@"\images from pdf";
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }
                Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
                pdf.LoadFromFile(op.FileName);
                for (int i = 0; i <= pdf.Pages.Count; i++)
                {
                    try
                    {
                        var source = pdf.SaveAsImage(i);
                        source.Save(path+@"\page" + i.ToString() + ".png", ImageFormat.Png);
           
                    }
                    catch
                    {

                    }
                }
                MessageBox.Show("Ok!");
               
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
            }
        }
        public string ReadPdfFile(object Filename)
        {
            PdfReader reader2 = new PdfReader((string)Filename);
            string strText = string.Empty;

          
            for (int page = 1; page <= reader2.NumberOfPages; page++)
            {
                ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy();
                PdfReader reader = new PdfReader((string)Filename);

                String s = PdfTextExtractor.GetTextFromPage(reader, page, its);

                s = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(s)));
                strText = strText + s;
                reader.Close();
            }
            return strText;
        }
    }
}
