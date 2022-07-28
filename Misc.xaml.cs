using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System;
using System.Windows.Shapes;

namespace WPFSort {
	public partial class MainWindow : Window {

        bool[] enableRndomSampleButton = { false, false };
        bool[] enableSortButton = { false, false };

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            if (sampleValueRectangleList.Count != 0 ) {
                RenderAvailableList();
            }
        }


        private List<int> FormatStringToint(char[] toConvert) {
            List<int> toReturn = new();
            maxSample = 0;
            minSample = int.MaxValue;

            int i = 0, length = toConvert.Length;
            while (i < length) {
                int index = i;

                while (index < length && char.IsDigit(toConvert[index])) {
                    if (index - i > 3) {
                        MessageBox.Show("Please introduce numbers in range 2-9999", "Numbers are too big!"); //deprecated, maxint!!!
                        InputTextBox.Text = "";
                        return toReturn;
                    }
                    index++;
                }

                string currentNumber = new string(toConvert, i, index - i);

                int currentInt = int.Parse(currentNumber);
                toReturn.Add(currentInt);
                maxSample = Math.Max(maxSample, currentInt);
                minSample = Math.Min(minSample, currentInt);

                i += index - i + 1;
                while (i < length && !char.IsDigit(toConvert[i]))
                    i++;
            }

            return toReturn;
        }

        private string FortmatRectIntToString() {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sampleValueRectangleList.Count; i++) {
                sb.Append(sampleValueRectangleList[i].sampleValue.ToString());
                sb.Append(' ');
            }
            return sb.ToString();
        }

        //
        private void RadioButton_RandomSample_Checked(object sender, RoutedEventArgs e) {
            InputTextBox.IsEnabled = false;
            OutputTextBox.IsEnabled = false;
            RandomSampleCountBox.IsEnabled = true;
            RandomSampleSizeBox.IsEnabled = true;
            sampleType = true;
        }
        private void RadioButton_CustomSample_Checked(object sender, RoutedEventArgs e) {
            RandomSampleCountBox.IsEnabled = false;
            RandomSampleSizeBox.IsEnabled = false;
            InputTextBox.IsEnabled = true;
            OutputTextBox.IsEnabled = true;
            sampleType = false;
        }

        private void PreviewTextInput_SampleBoxes(object sender, TextCompositionEventArgs e) {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        //RANDOM SAMPLE BOXES ENABLE LOGIC
        private void RandomSampleSizeBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (RandomSampleSizeBox.Text.Length > 0)
                enableRndomSampleButton[0] = true;
            else {
                enableRndomSampleButton[0] = false;
                RandomSampleSizeBox.Text = "1";
            }
            CheckRandomSampleButtonEnable();
        }

        private void RandomSampleCountBox_TextChanged(object sender, TextChangedEventArgs e) {//999 max, reasonable enough
            if (RandomSampleCountBox.Text.Length > 0)
                enableRndomSampleButton[1] = true;
            else {
                enableRndomSampleButton[1] = false;
                RandomSampleCountBox.Text = "2";
            }
            CheckRandomSampleButtonEnable();
        }

        private void CheckRandomSampleButtonEnable() {
            if (enableRndomSampleButton[0] == true && enableRndomSampleButton[1] == true)
                GenerateSampleButton.IsEnabled = true;
            else GenerateSampleButton.IsEnabled = false;
        }



        //SORT BUTTON ENABLE LOGIC
        private void SortOrder_Ascending_Checked(object sender, RoutedEventArgs e) {
            sortingOrder = true;
            enableSortButton[1] = true;
            CheckSortButtonEnable();
        }

        private void SortOrder_Descending_Checked(object sender, RoutedEventArgs e) {
            sortingOrder = false;
            enableSortButton[1] = true;
            CheckSortButtonEnable();
        }

        	private void CBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            enableSortButton[0] = true;
            CheckSortButtonEnable();
	    }

        private void CheckSortButtonEnable() {
            if (enableSortButton[0] == true && enableSortButton[1] == true)
                sortButton.IsEnabled = true;
            else sortButton.IsEnabled = false;
		}


        private void RenderTickBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (RenderTickBox.Text == string.Empty) RenderTickBox.Text = "0";
            pauseSlice = int.Parse(RenderTickBox.Text);
        }
    }

}
