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

namespace InVisual.controls
{
    /// <summary>
    /// Interaction logic for uTextButton.xaml
    /// </summary>
    public partial class uTextButton : UserControl
    {
        public static DependencyProperty captionProperty = DependencyProperty.Register("caption", typeof(string), typeof(uTextButton), new PropertyMetadata(default(string), OnPropertyChanged));
        public static DependencyProperty textProperty = DependencyProperty.Register("text", typeof(string), typeof(uTextButton), new PropertyMetadata(default(string), OnPropertyChanged));
        public delegate void OnButtonClickHander(object sender);
        public event OnButtonClickHander OnButtonClick;

        public uTextButton()
        {
            InitializeComponent();
        }

        public string caption
        {
            get { return (string)GetValue(captionProperty); }
            set
            {
                SetValue(captionProperty, value);
                tb.Text = value;
            }
        }


        public string text
        {
            get { return (string)GetValue(textProperty); }
            set
            {
                SetValue(textProperty, value);
                txt.Text = value;
            }
        }


        // la proprietà non viene invocata, richiede callback statico
        // invocata da xaml
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            uTextButton ub = d as uTextButton;
            

            switch (e.Property.Name.ToUpper())
            {
                case "CAPTION":
                    string descr = e.NewValue.ToString();
                    TextBlock tb = ub.btn.FindName("tb") as TextBlock;
                    tb.Text = descr;
                    break;

                case "TEXT":
                    string text = e.NewValue.ToString();
                    TextBox txt = ub.btn.FindName("txt") as TextBox;
                    txt.Text = text;
                    break;

            }

        }

        private void btn_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnButtonClick?.Invoke(this);
        }

    }
}
