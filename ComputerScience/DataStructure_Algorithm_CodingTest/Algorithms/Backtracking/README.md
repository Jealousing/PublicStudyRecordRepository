# 백트래킹 알고리즘 (Backtracking)

## 백트래킹 알고리즘이란?
 백트래킹 알고리즘은 모든 가능한 경우의 수를 탐색하면서 해답을 찾아가는 알고리즘입니다. 이 과정에서 현재까지 탐색한 경로가 해답으로 이어질 가능성이 없다고 판단되면, 더 이상 해당 경로를 따라가지 않고 이전 단계로 돌아가서 다른 가능성을 탐색합니다. 즉, 모든 가능성을 조사하는 과정에서 불필요한 경우의 수를 줄여나가면서 해답을 찾아내는 방식입니다.

## 백트래킹 알고리즘의 특징
 * 상태 공간 트리(State Space Tree)를 탐색하면서 문제의 해답을 찾습니다.   

		상태 공간 트리 (State Space Tree): 문제의 가능한 모든 상태를 나타내는 트리 구조를 만듭니다. 각 노드는 문제의 상태를 나타내며, 간선은 상태 간의 전이를 의미합니다.
 
 * 백트래킹은 주로 재귀적으로 구현되며, 재귀 호출을 통해 모든 가능한 경로를 탐색합니다.
 * 현재까지 탐색한 경로가 해답으로 이어질 가능성이 없는 경우, 해당 경로를 포기하고 이전 단계로 돌아가서 다른 경로를 탐색합니다.

## 백트래킹 알고리즘의 단계
1. **선택(Choose):** 현재 단계에서 가능한 선택지 중 하나를 선택합니다.
2. **제약(Constraint):** 선택한 선택지가 제약 조건을 만족하는지 확인합니다. 제약 조건을 만족하지 않으면 해당 선택지는 포기합니다.
3. **목표(Goal):** 현재 선택한 선택지가 문제의 해답이 되는지 확인합니다. 해답이라면 알고리즘을 종료하고, 그렇지 않으면 다음 선택지를 탐색합니다.
4. **순환(Recurse):** 선택한 선택지를 사용하여 다음 단계로 진행합니다. 이를 위해 재귀 호출을 사용합니다.
5. **백트래킹(Backtrack):** 현재 단계에서 선택한 선택지로 이어지는 경로가 해답으로 이어질 가능성이 없다고 판단되면 해당 선택지를 포기하고 이전 단계로 돌아갑니다.

## 백트래킹 알고리즘의 예시
 * **N-Queens 문제:** N x N 크기의 체스판에 N개의 퀸을 서로 공격할 수 없게끔 배치하는 문제
 * **스도쿠 문제:** 9 x 9 크기의 스도쿠 퍼즐을 채우는 문제
 * **조합 문제:** 주어진 집합에서 특정 개수의 요소로 이루어진 조합을 구하는 문제

## 백트래킹을 사용한 N-Queens 예시코드 c++
 
```cpp
﻿#include <iostream>
using namespace std;

#define MAXSIZE 16

int arr[MAXSIZE] = { 0, };
int n, checkCount=0;

void Backtracking(int num)
{
	if (num == n)
	{
		checkCount++;
	}
	else
	{
		for (int i = 0; i < n; i++)
		{
			arr[num] = i;
			bool check = true;
			for (int j = 0; j <num; j++)
			{
				if (arr[j] == arr[num] || abs(arr[num] - arr[j]) == num-j)
				{
					check = false;
					break;
				}
			}
			if(check) Backtracking(num + 1);
		}
	}
}


int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	cin >> n;
	Backtracking(0);
	cout << checkCount;
	return 0;
}
```