using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WPFSort {

    public partial class MainWindow : Window {

        bool sampleType;    //true = random; false = custom
        double sampleCount = 2;
        List<int>? sampleList;

        int maxSample = 0;
        int minSample = int.MaxValue;

        class ValueRectanglePair {
            public int sampleValue;
            public Rectangle sampleRectangle;
        }

        List<ValueRectanglePair> sampleValueRectangleList = new();
        public MainWindow() {
            InitializeComponent();
        }


        private void AddOnCanvas() {
            sampleValueRectangleList = new();
            double reductionRatio = canvas.ActualHeight / maxSample;

            for (int i = 0; i < sampleList.Count; i++) {
                Rectangle line = new();
                line.Stroke = Brushes.Black;
                line.Fill = Brushes.Black;
                line.Width = canvas.ActualWidth / sampleCount - 1;
                line.Height = (double)sampleList[i] * reductionRatio;

                canvas.Children.Add(line);
                Canvas.SetLeft(line, i * (canvas.ActualWidth / sampleCount));
                Canvas.SetTop(line, canvas.ActualHeight - sampleList[i] * reductionRatio);

                sampleValueRectangleList.Add(new ValueRectanglePair { sampleValue = sampleList[i], sampleRectangle = line });
            }
        }

        private void RenderAvailableList() {
            double reductionRatio = canvas.ActualHeight / maxSample;

            for (int i = 0; i < sampleValueRectangleList.Count; i++) {
                sampleValueRectangleList[i].sampleRectangle.Width = canvas.ActualWidth / sampleCount - 1;
                sampleValueRectangleList[i].sampleRectangle.Height = (double)sampleValueRectangleList[i].sampleValue * reductionRatio;
                Canvas.SetLeft(sampleValueRectangleList[i].sampleRectangle, i * (canvas.ActualWidth / sampleCount));
                Canvas.SetTop(sampleValueRectangleList[i].sampleRectangle, canvas.ActualHeight - sampleValueRectangleList[i].sampleValue * reductionRatio);
            }

        }


        private void RandomSampleButton_Click(object sender, RoutedEventArgs e) {
            ClearList();
            int count = int.Parse(RandomSampleCountBox.Text);
            int maxsize = int.Parse(RandomSampleSizeBox.Text);
            maxSample = 0;
            minSample = int.MaxValue;
            sampleList = new();

            for (int i = 0; i < count; i++) {
                var ran = new Random();
                int currInt = (int)ran.NextInt64(1, maxsize + 1);
                sampleList.Add(currInt);
                maxSample = Math.Max(maxSample, currInt);
                minSample = Math.Min(minSample, currInt);
            }

            sampleCount = count;

            AddOnCanvas();
        }

        private void OnKeyDownHandler_InputBox(object sender, KeyEventArgs e) {     //deprecated
            ClearList();

            if (e.Key == Key.Return) {

                RenderInputTextBoxContent();

                ClearList();

                AddOnCanvas();
            }
        }

        private void ClearList() {
            for (int i = 0; i < sampleValueRectangleList.Count; i++)
                canvas.Children.Remove(sampleValueRectangleList[i].sampleRectangle);

            sampleValueRectangleList.Clear();
            GC.Collect();
        }


        private async void SortButton_Click(object sender, RoutedEventArgs e) {

            if (sampleType == true) { //random sample
                await Sort();
            }
            else { //given sample

                RenderInputTextBoxContent();

                await Sort();
                OutputTextBox.Clear();
                OutputTextBox.Text += FortmatRectIntToString();
                
            }

        }


        private async Task Sort() {
            sortButton.IsEnabled = false;
            CBox.IsEnabled = false;
            sortOrderRButton_Ascending.IsEnabled = false;
            sortOrderRButton_Descending.IsEnabled = false;
            randomSampleButton.IsEnabled = false;
            customSampleRButton.IsEnabled = false;
            if(customSampleRButton.IsChecked==true) InputTextBox.IsEnabled = false;
            

            switch (CBox.SelectedIndex) {
                case 0: await Algo_NaiveS(); break;
                case 1: await Algo_BubbleS(); break;
                case 2: await Algo_SelectionS();  break;
                case 3: await Algo_QuickS(); break;
                case 4: await Algo_MergeS();break;
                case 5: await Algo_HeapS(); break;
                case 6: await Algo_CountingS(); break;
                case 7: await Algo_RadixS(); break;
                case 8: await Algo_BitonicS(); break;
            }

            sortButton.IsEnabled = true;
            CBox.IsEnabled = true;
            sortOrderRButton_Ascending.IsEnabled = true;
            sortOrderRButton_Descending.IsEnabled = true;
            randomSampleButton.IsEnabled = true;
            customSampleRButton.IsEnabled = true;
            if (customSampleRButton.IsChecked == true) InputTextBox.IsEnabled = true;
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            ClearList();

            RenderInputTextBoxContent();

            AddOnCanvas();
        }


        private void RenderInputTextBoxContent() {
            sampleList = FormatStringToint(InputTextBox.Text.ToCharArray());
            sampleCount = sampleList.Count;
        }
    }
}