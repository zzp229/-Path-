using System;
using System.Collections.Generic;
using System.ComponentModel;
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


namespace WpfApp8
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class CircularProgressBar : UserControl
    {

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(0d, new PropertyChangedCallback(OnPropertyChanged)));




        public CircularProgressBar()
        {
            InitializeComponent();

            this.SizeChanged += CircularProgressBar_SizeChanged;
        }

        private void CircularProgressBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.UpdateValue();
        }

        // Value值改变就调用这个
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CircularProgressBar).UpdateValue();
        }
         

        // Value或其它改变都调用这个
        private void UpdateValue()
        {
            this.layot.Width = Math.Min(this.RenderSize.Width, this.RenderSize.Height);
            double radius = Math.Min(this.RenderSize.Width, this.RenderSize.Height) / 2;
            if (radius <= 0) return;


            double newValue = this.Value % 100.0;
            double newX = 0.0, newY = 0.0;
            newX = radius + (radius - 3) * Math.Cos((newValue * 3.6 - 90) * Math.PI / 180);
            newY = radius + (radius - 3) * Math.Sin((newValue * 3.6 - 90) * Math.PI / 180);


            //M75 3A75 75 0 0 1 147 75

            string pathDataStr = "M{0} 3A{1} {1} 0 {4} 1 {2} {3}";
          pathDataStr = string.Format(pathDataStr,
                radius ,
                radius - 3,
                newX,
                newY,
              newValue < 50 && newValue > 0 ? 0 : 1
                );
           var converter = TypeDescriptor.GetConverter(typeof(Geometry));
           this.path.Data = (Geometry)converter.ConvertFrom(pathDataStr);
            this.path.ApplyTemplate();
            
        }
    }
}