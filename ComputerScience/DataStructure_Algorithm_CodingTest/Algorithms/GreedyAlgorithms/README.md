# 탐욕 알고리즘 (Greedy Algorithm)

## 탐욕 알고리즘이란?
 탐욕 알고리즘은 매 순간마다 가장 최선의 선택을 하는 알고리즘입니다. 즉, 각 단계에서 현재 상황에서 가장 이득이 되는 선택을 하여 전체적으로 최적의 해답을 찾아냅니다. 이 선택은 한 번 하면 다시 되돌릴 수 없습니다. 이러한 방식으로 단계별로 최선의 선택을 함으로써 최종적으로 문제를 해결합니다.

## 탐욕 알고리즘의 특징
 
 ### 장점
* 간단하고 직관적인 알고리즘입니다. 각 단계에서 간단한 비교만을 수행하기 때문에 이해하기 쉽습니다.
* 문제를 빠르게 해결할 수 있습니다. 매 순간마다 최선의 선택을 하기 때문에 시간 복잡도가 낮은 경우가 많습니다.
* 많은 경우에는 최적의 해를 찾습니다. 문제의 조건에 따라서는 최적의 해를 찾지 못하는 경우도 있지만, 대부분의 경우에는 최적의 해에 근접한 결과를 제공합니다.

 ### 단점
* 항상 최적의 해를 보장하지 않습니다. 때로는 탐욕적으로 선택한 해가 전체적으로 최적이 아닐 수 있습니다. 탐욕 알고리즘이 지역 최적해에 갇힐 수 있어서 전역 최적해를 찾지 못할 수 있습니다.
* 문제의 성질에 따라 탐욕 알고리즘을 사용할 수 없거나, 사용했을 때 원하는 결과를 얻을 수 없는 경우가 있습니다. 

## 탐욕 알고리즘의 예시
* **거스름돈 문제:** 최소한의 동전 개수로 거스름돈을 주는 방법을 찾는 문제
* **배낭 문제:** 주어진 무게와 가치를 가진 물건들을 가방에 담을 때, 가방에 담을 수 있는 가장 가치 있는 물건들을 선택하는 문제
* **탐욕적 스케줄링:** 주어진 일정과 각 작업의 완료 시간과 보상을 고려하여 가장 보상이 큰 작업들을 선택하는 문제 등에 적용될 수 있습니다.
 
## 예시 코드 C++
```cpp
#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

// 거스름돈 문제 해결을 위한 탐욕 알고리즘
vector<int> MakeChange(int amount, vector<int>& coins) 
{
    // 동전을 내림차순으로 정렬
    sort(coins.begin(), coins.end(), greater<int>()); 
    vector<int> change;

    for (int i = 0; i < coins.size(); ++i) 
    {
        int coin = coins[i];
        while (amount >= coin) 
        {
            change.push_back(coin);
            amount -= coin;
        }
    }
    return change;
}

int main() 
{
    int amount = 67;
    vector<int> coins = {50, 20, 10, 5, 1};
    vector<int> change = MakeChange(amount, coins);

    cout << "거스름돈: ";
    for (int i = 0; i < change.size(); ++i) 
    {
        cout << change[i] << " ";
    }
    cout << endl;
    return 0;
}
```

		출력결과:
		거스름돈: 50 10 5 1 1
