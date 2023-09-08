#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 25305

Problem Description :
2022 연세대학교 미래캠퍼스 슬기로운 코딩생활 N명의 학생들이 응시했다.
이들 중 점수가 가장 높은 k명은 상을 받을 것이다. 이 때, 상을 받는 커트라인이 몇 점인지 구하라.
커트라인이란 상을 받는 사람들 중 점수가 가장 가장 낮은 사람의 점수를 말한다.

Link: https://www.acmicpc.net/problem/25305

Input:
첫째 줄에는 응시자의 수 N과 상을 받는 사람의 수 k가 공백을 사이에 두고 주어진다.
둘째 줄에는 각 학생의 점수 x가 공백을 사이에 두고 주어진다

Output:
상을 받는 커트라인을 출력하라.

Limit: none
*/



int main()
{
	int N, k;
	int x[10000];

	cin >> N >> k;

	for (int i = 0; i < N; i++)
	{
		cin >> x[i];
	}

	//정렬
	for (int i = 0; i < N-1; i++)
	{
		for (int j = i+1; j < N; j++)
		{
			if (x[i] > x[j])
			{
				int temp = x[i];
				x[i] = x[j];
				x[j] = temp;
			}
		}
	}

	cout << x[N-k] << endl;

	return 0;
}