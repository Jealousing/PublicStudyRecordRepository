#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 2231

Problem Description :
어떤 자연수 N이 있을 때, 그 자연수 N의 분해합은 N과 N을 이루는 각 자리수의 합을 의미한다. 
어떤 자연수 M의 분해합이 N인 경우, M을 N의 생성자라 한다. 예를 들어, 245의 분해합은 256(=245+2+4+5)이 된다.
따라서 245는 256의 생성자가 된다. 물론, 어떤 자연수의 경우에는 생성자가 없을 수도 있다. 반대로, 생성자가 여러 개인 자연수도 있을 수 있다.

자연수 N이 주어졌을 때, N의 가장 작은 생성자를 구해내는 프로그램을 작성하시오..

Link: https://www.acmicpc.net/problem/2231

Input:
첫째 줄에 자연수 N(1 ≤ N ≤ 1,000,000)이 주어진다.

Output:
첫째 줄에 답을 출력한다. 생성자가 없는 경우에는 0을 출력한다.

Limit: none
*/

int main()
{
	//216 = 198 + 1 + 9 + 8

	// 자릿수 알아내기 -> /10 -> /100 -> /1000 반복? 몫이 없을때까지?
	// 444 = ??? + ?+?+? 
	int num, count = 1;
	bool isTrue = false;
	int temp = 0;
	cin >> num;
	vector<int> v;
	temp = num;
	while (true)
	{
		v.push_back(temp % 10);
		if (temp / 10 > 0)
		{
			temp /= 10;
			count++;
		}
		else break;
	}

	for (int i = num - count * 9; i < num; i++)
	{
		int data = i;
		int sum = 0;
		v.clear();
		while (true)
		{
			v.push_back(data % 10);
			if (data / 10 > 0)
			{
				data /= 10;
			}
			else break;
		}
		for (int j = 0; j < v.size(); j++)
		{
			sum += v[j];
		}
		if (num == sum + i)
		{
			temp = i;
			isTrue = true;
			break;
		}
	}
	
	if (isTrue) cout << temp;
	else cout << 0;

	return 0;
}