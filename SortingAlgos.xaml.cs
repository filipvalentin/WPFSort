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

		private bool VisualCompare(Rectangle a, Rectangle b) {
			if (sortingOrder==false) {
				if (a.Height > b.Height) return true;
				else return false;
			}
			if (sortingOrder==true) {
				if (a.Height < b.Height) return true;
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
					if (VisualCompare(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[j].sampleRectangle))
						VisualSwap(sampleValueRectangleList[i], sampleValueRectangleList[j]);
				}
			}
		}

		private void Algo_BubbleS() { 
		
		}

		private void Algo_QuickS() {

		}

		private void Algo_SelectionS() {

		}


		private void Algo_MergeS() {

		}

		private void Algo_HeapS() {

		}

		private void Algo_CountingS() {

		}
		private void Algo_BucketS() {

		}
		private void Algo_RadixS() {

		}

		private void Algo_OddEvenS() {

		}


	}
}
