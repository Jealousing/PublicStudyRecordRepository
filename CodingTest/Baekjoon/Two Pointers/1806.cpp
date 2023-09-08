#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;

/*
Problem Number: 1806

Problem Description :
10,000 이하의 자연수로 이루어진 길이 N짜리 수열이 주어진다. 이 수열에서 연속된 수들의 부분합 중에 그 합이 S 이상이 되는 것 중, 가장 짧은 것의 길이를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/1806

Input:
첫째 줄에 N (10 ≤ N < 100,000)과 S (0 < S ≤ 100,000,000)가 주어진다. 둘째 줄에는 수열이 주어진다. 수열의 각 원소는 공백으로 구분되어져 있으며, 10,000이하의 자연수이다.

Output:
첫째 줄에 구하고자 하는 최소의 길이를 출력한다. 만일 그러한 합을 만드는 것이 불가능하다면 0을 출력하면 된다.

Limit: none
*/


int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 수열 크기 및 
    int n,s;
    cin >> n>>s;

    vector<int> vec;
    vec.resize(n);

    // 수열 입력
    for (int i = 0; i < n; ++i) 
    {
        cin >> vec[i]; 
    }

    int sum = 0, left = 0, right = 0, minValue = 2000000001;
    while (left<=right)
    {
        if (sum >= s)
        {
            // 타겟값s 보다 큰 합계값중 길이가 짧으면 저장 및 합계 줄이기
            minValue = min(minValue, right - left);
            sum -= vec[left++];
        }
        else if (right == n)break;
        else sum += vec[right++];
    }
    
    // 출력
    if (minValue == 2000000001) cout << 0 << '\n';
    else cout <<minValue<< '\n';

    return 0;
}