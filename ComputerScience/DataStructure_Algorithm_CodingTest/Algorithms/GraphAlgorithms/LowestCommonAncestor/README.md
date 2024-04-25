# 최소 공통 조상(Lowest Common Ancestor, LCA)

## 최소 공통 조상이란?
 주로 트리 자료구조에서 주어진 두 노드의 가장 가까운 공통 조상을 찾는 알고리즘입니다. 주로 트리 구조 상에서 두 노드 간의 가장 가까운 공통 조상을 찾는 데 활용됩니다.

### 최소 공통 조상을 구하는 과정
1. 깊이 맞추기: 주어진 두 노드의 깊이(depth)를 일치시킵니다. 깊이가 더 깊은 노드를 부모 노드 방향으로 이동시켜 두 노드가 같은 깊이에 있도록 합니다.
2. 조상 노드 비교: 이제 두 노드의 깊이가 같으므로, 두 노드의 조상 노드들을 순차적으로 비교합니다. 두 노드가 같은 조상을 가질 때까지 각각의 노드를 부모 노드 방향으로 이동시킵니다.
3. 최소 공통 조상 찾기: 두 노드가 같은 조상을 가지게 되면, 그 조상이 두 노드의 최소 공통 조상(LCA)이 됩니다.

## 최소 공통 조상 특징
* 트리 자료구조 전용: LCA 알고리즘은 주로 트리 자료구조에 적용됩니다. 이는 트리가 그래프 중에서도 계층적인 구조를 가지며 루트 노드로부터의 거리가 정의되어 있기 때문입니다.
* 공통 조상 탐색: 주어진 두 노드의 가장 가까운 공통 조상을 찾는 데에 사용됩니다.


## 다양한 방법으로 LCA구하기

### 세그먼트 트리
* 세그먼트 트리를 사용하여 LCA를 구하는 방법은 주로 트리의 깊이 정보를 세그먼트 트리에 저장합니다.
* 두 노드 사이의 깊이 중 최소 값을 쿼리하여 LCA를 찾습니다.
* 전처리 시간이 조금 덜 소요되지만, 각 쿼리에 대한 응답 시간은 로그 시간에 비례합니다.

#### 시간복잡도
 세그먼트 트리를 활용하여 LCA를 구하는 경우, 트리를 전처리하는 데 O(N)의 시간이 소요되고, 각 쿼리마다 O(logN)의 시간이 필요합니다. 따라서 전체 시간 복잡도는 O(NlogN)입니다.

