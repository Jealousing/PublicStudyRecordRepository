#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 24416

Problem Description :
오늘도 서준이는 동적 프로그래밍 수업 조교를 하고 있다. 아빠가 수업한 내용을 학생들이 잘 이해했는지 문제를 통해서 확인해보자.
오늘은 n의 피보나치 수를 재귀호출과 동적 프로그래밍으로 구하는 알고리즘을 배웠다. 
재귀호출에 비해 동적 프로그래밍이 얼마나 빠른지 확인해 보자. 
아래 의사 코드를 이용하여 n의 피보나치 수를 구할 경우 코드1 코드2 실행 횟수를 출력하자.

피보나치 수 재귀호출 의사 코드는 다음과 같다.
fib(n) {
    if (n = 1 or n = 2)
    then return 1;  # 코드1
    else return (fib(n - 1) + fib(n - 2));
}

피보나치 수 동적 프로그래밍 의사 코드는 다음과 같다.
fibonacci(n) {
    f[1] <- f[2] <- 1;
    for i <- 3 to n
        f[i] <- f[i - 1] + f[i - 2];  # 코드2
    return f[n];
}

Link: https://www.acmicpc.net/problem/24416

Input:
첫째 줄에 n(5 ≤ n ≤ 40)이 주어진다.

Output:
코드1 코드2 실행 횟수를 한 줄에 출력한다.

Limit: none
*/

int recursionCnt = 0;
//피보나치 수 재귀호출 의사 코드는 다음과 같다.
int fib(int n) 
{
    if (n == 1 || n == 2)
    {
        recursionCnt++;
        return 1;  //# 코드1
    }
    else return (fib(n - 1) + fib(n - 2));
}
#define MAXSIZE 40
int dynamicCnt = 0;
int arr[MAXSIZE] = { 0, };
//피보나치 수 동적 프로그래밍 의사 코드는 다음과 같다.
void fibonacci(int n) 
{
    arr[1] = 1, arr[2] = 1;
    for (int i = 3; i <= n; i++)
    {
        dynamicCnt++;
        arr[i] = arr[i - 1] + arr[i - 2];    //# 코드2
    }
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL); cout.tie(NULL);

    int n;
    cin >> n;

    fib(n);
    fibonacci(n);

    cout << recursionCnt << ' ' << dynamicCnt << '\n';
    return 0;
}