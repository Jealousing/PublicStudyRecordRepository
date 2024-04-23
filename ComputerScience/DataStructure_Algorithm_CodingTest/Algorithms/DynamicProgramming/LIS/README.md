# 최장 증가 부분 수열 (Longest Increasing Subsequence, LIS)

## 최장 증가 부분 수열이란?
 최장 증가 부분 수열은 주어진 수열에서 일부 숫자를 선택하여 만든 증가하는 부분 수열 중에서 가장 긴 것을 말합니다.    
선택된 숫자들은 원래 수열에서 순서를 유지해야 합니다. (원래 수열의 순서대로 나열된 부분 수열 중에서 각 항이 이전 항보다 큰 부분 수열)

예를 들어, 수열 {10, 20, 10, 30, 20, 50}에서 최장 증가 부분 수열은 {10, 20, 30, 50}이며, 길이는 4입니다.

## 최장 증가 부분 수열의 특징
 * 최장 증가 부분 수열은 원래 수열의 순서를 유지하므로, 원래 수열의 순서에 영향을 받습니다.
 * 최장 증가 부분 수열은 여러 개 존재할 수 있습니다. 예를 들어, 수열 {10, 20, 30, 40, 50}에서 최장 증가 부분 수열은 {10, 20, 30, 40}, {10, 20, 30, 50}, {10, 20, 40, 50}, {10, 30, 40, 50} 등 여러 개가 있을 수 있습니다.
 * 동적 계획법을 이용하여 효율적으로 구할 수 있습니다.

## 최장 증가 부분 수열 구하는 알고리즘

### 1. 동적 계획법(DP)을 활용한 방법
 각 위치에서 그 위치를 마지막으로 하는 최장 증가 부분 수열의 길이를 저장하고, 이를 바탕으로 전체 최장 증가 부분 수열의 길이를 찾습니다.    
 이 방법의 시간 복잡도는 O(n^2)입니다. 이는 각 위치에서 이전 위치까지의 모든 위치를 검사하므로 발생합니다.   

#### 예시코드 c++
```cpp
#include <iostream>
#include <vector>

using namespace std;

int LIS(vector<int>& arr) 
{
    int n = arr.size();
    vector<int> dp(n, 1); // 길이가 i인 LIS의 길이 저장

    for (int i = 1; i < n; ++i) 
    {
        for (int j = 0; j < i; ++j) 
        {
            if (arr[i] > arr[j] && dp[i] < dp[j] + 1) 
            {
                dp[i] = dp[j] + 1;
            }
        }
    }

    int maxLen = 0;
    for (int i = 0; i < n; ++i) 
    {
        maxLen = max(maxLen, dp[i]);
    }

    return maxLen;
}

int main() 
{
    vector<int> arr = {10, 20, 10, 30, 20, 50};
    cout << "최장 증가 부분 수열의 길이: " << LIS(arr) << endl;
    return 0;
}
```

### 2. 이분 탐색을 활용한 방법
 입력 수열을 순회하면서 각 항에 대해 최장 증가 부분 수열을 찾아나가는 대신, 이분 탐색을 통해 이전에 발견한 부분 수열을 유지하면서 LIS를 찾아나가는 방법입니다.   
 이 방법의 시간 복잡도는 O(n log n)입니다. 이는 각 위치에서 이전 위치까지의 위치를 이분 탐색으로 검사하므로 발생합니다.
 
#### 예시코드 c++
```cpp
#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

int LIS(vector<int>& arr) 
{
    vector<int> vec;
    for (int i = 0; i < arr.size(); ++i) 
    {
        auto it = lower_bound(vec.begin(), vec.end(), arr[i]);
        if (vec.empty() || vec.back() < arr[i])
        {
            vec.push_back(arr[i]);
        } 
        else 
        {
            *it = arr[i];
        }
    }

    return vec.size();
}

int main() 
{
    vector<int> arr = {10, 20, 10, 30, 20, 50};
    cout << "최장 증가 부분 수열의 길이: " << LIS(arr) << endl;
    return 0;
}
```

### 3. 펜윅 트리를 활용한 방법
펜윅 트리는 구간 합 문제를 해결하기 위한 자료 구조로, LIS 문제에도 적용할 수 있습니다. 펜윅 트리를 활용하여 각 위치에서의 최장 증가 부분 수열의 길이를 업데이트하면서 LIS를 찾아나갈 수 있습니다.    
이 방법은 시간 복잡도가 O(n log n)이며, 구현이 비교적 간단하고 메모리 사용량이 적은 특징이 있습니다.   

