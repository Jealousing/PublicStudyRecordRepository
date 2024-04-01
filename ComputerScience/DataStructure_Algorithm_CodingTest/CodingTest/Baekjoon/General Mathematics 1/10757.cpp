#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 10757

Problem Description :
두 정수 A와 B를 입력받은 다음, A+B를 출력하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/10757

Input:
첫째 줄에 A와 B가 주어진다. (0 < A,B < 10^10000)

Output:
첫째 줄에 A+B를 출력한다.

Limit: none
*/

int main()
{
    string A, B,C;
    cin >> A >> B;
    
    int addNum = 0;
    int standard = 0;
    if (A.size() > B.size())
    {
        int check = A.size() - B.size();
        for (int i = 0; i < check; i++)
        {
            B = (char)(0 + '0') + B;
        }
    }
    else
    {
        int check = B.size() - A.size();
        for (int i = 0; i < check; i++)
        {
            A = (char)(0 + '0') + A;
        }
    }

    for (int i = A.size() - 1; i >= 0; i--)
    {
        int result = (int)(B[i] + A[i]) - ('0' * 2) + addNum;
        if (result > 9)
        {
            result -= 10;
            addNum = 1;
        }
        else
        {
            addNum = 0;
        }
        C = (char)(result + '0') + C;
    }
    if (addNum == 1)
    {
        C = (char)(1 + '0') + C;
    }
    cout << C << endl;

}