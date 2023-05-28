#include <iostream>
#include <algorithm>
#include <vector>
#include <string>
#include <map>
#include <cmath>
using namespace std;

/*
Problem Number: 2108

Problem Description :
수를 처리하는 것은 통계학에서 상당히 중요한 일이다. 통계학에서 N개의 수를 대표하는 기본 통계값에는 다음과 같은 것들이 있다. 단, N은 홀수라고 가정하자.
산술평균 : N개의 수들의 합을 N으로 나눈 값
중앙값 : N개의 수들을 증가하는 순서로 나열했을 경우 그 중앙에 위치하는 값
최빈값 : N개의 수들 중 가장 많이 나타나는 값
범위 : N개의 수들 중 최댓값과 최솟값의 차이
N개의 수가 주어졌을 때, 네 가지 기본 통계값을 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/2108

Input:
첫째 줄에 수의 개수 N(1 ≤ N ≤ 500,000)이 주어진다. 단, N은 홀수이다. 그 다음 N개의 줄에는 정수들이 주어진다. 입력되는 정수의 절댓값은 4,000을 넘지 않는다.

Output:
첫째 줄에는 산술평균을 출력한다. 소수점 이하 첫째 자리에서 반올림한 값을 출력한다.
둘째 줄에는 중앙값을 출력한다.
셋째 줄에는 최빈값을 출력한다. 여러 개 있을 때에는 최빈값 중 두 번째로 작은 값을 출력한다.
넷째 줄에는 범위를 출력한다.

Limit: none
*/


int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int t, temp , min =4001,max= -4001;
	double sum = 0;
	vector<int> value;
	cin >> t;

	for (int T = 0; T < t; T++)
	{
		cin >> temp;

		sum += temp;
		value.push_back(temp);
	}

	sort(value.begin(), value.end());

	//평균
	float result = round(sum / t);
	if (result == 0)result = 0;
	cout << result << '\n';
	//중앙값
	cout << value[t / 2 ] << '\n';
	//최빈값
	map<int, int> data;
	int maxCount = 0;
	int maxValue=0;
	bool check = false;
	for (int num : value) 
	{
		data[num]++;
	}
	for (const auto& map : data) 
	{
		if (map.second >= maxCount)
		{
			if (!check && map.second == maxCount)
			{
				check = true;
				maxValue = map.first;
			}
			else if(!check)
			{
				maxCount = map.second;
				maxValue = map.first;
			}
			else if(check && map.second > maxCount)
			{
				check = false;
				maxCount = map.second;
				maxValue = map.first;
			}
		}
	}
	cout << maxValue << '\n';
	//범위
	cout << value.back() - value.front() << '\n';

	return 0;
}