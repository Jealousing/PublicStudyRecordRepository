#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 1085

Problem Description :
한수는 지금 (x, y)에 있다. 직사각형은 각 변이 좌표축에 평행하고, 왼쪽 아래 꼭짓점은 (0, 0), 오른쪽 위 꼭짓점은 (w, h)에 있다. 
직사각형의 경계선까지 가는 거리의 최솟값을 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1085

Input:
첫째 줄에 x, y, w, h가 주어진다.

Output:
첫째 줄에 문제의 정답을 출력한다.

Limit: none
*/

int checkDistance(int pos, int max)
{
	int min = pos;

	if (min > abs(max - pos))
	{
		min = abs(max - pos);
	}

	return min;
}


int main()
{
	int x, y, w, h;
	int min = 5000;
	int temp = 0;

	cin >> x >> y >> w >> h;

	if (min > (temp = checkDistance(x, w)))
	{
		min = temp;
	}

	if (min > (temp = checkDistance(y, h)))
	{
		min = temp;
	}

	cout << min << endl;

	return 0;
}