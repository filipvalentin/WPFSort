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



		private async Task Algo_SelectionS() {

			for (int i = 0; i < sampleValueRectangleList.Count - 1; i++) {
				int min_idx = i;
				for (int j = i + 1; j < sampleValueRectangleList.Count; j++) {
					await ChangeColor(sampleValueRectangleList[j].sampleRectangle, sampleValueRectangleList[min_idx].sampleRectangle);
					if (VisualCompare1(sampleValueRectangleList[j].sampleRectangle, sampleValueRectangleList[min_idx].sampleRectangle))
						min_idx = j;
				}
				VisualSwap(sampleValueRectangleList[i], sampleValueRectangleList[min_idx]);
			}
		}



		private async Task Algo_QuickS() {
			await quickSort(sampleValueRectangleList, 0, sampleValueRectangleList.Count - 1);
		}
		async Task<int> partition(List<ValueRectanglePair> arr, int low, int high) {
			//int pivot = arr[high].sampleValue;
			int i = low - 1;

			for (int j = low; j < high; j++) {
				await ChangeColor(arr[j].sampleRectangle, arr[j].sampleRectangle);
				if (VisualCompare1(arr[j].sampleRectangle, arr[high].sampleRectangle)) {//arr[j].sampleValue < pivot 
					i++;
					//swap(arr, i, j);
					VisualSwap(arr[i], arr[j]);
				}
			}
			VisualSwap(arr[i+1], arr[high]); //swap(arr, i + 1, high);
			return i + 1;
		}
		async Task quickSort(List<ValueRectanglePair> arr, int low, int high) {
			if (low < high) {
				int partitionIndex = await partition(arr, low, high);

				await quickSort(arr, low, partitionIndex - 1);
				await quickSort(arr, partitionIndex + 1, high);
			}
		}


		
		private async Task Algo_MergeS() {		//wanky rendering red lines
			await mergeSort(sampleValueRectangleList, 0, sampleValueRectangleList.Count - 1);
		}
		async Task merge(List<ValueRectanglePair> arr, int l, int m, int r) {
			int n1 = m - l + 1;
			int n2 = r - m;

			int[] L = new int[n1];
			int[] R = new int[n2];
			int i, j;


			for (i = 0; i < n1; ++i)
				L[i] = arr[l + i].sampleValue;
			for (j = 0; j < n2; ++j)
				R[j] = arr[m + 1 + j].sampleValue;

			i = 0;
			j = 0;

			Action<ValueRectanglePair, int> assignTo_sampleValueRectangleList = (toAssign, newValue) => {
				Rectangle toSwap = toAssign.sampleRectangle;
				toSwap.Height = (double)newValue * (canvas.ActualHeight / maxSample);
				VisualSwap(toAssign, new ValueRectanglePair { sampleValue = newValue, sampleRectangle = toSwap });
			};

			int k = l;
			while (i < n1 && j < n2) {
				await ChangeColor(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[j].sampleRectangle);
				if (sortingOrder == true) {//ascendent
					if (L[i] <= R[j]) {
						assignTo_sampleValueRectangleList(arr[k], L[i]);//arr[k] = L[i];
						i++;
					}
					else {
						assignTo_sampleValueRectangleList(arr[k], R[j]);//arr[k] = R[j];
						j++;
					}
				}
				else {//descendent
					if (L[i] >= R[j]) {
						assignTo_sampleValueRectangleList(arr[k], L[i]);//arr[k] = L[i];
						i++;
					}
					else {
						assignTo_sampleValueRectangleList(arr[k], R[j]);//arr[k] = R[j];
						j++;
					}
				}
				k++;
			}

			while (i < n1) {
				await ChangeColor(sampleValueRectangleList[k].sampleRectangle, sampleValueRectangleList[k].sampleRectangle);

				assignTo_sampleValueRectangleList(arr[k], L[i]);//arr[k] = L[i];

				i++;
				k++;
			}

			while (j < n2) {
				await ChangeColor(sampleValueRectangleList[k].sampleRectangle, sampleValueRectangleList[k].sampleRectangle);

				assignTo_sampleValueRectangleList(arr[k], R[j]);//arr[k] = R[j];

				j++;
				k++;
			}
		}
		async Task mergeSort(List<ValueRectanglePair> arr, int l, int r) {
			if (l < r) {
				int m = l + (r - l) / 2;

				await mergeSort(arr, l, m);
				await mergeSort(arr, m + 1, r);

				await merge(arr, l, m, r);
			}
		}



		private async Task Algo_HeapS() {
			await heapSort(sampleValueRectangleList);
		}
		async Task heapSort(List<ValueRectanglePair> arr) {

			// Build heap (rearrange array)
			for (int i = arr.Count / 2 - 1; i >= 0; i--)
				await heapify(arr, arr.Count, i);

			// One by one extract an element from heap
			for (int i = arr.Count - 1; i > 0; i--) {
				// Move current root to end
				await ChangeColor(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[0].sampleRectangle);
				VisualSwap(sampleValueRectangleList[i], sampleValueRectangleList[0]);

				//int temp = arr[0];
				//arr[0] = arr[i];
				//arr[i] = temp;

				// call max heapify on the reduced heap
				await heapify(arr, i, 0);
			}
		}
		async Task heapify(List<ValueRectanglePair> arr, int n, int i) {
			int largest = i;
			int l = 2 * i + 1; // left = 2*i + 1
			int r = 2 * i + 2; // right = 2*i + 2

			if (l < n && VisualCompare2(arr[l].sampleRectangle, arr[largest].sampleRectangle) ) //arr[l].sampleRectangle.Height > arr[largest].sampleRectangle.Height
				largest = l;

			if (r < n && VisualCompare2(arr[r].sampleRectangle, arr[largest].sampleRectangle) ) //arr[r].sampleRectangle.Height > arr[largest].sampleRectangle.Height)
				largest = r;

			if (largest != i) {
				await ChangeColor(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[largest].sampleRectangle);
				VisualSwap(sampleValueRectangleList[i], sampleValueRectangleList[largest]);

				await heapify(arr, n, largest);
			}
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
