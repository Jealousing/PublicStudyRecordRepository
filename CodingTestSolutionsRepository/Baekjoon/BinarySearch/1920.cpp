#include <iostream>
#include <algorithm>
using namespace std;

/*
Problem Number: 1920

Problem Description :
N개의 정수 A[1], A[2], …, A[N]이 주어져 있을 때, 이 안에 X라는 정수가 존재하는지 알아내는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1920

Input:
첫째 줄에 자연수 N(1 ≤ N ≤ 100,000)이 주어진다. 
다음 줄에는 N개의 정수 A[1], A[2], …, A[N]이 주어진다.
다음 줄에는 M(1 ≤ M ≤ 100,000)이 주어진다. 다음 줄에는 M개의 수들이 주어지는데, 
이 수들이 A안에 존재하는지 알아내면 된다. 모든 정수의 범위는 -231 보다 크거나 같고 231보다 작다.

Output: 
M개의 줄에 답을 출력한다. 존재하면 1을, 존재하지 않으면 0을 출력한다.

Limit: none

*/

/*
5
4 1 5 2 3
5
1 3 7 9 5

1
1
0
0
1
*/

#define MAXSIZE 100001
int arr[MAXSIZE];
int n, m;

// 행렬 곱
void BinarySearch(int keyValue)
{
    int begin = 0, end = n - 1;

    while (begin<= end)
    {
        int mid = (begin + end) / 2;

        // 찾음
        if (arr[mid] == keyValue)
        {
            cout << "1\n";
            return;
        }
        // 작을경우
        else if (arr[mid] < keyValue)
        {
            begin = mid + 1;
        }
        else
        {
            end = mid - 1;
        }
    }
    cout << "0\n";
    return;
}


int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 입력 1
    cin >> n;
    for (int i = 0; i < n; i++) 
    {
        cin >> arr[i];
    }
   
    // 정렬
    sort(arr, arr + n);

    // 찾는 숫자 입력 및 결과 출력
    cin >> m;
    for (int i = 0; i < m; i++)
    {
        int temp;
        cin >> temp;
        BinarySearch(temp);
    }

    return 0;
}