#### 예시코드 c++
```cpp
#include <iostream>
#include <vector>
#include <cmath>

using namespace std;

const int MAX = 10005; // 최대 노드 수

vector<int> tree[MAX]; // 트리 구조를 저장할 배열
int depth[MAX]; // 각 노드의 깊이를 저장할 배열
int parent[MAX]; // 각 노드의 부모를 저장할 배열
int segTree[4 * MAX]; // 세그먼트 트리 배열

// DFS를 통해 각 노드의 깊이와 부모 노드를 설정하는 함수
void DFS(int node, int par, int d)
{
    depth[node] = d;
    parent[node] = par;
    for (int i = 0; i < tree[node].size(); ++i)
    {
        int next = tree[node][i];
        if (next != par)
        {
            DFS(next, node, d + 1);
        }
    }
}

// 세그먼트 트리를 구축하는 함수
void BuildSegmentTree(int node, int start, int end)
{
    if (start == end)
    {
        segTree[node] = start;
    }
    else
    {
        int mid = (start + end) / 2;
        BuildSegmentTree(2 * node, start, mid);
        BuildSegmentTree(2 * node + 1, mid + 1, end);
        // 깊이가 작은 노드를 선택하도록 함
        segTree[node] = (depth[segTree[2 * node]] < depth[segTree[2 * node + 1]]) ? segTree[2 * node] : segTree[2 * node + 1];
    }
}

// LCA를 찾는 함수
int Query(int node, int start, int end, int left, int right)
{
    if (start > right || end < left)
        return -1; // 범위를 벗어난 경우
    if (left <= start && end <= right)
        return segTree[node]; // 현재 노드의 범위가 쿼리 범위에 완전히 포함되는 경우

    int mid = (start + end) / 2;
    int leftChild = Query(2 * node, start, mid, left, right);
    int rightChild = Query(2 * node + 1, mid + 1, end, left, right);

    if (leftChild == -1)
        return rightChild;
    if (rightChild == -1)
        return leftChild;

    return (depth[leftChild] < depth[rightChild]) ? leftChild : rightChild; // 두 자식 노드 중 더 깊은 노드 반환
}

// LCA를 구하는 함수
int LCA(int u, int v)
{
    // 두 노드의 깊이를 같게 만듭니다.
    while (depth[u] != depth[v])
    {
        if (depth[u] > depth[v])
            u = parent[u];
        else
            v = parent[v];
    }

    // 두 노드가 같은 노드가 될 때까지 올라갑니다.
    while (u != v)
    {
        u = parent[u];
        v = parent[v];
    }

    return u; // 혹은 v를 반환해도 됩니다.
}

int main()
{
    int n; // 노드의 수
    cin >> n;

    // 트리 입력 받기
    for (int i = 0; i < n - 1; ++i)
    {
        int u, v;
        cin >> u >> v;
        tree[u].push_back(v);
        tree[v].push_back(u);
    }

    // 루트 설정 및 DFS 수행
    int root = 1; // 루트 노드 번호
    DFS(root, -1, 0);

    // 세그먼트 트리 구축
    BuildSegmentTree(1, 1, n);

    int q; // 쿼리의 수
    cin >> q;

    while (q--)
    {
        int u, v;
        cin >> u >> v;
        // 깊이가 더 깊은 노드를 LCA로 설정
        if (depth[u] > depth[v])
            swap(u, v);
        int lca = Query(1, 1, n, u, v);
        cout << "LCA of " << u << " and " << v << " is " << lca << endl;
    }

    return 0;
}
```

### DP (다이나믹 프로그래밍)
* DP를 사용하여 LCA를 구하는 방법은 주로 희소 테이블(Sparse Table)을 활용합니다.
* 각 노드의 2^i번째 조상을 저장하는 테이블을 구성하고, 이를 활용하여 두 노드의 가장 가까운 공통 조상을 찾습니다.
* 전처리 시간과 공간이 좀 더 많이 소요될 수 있지만, 각 쿼리에 대한 응답 시간은 상수 시간에 가깝습니다.

		희소 테이블(Sparse Table)은 주어진 배열 또는 표에서 각 구간에 대한 연산 결과를 미리 계산하여 저장하는 자료구조입니다. 이를 통해 배열 또는 표의 구간 쿼리를 효율적으로 처리할 수 있습니다.	

#### 시간복잡도
 * 일반적인 DP: 전처리 과정에서 각 노드의 깊이를 계산하는데 O(N),각 쿼리에서 두 노드의 LCA를 찾는 과정은 최악의 경우에 O(N),  따라서 전체 시간복잡도는 O(NQ)가 됩니다. 여기서 Q는 쿼리의 수입니다.
 * 희소테이블을 사용하는 DP: 전처리 과정에서 O(NlogN) , 각 쿼리에서 두 노드의 LCA를 찾는 과정은 O(logN), 따라서 전체 시간복잡도는 O(NlogN)

