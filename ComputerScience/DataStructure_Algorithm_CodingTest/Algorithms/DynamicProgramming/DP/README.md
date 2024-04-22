# 동적 계획법(Dynamic Programming)

## 동적 계획법이란?
 동적 계획법은 복잡한 문제를 간단한 하위 문제로 나누어 풀고, 그 결과를 저장하여 재활용함으로써 전체 문제를 효율적으로 해결하는 알고리즘 기법입니다. 이는 큰 문제를 해결하기 위해 작은 문제를 여러 번 해결하는 것보다 효율적입니다.

## 동적 계획법의 특징
 ### 장점
* 문제를 더 작은 하위 문제로 나누어 풀기 때문에, 복잡한 문제도 효율적으로 해결할 수 있습니다.
* 중복되는 하위 문제의 해를 저장하고 재활용함으로써 계산 시간을 단축할 수 있습니다.
* 여러 종류의 문제에 적용 가능하며, 최적화 문제나 최단 경로 문제 등 다양한 문제에 적용할 수 있습니다.

 ### 단점
* 모든 가능한 하위 문제의 해를 저장해야 하므로 메모리 사용량이 크다는 단점이 있습니다.
* 동적 계획법을 적용하려면 문제가 최적 부분 구조와 중복되는 부분 문제를 가지고 있어야 합니다.
 
## 동적 계획법의 접근 방법
1. 문제를 하위 문제로 나누기: 주어진 문제를 더 작은 하위 문제로 분할합니다.
2. 하위 문제의 해 구하기: 가장 작은 하위 문제부터 시작하여 해를 구합니다.
3. 해 저장 및 재활용: 계산한 결과를 저장하고 재활용하여 중복 계산을 피합니다.
4. 상위 문제 해 구하기: 작은 하위 문제의 해를 조합하여 전체 문제의 해를 구합니다.

## 동적 계획법의 종류
 
### Top-down (Memoization)
 Top-down 방식은 큰 문제를 작은 문제로 나누어 해결하는 방식입니다. 재귀 함수를 이용하여 구현할 수 있으며, 한 번 계산한 결과를 메모리에 저장해두고 필요할 때마다 재사용합니다.

### Bottom-up (Tabulation)
 Bottom-up 방식은 작은 문제부터 시작하여 큰 문제를 해결하는 방식입니다. 반복문을 이용하여 구현할 수 있으며, 작은 문제의 결과를 이용하여 큰 문제를 해결합니다.

## 종류별 예시코드 c++

### 피보나치 수열
```cpp
#include <iostream>
#include <vector>

using namespace std;

// Top-down 방식의 피보나치 수열 계산 (Memoization)
int MemoizationFibonacci(int n, vector<int>& memo) 
{
    if (n <= 1)
        return n;
    
    // 이미 계산한 값이 있으면 그 값을 반환
    if (memo[n] != -1)
        return memo[n];

    // 계산한 값을 메모이제이션
    memo[n] = MemoizationFibonacci(n - 1, memo) + MemoizationFibonacci(n - 2, memo);

    return memo[n];
}

// Bottom-up 방식의 피보나치 수열 계산 (Tabulation)
int TabulationFibonacci(int n) 
{
    vector<int> dp(n + 1); // Bottom-up 방식을 위한 DP 테이블

    // 초기값 설정
    dp[0] = 0;
    dp[1] = 1;

    // 바텀업 방식으로 피보나치 수열 계산
    for (int i = 2; i <= n; ++i) 
    {
        dp[i] = dp[i - 1] + dp[i - 2];
    }

    return dp[n];
}

int main() 
{
    int n = 10; // 피보나치 수열의 n번째 항
    vector<int> memo(n + 1, -1); // 메모이제이션을 위한 배열

    cout << "Top-down 방식 피보나치 수열의 " << n << "번째 항: " << MemoizationFibonacci(n, memo) << endl;
    cout << "Bottom-up 방식 피보나치 수열의 " << n << "번째 항: " << TabulationFibonacci(n) << endl;
    
return 0;
}
```
