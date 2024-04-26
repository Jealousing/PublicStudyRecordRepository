# 분할 정복 알고리즘 (Divide and Conquer)

## 분할 정복 알고리즘이란?
 분할 정복 알고리즘은 주어진 문제를 더 작은 부분으로 나누고(divide), 각 부분을 각각 해결한 다음에 이를 다시 합쳐서(conquer) 최종적인 해답을 얻는 방법입니다. 이 알고리즘은 대개 재귀적인 방법으로 구현되며, 문제를 더 작은 부분으로 분할하여 각각을 해결함으로써 전체 문제를 해결합니다.

## 분할 정복 알고리즘의 단계
1. **분할(Divide):** 주어진 문제를 더 작은 부분으로 나눕니다. 이 단계에서 문제는 여러 부분으로 나누어집니다.
2. **정복(Conquer):** 나누어진 작은 부분 문제를 각각 재귀적으로 해결합니다. 이 단계에서 각각의 부분 문제는 더 이상 나눌 수 없을 때까지 분할 정복 알고리즘을 적용합니다.
3. **결합(Combine):** 각각의 작은 부분 문제의 해답을 결합하여 전체 문제의 해답을 구합니다.

## 분할 정복 알고리즘의 예시
* **병합 정렬(Merge Sort):** 배열을 반으로 나누어 각각을 정렬하고, 이후에 정렬된 배열을 합치는 과정을 반복하여 전체 배열을 정렬합니다.
* **퀵 정렬(Quick Sort):** 배열을 기준 원소를 기준으로 작은 원소와 큰 원소로 나누어 정렬하는 방식을 사용합니다.
* **이진 탐색(Binary Search):** 정렬된 배열에서 원하는 값을 찾는 과정을 반복하여 찾아냅니다.
 
## 분할 정복 알고리즘의 예시로 퀵정렬을 구현한 예시코드 c++
```cpp
#include <iostream>
using namespace std;

// 배열 내 두 요소의 값을 교환하는 함수
void swap(int& a, int& b) 
{
    int temp = a;
    a = b;
    b = temp;
}

// 파티션 함수(분할 함수): 피벗을 기준으로 그룹나누기
int Partition(int arr[], int low, int high) 
{
    int pivot = arr[high];
    int i = low - 1;

    for (int j = low; j < high; j++) 
    {
        if (arr[j] < pivot) 
        {
            i++;
            swap(arr[i], arr[j]);
        }
    }

    swap(arr[i + 1], arr[high]);
    return i + 1;
}

// 퀵 정렬 함수
void QuickSort(int arr[], int low, int high) 
{
    if (low < high) 
    {
        int pi = Partition(arr, low, high);

        QuickSort(arr, low, pi - 1);
        QuickSort(arr, pi + 1, high);
    }
}

int main() 
{
    int arr[] = {10, 7, 8, 9, 1, 5};
    int n = sizeof(arr) / sizeof(arr[0]);

    QuickSort(arr, 0, n - 1);

    cout << "Sorted array: \n";
    for (int i = 0; i < n; i++)
        cout << arr[i] << " ";
    cout << endl;
    return 0;
}
```