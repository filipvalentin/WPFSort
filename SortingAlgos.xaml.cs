using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFSort {//23:30 26:06    26:30   2:36
	public partial class MainWindow : Window {

		int pauseSlice = 1; //miliseconds, the lowest the platform can support
		bool sortingOrder; // true = ascending, false = descending

		private async Task ChangeColor(Rectangle a, Rectangle b) {
			a.Stroke = Brushes.Red; a.Fill = Brushes.Red;
			b.Stroke = Brushes.Red; b.Fill = Brushes.Red;
														  
			await Task.Delay(pauseSlice);

			a.Stroke = Brushes.Black; a.Fill = Brushes.Black;
			b.Stroke = Brushes.Black; b.Fill = Brushes.Black;
		}

		private bool VisualCompare1(Rectangle a, Rectangle b) {
			if (sortingOrder==true) {	//ascending
				if (a.Height < b.Height) return true;
					else return false;				
			}
			if (sortingOrder==false) {	//descending
				if (a.Height > b.Height) return true;
				else return false;
			}
			return false;
		}

		private bool VisualCompare2(Rectangle a, Rectangle b) {
			if (sortingOrder == false) { //descending
				if (a.Height < b.Height) return true;
				else return false;
			}
			if (sortingOrder == true) {    //ascending
				if (a.Height > b.Height) return true;
				else return false;
			}
			return false;
		}

		private void VisualSwap(ValueRectanglePair a, ValueRectanglePair b) {
			(a.sampleRectangle.Height, b.sampleRectangle.Height) = (b.sampleRectangle.Height, a.sampleRectangle.Height);
			(a.sampleValue, b.sampleValue) = (b.sampleValue, a.sampleValue);

			Canvas.SetTop(a.sampleRectangle, canvas.ActualHeight - a.sampleRectangle.Height);
			Canvas.SetTop(b.sampleRectangle, canvas.ActualHeight - b.sampleRectangle.Height);
		}


		private async Task Algo_NaiveS() {

			for (int i = 0; i < sampleValueRectangleList.Count; i++) {
				for (int j = 0; j < sampleValueRectangleList.Count; j++) {
					await ChangeColor(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[j].sampleRectangle);
					if (VisualCompare1(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[j].sampleRectangle))
						VisualSwap(sampleValueRectangleList[i], sampleValueRectangleList[j]);
				}
			}
		}

		private async Task Algo_BubbleS() {

			for (int step = 0; step < (sampleValueRectangleList.Count - 1); ++step) {

				int swapped = 0;

				for (int i = 0; i < (sampleValueRectangleList.Count - step - 1); ++i) {

					await ChangeColor(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[i + 1].sampleRectangle);

					if (VisualCompare2(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[i+1].sampleRectangle)) {

						VisualSwap(sampleValueRectangleList[i], sampleValueRectangleList[i + 1]);

						swapped = 1;
					}
				}

				if (swapped == 0)
					break;
			}

		}

		private async Task Algo_QuickS() {

		}

		private async Task Algo_SelectionS() {

		}


		private async Task Algo_MergeS() {

		}

		private async Task Algo_HeapS() {

		}

		private async Task Algo_CountingS() {

		}
		private async Task Algo_BucketS() {

		}
		private async Task Algo_RadixS() {

		}

		private async Task Algo_OddEvenS() {

		}


	}
}
