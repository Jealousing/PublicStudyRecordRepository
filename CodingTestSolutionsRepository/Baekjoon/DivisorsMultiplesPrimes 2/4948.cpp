#include <iostream>
#include <algorithm>
#include <cmath>
#include <string>
using namespace std;

/*
Problem Number: 4948

Problem Description :
베르트랑 공준은 임의의 자연수 n에 대하여, n보다 크고, 2n보다 작거나 같은 소수는 적어도 하나 존재한다는 내용을 담고 있다.
이 명제는 조제프 베르트랑이 1845년에 추측했고, 파프누티 체비쇼프가 1850년에 증명했다.
예를 들어, 10보다 크고, 20보다 작거나 같은 소수는 4개가 있다. (11, 13, 17, 19) 또, 14보다 크고, 28보다 작거나 같은 소수는 3개가 있다. (17,19, 23)
자연수 n이 주어졌을 때, n보다 크고, 2n보다 작거나 같은 소수의 개수를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/4948

Input:
입력은 여러 개의 테스트 케이스로 이루어져 있다. 각 케이스는 n을 포함하는 한 줄로 이루어져 있다.
입력의 마지막에는 0이 주어진다.

Output:
각 테스트 케이스에 대해서, n보다 크고, 2n보다 작거나 같은 소수의 개수를 출력한다.

Limit: 1 ≤ n ≤ 123,456
*/

int main()
{
	ios::sync_with_stdio(false);
	cin.tie(NULL); cout.tie(NULL);

    int target, temp,count=0;
    while (true)
    {
        int N;
        cin >> N;

        if (N == 0) break;
        else if (N == 1) count++;

        for (int i = N+1; i <= 2 * N; i++)
        {
            target = i;

            bool isCheck = target < 3;

            // 값의 약수들의 곱은 그 값의 제곱근을 기준으로 대칭 이므로 제곱근 이하까지 검사
            temp = sqrt(target) + 1;
            for (int j = 2; j <= temp; j++)
            {
                if (target % j == 0) break;
                if (j == temp)
                {
                    count++;
                    isCheck = true;
                }
            }
        }

        cout << count << '\n';
        count = 0;
    }

	return 0;
}