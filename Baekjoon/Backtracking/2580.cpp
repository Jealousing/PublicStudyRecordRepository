#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 2580

Problem Description :
스도쿠는 18세기 스위스 수학자가 만든 '라틴 사각형'이랑 퍼즐에서 유래한 것으로 현재 많은 인기를 누리고 있다. 
이 게임은 아래 그림과 같이 가로, 세로 각각 9개씩 총 81개의 작은 칸으로 이루어진 정사각형 판 위에서 이뤄지는데, 게임 시작 전 일부 칸에는 1부터 9까지의 숫자 중 하나가 쓰여 있다.
https://upload.acmicpc.net/508363ac-0289-4a92-a639-427b10d66633/-/preview/
나머지 빈 칸을 채우는 방식은 다음과 같다.
각각의 가로줄과 세로줄에는 1부터 9까지의 숫자가 한 번씩만 나타나야 한다.
굵은 선으로 구분되어 있는 3x3 정사각형 안에도 1부터 9까지의 숫자가 한 번씩만 나타나야 한다.
위의 예의 경우, 첫째 줄에는 1을 제외한 나머지 2부터 9까지의 숫자들이 이미 나타나 있으므로 첫째 줄 빈칸에는 1이 들어가야 한다.
https://upload.acmicpc.net/38e505c6-0452-4a56-b01c-760c85c6909b/-/preview/
또한 위쪽 가운데 위치한 3x3 정사각형의 경우에는 3을 제외한 나머지 숫자들이 이미 쓰여있으므로 가운데 빈 칸에는 3이 들어가야 한다.
https://upload.acmicpc.net/89873d9d-56ae-44f7-adb2-bd5d7e243016/-/preview/
이와 같이 빈 칸을 차례로 채워 가면 다음과 같은 최종 결과를 얻을 수 있다
https://upload.acmicpc.net/fe68d938-770d-46ea-af71-a81076bc3963/-/preview/
게임 시작 전 스도쿠 판에 쓰여 있는 숫자들의 정보가 주어질 때 모든 빈 칸이 채워진 최종 모습을 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/2580

Input:
첫째 줄에 N이 주어진다. (1 ≤ N < 15)

Output:
첫째 줄에 퀸 N개를 서로 공격할 수 없게 놓는 경우의 수를 출력한다.

Limit: none
*/


/*
스도쿠의 완성조건
1) 행열에 같은숫자가 존재하지 않는다.
2) 3x3영역에 같은 숫자가 존재하지 않는다
*/

#define MAXSIZE 9

int arr[MAXSIZE][MAXSIZE];
vector<pair<int, int>> space;
int n, spaceCount=0;
bool flag = false;

bool check(int x, int y,int num)
{
	// 1) 행열 체크
	for (int i = 0; i < MAXSIZE; i++)
	{
		if (arr[i][y] == num) return false;
		if (arr[x][i] == num) return false;
	}

	// 2) 3x3 영역 체크
	int startX = (x / 3) * 3;
	int startY = (y / 3) * 3;
	for (int i = startX; i < startX + 3; i++)
	{
		for (int j = startY; j < startY + 3; j++)
		{
			if (arr[i][j] == num) return false;
		}
	}
	
	// 조건완료
	return true;
}

void Backtracking(int num)
{
	if (num == spaceCount)
	{
		for (int i = 0; i < MAXSIZE; i++)
		{
			for (int j = 0; j < MAXSIZE; j++)
			{
				cout<< arr[i][j]<<' ';
			}
			cout << '\n';
		}
		flag = true;
		return;
	}
	else
	{
		for (int i = 1; i <= MAXSIZE; i++)
		{
			// 조건확인
			if (check(space[num].first,space[num].second, i))
			{
				arr[space[num].first][space[num].second] = i;
				Backtracking(num + 1);
			}
			if (flag) return;
		}
		// flag는 안켜졌지만 모든 빈공간을 못채웠으면 그 곳은 다시 0으로 바꿈
		arr[space[num].first][space[num].second] = 0;
	}
}


int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);
	for (int i = 0; i < MAXSIZE; i++)
	{
		for (int j = 0; j < MAXSIZE; j++)
		{
			cin >> arr[i][j];
			if (arr[i][j] == 0)
			{
				space.push_back(make_pair(i, j));
				spaceCount++;
			}
		}
	}
	Backtracking(0);
	return 0;
}