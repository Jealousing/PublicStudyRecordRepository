/*
Problem Number: 1991

Problem Description :
이진 트리를 입력받아 전위 순회(preorder traversal), 중위 순회(inorder traversal), 후위 순회(postorder traversal)한 결과를 출력하는 프로그램을 작성하시오
https://www.acmicpc.net/JudgeOnline/upload/201007/trtr.png

예를 들어 위와 같은 이진 트리가 입력되면,

전위 순회한 결과 : ABDCEFG // (루트) (왼쪽 자식) (오른쪽 자식)
중위 순회한 결과 : DBAECFG // (왼쪽 자식) (루트) (오른쪽 자식)
후위 순회한 결과 : DBEGFCA // (왼쪽 자식) (오른쪽 자식) (루트)
가 된다.

Link: https://www.acmicpc.net/problem/1991

Input:
첫째 줄에는 이진 트리의 노드의 개수 N(1 ≤ N ≤ 26)이 주어진다. 둘째 줄부터 N개의 줄에 걸쳐 각 노드와 그의 왼쪽 자식 노드, 오른쪽 자식 노드가 주어진다. 
노드의 이름은 A부터 차례대로 알파벳 대문자로 매겨지며, 항상 A가 루트 노드가 된다. 자식 노드가 없는 경우에는 .으로 표현한다.

Output:
첫째 줄에 전위 순회, 둘째 줄에 중위 순회, 셋째 줄에 후위 순회한 결과를 출력한다. 각 줄에 N개의 알파벳을 공백 없이 출력하면 된다.

Limit: none
*/


#include <iostream>
#include <algorithm>
using namespace std;

#define MAXSIZE 26

pair<char, char> arr[MAXSIZE];

bool check(char c)
{
    if (c == '.') return true;
    else return false;
}

void preOrderTraversal(char c)
{
    if (check(c))return;

    cout << c;
    preOrderTraversal(arr[c - 'A'].first);
    preOrderTraversal(arr[c - 'A'].second);
}
void inOrderTraversal(char c)
{
    if (check(c))return;

    inOrderTraversal(arr[c - 'A'].first);
    cout << c;
    inOrderTraversal(arr[c - 'A'].second);
}
void postOrderTraversal(char c)
{
    if (check(c))return;

    postOrderTraversal(arr[c - 'A'].first);
    postOrderTraversal(arr[c - 'A'].second);
    cout << c;
}
int main()
{
    ios::sync_with_stdio(false);
    cin.tie(NULL);
    cout.tie(NULL);

    int n;
    cin >> n;

    // 입력
    for (int i = 0; i < n; i++)
    {
        char x, y,z;
        cin >> x >> y>>z;
        arr[x - 'A'].first = y;
        arr[x - 'A'].second = z;
    }

    // 탐색 및 출력
    preOrderTraversal('A');
    cout << "\n";
    inOrderTraversal('A');
    cout << "\n";
    postOrderTraversal('A');

    return 0;
}