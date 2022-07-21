using System;
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

namespace WPFSort{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window{

        bool sampleType;    //true = random; false = custom

        public MainWindow(){
            InitializeComponent();
        }

        private void RadioButton_Checked_RandomSample(object sender, RoutedEventArgs e){
			InputTextBox.IsEnabled = false;
			OutputTextBox.IsEnabled = false;
            GenerateSampleButton.IsEnabled = true;
			RandomSample_Slider.IsEnabled = true;
			sampleType = true;
        }

        private void RadioButton_Checked_CustomSample(object sender, RoutedEventArgs e){
			RandomSample_Slider.IsEnabled = false;
            GenerateSampleButton.IsEnabled = false;
            InputTextBox.IsEnabled = true;
			OutputTextBox.IsEnabled = true;
			sampleType = false;
        }


        public async void Draw(){
            Rectangle line = new();
            line.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
            line.Width = 500;
            line.Height = 22;
            line.StrokeThickness = 9;
            line.HorizontalAlignment = HorizontalAlignment.Left;
            line.VerticalAlignment = VerticalAlignment.Center;
            canvas.Children.Add(line);
            for (int i = 0; i < 500; i++) {
            Canvas.SetLeft(line, i);
            Canvas.SetTop(line, i);
                await Task.Delay(1);
            }

        }

        private void SortButton_Click(object sender, RoutedEventArgs e){
            if (sampleType == true){ //random sample

            }
            else { //given sample

                if(InputTextBox.Text.Any(x => char.IsLetter(x))) {
                    MessageBox.Show("You have introduced letters in the text box, I cannot sort them visually!","Ints only!");
                    InputTextBox.Text = "";
				}
                else{

                    List<int> ints = new();
                    ints = FormatStringToint(InputTextBox.Text.ToCharArray());

                    OutputTextBox.Text += FortmatIntToString(ints);
                }
            }

        }

        private List<int> FormatStringToint(char[] toConvert){

            List<int> toReturn = new();

            int i=0, length = toConvert.Length;
            while (i<length){
                int index = i;

                while (index<length && Char.IsDigit(toConvert[index])){
					if (index-i >= 3) {
                        MessageBox.Show("Please introduce numbers in range 2-100", "Numbers are too big!");
                        InputTextBox.Text = "";
                        return toReturn;
                    }
                    index++; 
                }

                string currentNumber = new String(toConvert, i, index-i);

                toReturn.Add(int.Parse(currentNumber));

                i+=index-i+1;
                while (i<length && !Char.IsDigit(toConvert[i]))
                    i++;
            }

            return toReturn;
        }

        private string FortmatIntToString(List<int> toConvert) {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < toConvert.Count; i++) {
                sb.Append(toConvert[i].ToString());
                sb.Append(' ');
            }
            return sb.ToString();
        }

        
		private void RandomSampleButton_Click(object sender, RoutedEventArgs e) {

		}

		private void OnKeyDownHandler_InputBox(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                Draw();
            }
        }
    }
}
