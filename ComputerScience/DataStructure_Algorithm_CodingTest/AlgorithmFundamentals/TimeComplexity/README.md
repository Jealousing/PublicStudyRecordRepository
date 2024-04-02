# 시간 복잡도 (Time Complexity)

## 시간복잡도란?
 시간 복잡도는 알고리즘이 입력 크기에 따라 실행되는 속도를 나타내는 지표입니다.    
알고리즘의 효율성을 분석하고 예측하는 데 사용됩니다.

## 시간복잡도의 특징
* 입력 크기에 따른 알고리즘 실행 속도의 증가율을 나타냄
* 일반적으로 Big O 표기법을 사용하여 표현됨
* 입력 크기에 비례하여 실행 시간이 증가하는 경향이 있음
* 알고리즘의 최선, 평균, 최악의 경우를 고려하여 분석하는 것이 중요합니다.

## 시간복잡도 표현방법
주로 다음과 같은 형태(Big O 표기법)로 표현됩니다

* O(1) : 상수 시간 복잡도 : 입력 크기에 관계없이 일정한 실행 시간을 갖는 알고리즘   
* O(log n) : 로그 시간 복잡도 : 입력 크기의 로그에 비례하여 실행 시간이 증가하는 알고리즘
* O(n) : 선형 시간 복잡도 : 입력 크기에 비례하여 실행 시간이 선형적으로 증가하는 알고리즘 
* O(n log n) : 선형 로그 시간 복잡도 : 입력 크기와 로그 값의 곱에 비례하여 실행 시간이 증가하는 알고리즘 
* O(n^2) : 이차 시간 복잡도 : 입력 크기의 제곱에 비례하여 실행 시간이 증가하는 알고리즘   
* O(2^n) : 지수 시간 복잡도 : 입력 크기에 대해 지수 함수의 형태로 실행 시간이 증가하는 알고리즘   

## 실행시간이 아닌 연산 횟수를 수치로 판별하는 이유?
 실행 시간은 컴퓨터의 하드웨어, 프로그래밍 언어 및 컴파일러 등의 요소에 의해 영향을 받을 수 있습니다.    
따라서 알고리즘의 효율성을 판별하기 위해 연산 횟수를 기준으로 삼는 것이 더 안정적입니다.

## 최선,최악,평균

* 최선의 경우 (Best Case): 알고리즘이 가장 빠르게 동작하는 경우를 나타냅니다. 이 경우는 일반적으로 입력이 이미 정렬되어 있는 경우나 특정 조건이 이미 만족되어 있는 경우와 같이 최상의 조건이 주어진 경우입니다. 최선의 경우 시간 복잡도는 주로 알고리즘이 실행되는 각 단계에서 최소한의 작업만 수행해도 되는 경우를 의미합니다.

* 최악의 경우 (Worst Case): 알고리즘이 가장 느리게 동작하는 경우를 나타냅니다. 이 경우는 일반적으로 입력이 역으로 정렬되어 있거나 최악의 조건이 주어진 경우입니다. 최악의 경우 시간 복잡도는 주로 알고리즘이 실행되는 각 단계에서 최대한의 작업을 수행해야 하는 경우를 의미합니다.

* 평균의 경우 (Average Case): 알고리즘이 다양한 입력에 대해 평균적으로 어떻게 동작하는지를 나타냅니다. 이 경우는 일반적으로 입력이 무작위로 주어지는 경우나 알고리즘이 다양한 조건에 따라 다른 동작을 할 수 있는 경우를 고려합니다. 평균의 경우 시간 복잡도는 알고리즘이 여러 입력에 대해 실행되는 각 단계에서 예상되는 작업량을 의미합니다.
  
## 예시코드

* O(1) : 상수 시간 복잡도
```cpp
void accessElement(int arr[], int index) 
{
    cout << "Element at index " << index << " is: " << arr[index] << endl;
}

int main() 
{
    int arr[] = {1, 2, 3, 4, 5};
    accessElement(arr, 2); // 상수 시간 내에 접근
    return 0;
}
```

* O(log n) : 로그 시간 복잡도
```cpp
int binarySearch(int arr[], int left, int right, int target) 
{
    while (left <= right) 
    {
        int mid = left + (right - left) / 2;
        if (arr[mid] == target)
            return mid;
        if (arr[mid] < target)
            left = mid + 1;
        else
            right = mid - 1;
    }
    return -1; // 찾지 못한 경우
}
```

* O(n) : 선형 시간 복잡도
```cpp
void linearSearch(int arr[], int n, int target) 
{
    for (int i = 0; i < n; ++i) 
    {
        if (arr[i] == target) 
        {
            cout << "Element found at index: " << i << endl;
            return;
        }
    }
    cout << "Element not found" << endl;
}
```

* O(n log n) : 선형 로그 시간 복잡도
```cpp
#include <algorithm>
void sortArray(int arr[], int n) 
{
    sort(arr, arr + n); // 퀵 정렬 등의 알고리즘을 사용
}
```

* O(n^2) : 이차 시간 복잡도
```cpp
void bubbleSort(int arr[], int n) 
{
    for (int i = 0; i < n - 1; ++i) 
    {
        for (int j = 0; j < n - i - 1; ++j) 
        {
            if (arr[j] > arr[j + 1]) 
            {
                int temp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = temp;
            }
        }
    }
}
```

* O(2^n) : 지수 시간 복잡도
```cpp
int fibonacci(int n) 
{
    if (n <= 1)
        return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}
```
 
