#include <iostream>
using namespace std;

/*
Problem Number: 1330

Problem Description : 두 정수 A와 B가 주어졌을 때, A와 B를 비교하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1330

Input: 첫째 줄에 A와 B가 주어진다. A와 B는 공백 한 칸으로 구분되어져 있다.

Output: 첫째 줄에 다음 세 가지 중 하나를 출력한다.
            A가 B보다 큰 경우에는 '>'를 출력한다.
            A가 B보다 작은 경우에는 '<'를 출력한다.
            A와 B가 같은 경우에는 '=='를 출력한다.

Limit: -10,000 ≤ A, B ≤ 10,000
*/


bool IsinRange(int value)
{
    if (-10000 <= value <= 10000) return true;
    else return false;
}

int main()
{
    int A = 0, B = 0;

    while (true)
    {
        cin >> A >> B;

        if (IsinRange(A) && IsinRange(B)) break;
    }

    if (A > B) cout << ">" << endl;
    else if (A < B) cout << "<" << endl;
    else cout << "==" << endl;

	return 0;
}