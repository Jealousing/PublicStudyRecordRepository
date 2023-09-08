#include <iostream>
#include <algorithm>
using namespace std;
/*
Problem Number: 5597

Problem Description :
X대학 M교수님은 프로그래밍 수업을 맡고 있다. 교실엔 학생이 30명이 있는데, 학생 명부엔 각 학생별로 1번부터 30번까지 출석번호가 붙어 있다.
교수님이 내준 특별과제를 28명이 제출했는데, 그 중에서 제출 안 한 학생 2명의 출석번호를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/5597

Input: 
입력은 총 28줄로 각 제출자(학생)의 출석번호 n(1 ≤ n ≤ 30)가 한 줄에 하나씩 주어진다. 출석번호에 중복은 없다.

Output: 
출력은 2줄이다. 1번째 줄엔 제출하지 않은 학생의 출석번호 중 가장 작은 것을 출력하고, 2번째 줄에선 그 다음 출석번호를 출력한다.

Limit: none
*/

int main()
{
	int temp;
	int count = 0;
	int check[30] = { 0 };
	int notcheck[2] = { 0 };
	for (int i = 0; i < 28; i++)
	{
		cin >> temp;

		check[temp-1] = 1;
	}
	
	for (int i = 0; i < 30; i++)
	{
		if (check[i] == 0) 
		{
			notcheck[count++] = i+1;
		}
	}
	sort(notcheck, notcheck + 2);
	cout << notcheck[0] << endl;
	cout << notcheck[1] << endl;


	return 0;
}