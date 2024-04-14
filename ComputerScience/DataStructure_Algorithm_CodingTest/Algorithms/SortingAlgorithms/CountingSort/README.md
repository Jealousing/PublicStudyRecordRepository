# 계수 정렬(Counting Sort)

## 계수 정렬이란?
 계수 정렬(Counting Sort)은 입력된 숫자들의 값을 직접 비교하는 대신, 각 숫자들의 출현 빈도를 세어 정렬하는 알고리즘입니다. 이를 통해 정렬을 수행하며, 입력 데이터의 값이 정수이고 비교할 수 있는 범위가 제한되어 있을 때 효과적으로 사용됩니다.

### 계수 정렬의 동작 원리
1. 입력된 숫자들의 최소값과 최대값을 확인하여 정렬에 필요한 범위를 파악합니다.
2. 정렬을 위한 배열을 생성하고, 입력된 숫자들의 출현 빈도를 해당 숫자의 인덱스에 기록합니다.
3. 출현 빈도를 기반으로 정렬된 배열을 만듭니다. 이를 위해 정렬할 숫자들을 순회하며, 해당 숫자의 출현 빈도에 따라 정렬된 배열에 값을 채워 넣습니다.
4. 정렬된 배열을 반환합니다.

## 계수 정렬의 특징
 ### 장점
 * 비교 연산을 하지 않고 정렬하기 때문에 시간 복잡도가 O(n+k)로 매우 빠릅니다. 여기서 n은 입력 데이터의 개수이고, k는 입력 데이터의 값의 범위입니다.
 * 안정적인 정렬 알고리즘 중 하나입니다.

 ### 단점
* 입력 데이터의 값의 범위가 크면 메모리 사용량이 많아질 수 있습니다.
* 입력 데이터가 정수나 정수로 표현 가능한 범위의 실수인 경우에만 사용 가능합니다.
 
## 계수 정렬의 시간복잡도
계수 정렬의 시간 복잡도는 입력 데이터의 개수를 n이라고 할 때, O(n+k)입니다. 여기서 k는 입력 데이터의 값의 범위를 의미합니다.
 
## 예시코드 c++
```cpp
#include <iostream>
#include <vector>
using namespace std;

void countingSort(vector<int>& arr) 
{
    if (arr.empty()) return;

    // 입력된 숫자들의 최소값과 최대값 찾기
    int minValue = arr[0];
    int maxValue = arr[0];
    for (int i = 1; i < arr.size(); ++i) 
    {
        if (arr[i] < minValue) minValue = arr[i];
        if (arr[i] > maxValue) maxValue = arr[i];
    }

    // 값의 범위 계산
    int range = maxValue - minValue + 1;

    // 각 값의 출현 빈도를 기록할 배열 생성
    vector<int> count(range, 0);

    // 각 숫자의 출현 빈도 기록
    for (int i = 0; i < arr.size(); ++i) 
    {
        count[arr[i] - minValue]++;
    }

    // 정렬된 배열에 값 채워 넣기
    int index = 0;
    for (int i = 0; i < range; ++i) 
    {
        while (count[i] > 0) 
        {
            arr[index++] = i + minValue;
            count[i]--;
        }
    }
}

int main() 
{
    vector<int> arr = {4, 2, 2, 8, 3, 3, 1};

    countingSort(arr);

    cout << "Sorted array: \n";
    for (int i = 0; i < arr.size(); ++i) 
        cout << arr[i] << " ";
    cout << endl;

    return 0;
}
```
### 정렬 과정 및 실행 결과
1. 최초의 배열: {4, 2, 2, 8, 3, 3, 1}
2. 각 숫자들의 출현 빈도 기록: {1, 2, 2, 1, 0, 0, 0, 1}
3. 출현 빈도를 기반으로 정렬된 배열 생성: {1, 2, 2, 3, 3, 4, 8}
 
## 사용예시 & 코딩테스트 유형
* 성적이나 연령대와 같이 범위가 제한된 데이터의 정렬에 효과적으로 사용될 수 있습니다. 특히, 데이터의 범위가 크지 않고 중복된 값이 많은 경우에 유용합니다.
