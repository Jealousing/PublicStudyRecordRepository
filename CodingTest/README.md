# 코딩테스트 관련 폴더
   
## 코딩테스트 정답 저장을 위해 만들어진 폴더입니다.

### 구조
  코딩 문제의 분류에 따라 폴더로 나눠져 있고 각 폴더는 특정 주제나 알고리즘 집합을 나타냅니다.
 각 폴더 내에서 해당 코딩 문제 번호에 대한 제가 풀었던 코드가 있습니다.

### 목적
 이 폴더의 주 목적은 코딩 문제를 해결하는 과정을 작성하고 문제 해결 능력을 보여주는 것입니다.
저는 잘 정리된 폴더를 유지함으로써 다양한 알고리즘, 데이터 구조, 프로그래밍 개념에 대한 이해를 보여주고자 합니다. 

### 알고리즘 간단 정리

#### 정렬

<details>
 <summary><b><em> 선택 정렬(Selection Sort) </em></b> </summary>

 가장 작거나 큰 원소를 선택하여 정해진 위치에 정렬하는 알고리즘이다

 ``` C++

void selectionSort(int arr[], int n)
{
    for (int i = 0; i < n - 1; i++) 
    {
        int minIndex = i;  // 현재 인덱스를 최소값으로 가정

        // 남은 정렬되지 않은 부분에서 최소 원소의 인덱스를 찾기
        for (int j = i + 1; j < n; j++)
        {
            if (arr[j] < arr[minIndex]) 
            {
                minIndex = j;
            }
        }

        // 최소 원소를 정렬되지 않은 부분의 가장 왼쪽 원소와 교환
        if (minIndex != i) 
        {
            int temp = arr[i];
            arr[i] = arr[minIndex];
            arr[minIndex] = temp;
        }
    }
}

 ```


 </details>


<details>
 <summary><b><em> 삽입 정렬(Insertion Sort) </em></b> </summary>

 각 원소를 이미 정렬된 부분에 삽입하는 알고리즘.

 ``` C++

void insertionSort(int arr[], int n) 
{
    for (int i = 1; i < n; i++)
    {
        int temp = arr[i];
        int j = i - 1;

        while (j >= 0 && arr[j] > temp)
        {
            arr[j + 1] = arr[j];
            j--;
        }
        arr[j + 1] = temp;
    }
}

```

 </details>

<details>
 <summary><b><em> 거품 정렬(Bubble Sort) </em></b> </summary>

 서로 인접한 두 원소의 크기를 비교하고 조건에 맞지 않다면 교환하며 정렬하는 알고리즘이다.

 ``` C++

void bubbleSort(int arr[], int n) 
{
    for (int i = 0; i < n - 1; i++) 
    {
        for (int j = 0; j < n - i - 1; j++) 
        {
            if (arr[j] > arr[j + 1]) 
            {
                // 인접한 두 원소를 비교하여 정렬
                int temp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = temp;
            }
        }
    }
}

 ```
 
 </details>

<details>
 <summary><b><em> 병합 정렬(Merge Sort) </em></b> </summary>

 병합 정렬 알고리즘은 배열을 반으로 나눈 후 각 부분을 정렬하고 병합하여 정렬

 ``` C++

void merge(int arr[], int left, int middle, int right) 
{
    int n1 = middle - left + 1, n2 = right - middle;
    int leftArr[n1], rightArr[n2];

    for (int i = 0; i < n1; i++) leftArr[i] = arr[left + i];
    for (int i = 0; i < n2; i++) rightArr[i] = arr[middle + 1 + i];

    int i = 0,  j = 0, k = left;

    while (i < n1 && j < n2) 
    {
        if (leftArr[i] <= rightArr[j]) arr[k++] = leftArr[i++];
        else arr[k++] = rightArr[j++];
    }

    while (i < n1) arr[k++] = leftArr[i++];
    while (j < n2) arr[k++] = rightArr[j++]; 
}

void mergeSort(int arr[], int left, int right) 
{
    if (left < right) 
    {
        int middle = left + (right - left) / 2;
        mergeSort(arr, left, middle);
        mergeSort(arr, middle + 1, right);
        merge(arr, left, middle, right);
    }
}

 ```

 </details>

<details>
 <summary><b><em> 퀵 정렬(Quick Sort) </em></b> </summary>

 배열을 빠르게 분할 정복 방식으로 정렬하는 알고리즘.
 분할정복: 문제를 작은 2개의 문제로 분리하고 해결 후 결과를 모아서 다시 문제를 해결하는 전략.

 ``` C++

int partition(int arr[], int left, int right) 
{
    int pivot = arr[right];
    int i = left - 1;

    for (int j = left; j < right; j++) 
    {
        if (arr[j] < pivot) 
        {
            i++;
            swap(arr[i], arr[j]);
        }
    }
    swap(arr[i + 1], arr[right]);
    return i + 1;
}

void quickSort(int arr[], int left, int right) 
{
    if (left < right) 
    {
        int pivotIndex = partition(arr, left, right);

        quickSort(arr, left, pivotIndex - 1);
        quickSort(arr, pivotIndex + 1, right);
    }
}

 ```

 </details>

