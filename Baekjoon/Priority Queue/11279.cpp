#include <iostream>
#include <queue>
using namespace std;

/*
Problem Number: 11279

Problem Description :
널리 잘 알려진 자료구조 중 최대 힙이 있다. 최대 힙을 이용하여 다음과 같은 연산을 지원하는 프로그램을 작성하시오.

배열에 자연수 x를 넣는다.
배열에서 가장 큰 값을 출력하고, 그 값을 배열에서 제거한다.
프로그램은 처음에 비어있는 배열에서 시작하게 된다.

Link: https://www.acmicpc.net/problem/11279

Input:
첫째 줄에 연산의 개수 N(1 ≤ N ≤ 100,000)이 주어진다.
다음 N개의 줄에는 연산에 대한 정보를 나타내는 정수 x가 주어진다. 
만약 x가 자연수라면 배열에 x라는 값을 넣는(추가하는) 연산이고, x가 0이라면 배열에서 가장 큰 값을 출력하고 그 값을 배열에서 제거하는 경우이다.
입력되는 자연수는 231보다 작다.

Output: 
입력에서 0이 주어진 회수만큼 답을 출력한다. 만약 배열이 비어 있는 경우인데 가장 큰 값을 출력하라고 한 경우에는 0을 출력하면 된다.

Limit: none

*/

/*
13
0
1
2
0
0
3
2
1
0
0
0
0
0

0
2
1
3
2
1
0
0
*/

#define MAXSIZE 100001
int arr[MAXSIZE];
int n,cnt=0;

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);
    priority_queue<int> queue;
    // 입력
    cin >> n;
    for (int i = 0; i < n; i++) 
    {
        int temp;
        cin >> temp;
       
        if (temp == 0)
        {
            if (!queue.empty())
            {
                cout << queue.top() << '\n';
                queue.pop();
            }
            else
            {
                cout << '0'<<'\n';
            }
        }
        else
        {
            queue.push(temp);
        }
    }

    return 0;
}