#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 2565

Problem Description :
 전봇대 A와 B 사이에 하나 둘씩 전깃줄을 추가하다 보니 전깃줄이 서로 교차하는 경우가 발생하였다. 합선의 위험이 있어 이들 중 몇 개의 전깃줄을 없애 전깃줄이 교차하지 않도록 만들려고 한다.
예를 들어, < 그림 1 >과 같이 전깃줄이 연결되어 있는 경우 A의 1번 위치와 B의 8번 위치를 잇는 전깃줄, A의 3번 위치와 B의 9번 위치를 잇는 전깃줄, 
A의 4번 위치와 B의 1번 위치를 잇는 전깃줄을 없애면 남아있는 모든 전깃줄이 서로 교차하지 않게 된다.
https://upload.acmicpc.net/d90221dd-eb80-419f-bdfb-5dd4ebac23af/-/preview/
전깃줄이 전봇대에 연결되는 위치는 전봇대 위에서부터 차례대로 번호가 매겨진다. 
전깃줄의 개수와 전깃줄들이 두 전봇대에 연결되는 위치의 번호가 주어질 때, 남아있는 모든 전깃줄이 서로 교차하지 않게 하기 위해 없애야 하는 전깃줄의 최소 개수를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/2565

Input:
첫째 줄에는 두 전봇대 사이의 전깃줄의 개수가 주어진다. 전깃줄의 개수는 100 이하의 자연수이다. 
둘째 줄부터 한 줄에 하나씩 전깃줄이 A전봇대와 연결되는 위치의 번호와 B전봇대와 연결되는 위치의 번호가 차례로 주어진다. 
위치의 번호는 500 이하의 자연수이고, 같은 위치에 두 개 이상의 전깃줄이 연결될 수 없다.

Output:
첫째 줄에 남아있는 모든 전깃줄이 서로 교차하지 않게 하기 위해 없애야 하는 전깃줄의 최소 개수를 출력한다.

Limit: none
*/

/* 
8

1 8               1  8
3 9              2  2
2 2              3  9
4 1              4  1
6 4             6   4
10 10           7   6
9 7             9   7
7 6             10  10

3
*/

#define MAXSIZE 1001
int dp[MAXSIZE];
vector<pair<int, int>> arr;
int n, maximum = 0;

int DynamicProgramming()
{
    sort(arr.begin(), arr.end());

    for (int i = 0; i < n; i++)
    {
        dp[i] = 1;
        for (int j = 0; j < i; j++)
        {
            if (arr[i].second > arr[j].second)
            {
                dp[i] = max(dp[i], dp[j] + 1);
            }
        }
        maximum = max(maximum, dp[i]);
    }	

    return n - maximum;
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL); cout.tie(NULL);

    cin >> n;
    for (int i = 0; i < n; i++)
    {
        int temp1, temp2;
        cin >> temp1 >> temp2;
        arr.push_back(make_pair(temp1, temp2));
    }
    cout << DynamicProgramming() << '\n';

    return 0;
}