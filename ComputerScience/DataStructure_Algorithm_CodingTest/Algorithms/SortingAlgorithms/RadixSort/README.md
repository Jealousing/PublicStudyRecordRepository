# 기수 정렬(Radix Sort)

## 기수 정렬이란?
 기수 정렬(Radix Sort)은 비교 기반 정렬 알고리즘이 아닌 정렬 방식 중 하나입니다. 기수 정렬은 비교 없이 정렬을 수행하며, 입력된 숫자들의 자릿수를 기반으로 정렬합니다.

### 기수 정렬의 동작 원리
 기수 정렬은 주어진 숫자들을 가장 작은 자릿수부터 가장 큰 자릿수까지 비교하여 정렬합니다. 각 자릿수를 기준으로 숫자들을 재배치하면서 정렬을 수행합니다.

## 기수 정렬 특징
 ### 장점
* 비교 연산을 하지 않고 정렬하기 때문에 기수 정렬은 O(nw)라는 시간 복잡도를 가지는 정렬 방법으로, 매우 빠른 속도를 가지고 있습니다1. 여기서 n은 키의 수이고 w는 키의 길이입니다.
* 안정적인 정렬 알고리즘 중 하나입니다.

 ### 단점
* 데이터 전체 크기에 기수 테이블의 크기만한 메모리가 더 필요하다.
* 기수 정렬은 정렬 방법의 특수성 때문에, 일부 특수한 비교 연산이 필요한 데이터에는 적용할 수 없을 수 있습니다.
* 기수 정렬은 자릿수가 있는 데이터(정수, 문자열 등)에만 사용 가능합니다.
 
## 기수 정렬의 시간복잡도
기수 정렬의 시간 복잡도는 주어진 데이터의 자릿수와 범위에 따라 달라집니다. 일반적으로 입력된 데이터의 자릿수가 ddd이고 범위가 kkk일 때, 시간 복잡도는 O(d⋅(n+k))O(d \cdot (n+k))O(d⋅(n+k))입니다.
 
## 예시코드 c++
```cpp
#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;

// 기수 정렬 함수
void radixSort(vector<int>& arr) 
{
    if (arr.empty()) return;

    // 입력된 숫자들 중 최댓값 찾기
    int maxNum = *max_element(arr.begin(), arr.end());
    
    // 가장 큰 자릿수 확인
    int digitCount = 0;
    while (maxNum != 0) 
    {
        maxNum /= 10;
        ++digitCount;
    }

    // 기수 정렬
    for (int exp = 1; exp <= digitCount; ++exp)  
    {
        vector<vector<int>> buckets(10); // 0부터 9까지의 버킷 생성

        // 각 자릿수 별로 숫자를 버킷에 분배
        for (int i = 0; i < arr.size(); ++i) 
        {
            int num = arr[i];
            int digit = (num / exp) % 10;
            buckets[digit].push_back(num);
        }

        // 버킷에 저장된 숫자들을 다시 배열에 합치기
        arr.clear();
        for (int i = 0; i < buckets.size(); ++i) 
        {
            for (int j = 0; j < buckets[i].size(); ++j) 
            {
                arr.push_back(buckets[i][j]);
            }
        }
    }
}

int main() {
    vector<int> arr = {170, 45, 75, 90, 802, 24, 2, 66};

    radixSort(arr);

    cout << "Sorted array: \n";
    for (int i = 0; i < arr.size(); ++i) 
        cout << arr[i] << " ";
    cout << endl;

    return 0;
}
```

### 정렬 과정 및 실행 결과
1. 최초의 배열  {170, 45, 75, 90, 802, 24, 2, 66}
2. 1의 자리수 별로 분배 및 배열에 합치기 : {170, 90, 802, 2, 24, 45, 75, 66}
2. 2의 자리수 별로 분배 및 배열에 합치기 : {802, 2, 24, 45, 66, 170, 75, 90}
3. 3의 자리수 별로 분배 및 배열에 합치기 : {2, 24, 45, 66, 75, 90, 170, 802}


## 사용예시 & 코딩테스트 유형
 기수 정렬은 정수들의 정렬에 주로 사용되며, 정수들의 자릿수를 기준으로 정렬하기 때문에 자릿수가 있는 데이터에 적용됩니다. 주어진 문제에서 입력된 데이터가 정수들이며, 정렬해야 하는 경우에 기수 정렬을 사용할 수 있습니다.