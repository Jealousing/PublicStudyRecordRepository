#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 18870

Problem Description :
수직선 위에 N개의 좌표 X1, X2, ..., XN이 있다. 이 좌표에 좌표 압축을 적용하려고 한다.
Xi를 좌표 압축한 결과 X'i의 값은 Xi > Xj를 만족하는 서로 다른 좌표의 개수와 같아야 한다.
X1, X2, ..., XN에 좌표 압축을 적용한 결과 X'1, X'2, ..., X'N를 출력해보자.

Link: https://www.acmicpc.net/problem/18870

Input:
첫째 줄에 N이 주어진다.
둘째 줄에는 공백 한 칸으로 구분된 X1, X2, ..., XN이 주어진다.

Output:
첫째 줄에 X'1, X'2, ..., X'N을 공백 한 칸으로 구분해서 출력한다.

Limit: none
*/


int main()
{
	int N;
	cin >> N;
	
	long long tempValue;
	vector<long long> data1, data2;
	
	for (int i = 0; i < N; i++)
	{
		cin >> tempValue;
		data1.push_back(tempValue);
		data2.push_back(tempValue);
	}
	
	// 정렬 및 중복값 제거
	sort(data2.begin(), data2.end());
	data2.erase(unique(data2.begin(), data2.end()), data2.end());	

	/*
	시간초과
	for (int i = 0; i < N; i++)
	{
		for (int j = 0; j < data2.size();j++)
		{
			if (data1[i] == data2[j])
			{
				cout << j << " ";
			}
		}
	}
	*/
	for (int i = 0; i < N; i++)
	{
		cout << lower_bound( data2.begin(), data2.end(), data1[i]) - data2.begin() << " ";
	}

	return 0;
}