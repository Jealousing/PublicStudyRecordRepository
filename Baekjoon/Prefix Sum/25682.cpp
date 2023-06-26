#include <iostream>
#include <algorithm>
#include <vector>
using namespace std;

/*
Problem Number: 25682

Problem Description :
지민이는 자신의 저택에서 MN개의 단위 정사각형으로 나누어져 있는 M×N 크기의 보드를 찾았다. 
어떤 정사각형은 검은색으로 칠해져 있고, 나머지는 흰색으로 칠해져 있다. 
지민이는 이 보드를 잘라서 K×K 크기의 체스판으로 만들려고 한다.
체스판은 검은색과 흰색이 번갈아서 칠해져 있어야 한다. 구체적으로, 
각 칸이 검은색과 흰색 중 하나로 색칠되어 있고, 변을 공유하는 두 개의 사각형은 다른 색으로 칠해져 있어야 한다.
따라서 이 정의를 따르면 체스판을 색칠하는 경우는 두 가지뿐이다. 하나는 맨 왼쪽 위 칸이 흰색인 경우, 하나는 검은색인 경우이다.
보드가 체스판처럼 칠해져 있다는 보장이 없어서, 지민이는 K×K 크기의 체스판으로 잘라낸 후에 몇 개의 정사각형을 다시 칠해야겠다고 생각했다. 
당연히 K×K 크기는 아무데서나 골라도 된다. 지민이가 다시 칠해야 하는 정사각형의 최소 개수를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/25682

Input:
첫째 줄에 정수 N, M, K가 주어진다. 둘째 줄부터 N개의 줄에는 보드의 각 행의 상태가 주어진다. B는 검은색이며, W는 흰색이다.

Output:
첫째 줄에 지민이가 잘라낸 K×K 보드를 체스판으로 만들기 위해 다시 칠해야 하는 정사각형 개수의 최솟값을 출력한다.

Limit:
1 ≤ N, M ≤ 2000
1 ≤ K ≤ min(N, M)
*/


#define MAXSIZE 2001
int arr[MAXSIZE][MAXSIZE];
int n, m,k, minValue= 2000001;

// 누적합
int PrefixSum()
{
    for (int i = 1; i <= n; i++)
    {
        for (int j = 1; j <= m; j++)
        {
            char inputColor;
            cin >> inputColor;
            // 1.1이 검정이여야 한다는 가정을 깔고 시작
            // i+j가 짝수 = 검정 , 홀수 = 흰색 / 다시칠해야되는경우 1로 설정.
            if (( (i + j) % 2 == 0 && inputColor != 'B') || ((i + j) % 2 != 0 && inputColor != 'W' )) arr[i][j] = 1;

            // 2차원배열의 누적합 =  전행값 + 전열값 - 중복값 + 자기값
            arr[i][j] = arr[i - 1][j] + arr[i][j - 1] - arr[i - 1][j - 1] + arr[i][j];
        }
    }

    for (int i = k; i <= n; i++)
    {
        for (int j = k; j <= m; j++)
        {
            // 2차원 배열에서 k*k 크기의 i,j까지의 합은
            // 칠하는 횟수 = 전체 영역 - 제거영역1 - 제거영역2 + 중복영역
            int cnt = arr[i][j] - arr[i - k][j] - arr[i][j - k] + arr[i - k][j - k];

            // 1.1이 검정으로 가정했으므로 흰색일 경우 반대  둘중더 작은 횟수 저장
            cnt = min(k * k - cnt, cnt);
            minValue = min(minValue, cnt);
       } 
    }

    return minValue;
}

int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    cin >> n >> m>>k;
    cout<<PrefixSum()<<'\n';


    return 0;
}