<details>
 <summary><b><em> 힙 정렬(Heap Sort) </em></b> </summary>

 힙(Heap) 자료구조를 사용하여 배열을 정렬하는 비교 기반 정렬 알고리즘

 ``` C++

void heapify(int arr[], int n, int i) 
{
    int largest = i, left = 2 * i + 1, right = 2 * i + 2;

    if (left < n && arr[left] > arr[largest]) largest = left;
    if (right < n && arr[right] > arr[largest]) largest = right;
    
    if (largest != i) 
    {
        swap(arr[i], arr[largest]);
        heapify(arr, n, largest);
    }
}

void heapSort(int arr[], int n) 
{
    for (int i = n / 2 - 1; i >= 0; i--) heapify(arr, n, i);
    for (int i = n - 1; i >= 0; i--) 
    {
        swap(arr[0], arr[i]);
        heapify(arr, i, 0);
    }
}

 ```

 </details>

<details>
 <summary><b><em> 기수 정렬(Radix sort) </em></b> </summary>

 데이터를 구성하는 기본 요소(라디스)를 이용하여 정렬하는 정렬 알고리즘 중 하나입니다. 
 이 알고리즘은 숫자 키를 각 자릿수별로 그룹화하여 정렬하는 정수 정렬 알고리즘입니다.

 ``` C++

int findMax(int arr[], int n)
{
    int max = arr[0];
    for (int i = 1; i < n; i++) 
    {
        if (arr[i] > max) max = arr[i];
    }
    return max;
}

void countingSort(int arr[], int n, int exp) 
{
    int base = 10;
    int output[n], count[base] = { 0 };

    // 현재 자릿수를 기준으로 각 숫자의 등장 횟수를 센다
    for (int i = 0; i < n; i++) count[(arr[i] / exp) % base]++;

    // 누적합
    for (int i = 1; i < base; i++) count[i] += count[i - 1];
    
    // output 배열을 구성하여 count 배열을 사용해 정렬한다
    for (int i = n - 1; i >= 0; i--) 
    {
        output[count[(arr[i] / exp) % base] - 1] = arr[i];
        count[(arr[i] / exp) % base]--;
    }

    // output 배열을 다시 원래 배열에 복사한다
    for (int i = 0; i < n; i++) arr[i] = output[i]; 
}

void radixSort(int arr[], int n) 
{
    int max = findMax(arr, n);
    for (int exp = 1; max / exp > 0; exp *= 10) countingSort(arr, n, exp);
}

 ```

 </details>

<details>
 <summary><b><em> 계수 정렬(Counting Sort) </em></b> </summary>

 정수나 정수 형태의 키를 가진 데이터를 정렬하는 비교 기반 정렬 알고리즘 중 하나

 ``` C++

void countingSort(int arr[]) 
{
    int n = sizeof(arr) / sizeof(arr[0]);

    // 최댓값을 찾아 범위를 확인
    int max = arr[0];
    for (int i = 1; i < n; i++)
    {
        if (arr[i] > max) max = arr[i]; 
    }

    // 빈도수를 저장하기 위한 카운트 동적 배열을 생성하고 초기화
    int* count = new int[max + 1]();

    // 각 요소의 빈도수 체크
    for (int i = 0; i < n; i++)
    {
        count[arr[i]]++;
    }

    // 정렬
    int index = 0;
    for (int i = 0; i <= max; i++) 
    {
        while (count[i] > 0) 
        {
            arr[index++] = i;
            count[i]--;
        }
    }

    delete[] count;  // 동적 배열을 해제
}

 ```

 </details>

#### 탐색

<details>
 <summary><b><em> 이분 탐색(Binary Search) </em></b> </summary>

 배열을 반으로 나누어 탐색 범위를 줄여나가는 알고리즘 중 하나

 ``` C++

// arr 배열에서 target을 탐색
int binarySearch(int arr[], int left, int right, int target) 
{
    while (left <= right) 
    {
        int mid = left + (right - left) / 2;
        if (arr[mid] == target) return mid;
        if (arr[mid] < target) left = mid + 1;
        else right = mid - 1;
    }
    return -1;
}

 ```

 </details>

<details>
 <summary><b><em> 해시 탐색(Hash Search) </em></b> </summary>

 해시 테이블을 사용하여 키(key)를 해시값(hash)에 매핑하고 해당 해시값을 인덱스로 사용하여 데이터를 검색

 ``` C++

#include <iostream>
#include <unordered_map>
using namespace std;

int main()
{
    // 해시 테이블 생성
    unordered_map<string, int> hashTable;

    // 데이터 추가
    hashTable["사과"] = 1000;
    hashTable["바나나"] = 1500;
    hashTable["딸기"] = 2000;
    hashTable["포도"] = 2500;

    // 검색할 데이터
    string findValue = "딸기";

    // 검색
    if (hashTable.find(findValue) != hashTable.end())  cout << findValue << "의 가격: " << hashTable[findValue] << "원" << endl;
    else cout << findValue << "을(를) 찾을 수 없음." << endl;

    return 0;
}

 ```

 </details>

#### 완전 탐색

브루트 포스
백트래킹
재귀
DFS
BFS