#include <iostream>
#include <algorithm>
#include <cmath>
#include <string>
using namespace std;

/*
Problem Number: 17103

Problem Description :
골드바흐의 추측: 2보다 큰 짝수는 두 소수의 합으로 나타낼 수 있다.
짝수 N을 두 소수의 합으로 나타내는 표현을 골드바흐 파티션이라고 한다. 짝수 N이 주어졌을 때, 골드바흐 파티션의 개수를 구해보자. 두 소수의 순서만 다른 것은 같은 파티션이다.

Link: https://www.acmicpc.net/problem/17103

Input:
첫째 줄에 테스트 케이스의 개수 T (1 ≤ T ≤ 100)가 주어진다. 각 테스트 케이스는 한 줄로 이루어져 있고, 정수 N은 짝수이고, 2 < N ≤ 1,000,000을 만족한다.

Output:
각각의 테스트 케이스마다 골드바흐 파티션의 수를 출력한다.

Limit: none
*/

#define MAX 1000001
bool prime[MAX] = { false, };

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	//에라토스테네스의 체
	for (int i = 0; i < MAX; i++) 
	{
		prime[i] = true;
	}
	for (int j = 2; j < MAX; j++)
	{
		if (!prime[j]) continue;
		for (int k = j + j; k < MAX; k += j)
		{
			prime[k] = false;;
		}
	}

	int T,target, temp,count=0;
	cin >> T;
	for (int i = 0; i < T; i++)
	{
		cin >> target;

		temp = target / 2;
		for (int j = 2; j <= temp; j++)
		{
			if (!prime[j]) continue;

			if (prime[target - j])
			{
				count++;
			}
		}

		cout << count << '\n';
		count = 0;
	}


	return 0;
}