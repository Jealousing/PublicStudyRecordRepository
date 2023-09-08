#include <iostream>
#include <algorithm>
#include <string>
using namespace std;
/*
Problem Number: 2444

Problem Description :
예제를 보고 규칙을 유추한 뒤에 별을 찍어 보세요.

Link: https://www.acmicpc.net/problem/2444

Input:
첫째 줄에 N(1 ≤ N ≤ 100)이 주어진다.

Output:
첫째 줄부터 2×N-1번째 줄까지 차례대로 별을 출력한다.

Limit: none
*/

int main()
{
    int N;
    int count = 0;
    int addnum = 1;
    cin >> N;

    for (int i = 0; i < 2 * N - 1; i++)
    {
        for (int j = 0; j < 2 * N - 1; j++)
        {
            if (j < (2 * N - 1) / 2 - count)
            {
                cout << " ";
            }
            else if (j > (2 * N - 1) / 2 + count)
            {
               
            }
            else
            {
                cout << "*";
            }
        }
  
        if (i == (2 * N - 1) / 2)
        {
            addnum = -1;
        }
        count += addnum;
        cout << endl;
    }
   


    return 0;
}