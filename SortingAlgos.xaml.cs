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

		private async Task RectangleVisualSelection(Rectangle a) {
			a.Stroke = Brushes.Red; a.Fill = Brushes.Red;

			await Task.Delay(pauseSlice);

			a.Stroke = Brushes.Black; a.Fill = Brushes.Black;
		}

		private async Task RectangleVisualSelection(Rectangle a, Rectangle b) {
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

		private void Swap(ValueRectanglePair a, ValueRectanglePair b) {
			(a.sampleRectangle.Height, b.sampleRectangle.Height) = (b.sampleRectangle.Height, a.sampleRectangle.Height);
			(a.sampleValue, b.sampleValue) = (b.sampleValue, a.sampleValue);

			Canvas.SetTop(a.sampleRectangle, canvas.ActualHeight - a.sampleRectangle.Height);
			Canvas.SetTop(b.sampleRectangle, canvas.ActualHeight - b.sampleRectangle.Height);
		}



		private async Task Algo_NaiveS() {

			for (int i = 0; i < sampleValueRectangleList.Count; i++) {
				for (int j = 0; j < sampleValueRectangleList.Count; j++) {
					await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[j].sampleRectangle);
					if (VisualCompare1(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[j].sampleRectangle))
						Swap(sampleValueRectangleList[i], sampleValueRectangleList[j]);
				}
			}
		}



		private async Task Algo_BubbleS() {

			for (int step = 0; step < (sampleValueRectangleList.Count - 1); ++step) {
				int swapped = 0;
				for (int i = 0; i < (sampleValueRectangleList.Count - step - 1); ++i) {
					await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[i + 1].sampleRectangle);
					if (VisualCompare2(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[i+1].sampleRectangle)) {
						Swap(sampleValueRectangleList[i], sampleValueRectangleList[i + 1]);
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
					await RectangleVisualSelection(sampleValueRectangleList[j].sampleRectangle, sampleValueRectangleList[min_idx].sampleRectangle);
					if (VisualCompare1(sampleValueRectangleList[j].sampleRectangle, sampleValueRectangleList[min_idx].sampleRectangle))
						min_idx = j;
				}
				Swap(sampleValueRectangleList[i], sampleValueRectangleList[min_idx]);
			}
		}



		private async Task Algo_QuickS() {
			await quickSort(sampleValueRectangleList, 0, sampleValueRectangleList.Count - 1);
		}
		async Task<int> partition(List<ValueRectanglePair> arr, int low, int high) {
			int i = low - 1;

			for (int j = low; j < high; j++) {
				await RectangleVisualSelection(arr[j].sampleRectangle);
				if (VisualCompare1(arr[j].sampleRectangle, arr[high].sampleRectangle)) {
					i++;

					Swap(arr[i], arr[j]);
				}
			}
			Swap(arr[i+1], arr[high]); //swap(arr, i + 1, high);
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
				Swap(toAssign, new ValueRectanglePair { sampleValue = newValue, sampleRectangle = toSwap });
			};

			int k = l;
			while (i < n1 && j < n2) {
				await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[j].sampleRectangle);
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
				await RectangleVisualSelection(sampleValueRectangleList[k].sampleRectangle);

				assignTo_sampleValueRectangleList(arr[k], L[i]);//arr[k] = L[i];

				i++;
				k++;
			}

			while (j < n2) {
				await RectangleVisualSelection(sampleValueRectangleList[k].sampleRectangle);

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

			for (int i = arr.Count / 2 - 1; i >= 0; i--)
				await heapify(arr, arr.Count, i);

			for (int i = arr.Count - 1; i > 0; i--) {
				await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[0].sampleRectangle);
				Swap(sampleValueRectangleList[i], sampleValueRectangleList[0]);

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
				await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle, sampleValueRectangleList[largest].sampleRectangle);
				Swap(sampleValueRectangleList[i], sampleValueRectangleList[largest]);

				await heapify(arr, n, largest);
			}
		}

		private async Task Algo_CountingS() {

			Action<ValueRectanglePair, int> assignTo_sampleValueRectangleList = (toAssign, newValue) => {
				Rectangle toSwap = toAssign.sampleRectangle;
				toSwap.Height = (double)newValue * (canvas.ActualHeight / maxSample);
				Swap(toAssign, new ValueRectanglePair { sampleValue = newValue, sampleRectangle = toSwap });
			};

			//int max = arr.Max();
			//int min = arr.Min();
			int range = maxSample - minSample + 1;
			int[] count = new int[range];
			int[] output = new int[sampleValueRectangleList.Count];

			for (int i = 0; i < sampleValueRectangleList.Count; i++) {
				await RectangleVisualSelection(sampleValueRectangleList [i].sampleRectangle);
				count[sampleValueRectangleList[i].sampleValue - minSample]++;
			}
			for (int i = 1; i < count.Length; i++) {
				count[i] += count[i - 1];
			}
			for (int i = sampleValueRectangleList.Count - 1; i >= 0; i--) {
				await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle);
				output[count[sampleValueRectangleList[i].sampleValue - minSample] - 1] = sampleValueRectangleList[i].sampleValue;
				count[sampleValueRectangleList[i].sampleValue - minSample]--;
			}
			if(sortingOrder){
			for (int i = 0; i < sampleValueRectangleList.Count; i++) {
				//arr[i] = output[i];
				await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle);
				assignTo_sampleValueRectangleList(sampleValueRectangleList[i], output[i]);
				
			} }
			else {
				for (int i = sampleValueRectangleList.Count - 1; i >=0 ; i--) {
					//arr[i] = output[i];
					await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle);
					assignTo_sampleValueRectangleList(sampleValueRectangleList[i], output[sampleValueRectangleList.Count-1-i]);

				}
			}
		}



		private async Task Algo_RadixS() {
			if (sortingOrder) {
				for (int exp = 1; maxSample / exp > 0; exp *= 10)
					await countSort_RadixDedicated(sampleValueRectangleList, sampleValueRectangleList.Count, exp);
			}
			else{
				await descendingRadixSort();
			}
		}

		private async Task countSort_RadixDedicated(List<ValueRectanglePair> arr, int n, int exp) {

			Action<ValueRectanglePair, int> assignTo_arr = (toAssign, newValue) => {
				Rectangle toSwap = toAssign.sampleRectangle;
				toSwap.Height = (double)newValue * (canvas.ActualHeight / maxSample);
				Swap(toAssign, new ValueRectanglePair { sampleValue = newValue, sampleRectangle = toSwap });
			};

			int[] output = new int[n];
			int i;
			int[] count = new int[10];

			for (i = 0; i < 10; i++)
				count[i] = 0;

			// Store count of occurrences in count[]
			for (i = 0; i < n; i++){
				await RectangleVisualSelection(arr[i].sampleRectangle);
				count[(arr[i].sampleValue / exp) % 10]++;
			}
			for (i = 1; i < 10; i++)
				count[i] += count[i - 1];

			for (i = n - 1; i >= 0; i--) {
				await RectangleVisualSelection(arr[i].sampleRectangle);
				output[count[(arr[i].sampleValue / exp) % 10] - 1] = arr[i].sampleValue;
				count[(arr[i].sampleValue / exp) % 10]--;
			}

			if(sortingOrder){
				for (i = 0; i < n; i++){
					await RectangleVisualSelection(arr[i].sampleRectangle);
					assignTo_arr(arr[i], output[i]);//arr[i] = output[i];
				} 
			}
			else {
				for (i = n-1 ; i >=0; i--) {
					await RectangleVisualSelection(arr[i].sampleRectangle);
					assignTo_arr(arr[i], output[n-1-i]);//arr[i] = output[i];
				}
			}
		}
		private async Task descendingRadixSort() {
			Action<ValueRectanglePair, int> assignTo_arr = (toAssign, newValue) => {
				Rectangle toSwap = toAssign.sampleRectangle;
				toSwap.Height = (double)newValue * (canvas.ActualHeight / maxSample);
				Swap(toAssign, new ValueRectanglePair { sampleValue = newValue, sampleRectangle = toSwap });
			};

			int i, exp = 1;
			int[] b = new int[sampleValueRectangleList.Count];

			while (maxSample / exp > 0) {
				int[] bucket = new int[10];
				for (i = 0; i < 10; i++)
					bucket[i] = 0;

				for (i = 0; i < sampleValueRectangleList.Count; i++){
					await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle);
					bucket[9 - sampleValueRectangleList[i].sampleValue / exp % 10]++; }
				for (i = 1; i < 10; i++)
					bucket[i] += bucket[i - 1];
				for (i = sampleValueRectangleList.Count - 1; i >= 0; i--){
					await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle);
					b[--bucket[9 - sampleValueRectangleList[i].sampleValue / exp % 10]] = sampleValueRectangleList[i].sampleValue; }
				for (i = 0; i < sampleValueRectangleList.Count; i++) {
					await RectangleVisualSelection(sampleValueRectangleList[i].sampleRectangle);
					assignTo_arr(sampleValueRectangleList[i], b[i]);//a[i] = b[i];
				}
				exp *= 10;
			}
		}



		private async Task Algo_BitonicS() {
			//await bitonicSort(sampleValueRectangleList, 0, sampleValueRectangleList.Count, sortingOrder ? 1 : 0 );

			for (int k = 2; k <= sampleValueRectangleList.Count; k *= 2) // k is doubled every iteration
				for (int j = k / 2; j > 0; j /= 2) // j is halved at every iteration, with truncation of fractional parts
					for (int i = 0; i < sampleValueRectangleList.Count; i++){
						int l = i ^ j; // in C-like languages this is "i ^ j"
						if (l > i)
							if (((i & k) == 0) && (sampleValueRectangleList[i].sampleValue > sampleValueRectangleList[l].sampleValue) ||
								   ((i & k) != 0) && (sampleValueRectangleList[i].sampleValue < sampleValueRectangleList[l].sampleValue))
								Swap(sampleValueRectangleList[i], sampleValueRectangleList[l]);
				}
		}
		/*
		async Task compAndSwap(List<ValueRectanglePair> a, int i, int j, int dir) {
			int k;
			await RectangleVisualSelection(a[i].sampleRectangle, a[j].sampleRectangle);
			if (a[i].sampleRectangle.Height > a[j].sampleRectangle.Height)
				k = 1;
			else
				k = 0;

			if (dir == k)
				Swap(a[i], a[j]);
		}
		*/

		private async Task bitonicMerge(List<ValueRectanglePair> a, int low, int cnt, int dir) {
			if (cnt > 1) {
				int k = cnt / 2;

				for (int i = low; i < low + k; i++){
					await RectangleVisualSelection(a[i].sampleRectangle, a[i+k].sampleRectangle);
					//await compAndSwap(a, i, i + k, dir); 
					if ( dir == ((a[i].sampleValue > a[i+k].sampleValue)?1:0))
						Swap(a[i], a[i+k]);
				}

				await bitonicMerge(a, low, k, dir);
				await bitonicMerge(a, low + k, k, dir);
			}
		}


		private async Task bitonicSort(List<ValueRectanglePair> a, int low, int cnt, int dir) {
			if (cnt > 1) {
				int k = cnt / 2;

				await bitonicSort(a, low, k, 1);

				await bitonicSort(a, low + k, k, 0);

				await bitonicMerge(a, low, cnt, dir);
			}
		}

	}
}
