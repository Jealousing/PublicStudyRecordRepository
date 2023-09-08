#include <iostream>
#include <algorithm>
using namespace std;

/*
Problem Number: 2579

Problem Description :
계단 오르기 게임은 계단 아래 시작점부터 계단 꼭대기에 위치한 도착점까지 가는 게임이다. <그림 1>과 같이 각각의 계단에는 일정한 점수가 쓰여 있는데 계단을 밟으면 그 계단에 쓰여 있는 점수를 얻게 된다.
https://upload.acmicpc.net/7177ea45-aa8d-4724-b256-7b84832c9b97/-/preview/
예를 들어 <그림 2>와 같이 시작점에서부터 첫 번째, 두 번째, 네 번째, 여섯 번째 계단을 밟아 도착점에 도달하면 총 점수는 10 + 20 + 25 + 20 = 75점이 된다.
https://upload.acmicpc.net/f00b6121-1c25-492e-9bc0-d96377c586b0/-/preview/
계단 오르는 데는 다음과 같은 규칙이 있다.

계단은 한 번에 한 계단씩 또는 두 계단씩 오를 수 있다. 즉, 한 계단을 밟으면서 이어서 다음 계단이나, 다음 다음 계단으로 오를 수 있다.
연속된 세 개의 계단을 모두 밟아서는 안 된다. 단, 시작점은 계단에 포함되지 않는다.
마지막 도착 계단은 반드시 밟아야 한다.
따라서 첫 번째 계단을 밟고 이어 두 번째 계단이나, 세 번째 계단으로 오를 수 있다. 하지만, 첫 번째 계단을 밟고 이어 네 번째 계단으로 올라가거나, 첫 번째, 두 번째, 세 번째 계단을 연속해서 모두 밟을 수는 없다.

각 계단에 쓰여 있는 점수가 주어질 때 이 게임에서 얻을 수 있는 총 점수의 최댓값을 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/2579

Input:
입력의 첫째 줄에 계단의 개수가 주어진다.
둘째 줄부터 한 줄에 하나씩 제일 아래에 놓인 계단부터 순서대로 각 계단에 쓰여 있는 점수가 주어진다. 
계단의 개수는 300이하의 자연수이고, 계단에 쓰여 있는 점수는 10,000이하의 자연수이다.

Output:
첫째 줄에 계단 오르기 게임에서 얻을 수 있는 총 점수의 최댓값을 출력한다.

Limit: none
*/

/*
6

n1 10      10 = n1                                                      -> maxSum[0] = arr[0]
n2 20      30 = n1+n2                                               -> maxSum[1] = maxSum[0] + arr[1]
n3 15       25 = n1+n3,  35 = n2+n3                           -> maxSum[2] =  max( maxSum[0]+arr[2], arr[1]+arr[2] )
n4 25      50 = n1+n3+n4, 55= n1+n2+n4                  -> maxSum[3] =  max( maxSum[1]+arr[3], maxSum[0]+arr[2]+arr[3] )
n5 10      65 = n1+n2+n4+n5 45 = n2+n3 +n5           -> maxSum[4] =  max( maxSum[2]+arr[4], maxSum[1]+arr+[3]+arr[4] )
n6 20     65 = n2+n3+n5+n6 75 = n1+n2+n4+n6      -> maxSum[5] =  max( maxSum[3]+arr[5], maxSum[2]+arr[5]+arr[6] )

75
*/

#define MAXSIZE 301
int arr[MAXSIZE], maxSum[MAXSIZE];
int n;

 int maxValue()
{
    maxSum[0] = arr[0];
    maxSum[1] = maxSum[0]+arr[1];
    maxSum[2] = max(maxSum[0] + arr[2], arr[1] + arr[2]);

    for (int i = 3; i < n; i++)
    {
        maxSum[i] = max(maxSum[i - 2] + arr[i], maxSum[i - 3] + arr[i - 1] + arr[i]);
    }

    return maxSum[n-1];
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL); cout.tie(NULL);

    cin >> n;
    for (int i = 0; i < n; i++)
    {
        cin >> arr[i];
    }

    cout << maxValue() << '\n';

    return 0;
}