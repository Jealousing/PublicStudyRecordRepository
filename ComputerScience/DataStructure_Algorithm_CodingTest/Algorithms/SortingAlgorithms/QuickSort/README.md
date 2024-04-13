# 퀵 정렬 (QuickSort)

## 퀵 정렬이란?
퀵 정렬은 분할 정복(divide and conquer) 방법을 사용하여 정렬하는 알고리즘 중 하나입니다. 배열을 피벗(pivot)을 기준으로 두 개의 하위 배열로 분할하고, 각각의 하위 배열을 재귀적으로 정렬하여 전체 배열을 정렬합니다.

### 퀵 정렬의 동작 원리
1. 분할(Divide): 배열에서 하나의 원소를 피벗(pivot)으로 선택합니다.
2. 파티션(Partition): 피벗을 기준으로 배열을 두 그룹으로 나눕니다. 피벗보다 작은 원소들은 피벗의 왼쪽에 위치하고, 큰 원소들은 오른쪽에 위치합니다. 피벗은 자신의 최종 위치에 들어가게 됩니다.
3. 정복(Conquer): 각 그룹에 대해 재귀적으로 위의 과정을 반복합니다.
4. 결합(Combine): 더 이상 나눌 수 없을 때까지 반복한 후, 최종적으로 정렬된 배열을 얻습니다.

## 퀵 정렬의 특징
 ### 장점
* 평균적으로 매우 빠른 속도: 퀵 정렬은 평균적으로 O(n log n)의 시간 복잡도를 가지며, 빠른 정렬 속도를 보장합니다. 또한,  특히 입력 배열이 무작위로 섞여있는 경우에 빠르게 동작합니다.
* 제자리(in-place) 정렬: 추가적인 메모리 공간이 필요하지 않습니다.
 
 ### 단점
* 최악의 경우 시간 복잡도가 O(n^2): 피벗의 선택이나 입력 배열의 상태에 따라 최악의 경우 성능이 저하될 수 있습니다. 랜덤화된 퀵 정렬 알고리즘을 사용하여 이러한 문제를 완화할 수 있습니다.
* 피벗의 선택에 따라 성능 차이: 피벗의 선택이 정렬 속도에 영향을 미치므로, 효율적인 피벗 선택 알고리즘이 필요합니다.
 
## 퀵 정렬의 시간 복잡도
* 최선 시간 복잡도: O(n log n)입니다. 이는 피벗이 중간값을 선택하는 경우에 해당합니다.
* 평균 시간 복잡도: O(n log n)입니다. 피벗이 균등하게 나누는 경우에 해당합니다.
* 최악 시간 복잡도: O(n^2)입니다. 피벗이 항상 최소값 또는 최대값을 선택하는 경우에 해당합니다.
 
## 예시코드 c++
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

// 파티션 함수: 피벗을 기준으로 그룹나누기
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

### 실행결과

#### {10, 7, 8, 9, 1, 5}이 퀵정렬로 정렬되는 과정
1. 처음 배열: {10, 7, 8, 9, 1, 5}
 * 피벗: 5
 * 왼쪽 배열: {1} 
 * 오른쪽 배열: {10, 7, 8, 9} 

2. 오른쪽 배열: {10, 7, 8, 9}
 * 피벗: 9
 * 왼쪽 배열: {7, 8} 
 * 오른쪽 배열: {10} 

3. 왼쪽 배열: {7, 8}
 * 피벗: 8
 * 왼쪽 배열: {7}
 * 오른쪽 배열: {}

4. 결과: {1, 5, 7, 8, 9, 10}

#### {5, 3, 8, 4, 9, 1, 6, 2, 7}이 퀵정렬로 정렬되는 과정
1. 처음 배열: {5, 3, 8, 4, 9, 1, 6, 2, 7}
 * 피벗: 7
 * 왼쪽 배열: {5, 3, 4, 1, 6, 2} 
 * 오른쪽 배열: {8, 9}

2. 왼쪽 배열: {5, 3, 4, 1, 6, 2}
 * 피벗: 2
 * 왼쪽 배열: {1} 
 * 오른쪽 배열: {5, 3, 4, 6}

3. 오른쪽 배열: {5, 3, 4, 6}
 * 피벗: 6
 * 왼쪽 배열: {5, 3, 4} 
 * 오른쪽 배열: {}

4. 왼쪽 배열: {5, 3, 4}
 * 피벗: 4
 * 왼쪽 배열: {3} 
 * 오른쪽 배열: {5}

5. 결과: {1, 2, 3, 4, 5, 6, 7, 8, 9}
 
## 퀵 정렬의 사용 예시 및 코딩 테스트 유형
* 퀵 정렬은 대부분의 정렬 상황에서 사용할 수 있으며, 특히 대량의 데이터를 빠르게 정렬해야 할 때 적합합니다.
* 코딩 테스트에서도 퀵 정렬은 많이 사용되는 정렬 알고리즘 중 하나이며, 구현이 비교적 간단하기 때문에 자주 등장합니다.

## 퀵 정렬의 변형
3-way 퀵 정렬: 피벗을 중심으로 작은, 같은, 큰 값을 갖는 세 그룹으로 나누어 정렬하는 방식입니다. 이를 통해 동일한 값들의 묶음이 많은 경우에도 효율적으로 정렬할 수 있습니다.

### 3-way 퀵 정렬 예시코드 c++
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

// 3-way 퀵 정렬을 수행하는 함수
void QuickSort3Way(int arr[], int low, int high) 
{
    if (low >= high) return;

    int pivot = arr[low]; // 피벗은 배열의 첫 번째 요소로 선택
    int lt = low; // 피벗보다 작은 요소들의 끝 인덱스
    int gt = high; // 피벗보다 큰 요소들의 시작 인덱스
    int i = low + 1; // 현재 탐색 중인 인덱스

    // 배열을 피벗보다 작은 값, 같은 값, 큰 값으로 분할
    while (i <= gt) 
    {
        if (arr[i] < pivot) 
        {
            swap(arr[i], arr[lt]);
            lt++;
            i++;
        } 
        else if (arr[i] > pivot) 
        {
            swap(arr[i], arr[gt]);
            gt--;
        } 
        else 
        {
            i++;
        }
    }

    // 재귀적으로 피벗보다 작은 값과 큰 값들을 각각 정렬
    QuickSort3Way(arr, low, lt - 1);
    QuickSort3Way(arr, gt + 1, high);
}

int main() 
{
    int arr[] = {10, 7, 8, 9, 1, 5, 5, 5, 3, 2};
    int n = sizeof(arr) / sizeof(arr[0]);

    QuickSort3Way(arr, 0, n - 1);

    cout << "Sorted array: \n";
    for (int i = 0; i < n; i++)
        cout << arr[i] << " ";
    cout << endl;
    return 0;
}
```