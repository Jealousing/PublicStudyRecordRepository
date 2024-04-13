# 힙 정렬 (HeapSort)

## 힙 정렬이란?
 힙 정렬은 **[힙(heap) 자료구조를][Heaplink]** 활용하여 정렬하는 알고리즘 중 하나입니다. 배열을 최대 힙(max heap) 또는 최소 힙(min heap)으로 만든 후, 최대값(또는 최소값)을 반복적으로 제거하여 정렬합니다.

### 힙 정렬의 동작 원리
1. 주어진 배열을 최대 힙(max heap)으로 만듭니다.
2. 최대 힙에서 최대값(루트)을 제거하고 배열의 끝으로 이동시킵니다. 그 후, 힙 속성을 복원합니다.
3. 위 과정을 반복하여 힙이 빌 때까지 정렬합니다.

## 힙 정렬의 특징
 ### 장점
* 일관된 성능: 모든 경우에 시간 복잡도가 O(n log n)입니다.
* 메모리 사용량이 적음: 추가적인 메모리 공간이 필요하지 않습니다.( 제자리 정렬)
 ### 단점
* 상대적으로 구현이 복잡하고 평균적으로 퀵정렬과 병합정렬보다 상대적으로 느린 속도를 보입니다.
* 불안정한 정렬 : 같은 값의 원소들이 정렬 과정에서 상대적인 순서가 변경될 수 있습니다.

 
## 힙 정렬의 시간 복잡도
* 최선, 평균, 최악의 경우 모두 O(n log n)입니다.
 
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

// 힙 조건을 복원하는 함수
void heapify(int arr[], int n, int i) 
{
    int largest = i;    // 루트를 가장 큰 값으로 설정
    int left = 2 * i + 1;   // 왼쪽 자식 노드 인덱스
    int right = 2 * i + 2;  // 오른쪽 자식 노드 인덱스

    // 왼쪽 자식이 루트보다 큰 경우
    if (left < n && arr[left] > arr[largest])
        largest = left;

    // 오른쪽 자식이 루트보다 큰 경우
    if (right < n && arr[right] > arr[largest])
        largest = right;

    // 루트가 가장 큰 값이 아니면 교환
    if (largest != i) 
    {
        swap(arr[i], arr[largest]);
        // 변경된 서브트리에 대해 재귀로 heapify 호출
        heapify(arr, n, largest);
    }
}

// 힙 정렬 함수
void heapSort(int arr[], int n) 
{
    // 최대 힙 구축
    for (int i = n / 2 - 1; i >= 0; i--)
        heapify(arr, n, i);

    // 힙에서 요소 하나씩 제거하면서 정렬
    for (int i = n - 1; i > 0; i--) 
    {
        swap(arr[0], arr[i]);   // 최대값(루트)을 배열 끝으로 이동
        heapify(arr, i, 0);     // 힙 속성 복원
    }
}

int main() 
{
    int arr[] = {5, 3, 8, 4, 9, 1, 6, 2, 7};
    int n = sizeof(arr) / sizeof(arr[0]);

    heapSort(arr, n);

    cout << "Sorted array: \n";
    for (int i = 0; i < n; i++)
        cout << arr[i] << " ";
    cout << endl;
    return 0;
}
```

### 실행결과
힙 정렬 알고리즘의 작동 과정은 다음과 같습니다
1. 최대 힙 구축: 배열의 중간부터 시작하여 루트까지 역순으로 heapify 함수를 호출합니다. 이렇게 하면 가장 큰 값이 루트에 위치하게 됩니다.
2. 정렬: 배열의 마지막 요소와 루트를 교환합니다. 이렇게 하면 현재 최대값이 배열의 맨 끝에 위치하게 됩니다. 그런 다음 힙 크기를 하나 줄이고 heapify를 루트에 대해 호출합니다. 이 과정을 반복하면 배열이 정렬됩니다.

위의 코드에서 주어진 배열 {5, 3, 8, 4, 9, 1, 6, 2, 7}에 대한 힙 정렬 과정은 다음과 같습니다

1. 최대 힙 구축: {9, 8, 6, 7, 3, 1, 5, 2, 4}
2. 정렬 첫 번째 반복: {8, 7, 6, 4, 3, 1, 5, 2, 9}
3. 정렬 두 번째 반복: {7, 4, 6, 2, 3, 1, 5, 8, 9}
4. 정렬 세 번째 반복: {6, 4, 5, 2, 3, 1, 7, 8, 9}
5. 정렬 네 번째 반복: {5, 4, 1, 2, 3, 6, 7, 8, 9}
6. 정렬 다섯 번째 반복: {4, 3, 1, 2, 5, 6, 7, 8, 9}
7. 정렬 여섯 번째 반복: {3, 2, 1, 4, 5, 6, 7, 8, 9}
8. 정렬 일곱 번째 반복: {2, 1, 3, 4, 5, 6, 7, 8, 9}
9. 정렬 여덟 번째 반복: {1, 2, 3, 4, 5, 6, 7, 8, 9}
따라서, 최종적으로 정렬된 배열은 {1, 2, 3, 4, 5, 6, 7, 8, 9}

## 힙 정렬의 사용 예시 및 코딩 테스트 유형
* 힙 정렬은 대량의 데이터를 정렬할 때 효과적입니다.
* 코딩 테스트에서도 힙 정렬은 많이 사용되는 정렬 알고리즘 중 하나입니다.

[Heaplink]: https://github.com/Jealousing/PublicStudyRecordRepository/tree/main/ComputerScience/DataStructure_Algorithm_CodingTest/DataStructures/Tree/Heap