#### 일반적인 DP를 사용한 예시코드 c++
```cpp
#include <iostream>
#include <vector>

using namespace std;

const int MAX = 10005; // 최대 노드 수

vector<int> tree[MAX]; // 트리 구조를 저장할 배열
int depth[MAX]; // 각 노드의 깊이를 저장할 배열
int parent[MAX]; // 각 노드의 부모를 저장할 배열

// DFS를 통해 각 노드의 깊이와 부모 노드를 설정하는 함수
void DFS(int node, int par, int d)
{
    depth[node] = d;
    parent[node] = par;
    for (int i = 0; i < tree[node].size(); ++i)
    {
        int next = tree[node][i];
        if (next != par)
        {
            DFS(next, node, d + 1);
        }
    }
}

// LCA를 구하는 함수
int LCA(int u, int v)
{
    // 두 노드의 깊이를 같게 만듭니다.
    while (depth[u] > depth[v]) u = parent[u];
    while (depth[v] > depth[u]) v = parent[v];

    // 두 노드가 같은 부모를 가질 때까지 부모를 비교합니다.
    while (u != v)
    {
        u = parent[u];
        v = parent[v];
    }

    return u; // 혹은 v를 반환해도 됩니다.
}

int main()
{
    int n; // 노드의 수
    cin >> n;

    // 트리 입력 받기
    for (int i = 0; i < n - 1; ++i)
    {
        int u, v;
        cin >> u >> v;
        tree[u].push_back(v);
        tree[v].push_back(u);
    }

    // 루트 설정 및 DFS 수행
    int root = 1; // 루트 노드 번호
    DFS(root, -1, 0);

    int q; // 쿼리의 수
    cin >> q;

    while (q--)
    {
        int u, v;
        cin >> u >> v;
        int ancestor = LCA(u, v);
        cout << "LCA of " << u << " and " << v << " is " << ancestor << endl;
    }

    return 0;
}
```

#### 희소테이블을 사용한 예시코드 c++
```cpp
#include <iostream>
#include <vector>

using namespace std;

const int MAX = 10005; // 최대 노드 수
const int LOGMAX = 15; // log(MAX)의 올림값

vector<int> tree[MAX]; // 트리 구조를 저장할 배열
int parent[MAX][LOGMAX]; // 각 노드의 2^i번째 조상을 저장할 배열
int depth[MAX]; // 각 노드의 깊이를 저장할 배열

// DFS를 통해 각 노드의 깊이와 부모 노드를 설정하는 함수
void DFS(int node, int par, int d)
{
    depth[node] = d;
    parent[node][0] = par;
    for (int i = 0; i < tree[node].size(); ++i)
    {
        int next = tree[node][i];
        if (next != par)
        {
            DFS(next, node, d + 1);
        }
    }
}

// 희소 테이블을 미리 계산하는 함수
void PrecomputeSparseTable(int n)
{
    for (int j = 1; j < LOGMAX; ++j)
    {
        for (int i = 1; i <= n; ++i)
        {
            if (parent[i][j - 1] != -1)
            {
                parent[i][j] = parent[parent[i][j - 1]][j - 1];
            }
        }
    }
}

// LCA를 구하는 함수
int LCA(int u, int v)
{
    // 두 노드의 깊이가 같아질 때까지 올라감
    if (depth[u] < depth[v]) swap(u, v);
    int diff = depth[u] - depth[v];
    for (int i = 0; diff; ++i)
    {
        if (diff & 1) u = parent[u][i];
        diff >>= 1;
    }

    // 두 노드가 같은 깊이에 위치할 때까지 같이 올라감
    if (u != v)
    {
        for (int i = LOGMAX - 1; i >= 0; --i)
        {
            if (parent[u][i] != -1 && parent[u][i] != parent[v][i])
            {
                u = parent[u][i];
                v = parent[v][i];
            }
        }
        u = parent[u][0];
    }

    return u;
}

int main()
{
    int n; // 노드의 수
    cin >> n;

    // 트리 입력 받기
    for (int i = 0; i < n - 1; ++i)
    {
        int u, v;
        cin >> u >> v;
        tree[u].push_back(v);
        tree[v].push_back(u);
    }

    // 루트 설정 및 DFS 수행
    int root = 1; // 루트 노드 번호
    DFS(root, -1, 0);

    // 희소테이블설정
    PrecomputeSparseTable(n);

    int q; // 쿼리의 수
    cin >> q;

    while (q--)
    {
        int u, v;
        cin >> u >> v;
        int ancestor = LCA(u, v);
        cout << "LCA of " << u << " and " << v << " is " << ancestor << endl;
    }

    return 0;
}
```