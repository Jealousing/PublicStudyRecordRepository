#include <iostream>
#include <vector>
#include <algorithm>
using namespace std;

/*
Problem Number: 3273

Problem Description :
n개의 서로 다른 양의 정수 a1, a2, ..., an으로 이루어진 수열이 있다. 
ai의 값은 1보다 크거나 같고, 1000000보다 작거나 같은 자연수이다. 
자연수 x가 주어졌을 때, ai + aj = x (1 ≤ i < j ≤ n)을 만족하는 (ai, aj)쌍의 수를 구하는 프로그램을 작성하시오.

Link: https://www.acmicpc.net/problem/3273

Input:
첫째 줄에 수열의 크기 n이 주어진다. 다음 줄에는 수열에 포함되는 수가 주어진다. 셋째 줄에는 x가 주어진다. (1 ≤ n ≤ 100000, 1 ≤ x ≤ 2000000)

Output:
문제의 조건을 만족하는 쌍의 개수를 출력한다.

Limit: none
*/


int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    // 수열 크기
    int n;
    cin >> n;

    vector<int> vec;
    vec.resize(n);

    // 수열 입력
    for (int i = 0; i < n; ++i) 
    {
        cin >> vec[i]; 
    }
    // 정렬
    sort(vec.begin(), vec.end());

    // 목표 합계
    int target;
    cin >> target;

    int left = 0, right = n - 1, cnt = 0;
    while (left<right)
    {
        int sum = vec[left] + vec[right];

        if (sum == target)
        {
            cnt++; left++; right--;
        }
        else if (sum < target) left++;
        else right--;
    }
    
    // 출력
    cout << cnt << '\n';

    return 0;
}