#### 예시코드 c++
```cpp
#include <iostream>
#include <vector>

using namespace std;

// 펜윅 트리 클래스 정의
class FenwickTree 
{
private:
    vector<int> tree; // 트리를 저장하는 배열

public:
    // 펜윅 트리 초기화
    FenwickTree(int n) 
    {
        tree.assign(n + 1, 0); // 1부터 시작하는 인덱스를 사용하기 위해 크기를 n + 1로 설정
    }

    // 최대값 반환
    int Query(int idx) 
    {
        int maxVal = 0;
        while (idx > 0) 
        {
            maxVal = max(maxVal, tree[idx]);
            idx = getPrev(idx); // 현재 인덱스에서 이전 인덱스로 이동하는 과정
        }
        return maxVal;
    }

    // 구간 합을 업데이트하는 함수
    void Update(int idx, int val) 
    {
        while (idx < tree.size()) 
        {
            tree[idx] = max(tree[idx], val);
            idx = getNext(idx); // 현재 인덱스에서 다음 인덱스로 이동하는 과정
        }
    }
private:
    // 현재 인덱스에서 이전 인덱스를 찾는 함수
    int getPrev(int idx) 
    {
        // 이진 표현에서 가장 오른쪽에 있는 1을 제거하여 이전 인덱스를 찾음
        return idx - (idx & -idx);
    }

    // 현재 인덱스에서 다음 인덱스를 찾는 함수
    int getNext(int idx) 
    {
        // 이진 표현에서 가장 오른쪽에 있는 0을 추가하여 다음 인덱스를 찾음
        return idx + (idx & -idx);
    }
};

int LIS(vector<int>& arr) 
{
    int n = arr.size();
    vector<int> dp(n);
    FenwickTree fenwick(n);
    int maxLen = 0;

    for (int i = 0; i < n; ++i) 
    {
        dp[i] = fenwick.Query(arr[i] - 1) + 1;
        fenwick.Update(arr[i], dp[i]);
        maxLen = max(maxLen, dp[i]);
    }

    return maxLen;
}

int main() 
{
    vector<int> arr = {10, 20, 10, 30, 20, 50};
    cout << "최장 증가 부분 수열의 길이: " << LIS(arr) << endl;
    return 0;
}
```

### 4. 세그먼트 트리를 활용한 방법
세그먼트 트리 역시 구간 합 문제를 해결하는 자료 구조로, LIS 문제에도 적용할 수 있습니다. 세그먼트 트리를 활용하여 각 구간의 최대값을 저장하고, 이를 이용하여 LIS를 구할 수 있습니다.    
이 방법 또한 시간 복잡도가 O(n log n)이며, 구현이 상대적으로 복잡할 수 있습니다.   

#### 예시코드 c++
```cpp
#include <iostream>
#include <vector>

using namespace std;

// 세그먼트 트리 클래스 정의
class SegmentTree 
{
private:
    vector<int> tree; // 트리를 저장하는 배열
    vector<int> arr;  // 입력 배열
    int n;            // 입력 배열의 크기

    // 세그먼트 트리 초기화
    void Build(int node, int start, int end) 
    {
        if (start == end) 
        {
            tree[node] = arr[start];
        } 
        else 
        {
            int mid = (start + end) / 2;
            Build(2 * node, start, mid);
            Build(2 * node + 1, mid + 1, end);
            tree[node] = max(tree[2 * node], tree[2 * node + 1]); // 최대값을 저장
        }
    }

    // 구간 최대값을 계산하는 함수
    int Query(int node, int start, int end, int left, int right) 
    {
        if (right < start || left > end) 
        {
            return 0; // 구간이 현재 노드의 구간과 겹치지 않음
        }
        if (left <= start && right >= end) 
        {
            return tree[node]; // 구간이 현재 노드의 구간을 완전히 포함함
        }
        int mid = (start + end) / 2;
        return max(Query(2 * node, start, mid, left, right),
                   Query(2 * node + 1, mid + 1, end, left, right));
    }

public:
    SegmentTree(vector<int>& input) 
    {
        arr = input;
        n = arr.size();
        tree.resize(4 * n); // 트리 배열의 크기는 입력 배열 크기의 4배로 설정
        Build(1, 0, n - 1); // 트리 구축
    }

    // 구간 최대값을 반환하는 함수
    int RangeMaxQuery(int left, int right) 
    {
        return query(1, 0, n - 1, left, right);
    }
};

int LIS(vector<int>& arr) 
{
    SegmentTree st(arr);
    return st.RangeMaxQuery(0, arr.size() - 1);
}

int main() 
{
    vector<int> arr = {10, 20, 10, 30, 20, 50};
    cout << "최장 증가 부분 수열의 길이: " << LIS(arr) << endl;
    return 0;
}
```

## 길이 뿐만 아니라 LIS 자체를 구하는 방법
```cpp
#include <iostream>
#include <vector>

using namespace std;

// LIS를 구하는 함수
vector<int> LIS(vector<int>& arr) 
{
    // 각 위치에서의 LIS를 저장할 2차원 벡터
    vector<vector<int>> dp(arr.size());
    dp[0].push_back(arr[0]);
    vector<int> lis;

    for (int i = 1; i < arr.size(); ++i) 
    {
        // 현재 위치에서 이전 위치까지 확인하여 가장 긴 LIS를 찾음
        for (int j = 0; j < i; ++j) 
        {
            if (arr[i] > arr[j] && dp[i].size() < dp[j].size() + 1) 
            {
                dp[i] = dp[j];
            }
        }
        // 현재 위치의 숫자를 LIS에 추가
        dp[i].push_back(arr[i]);
        // 만약 현재 위치의 LIS가 더 긴 경우 최장 증가 부분 수열 갱신
        if (dp[i].size() > lis.size()) 
        {
            lis = dp[i];
        }
    }

    return lis;
}

int main() {
    vector<int> arr = {10, 20, 10, 30, 20, 50};
    vector<int> longArr = LIS(arr);

    // 최장 증가 부분 수열 출력
    cout << "최장 증가 부분 수열:";
    for (int i = 0; i < longArr.size(); ++i) 
    {
        cout << " " << longArr[i];
    }
    cout << "\n최장 증가 부분 수열의 길이: " << longArr.size() << endl;
    return 0;
}
```