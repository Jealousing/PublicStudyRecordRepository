/*
Problem Number: 28278

Problem Description :
정수를 저장하는 스택을 구현한 다음, 입력으로 주어지는 명령을 처리하는 프로그램을 작성하시오.

명령은 총 다섯 가지이다.

1 X: 정수 X를 스택에 넣는다. (1 ≤ X ≤ 100,000)
2: 스택에 정수가 있다면 맨 위의 정수를 빼고 출력한다. 없다면 -1을 대신 출력한다.
3: 스택에 들어있는 정수의 개수를 출력한다.
4: 스택이 비어있으면 1, 아니면 0을 출력한다.
5: 스택에 정수가 있다면 맨 위의 정수를 출력한다. 없다면 -1을 대신 출력한다.

Link: https://www.acmicpc.net/problem/28278

Input:
첫째 줄에 명령의 수 N이 주어진다. (1 ≤ N ≤ 1,000,000)
둘째 줄부터 N개 줄에 명령이 하나씩 주어진다.
출력을 요구하는 명령은 하나 이상 주어진다.

Output:
출력을 요구하는 명령이 주어질 때마다 명령의 결과를 한 줄에 하나씩 출력한다.

Limit: none
*/

#include <iostream>
#include <algorithm>
using namespace std;

#define MAXSIZE 1000002
int stack[MAXSIZE], point = 1;

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 입력 
    int n;
    cin >> n;

    for (int i = 0; i < n; i++)
    {
        int temp;
        cin >> temp;
        switch (temp)
        {
        case 1:
            int x;
            cin >> x;
            stack[point++] = x;
            break;
        case 2:
            if (point > 1)
            {
                point--;
                cout << stack[point] << '\n';
            }
            else
            {
                cout << -1 << '\n';
            }
            break;
        case 3:
            cout << point-1 << '\n';
            break;
        case 4:
            if (point > 1)
            {
                cout << 0 << '\n';
            }
            else
            {
                cout << 1 << '\n';
            }
            break;
        case 5:
            if (point > 1)
            {
                cout << stack[point-1] << '\n';
            }
            else
            {
                cout << -1 << '\n';
            }
            break;
        default:
            break;
        }
    }
    return 0;
}