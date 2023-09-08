#include <iostream>
#include <algorithm>
using namespace std;

/*
Problem Number: 1463

Problem Description :
정수 X에 사용할 수 있는 연산은 다음과 같이 세 가지 이다.

X가 3으로 나누어 떨어지면, 3으로 나눈다.
X가 2로 나누어 떨어지면, 2로 나눈다.
1을 뺀다.
정수 N이 주어졌을 때, 위와 같은 연산 세 개를 적절히 사용해서 1을 만들려고 한다. 연산을 사용하는 횟수의 최솟값을 출력하시오.

Link: https://www.acmicpc.net/problem/1463

Input:
첫째 줄에 1보다 크거나 같고, 10^6보다 작거나 같은 정수 N이 주어진다.

Output:
첫째 줄에 연산을 하는 횟수의 최솟값을 출력한다.

Limit: none
*/

/*
n      과정                       횟수
1       1                            0
2       2,1                         1           2 나누어짐
3       3,1                         1           3 나누어짐
4       4,2,1                      2          2 나누어짐
5       5,4,2,1                   3          1빼고 2로나누어짐
6       6,3,1 or 6,2,1         2           둘다 나누어짐
7       7, +6,3,1 or 6,2,1    3          1빼고 둘다 나누어짐
8       8,4,2,1                   3          2 나누어짐
9       9,3,1                      2          3 나누어짐
10     10,9,3,1                  3          1빼고 3나누어짐 


3or 2로 나누어떨어지면 해당값/3 or 해당값/2 번째의 최소 횟수 +1로 횟수 체크가능
1을 빼면 그 전의 최소 횟수+1로 횟수체크가능


*/
#define MAXSIZE 1000001
int arr[MAXSIZE];
int n;

int dp()
{
    arr[1] = 0;
    arr[2] = 1;
    for (int i = 3; i <= n; i++)
    {
        if (i % 2 != 0 && i % 3 != 0) arr[i] = arr[i - 1] + 1;
        else if (i % 2 == 0 && i % 3 == 0) arr[i] = min(arr[i / 2] + 1, arr[i / 3] + 1);
        else if(i%2==0) arr[i] = min(arr[i / 2] + 1, arr[i - 1] + 1);
        else if (i%3==0) arr[i]= min(arr[i / 3] + 1, arr[i - 1] + 1);
    }

    return arr[n];

}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL); cout.tie(NULL);

    cin >> n;
    cout << dp() << '\n';

    return 0;
}