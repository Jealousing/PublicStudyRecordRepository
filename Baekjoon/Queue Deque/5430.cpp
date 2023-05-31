#include <iostream>
#include <algorithm>
#include<deque>
#include<string>
using namespace std;

/*
Problem Number: 5430

Problem Description :
선영이는 주말에 할 일이 없어서 새로운 언어 AC를 만들었다. AC는 정수 배열에 연산을 하기 위해 만든 언어이다. 
이 언어에는 두 가지 함수 R(뒤집기)과 D(버리기)가 있다.
함수 R은 배열에 있는 수의 순서를 뒤집는 함수이고, D는 첫 번째 수를 버리는 함수이다. 
배열이 비어있는데 D를 사용한 경우에는 에러가 발생한다.
함수는 조합해서 한 번에 사용할 수 있다. 예를 들어, "AB"는 A를 수행한 다음에 바로 이어서 B를 수행하는 함수이다. 
예를 들어, "RDD"는 배열을 뒤집은 다음 처음 두 수를 버리는 함수이다.
배열의 초기값과 수행할 함수가 주어졌을 때, 최종 결과를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/5430

Input:
첫째 줄에 테스트 케이스의 개수 T가 주어진다. T는 최대 100이다.
각 테스트 케이스의 첫째 줄에는 수행할 함수 p가 주어진다. p의 길이는 1보다 크거나 같고, 100,000보다 작거나 같다.
다음 줄에는 배열에 들어있는 수의 개수 n이 주어진다. (0 ≤ n ≤ 100,000)
다음 줄에는 [x1,...,xn]과 같은 형태로 배열에 들어있는 정수가 주어진다. (1 ≤ xi ≤ 100)
전체 테스트 케이스에 주어지는 p의 길이의 합과 n의 합은 70만을 넘지 않는다.

Output:
각 테스트 케이스에 대해서, 입력으로 주어진 정수 배열에 함수를 수행한 결과를 출력한다. 만약, 에러가 발생한 경우에는 error를 출력한다.

Limit: none
*/

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int t;
	cin >> t;

	for (int i = 0; i < t; i++)
	{
		string p, x, temp;
		int n;
		bool isCheck = false, isDir = true;;
		deque<int> Deque;
		cin >> p;
		cin >> n;
		cin >> x;

		for (int j = 0; j < x.size(); j++)
		{
			if (x[j] != ',' && x[j] != '[' && x[j] != ']')
			{
				temp += x[j];
			}
			else
			{
				if (!temp.empty())
				{
					Deque.push_back(stoi(temp));
					temp.clear();
				}
			}
		}

		int stack = 0;
		for (int j = 0; j < p.size(); j++)
		{
			if (p[j] == 'R')
			{
				if (isDir) isDir = false;
				else isDir = true;

				stack++;
			}
			else if (p[j] == 'D')
			{
				if (Deque.empty())
				{
					isCheck = true;
					break;
				}
				else
				{
					if (isDir) Deque.pop_front();
					else Deque.pop_back();
				}
			}
		}
		
		if (isCheck) cout << "error" << '\n';
		else
		{
			if (stack % 2 == 1) reverse(Deque.begin(), Deque.end());

			cout << '[';
			int size = Deque.size();
			for (int j = 0; j < size; j++)
			{
				cout << Deque.front();
				Deque.pop_front();

				if (j + 1 != size)
				{
					cout << ',';
				}
			}
			cout << ']'<<'\n';
		}

	}

	return 0;
}