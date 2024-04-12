# 병합 정렬 (Merge Sort)

## 병합 정렬이란?
병합 정렬은 분할 정복(divide and conquer) 알고리즘의 하나로, 배열을 반으로 나눈 뒤 각각을 정렬하고, 그 결과를 합하여 전체를 정렬하는 방식입니다. 이는 배열의 크기가 0 또는 1이 될 때까지 재귀적으로 반복됩니다.

		분할 정복(divide and conquer)이란? : 문제를 작은 문제로 분할하여 문제를 해결하는 방법

### 병합 정렬의 동작 원리
1. 배열을 반으로 나눕니다.
2. 각 하위 배열을 병합 정렬을 사용하여 정렬합니다.
3. 정렬된 하위 배열을 병합하여 하나의 정렬된 배열로 만듭니다.

## 병합 정렬의 특징
 ### 장점
 * 안정적인 정렬 방법으로 모든 경우에 대해 일정한 시간을 소요합니다.
 * 대부분의 경우 퀵 정렬보다 빠르게 동작합니다.
 * 배열의 분할 및 병합 과정에서 추가적인 메모리를 사용하므로, 데이터 이동이 필요하지 않습니다.

 ### 단점
 * 추가적인 메모리 공간이 필요하다는 점에서 메모리 사용량이 크다고 볼 수 있습니다.
 * 재귀 호출에 의해 스택 오버플로우가 발생할 수 있습니다.
 
## 병합 정렬의 시간 복잡도
 병합 정렬의 시간 복잡도는 항상 O(n log n)입니다. 이는 배열을 반으로 나누는 데 O(log n)이 필요하고, 각 단계에서 O(n)의 병합 작업을 수행하기 때문입니다.

## 예시코드 c++
```cpp
#include <iostream>

using namespace std;

// 두 개의 정렬된 하위 배열을 병합하는 함수
void merge(int arr[], int left, int middle, int right) 
{
    int n1 = middle - left + 1;
    int n2 = right - middle;

    int L[n1], R[n2];

    for (int i = 0; i < n1; i++)
        L[i] = arr[left + i];
    for (int j = 0; j < n2; j++)
        R[j] = arr[middle + 1 + j];

    int i = 0, j = 0, k = left;

    while (i < n1 && j < n2) 
    {
        if (L[i] <= R[j]) 
        {
            arr[k] = L[i];
            i++;
        } 
        else 
        {
            arr[k] = R[j];
            j++;
        }
        k++;
    }

    while (i < n1) 
    {
        arr[k] = L[i];
        i++;
        k++;
    }

    while (j < n2) 
    {
        arr[k] = R[j];
        j++;
        k++;
    }
}

// 병합 정렬을 수행하는 함수
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

int main() 
{
    int arr[] = {12, 11, 13, 5, 6, 7};
    int arr_size = sizeof(arr) / sizeof(arr[0]);

    cout << "Given array is \n";
    for (int i = 0; i < arr_size; i++)
        cout << arr[i] << " ";
    cout << endl;

    mergeSort(arr, 0, arr_size - 1);

    cout << "Sorted array is \n";
    for (int i = 0; i < arr_size; i++)
        cout << arr[i] << " ";
    cout << endl;
    return 0;
}
```
## 병합 정렬의 사용 예시 & 코딩 테스트 유형
* 코딩 테스트에서 배열을 정렬해야 할 때 병합 정렬은 안정적이고 효율적인 구현으로 인해 자주 사용됩니다.

## 알고리즘 변형
* 3-way 병합 정렬: 병합 단계에서 두 개가 아닌 세 개의 하위 배열을 병합합니다. 이는 대량의 데이터를 처리할 때 효과적입니다.