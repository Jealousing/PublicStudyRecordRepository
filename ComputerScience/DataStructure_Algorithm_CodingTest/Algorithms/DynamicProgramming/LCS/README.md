# 최장 공통 부분 수열 or 문자열 (Longest Common Subsequence or Substring, LCS)

## 최장 공통 부분 수열이란?
 최장 공통 부분 수열(Longest Common Subsequence, LCS)은 두 개의 주어진 수열에서 공통된 부분 수열 중에서 가장 긴 것을 말합니다. 선택된 요소들은 원래 수열에서 순서를 유지해야 합니다.   
예를 들어, 수열 A = {1, 3, 4, 1, 2, 1, 5, 3}와 수열 B = {1, 2, 3, 2, 1, 7, 1}에서 최장 공통 부분 수열은 {1, 2, 1}이며, 길이는 3입니다.   

## 최장 공통 부분 수열을 구하는 과정
 최장 공통 부분 수열을 구하는 과정은 일반적으로 동적 계획법(Dynamic Programming)을 사용합니다. 이 알고리즘은 두 개의 주어진 수열을 비교하면서 공통 부분 수열의 길이를 계산하고, 이를 테이블에 저장하여 최장 공통 부분 수열을 찾아냅니다.

1. 두 수열을 비교하여 공통 부분을 찾습니다.
2. 공통 부분을 찾을 때, 이전까지의 공통 부분 수열의 길이를 저장한 테이블을 활용합니다.
3. 동적 계획법을 통해 각 위치에서의 최장 공통 부분 수열의 길이를 계산하고, 이를 테이블에 저장합니다.
4. 테이블을 역추적하여 최장 공통 부분 수열을 찾아냅니다.

## 최장 공통 부분 수열의 특징
* 최장 공통 부분 수열은 원래 수열의 순서를 유지하므로, 원래 수열의 순서에 영향을 받습니다.
* 최장 공통 부분 수열은 여러 개 존재할 수 있습니다.
* 동적 계획법을 이용하여 효율적으로 구할 수 있습니다.
 
## 활용 예시코드

### int 수열의 최장 공통 부분 수열과 길이
```cpp
#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

// 최장 공통 부분 수열을 찾는 함수
vector<int> LCS(vector<int>& seq1, vector<int>& seq2) 
{
    int n = seq1.size();
    int m = seq2.size();
    vector<vector<int>> dp(n + 1, vector<int>(m + 1, 0));

    // 동적 프로그래밍을 사용하여 LCS 길이를 구함
    for (int i = 1; i <= n; ++i) 
    {
        for (int j = 1; j <= m; ++j) 
        {
            if (seq1[i - 1] == seq2[j - 1]) 
            {
                dp[i][j] = dp[i - 1][j - 1] + 1;
            } 
            else 
            {
                dp[i][j] = max(dp[i - 1][j], dp[i][j - 1]);
            }
        }
    }

    // 최장 공통 부분 수열을 구하기 위해 거꾸로 추적
    vector<int> lcs;
    int i = n, j = m;
    while (i > 0 && j > 0) 
    {
        if (seq1[i - 1] == seq2[j - 1]) 
        {
            lcs.push_back(seq1[i - 1]);
            i--;
            j--;
        } 
        else if (dp[i - 1][j] > dp[i][j - 1]) 
        {
            i--;
        } 
        else 
        {
            j--;
        }
    }
    reverse(lcs.begin(), lcs.end());
    return lcs;
}

int main() 
{
    vector<int> seq1 = {1, 3, 4, 1, 2, 1, 5, 3};
    vector<int> seq2 = {1, 2, 3, 2, 1, 7, 1};
    vector<int> lcs = LCS(seq1, seq2);
    
    cout << "최장 공통 부분 수열의 길이: " << lcs.size() << endl;
    cout << "최장 공통 부분 수열: ";
    for (int i = 0; i < lcs.size(); ++i) 
    {
        cout << lcs[i] << " ";
    }
    cout << endl;
    
    return 0;
}
```
		출력:
		최장 공통 부분 수열의 길이: 4
		최장 공통 부분 수열: {1 ,3 ,2 ,1}

### 문자열의 최장 공통 부분 문자열과 길이
```cpp
#include <iostream>
#include <vector>
#include <algorithm>

using namespace std;

// 최장 공통 부분 수열을 찾는 함수
pair<int, string> LCS(string& str1, string& str2)
{
    int n = str1.size();
    int m = str2.size();
    vector<vector<int>> dp(n + 1, vector<int>(m + 1, 0));
    vector<vector<string>> lcs(n + 1, vector<string>(m + 1, ""));

    // 동적 프로그래밍을 사용하여 LCS 길이를 구함
    for (int i = 1; i <= n; ++i)
    {
        for (int j = 1; j <= m; ++j)
        {
            if (str1[i - 1] == str2[j - 1])
            {
                dp[i][j] = dp[i - 1][j - 1] + 1;
                lcs[i][j] = lcs[i - 1][j - 1] + str1[i - 1];
            }
            else
            {
                if (dp[i - 1][j] > dp[i][j - 1])
                {
                    dp[i][j] = dp[i - 1][j];
                    lcs[i][j] = lcs[i - 1][j];
                }
                else
                {
                    dp[i][j] = dp[i][j - 1];
                    lcs[i][j] = lcs[i][j - 1];
                }
            }
        }
    }

    return { dp[n][m], lcs[n][m] };
}

int main()
{
    string str1 = "ABCBDAB";
    string str2 = "BDCAB";
    pair<int, string> result = LCS(str1, str2);

    cout << "최장 공통 부분 수열의 길이: " << result.first << endl;
    cout << "최장 공통 부분 수열: " << result.second << endl;

    return 0;
}
```

		출력:
		최장 공통 부분 문자열의 길이: 4
		최장 공통 부분 문자열: BDAB

### 문자열의 최장 공통 연속된 부분 문자열과 길이 
```cpp
#include <iostream>
#include <string>
#include <vector>

using namespace std;

// 두 문자열에서 최장 공통 연속 부분 문자열을 찾는 함수
pair<int, string> LCS(string& str1, string& str2) 
{
    int n = str1.size();
    int m = str2.size();
    vector<vector<int>> dp(n + 1, vector<int>(m + 1, 0));
    int maxLength = 0;
    int endX= 0, endY= 0;
    
    for (int i = 1; i <= n; ++i) 
    {
        for (int j = 1; j <= m; ++j) 
        {
            if (str1[i - 1] == str2[j - 1]) 
            {
                dp[i][j] = dp[i - 1][j - 1] + 1;
                if (dp[i][j] > maxLength) 
                {
                    maxLength = dp[i][j];
                    endX = i;
                    endY = j;
                }
            }
        }
    }

    // 최장 공통 연속 부분 문자열을 구함
    string lcs;
    while (dp[endX][endY] != 0) 
    {
        lcs = str1[endX - 1] + lcs;
        --endX;
        --endY;
    }
    
    return {maxLength, lcs};
}

int main() 
{
    string str1 = "ABCBDAB";
    string str2 = "BDCAB";
    pair<int, string> result = LCS(str1, str2);
    
    cout << "가장 긴 공통 연속 부분 문자열의 길이: " << result.first << endl;
    cout << "가장 긴 공통 연속 부분 문자열: " << result.second << endl;
    
    return 0;
}
```
		출력:
		가장 긴 공통 연속 부분 문자열의 길이: 2
		가장 긴 공통 연속 부분 문자열: AB