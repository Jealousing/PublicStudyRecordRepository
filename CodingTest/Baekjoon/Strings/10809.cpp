#include <iostream>
#include <algorithm>
#include <cstring>
using namespace std;
/*
Problem Number: 10809

Problem Description :
알파벳 소문자로만 이루어진 단어 S가 주어진다. 각각의 알파벳에 대해서, 단어에 포함되어 있는 경우에는 처음 등장하는 위치를,
포함되어 있지 않은 경우에는 -1을 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10809

Input:
첫째 줄에 단어 S가 주어진다. 단어의 길이는 100을 넘지 않으며, 알파벳 소문자로만 이루어져 있다.

Output:
각각의 알파벳에 대해서, a가 처음 등장하는 위치, b가 처음 등장하는 위치, ... z가 처음 등장하는 위치를 공백으로 구분해서 출력한다.
만약, 어떤 알파벳이 단어에 포함되어 있지 않다면 -1을 출력한다. 단어의 첫 번째 글자는 0번째 위치이고, 두 번째 글자는 1번째 위치이다.

Limit: none
*/


int main()
{
	int abc[26]{-1};
	char string[101];
	fill_n(abc, 26, -1);
	int count = 0;
	cin >> string;
	
	for (int j = 0; j < strlen(string); j++)
	{
		for (int i = 0; i < 26; i++)
		{
			if (i == string[j] - 97 )
			{
				if (abc[i] == -1)
				{
					abc[i] = count;
				}
				count++;
			}
		}
	}

	for (int i = 0; i < 26; i++)
	{
		cout << abc[i]<<" ";
	}
	


	return 0;
}