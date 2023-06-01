#include <iostream>
using namespace std;

/*
Problem Number: 15649

Problem Description :
자연수 N과 M이 주어졌을 때, 아래 조건을 만족하는 길이가 M인 수열을 모두 구하는 프로그램을 작성하시오.
1부터 N까지 자연수 중에서 중복 없이 M개를 고른 수열

Link: https://www.acmicpc.net/problem/15649

Input:
첫째 줄에 자연수 N과 M이 주어진다. (1 ≤ M ≤ N ≤ 8)

Output:
한 줄에 하나씩 문제의 조건을 만족하는 수열을 출력한다. 중복되는 수열을 여러 번 출력하면 안되며, 각 수열은 공백으로 구분해서 출력해야 한다.
수열은 사전 순으로 증가하는 순서로 출력해야 한다.


Limit: none
*/

#define MAXSIZE 9

int check[MAXSIZE] = { 0, };
int num[MAXSIZE] = { 0, };

void Backtracking(int n, int m, int t)
{
	if (t == m)
	{
		for (int i = 0; i < m; i++) cout << num[i] << ' '; 

		cout << '\n';
		return;
	}
	else
	{
		for (int i = 1; i <= n; i++)
		{
			if (check[i] == 0)
			{
				check[i] = 1;
				num[t] = i;
				Backtracking(n, m, t+ 1);
				check[i] = 0;
			}
		}
	}
}


int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int n, m;
	cin >> n >> m;

	Backtracking(n, m, 0);

	return 0;
}