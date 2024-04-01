#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 11005

Problem Description :
10진법 수 N이 주어진다. 이 수를 B진법으로 바꿔 출력하는 프로그램을 작성하시오.
10진법을 넘어가는 진법은 숫자로 표시할 수 없는 자리가 있다. 이런 경우에는 다음과 같이 알파벳 대문자를 사용한다.
A: 10, B: 11, ..., F: 15, ..., Y: 34, Z: 35

Link: https://www.acmicpc.net/problem/11005

Input:
첫째 줄에 N과 B가 주어진다. (2 ≤ B ≤ 36) N은 10억보다 작거나 같은 자연수이다.

Output:
첫째 줄에 10진법 수 N을 B진법으로 출력한다.

Limit: none
*/

int main()
{
    int n = 0;
    int b;
    string result;

    cin >> n >> b;

    if (n == 0)
    {
        result = "0";
    }
    else
    {
        while (n > 0)
        {
            if (n % b < 10)
            {
                result += (char)(n % b + '0');
            }
            else
            {
                result += (char)(n % b - 10 + 'A');
            }
            n /= b;
        }
        reverse(result.begin(), result.end());
    }

    cout << result;

    return 0;
}