#include <iostream>
using namespace std;
const int MAX_SIZE = 1000;
/*
Problem Number: 10950

Problem Description : 
두 정수 A와 B를 입력받은 다음, A+B를 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10950

Input: 첫째 줄에 테스트 케이스의 개수 T가 주어진다.
각 테스트 케이스는 한 줄로 이루어져 있으며, 각 줄에 A와 B가 주어진다. (0 < A, B < 10)

Output: 각 테스트 케이스마다 A+B를 출력한다.

Limit: none
*/

int main()
{
    int A[MAX_SIZE],B[MAX_SIZE];
    int T = 0;

    while (true)
    {
        cin >> T;

        if (T < 0) continue;

        bool returnbool = true;
        for (int i = 0; i < T; i++)
        {
            cin >> A[i] >> B[i];
            if (! (0 < A[i] && B[i] < 10)) returnbool = false;
        }

        if (returnbool) break;
    }

    for (int i = 0; i < T; i++)
    {
        cout <<A[i]+B[i]<< endl;
    }

	return 0;
}