#include <iostream>
using namespace std;

/*
Problem Number: 1629

Problem Description :
자연수 A를 B번 곱한 수를 알고 싶다. 단 구하려는 수가 매우 커질 수 있으므로 이를 C로 나눈 나머지를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1629

Input:
첫째 줄에 A, B, C가 빈 칸을 사이에 두고 순서대로 주어진다. A, B, C는 모두 2,147,483,647 이하의 자연수이다.

Output:
첫째 줄에 A를 B번 곱한 수를 C로 나눈 나머지를 출력한다.

Limit: none

*/

/*
input: 10 11 12
output: 4

10^10 = 10^5 * 10^5

*/

long long A, B, C;

long long DivideAndConquer(int b)
{
    if (b == 0) return 1;
    else
    {
        long long temp = DivideAndConquer(b / 2);
        temp = temp * temp % C;
        if (b % 2 == 0) 
            return temp;
        else 
            return temp * A % C;
    }
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    cin >> A >> B >> C;
    cout << DivideAndConquer(B);

    return 0;
}