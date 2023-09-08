#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 3009

Problem Description :
세 점이 주어졌을 때, 축에 평행한 직사각형을 만들기 위해서 필요한 네 번째 점을 찾는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/3009

Input:
세 점의 좌표가 한 줄에 하나씩 주어진다. 좌표는 1보다 크거나 같고, 1000보다 작거나 같은 정수이다.

Output:
직사각형의 네 번째 점의 좌표를 출력한다.

Limit: none
*/



int main()
{
	int X[3], Y[3];
	for (int i = 0; i < 3; i++)
	{
		cin >> X[i] >> Y[i];
	}

	if (X[0] == X[1])
	{
		cout << X[2] << " ";
	}
	else if (X[0] == X[2])
	{
		cout << X[1] << " ";
	}
	else
	{
		cout << X[0] << " ";
	}

	if (Y[0] == Y[1])
	{
		cout << Y[2] << " ";
	}
	else if (Y[0] == Y[2])
	{
		cout << Y[1] << " ";
	}
	else
	{
		cout << Y[0] << " ";
	}

	return 0;
}