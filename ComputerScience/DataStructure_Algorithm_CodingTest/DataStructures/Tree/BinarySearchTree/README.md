# 이진 탐색 트리 (Binary Search Tree, BST)

## 이진 탐색 트리란
 이진 탐색 트리는 각 노드가 최대 두 개의 자식을 가지며, 왼쪽 자식은 부모보다 작은 값을, 오른쪽 자식은 부모보다 큰 값을 가지는 이진 트리의 특별한 형태입니다. 이진 탐색 트리는 데이터를 효율적으로 탐색하기 위해 설계된 자료구조입니다.

## 이진 탐색 트리의 특징
* 각 노드는 최대 두 개의 자식을 가질 수 있습니다.
* 모든 하위 트리도 이진 탐색 트리입니다.
* 왼쪽 자식은 부모보다 작은 값을 가지며, 오른쪽 자식은 부모보다 큰 값을 가집니다.

 ### 장점
 * 데이터의 검색, 삽입, 삭제 등의 연산이 평균적으로 O(log n)의 시간 복잡도를 가집니다.
 * 데이터가 정렬된 상태로 유지됩니다.

 ### 단점
 * 트리가 불균형적으로 성장할 경우 (즉, 한 쪽으로 치우친 형태로 성장할 경우) 연산의 효율성이 떨어질 수 있습니다.
 * 최악의 경우 트리가 선형 구조가 되어 검색이나 삽입 시간이 O(n)이 될 수 있습니다. 

## 이진 탐색 트리의 활용
 이진 탐색 트리는 정렬된 데이터를 효율적으로 관리하는 데 사용됩니다. 주로 검색 연산이 많이 요구되는 상황에서 활용됩니다. 예를 들어, 주소록이나 사전과 같이 데이터의 검색이 빈번한 경우에 유용하게 사용될 수 있습니다.
 
## 예시코드 
```cpp
#include <iostream>

using namespace std;

struct Node 
{
    int data;
    Node* left;
    Node* right;

    Node(int val) : data(val), left(nullptr), right(nullptr) {}
};

class BinarySearchTree 
{
private:
    Node* root;

public:
    BinarySearchTree() : root(nullptr) {}

    // 이진 탐색 트리에 노드 추가
    void insert(int val) 
    {
        root = insertRecursive(root, val);
    }

    // 재귀적으로 노드 추가
    Node* insertRecursive(Node* node, int val) 
    {
        if (node == nullptr)
        {
            return new Node(val);
        }

        if (val < node->data) 
        {
            node->left = insertRecursive(node->left, val);
        } 
        else if (val > node->data)
        {
            node->right = insertRecursive(node->right, val);
        }

        return node;
    }

    // 중위 순회
    void inorderTraversal(Node* node) 
    {
        if (node != nullptr) 
        {
            inorderTraversal(node->left);
            cout << node->data << " ";
            inorderTraversal(node->right);
        }
    }

    // 중위 순회 호출
    void inorderTraversal() 
    {
        inorderTraversal(root);
    }

    // 이진 탐색 트리에서 값 찾기
    bool find(int val)
    {
        return findRecursive(root, val);
    }

    // 재귀적으로 값 찾기
    bool findRecursive(Node* node, int val)
    {
        if (node == nullptr)
        {
            return false;
        }
        
        if (node->data == val)
        {
            return true;
        }
        else if (val < node->data)
        {
            return findRecursive(node->left, val);
        }
        else
        {
            return findRecursive(node->right, val);
        }
    }
};

int main() 
{
    BinarySearchTree bst;
    bst.insert(5);
    bst.insert(3);
    bst.insert(7);
    bst.insert(1);
    bst.insert(4);
    
    cout << "Inorder Traversal: ";
    bst.inorderTraversal();
    cout << endl;
    
    int searchValue = 4;
    if (bst.find(searchValue))
    {
        cout << " true " << endl;
    }
    else
    {
        cout << " false " << endl;
    }
    
    searchValue = 6;
    if (bst.find(searchValue))
    {
        cout << " true " << endl;
    }
    else
    {
        cout << " false " << endl;
    }
    
    return 0;
}
```