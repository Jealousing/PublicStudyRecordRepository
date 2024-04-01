#include <iostream>
#include <algorithm>
#include <cmath>
#include <string>
#include <vector>
using namespace std;

/*
Problem Number: 15439

Problem Description :
베라는 상의 N 벌과 하의 N 벌이 있다. i 번째 상의와 i 번째 하의는 모두 색상 i를 가진다. N 개의 색상은 모두 서로 다르다.
상의와 하의가 서로 다른 색상인 조합은 총 몇 가지일까?

Link: https://www.acmicpc.net/problem/15439

Input:
입력은 아래와 같이 주어진다.
N

Output:
상의와 하의가 서로 다른 색상인 조합의 가짓수를 출력한다.

Limit: none
*/



int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	unsigned int N,count=0;
	cin >> N;

	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < N; j++)
		{
			if (i != j) count++;
		}
	}
	cout << count << '\n';
	return 0;
}