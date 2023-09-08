#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 11650

Problem Description :
2차원 평면 위의 점 N개가 주어진다. 좌표를 x좌표가 증가하는 순으로, x좌표가 같으면 y좌표가 증가하는 순서로 정렬한 다음 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/11650

Input:
첫째 줄에 점의 개수 N (1 ≤ N ≤ 100,000)이 주어진다. 둘째 줄부터 N개의 줄에는 i번점의 위치 xi와 yi가 주어진다.
(-100,000 ≤ xi, yi ≤ 100,000) 좌표는 항상 정수이고, 위치가 같은 두 점은 없다.

Output:
첫째 줄부터 N개의 줄에 점을 정렬한 결과를 출력한다.

Limit: none
*/

const int MAX_SIZE = 100000;

typedef struct
{
	int x;
	int y;
}Pos;

int compare(const void* a, const void* b)
{
	Pos A = *(Pos*)a;
	Pos B = *(Pos*)b;

	if (A.x > B.x) return 1;
	else if (A.x == B.x)
	{
		if (A.y > B.y)  return 1;
		else return -1;
	}
	else return -1;
}

int main()
{
	ios::sync_with_stdio(0); cin.tie(0);
	Pos pos[MAX_SIZE];
	int N;
	cin >> N;

	for (int i = 0; i < N; i++)
	{
		cin >> pos[i].x >> pos[i].y;
	}

	// 퀵정렬 알고리즘사용
	qsort(pos, N, sizeof(Pos), compare);

	for (int i = 0; i < N; i++)
	{
		cout<< pos[i].x <<" "<< pos[i].y<<'\n';
	}

	return 0;
}