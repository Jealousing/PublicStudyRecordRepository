# 투 포인터 알고리즘 (Two Pointers)

## 투 포인터 알고리즘이란?
 투 포인터 알고리즘은 두 개의 포인터를 사용하여 배열이나 리스트 내에서 원하는 조건을 만족하는 부분을 찾는 알고리즘입니다. 주로 정렬된 배열이나 리스트에서 연속적인 부분집합을 찾거나 특정한 합이나 길이를 갖는 부분집합을 찾는 데 사용됩니다.

## 투 포인터 알고리즘의 특징
* **정렬된 배열이나 리스트가 필요합니다:** 효과적으로 사용하려면 입력 데이터가 정렬되어 있어야 합니다.
* **두 개의 포인터를 사용합니다:** 일반적으로 배열의 시작과 끝을 가리키는 두 개의 포인터를 조작하면서 원하는 조건을 만족하는 부분을 찾습니다.
* **시간 복잡도가 O(N)으로 매우 효율적입니다.**
 
## 투 포인터 알고리즘의 단계
1. **포인터 초기화:** 배열의 양 끝에 두 개의 포인터를 초기화합니다.
2. **조건 확인:** 두 포인터가 가리키는 값들을 조건에 맞게 비교합니다.
3. **포인터 이동:** 조건에 따라 포인터를 이동시킵니다. 조건을 만족하는 부분을 찾을 때까지 반복합니다.

## 투 포인터 알고리즘의 예시
* **두 수의 합 찾기:** 정렬된 배열에서 두 수의 합이 특정한 값이 되는 쌍을 찾는 문제를 투 포인터 알고리즘을 사용하여 해결할 수 있습니다.
* **부분합 찾기:** 정렬된 배열에서 특정한 합을 갖는 연속적인 부분집합을 찾는 문제를 투 포인터 알고리즘을 사용하여 해결할 수 있습니다.
* **배열 안에서의 연속적인 수열의 합 구하기:** 연속적인 수열의 합이 특정한 값이 되는 경우를 찾는 문제를 투 포인터 알고리즘을 사용하여 해결할 수 있습니다.

## 투 포인터 알고리즘의 예시 코드 (C++)

아래는 백준에서 풀었던 투 포인터 알고리즘을 사용한 문제의 코드입니다.   
[ [문제링크](https://www.acmicpc.net/problem/2470) / [코드링크](https://github.com/Jealousing/PublicStudyRecordRepository/blob/main/ComputerScience/DataStructure_Algorithm_CodingTest/CodingTest/Baekjoon/Two%20Pointers/2470.cpp) ]

```cpp
﻿#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 수열 크기
    int n;
    cin >> n;

    vector<int> vec;
    vec.resize(n);

    // 수열 입력
    for (int i = 0; i < n; ++i) 
    {
        cin >> vec[i]; 
    }
    // 정렬
    sort(vec.begin(), vec.end());

    int left = 0, right = n - 1, min = 2000000001, result[2]={0,0};
    while (left<right)
    {
        int sum = vec[left] + vec[right];
        int absSum = abs(sum);

        if (absSum < min)
        {
            min = absSum;
            result[0] = vec[left];
            result[1] = vec[right];

            if (sum == 0) break;
       }

        if (sum < 0) left++;
        else right--;
    }
    
    // 출력
    cout << result[0]<<' '<< result[1] << '\n';

    return 0;
}
```