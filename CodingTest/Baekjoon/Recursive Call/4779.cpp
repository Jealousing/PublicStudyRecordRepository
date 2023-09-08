#include <iostream>
#include <algorithm>
#include <cmath>
using namespace std;

/*
Problem Number: 4779

Problem Description :
칸토어 집합은 0과 1사이의 실수로 이루어진 집합으로, 구간 [0, 1]에서 시작해서 각 구간을 3등분하여 가운데 구간을 반복적으로 제외하는 방식으로 만든다.
전체 집합이 유한이라고 가정하고, 다음과 같은 과정을 통해서 칸토어 집합의 근사를 만들어보자.
1. -가 3N개 있는 문자열에서 시작한다.
2. 문자열을 3등분 한 뒤, 가운데 문자열을 공백으로 바꾼다. 이렇게 하면, 선(문자열) 2개가 남는다.
3. 이제 각 선(문자열)을 3등분 하고, 가운데 문자열을 공백으로 바꾼다. 이 과정은 모든 선의 길이가 1일때 까지 계속 한다.
예를 들어, N=3인 경우, 길이가 27인 문자열로 시작한다.
---------------------------
여기서 가운데 문자열을 공백으로 바꾼다.
---------         ---------
남은 두 선의 가운데 문자열을 공백으로 바꾼다.
---   ---         ---   ---
한번 더
- -   - -         - -   - -
모든 선의 길이가 1이면 멈춘다. N이 주어졌을 때, 마지막 과정이 끝난 후 결과를 출력하는 프로그램을 작성하시오
Link: https://www.acmicpc.net/problem/4779

Input:
입력을 여러 줄로 이루어져 있다. 각 줄에 N이 주어진다. 파일의 끝에서 입력을 멈춘다. N은 0보다 크거나 같고, 12보다 작거나 같은 정수이다.

Output:
입력으로 주어진 N에 대해서, 해당하는 칸토어 집합의 근사를 출력한다.

Limit: none
*/

#define MAXSIZE 531442
char value[MAXSIZE];
int cnt = 0;

void Draw(int min ,int max)
{
	if (max - min > 1)
	{
		int size = (max-min) / 3;
	
		fill(value + (min+size), value + (max - size), ' ');

		if (max - min > 3)
		{
			Draw(min, min + size);
			Draw(min + (size * 2), max);
		}
	}
}

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int n;
	while (cin >> n)
	{
		int max = (int)pow(3, n);
		fill(value, value+max + 1, '-');

		Draw(0,max);

		for (int i = 0; i < max; i++)
		{
			cout<<value[i];
		}
		cout << '\n';
		// 파일의 끝에서 입력을 멈춘다
		if (cin.eof() == true) { break; }
	}
	return 0;
}