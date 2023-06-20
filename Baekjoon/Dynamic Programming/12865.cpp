#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 12865

Problem Description :
이 문제는 아주 평범한 배낭에 관한 문제이다.
한 달 후면 국가의 부름을 받게 되는 준서는 여행을 가려고 한다. 
세상과의 단절을 슬퍼하며 최대한 즐기기 위한 여행이기 때문에, 가지고 다닐 배낭 또한 최대한 가치 있게 싸려고 한다.
준서가 여행에 필요하다고 생각하는 N개의 물건이 있다. 각 물건은 무게 W와 가치 V를 가지는데, 해당 물건을 배낭에 넣어서 가면 준서가 V만큼 즐길 수 있다.
아직 행군을 해본 적이 없는 준서는 최대 K만큼의 무게만을 넣을 수 있는 배낭만 들고 다닐 수 있다. 
준서가 최대한 즐거운 여행을 하기 위해 배낭에 넣을 수 있는 물건들의 가치의 최댓값을 알려주자.

Link: https://www.acmicpc.net/problem/12865

Input:
첫 줄에 물품의 수 N(1 ≤ N ≤ 100)과 준서가 버틸 수 있는 무게 K(1 ≤ K ≤ 100,000)가 주어진다.
두 번째 줄부터 N개의 줄에 거쳐 각 물건의 무게 W(1 ≤ W ≤ 100,000)와 해당 물건의 가치 V(0 ≤ V ≤ 1,000)가 주어진다.
입력으로 주어지는 모든 수는 정수이다.

Output:
한 줄에 배낭에 넣을 수 있는 물건들의 가치합의 최댓값을 출력한다.

Limit: none
*/

/* 
4 7

6 13
4 8
3 6
5 12

14

    0   1   2   3   4   5   6   7
1                                13  13
2                      8  8   13  13
3                6   8  8   13  14
4                6   8  12  13  14


*/

#define MAXSIZE1 101
#define MAXSIZE2 100001
int dp[MAXSIZE1][MAXSIZE2];
int w[MAXSIZE1], v[MAXSIZE2];
int n,k;

int DynamicProgramming()
{
    for (int i = 1; i <= n; i++)
    {
        for (int j = 1; j <= k; j++)
        {
            // 포함이 될 수 있는 무게일 경우, max( 탐색했던 물건들로만 무게 j를 만드는 경우, 탐색했던 물건들로 무게 [j - w[i-1]] 를 만들고 지금 해당하는 물건을 넣는경우 )  
            if (j >= w[i - 1]) dp[i][j] = max(dp[i - 1][j], dp[i - 1][j - w[i - 1]] + v[i - 1]);
            else dp[i][j] = dp[i - 1][j];
        }
    }
    return dp[n][k];
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    cin >> n >> k;

    for (int i = 0; i < n; i++)
    {
        cin >> w[i] >> v[i];
    }
    cout << DynamicProgramming() << '\n';

    return 0;
}