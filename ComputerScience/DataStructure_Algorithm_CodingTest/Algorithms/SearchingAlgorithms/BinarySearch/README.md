# 이진 탐색(Binary Search)

## 이진 탐색이란?
 이진 탐색(Binary Search)은 정렬된 배열에서 특정한 값을 찾기 위해 사용되는 알고리즘으로, 배열의 중간 요소와 비교하여 탐색 범위를 반으로 줄여가며 값을 찾습니다.

### 이진 탐색의 동작 원리
1. 배열이나 리스트를 정렬합니다.
2. 배열의 중간 요소를 선택합니다.
3. 중간 요소와 찾고자 하는 값을 비교합니다.
4. 만약 중간 요소가 찾고자 하는 값과 같다면 탐색을 종료합니다.
5. 중간 요소가 찾고자 하는 값보다 작다면, 배열의 오른쪽 절반에 대해서 이진 탐색을 재귀적으로 수행합니다.
6. 중간 요소가 찾고자 하는 값보다 크다면, 배열의 왼쪽 절반에 대해서 이진 탐색을 재귀적으로 수행합니다.
7. 탐색 범위가 더 이상 없을 때까지 반복합니다.

## 이진 탐색의 특징
 ### 장점
* 탐색 시간이 O(log n)으로 매우 빠릅니다.
* 정렬된 배열에서 사용 가능하며, 반복적으로 탐색을 수행할 때 효율적입니다.
 ### 단점
* 배열이나 리스트가 정렬되어 있어야만 사용 가능합니다.
* 삽입, 삭제 등의 연산이 빈번하게 일어나는 경우에는 추가적인 비용이 발생합니다.
 
## 시간복잡도
 이진 탐색의 시간 복잡도는 탐색 범위를 반으로 줄여나가는 과정을 반복하므로 O(log n)입니다. 여기서 n은 배열의 크기를 의미합니다.

## 예시코드 c++
```cpp
#include <iostream>
#include <vector>
using namespace std;

int BinarySearch(const vector<int>& arr, int target, int left, int right) 
{
    if (left > right) return -1; // 찾지 못한 경우

    // 중간 요소의 인덱스 계산
    int mid = left + (right - left) / 2; 

    // 찾은 경우 중간 요소의 인덱스 반환
    if (arr[mid] == target)
        return mid; 
    // 중간 값이 찾는 값보다 작은 경우, 오른쪽 부분 배열 탐색
    else if (arr[mid] < target)
        return BinarySearch(arr, target, mid + 1, right);
    // 중간 값이 찾는 값보다 큰 경우, 왼쪽 부분 배열 탐색
    else
        return BinarySearch(arr, target, left, mid - 1); 
}

int main() 
{
    vector<int> arr = {1, 3, 5, 7, 9, 11, 13, 15};

    int target = 7;
    int index = BinarySearch(arr, target, 0, arr.size() - 1);

    if (index != -1) cout << "true" << endl; 
    else cout << "false" << endl;

    return 0;
}
``` 

## 사용예시 & 코딩테스트 유형
* 정렬된 배열에서 특정한 값을 찾아야 할 때 사용됩니다.
* 이진 탐색은 탐색 시간이 매우 빠르기 때문에 대용량 데이터에서 효율적으로 사용됩니다.