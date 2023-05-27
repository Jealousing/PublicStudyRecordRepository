#include <iostream>
#include <algorithm>
#include <cmath>
#include <string>
#include <vector>
using namespace std;

/*
Problem Number: 13909

Problem Description :
서강대학교 컴퓨터공학과 실습실 R912호에는 현재 N개의 창문이 있고 또 N명의 사람이 있다. 1번째 사람은 1의 배수 번째 창문을 열려 있으면 닫고 닫혀 있으면 연다.  
2번째 사람은 2의 배수 번째 창문을 열려 있으면 닫고 닫혀 있으면 연다. 이러한 행동을 N번째 사람까지 진행한 후 열려 있는 창문의 개수를 구하라. 단, 처음에 모든 창문은 닫혀 있다.

예를 들어 현재 3개의 창문이 있고 3명의 사람이 있을 때,
1번째 사람은 1의 배수인 1,2,3번 창문을 연다. (1, 1, 1)
2번째 사람은 2의 배수인 2번 창문을 닫는다. (1, 0, 1)
3번째 사람은 3의 배수인 3번 창문을 닫는다. (1, 0, 0)
결과적으로 마지막에 열려 있는 창문의 개수는 1개 이다.

Link: https://www.acmicpc.net/problem/13909

Input:
첫 번째 줄에는 창문의 개수와 사람의 수 N(1 ≤ N ≤ 2,100,000,000)이 주어진다.

Output:
각각의 테스트 케이스마다 골드바흐 파티션의 수를 출력한다.

Limit: none
*/



int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	unsigned int N,count=0;
	cin >> N;

	// 1~N까지 제곱수의 개수 = 열려있는 창문
	for (int i = 1; i * i <= N; ++i)
		count++;
	cout << count;

	/*
	메모리 초과 
	-> 패턴확인
	#define MAX  2100000001
	vector<bool> window(MAX, false);
	for (int i = 1; i < N; i++)
	{
		for (int j = i; j < N; j += i)
		{
			if (window[j])
			{
				window[j] = false;
				count--;
			}
			else
			{
				window[j] = true;
				count++;
			}
		}
	}

	cout << count << '\n';

	for (int i = 0; i < N; i++)
	{
		if (window[i]) cout << i << ' ';
	}
	*/
	
	return 0;
}