#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 1912

Problem Description :
n개의 정수로 이루어진 임의의 수열이 주어진다. 우리는 이 중 연속된 몇 개의 수를 선택해서 구할 수 있는 합 중 가장 큰 합을 구하려고 한다.
단, 수는 한 개 이상 선택해야 한다.
예를 들어서 10, -4, 3, 1, 5, 6, -35, 12, 21, -1 이라는 수열이 주어졌다고 하자. 여기서 정답은 12+21인 33이 정답이 된다.

Link: https://www.acmicpc.net/problem/1912

Input:
첫째 줄에 정수 n(1 ≤ n ≤ 100,000)이 주어지고 둘째 줄에는 n개의 정수로 이루어진 수열이 주어진다. 
수는 -1,000보다 크거나 같고, 1,000보다 작거나 같은 정수이다.

Output:
첫째 줄에 답을 출력한다.

Limit: none
*/


#define MAXSIZE 100001
long long int arr[MAXSIZE];
int maxValue = -1001,n;

void maxCheck()
{
    int sum = arr[0];

    for (int i = 1; i < n; i++)
    {
        // 진행해야될 합보다 새로운 값이 더 클경우 재설정
        if (sum + arr[i] < arr[i])
        {
            sum = arr[i];
        }
        else
        {
            sum += arr[i];
        }

        if (maxValue < sum) maxValue = sum;
    }

}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL); cout.tie(NULL);

    cin >> n;
    for (int i = 0; i < n; i++)
    {
        cin >> arr[i];
        if (maxValue < arr[i])
        {
            maxValue = arr[i];
        }
    }

    maxCheck();
    cout << maxValue << '\n';

    return 0;
}