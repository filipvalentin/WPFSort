using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WPFSort{

    public partial class MainWindow : Window{

        bool sampleType;    //true = random; false = custom
        double sampleCount = 2;
        List<int>? sampleList;
 
        int maxSample=0;

        class ValueRectanglePair {
            public int sampleValue;
            public Rectangle sampleRectangle;
		}

        List<ValueRectanglePair> sampleValueRectangleList = new();
        public MainWindow(){
            InitializeComponent();
        }


        private void AddOnCanvas3(List<int> toAdd) {
            sampleValueRectangleList = new();
            double reductionRatio = canvas.ActualHeight / maxSample;

            for (int i = 0; i < toAdd.Count; i++) {
                Rectangle line = new();
                line.Stroke = Brushes.Black;
                line.Fill = Brushes.Black;
                line.Width = canvas.ActualWidth / sampleCount - 1;
                line.Height = (double)toAdd[i] * reductionRatio;

                canvas.Children.Add(line);
                Canvas.SetLeft(line, i * (canvas.ActualWidth / sampleCount));
                Canvas.SetTop(line, canvas.ActualHeight - toAdd[i] * reductionRatio);

                sampleValueRectangleList.Add( new ValueRectanglePair{sampleValue=toAdd[i], sampleRectangle=line });
            }
        }

        private void RenderAvailableList() {
            double reductionRatio = canvas.ActualHeight / maxSample;

            for (int i = 0; i < sampleValueRectangleList.Count; i++) {
                sampleValueRectangleList[i].sampleRectangle.Width = canvas.ActualWidth / sampleCount - 1;
                sampleValueRectangleList[i].sampleRectangle.Height = (double)sampleValueRectangleList[i].sampleValue * reductionRatio;
                Canvas.SetLeft( sampleValueRectangleList[i].sampleRectangle, i * (canvas.ActualWidth / sampleCount));
                Canvas.SetTop( sampleValueRectangleList[i].sampleRectangle, canvas.ActualHeight - sampleValueRectangleList[i].sampleValue * reductionRatio);
            }

        }

        private async void SortButton_Click(object sender, RoutedEventArgs e){

            if (sampleType == true){ //random sample
                await Sort();
            }
            else { //given sample

                if(InputTextBox.Text.Any(x => char.IsLetter(x))) {
                    MessageBox.Show("You have introduced letters in the text box, I cannot sort them visually!","Ints only!");
                    InputTextBox.Text = "";
				}
                else{   //de mutat

                    List<int> ints = new();
                    ints = FormatStringToint(InputTextBox.Text.ToCharArray());
                    sampleCount = ints.Count;

                    await Sort();
                    OutputTextBox.Clear();
                    OutputTextBox.Text += FortmatRectIntToString();
                }
            }

        }



        
		private void RandomSampleButton_Click(object sender, RoutedEventArgs e) {
            ClearList3();
            int count = int.Parse(RandomSampleCountBox.Text);
            int maxsize = int.Parse(RandomSampleSizeBox.Text);
            maxSample = 0;
            sampleList = new();

            for (int i = 0; i < count; i++) {
                var ran = new Random();
                int currInt = (int)ran.NextInt64(1, maxsize+1);
                sampleList.Add(currInt);
                maxSample = Math.Max(maxSample, currInt);
            }

            sampleCount = count;

            AddOnCanvas3(sampleList);
            
        }

        private void OnKeyDownHandler_InputBox(object sender, KeyEventArgs e) {     //deprecated
            ClearList3();
            if (e.Key == Key.Return) {

                if (InputTextBox.Text.Any(x => char.IsLetter(x))) {
                    MessageBox.Show("You have introduced letters in the text box, I cannot sort them visually!", "Ints only!");
                    InputTextBox.Text = "";
                }
                else {

                    List<int> ints = new();
                    ints = FormatStringToint(InputTextBox.Text.ToCharArray());
                    sampleCount = ints.Count;
                    sampleList = ints;
                    //OutputTextBox.Text += FortmatIntToString(ints);
                }

                ClearList3();

                AddOnCanvas3(sampleList);
            }
        }

        private void ClearList3() {
            for (int i = 0; i < sampleValueRectangleList.Count; i++)
                canvas.Children.Remove(sampleValueRectangleList[i].sampleRectangle);

            sampleValueRectangleList.Clear();
            GC.Collect();
        }


        private async Task Sort() {
            sortButton.IsEnabled = false;
            switch (CBox.SelectedIndex) {                
                case 0: await Algo_NaiveS(); break;
                case 1: Algo_BubbleS(); break;
                case 2: Algo_QuickS(); break;
                case 3: Algo_SelectionS(); break;
                case 4: Algo_MergeS(); break;
                case 5: Algo_HeapS(); break;
                case 6: Algo_CountingS(); break;
                case 7: Algo_BucketS(); break;
                case 8: Algo_RadixS(); break;
                case 9: Algo_OddEvenS(); break;
            }
            sortButton.IsEnabled = true;
        }

		private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            ClearList3();

            if (InputTextBox.Text.Any(x => char.IsLetter(x))) {
                MessageBox.Show("You have introduced letters in the text box, I cannot sort them visually!", "Ints only!");
                InputTextBox.Text = "";
            }
            else {

                List<int> ints = new();
                ints = FormatStringToint(InputTextBox.Text.ToCharArray());
                sampleCount = ints.Count;
                sampleList = ints;
                //OutputTextBox.Text += FortmatIntToString(ints);
            }

            AddOnCanvas3(sampleList);
        }
	}
}
