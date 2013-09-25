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

        private void image1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();
            Point mouse =e.GetPosition(image1);
            image1.RenderTransform = mouse;
        }

        private void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
