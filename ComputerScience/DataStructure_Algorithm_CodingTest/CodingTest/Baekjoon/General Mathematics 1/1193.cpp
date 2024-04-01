#include <iostream>
#include <algorithm>
#include <string>
using namespace std;

/*
Problem Number: 1193

Problem Description :
무한히 큰 배열에 다음과 같이 분수들이 적혀있다.
이와 같이 나열된 분수들을 1/1 → 1/2 → 2/1 → 3/1 → 2/2 → … 과 같은 지그재그 순서로 차례대로 1번, 2번, 3번, 4번, 5번, … 분수라고 하자.
X가 주어졌을 때, X번째 분수를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1193

Input:
첫째 줄에 X(1 ≤ X ≤ 10,000,000)가 주어진다.

Output:
첫째 줄에 분수를 출력한다.

Limit: none
*/

int main()
{
   /* 
                                             총합 갯수= 총합-1
    1/1                                     2                      
    1/2 2/1                               3
    3/1 2/2 1/3                         4
    1/4 2/3 3/2 4/1                   5
    5/1 4/2 3/3 2/4 1/5            6
    1/6 ~ 6/1

   */
 
    int count = 0;
    cin >> count;

    int maxNum = 2, head = 0, tail = 1;
    bool dir = false;

    for (int i = 0; i < count; i++)
    {
        if (dir)
        {
            if (head - 1 > 0)
            {
                head--;
                tail++;
            }
            else
            {
                tail++;
                dir = false;
            }
        }
        else
        {
            if (tail - 1 > 0)
            {
                head++;
                tail--;
            }
            else
            {
                head++;
                dir = true;
            }
        }
    }
    cout << head << "/" << tail << endl;

    return 0;
}