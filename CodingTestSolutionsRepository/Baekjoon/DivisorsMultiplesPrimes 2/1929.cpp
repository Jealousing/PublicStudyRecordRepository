#include <iostream>
#include <algorithm>
#include <cmath>
#include <string>
using namespace std;

/*
Problem Number: 1929

Problem Description :
M이상 N이하의 소수를 모두 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1929

Input:
첫째 줄에 자연수 M과 N이 빈 칸을 사이에 두고 주어진다. (1 ≤ M ≤ N ≤ 1,000,000) M이상 N이하의 소수가 하나 이상 있는 입력만 주어진다.

Output:
한 줄에 하나씩, 증가하는 순서대로 소수를 출력한다

Limit: none
*/

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

	int N,M;
	cin >> N>>M;

	int target, result, temp;

    for (int i = N; i <= M; i++) 
    {
        target = i;

        bool isCheck = target < 3;
        result = (target < 3) ? 2 : target;


        // 값의 약수들의 곱은 그 값의 제곱근을 기준으로 대칭 이므로 제곱근 이하까지 검사
        temp = sqrt(target)+1;
        for (int j = 2; j <= temp; j++)
        {
            if (target % j == 0) break;
            if (j == temp)
            {
                result = target;
                isCheck = true;
            }
        }

        if (isCheck && result>1) cout << result << '\n';

    }

	return 0;
}