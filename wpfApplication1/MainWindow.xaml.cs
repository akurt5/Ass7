using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.Xml;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void bOpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "All Image Files|*.jpg; *.bmp; *.png;";
            openFileDialog1.Multiselect = true;
            openFileDialog1.RestoreDirectory = true;

            bool? userClickedOK = openFileDialog1.ShowDialog();

            if (userClickedOK == true)
            {
                foreach (String file in openFileDialog1.FileNames)
                {
                    Image Picture = new Image();
                    Picture.Source = new BitmapImage(new Uri(file));
                    canvas1.Children.Add(Picture);
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Good Press");
        }
        private Point mousepos;
        private Image draggedimage;

        private void canvas1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = e.Source as Image;
            if (image != null && canvas1.CaptureMouse())
            {
                Point mousepos = e.GetPosition(canvas1);
                draggedimage = image;
                Console.WriteLine("ButtonDown");
            }
        }

        private void canvas1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (draggedimage != null)
            {
                canvas1.ReleaseMouseCapture();
                draggedimage = null;
                Console.WriteLine("ButtonUp");
            }
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggedimage != null)
            {
                var offset =  e.GetPosition(canvas1) - mousepos;
                draggedimage.SetValue(Canvas.LeftProperty, offset.X);
                draggedimage.SetValue(Canvas.TopProperty, offset.Y);
                Console.WriteLine(String.Format("{0}",offset.X));             
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefiledialog1 = new SaveFileDialog();
            //savefiledialog1.
            savefiledialog1.DefaultExt = ".png";
            bool? userClickedOK = savefiledialog1.ShowDialog();

            

            if (userClickedOK == true)
            {
                string path = savefiledialog1.FileName;
               // canvas1.Background.Opacity = 0.0;
                RenderTargetBitmap RTB_Image = new RenderTargetBitmap((int)canvas1.ActualWidth, (int)canvas1.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                RTB_Image.Render(canvas1);

                var stm = System.IO.File.Create(path);

                BitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(RTB_Image));
                pngEncoder.Save(stm);
                stm.Close();

                XmlWriterSettings XWSettings = new XmlWriterSettings();
                XWSettings.Indent = true;

                int offset = path.LastIndexOf("\\");
                path = path.Substring(0, path.LastIndexOf("."));
                string name = path.Substring(offset + 1, path.Length - offset - 1);


                using (XmlWriter XMLWriter = XmlWriter.Create(path + ".xml", XWSettings))
                {
                    XMLWriter.WriteStartDocument();
                    XMLWriter.WriteStartElement("SprSheet");

                    XMLWriter.WriteAttributeString("CWidth", canvas1.ActualWidth.ToString());
                    XMLWriter.WriteAttributeString("CHeight", canvas1.ActualHeight.ToString());
                    XMLWriter.WriteAttributeString("Image", name);
                    int i = 0;
                    foreach (Image img in canvas1.Children)
                    {
                        XMLWriter.WriteStartElement("Image");
                        XMLWriter.WriteAttributeString("Name", name);//img.Source.ToString().Substring(img.Source.ToString().LastIndexOf("/") + 1, img.Source.ToString().Length - img.Source.ToString().LastIndexOf("/") - System.IO.Path.GetExtension(img.Source.ToString()).Length - 1));
                        XMLWriter.WriteAttributeString("X", img.GetValue(Canvas.LeftProperty).ToString());
                        XMLWriter.WriteAttributeString("Y", img.GetValue(Canvas.TopProperty).ToString());
                        XMLWriter.WriteAttributeString("Width", img.Width.ToString());
                        XMLWriter.WriteAttributeString("Height", img.Height.ToString());
                        XMLWriter.WriteAttributeString("Id", i.ToString());
                        XMLWriter.WriteEndElement();
                        i++;

                    }

                    XMLWriter.WriteEndDocument();
                } 
            }
        }
    }
}
