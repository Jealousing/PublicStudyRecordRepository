#include <iostream>
using namespace std;

/*
Problem Number: 1330

Problem Description : �� ���� A�� B�� �־����� ��, A�� B�� ���ϴ� ���α׷��� �ۼ��Ͻÿ�.

Link: https://www.acmicpc.net/problem/1330

Input: ù° �ٿ� A�� B�� �־�����. A�� B�� ���� �� ĭ���� ���еǾ��� �ִ�.

Output: ù° �ٿ� ���� �� ���� �� �ϳ��� ����Ѵ�.
            A�� B���� ū ��쿡�� '>'�� ����Ѵ�.
            A�� B���� ���� ��쿡�� '<'�� ����Ѵ�.
            A�� B�� ���� ��쿡�� '=='�� ����Ѵ�.

Limit: -10,000 �� A, B �� 10,000
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