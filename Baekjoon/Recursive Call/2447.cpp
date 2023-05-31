#include <iostream>
#include <algorithm>
#include <cmath>
using namespace std;

/*
Problem Number: 2447

Problem Description :
재귀적인 패턴으로 별을 찍어 보자. N이 3의 거듭제곱(3, 9, 27, ...)이라고 할 때, 크기 N의 패턴은 N×N 정사각형 모양이다.
크기 3의 패턴은 가운데에 공백이 있고, 가운데를 제외한 모든 칸에 별이 하나씩 있는 패턴이다.
N이 3보다 클 경우, 크기 N의 패턴은 공백으로 채워진 가운데의 (N/3)×(N/3) 정사각형을 크기 N/3의 패턴으로 둘러싼 형태이다.
예를 들어 크기 27의 패턴은 예제 출력 1과 같다
Link: https://www.acmicpc.net/problem/2447

Input:
첫째 줄에 N이 주어진다. N은 3의 거듭제곱이다. 즉 어떤 정수 k에 대해 N=3k이며, 이때 1 ≤ k < 8이다.

Output:
첫째 줄부터 N번째 줄까지 별을 출력한다.

Limit: none

(1,1), (4,1), (7,1), (1,4), (7,4), (1,7), (4,7), (7,7) 
i % 3 == 1 &&j % 3 == 1 
(3,3), (3,4), (3,5), (4,3), (4,4), (4,5), (5,3), (5,4), (5,5) 
(i / n) % 3 == 1 && (j / n) % 3 == 1
*/

void Draw(int n,int i, int j)
{
	if ((i / n) % 3 == 1 && (j / n) % 3 == 1) cout << " ";
	else if (n / 3 == 0) cout << "*";
	else 	Draw(n/3,i, j);
}

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int n;
	cin >> n;
	for (int i = 0; i < n; i++) 
	{
		for (int j = 0; j < n; j++)
		{
			Draw(n,i, j);
		}
		cout << '\n';
	}
	return 0;
}