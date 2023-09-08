#include <iostream>
#include <algorithm>
#include <cmath>
#include <string>
using namespace std;

/*
Problem Number: 4134

Problem Description :
정수 n(0 ≤ n ≤ 4*10^9)가 주어졌을 때, n보다 크거나 같은 소수 중 가장 작은 소수 찾는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/4134

Input:
첫째 줄에 테스트 케이스의 개수가 주어진다. 각 테스트 케이스는 한 줄로 이루어져 있고, 정수 n이 주어진다.

Output:
각각의 테스트 케이스에 대해서 n보다 크거나 같은 소수 중 가장 작은 소수를 한 줄에 하나씩 출력한다.

Limit: none
*/

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int N;
	cin >> N;

	unsigned long long target, result, temp;

    for (int i = 0; i < N; i++) 
    {
        cin >> target;

        bool isCheck = target < 3;
        result = (target < 3) ? 2 : target;

        while (!isCheck)
        {
            // 값의 약수들의 곱은 그 값의 제곱근을 기준으로 대칭 이므로 제곱근 이하까지 검사
            temp = sqrt(target) + 1;
            for (int j = 2; j <= temp; j++)
            {
                if (target % j == 0) break;
                if (j == temp)
                {
                    result = target;
                    isCheck = true;
                }
            }
            target++;
        }
        cout << result << '\n';
    }

	return 0;
}