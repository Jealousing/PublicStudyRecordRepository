#include <iostream>
using namespace std;

/*
Problem Number: 2739

Problem Description : 
N을 입력받은 뒤, 구구단 N단을 출력하는 프로그램을 작성하시오. 출력 형식에 맞춰서 출력하면 된다.

Link: https://www.acmicpc.net/problem/2739

Input: 첫째 줄에 N이 주어진다. N은 1보다 크거나 같고, 9보다 작거나 같다.

Output: 출력형식과 같게 N*1부터 N*9까지 출력한다.

Limit: none
*/


bool IsinRange(int value)
{
    if (1 <= value <= 9) return true;
    else return false;
}

int main()
{
    int A = 0;

    while (true)
    {
        cin >> A;

        if (IsinRange(A)) break;
    }

    for (int i = 1; i <= 9; i++)
    {
        cout << A <<" * " << i << " = " << A * i << endl;
    }

	return 0;
}