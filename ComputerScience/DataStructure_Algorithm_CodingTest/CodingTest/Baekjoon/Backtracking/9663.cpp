#include <iostream>
using namespace std;

/*
Problem Number: 9663

Problem Description :
N-Queen 문제는 크기가 N × N인 체스판 위에 퀸 N개를 서로 공격할 수 없게 놓는 문제이다.
N이 주어졌을 때, 퀸을 놓는 방법의 수를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/9663

Input:
첫째 줄에 N이 주어진다. (1 ≤ N < 15)

Output:
첫째 줄에 퀸 N개를 서로 공격할 수 없게 놓는 경우의 수를 출력한다.

Limit: none
*/


/*

체스에서 Queen은 총 8방향(상하좌우, 모든대각선)의 이동이 자유로운 체스말이다.
같은 행열에 존재하지 않고, 대각선에도 존재하지 않아야된다.
같은 행에 존재할수 없으니 배열이름[행번호] = 열번호로 데이터 지정해서 1차원 배열 사용
*/

#define MAXSIZE 16

int arr[MAXSIZE] = { 0, };
int n, checkCount=0;

void Backtracking(int num)
{
	if (num == n)
	{
		checkCount++;
	}
	else
	{
		for (int i = 0; i < n; i++)
		{
			arr[num] = i;
			bool check = true;
			for (int j = 0; j <num; j++)
			{
				if (arr[j] == arr[num] || abs(arr[num] - arr[j]) == num-j)
				{
					check = false;
					break;
				}
			}
			if(check) Backtracking(num + 1);
		}
	}
}


int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	cin >> n;
	Backtracking(0);
	cout << checkCount;
	return 0;
}