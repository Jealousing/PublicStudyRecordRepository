# 이진 트리 (Binary Tree)

## 이진 트리란
 이진 트리는 각 노드가 최대 두 개의 자식을 가질 수 있는 트리 구조입니다. 각 노드는 최대 두 개의 하위 트리를 가질 수 있으며, 이진 트리에서는 왼쪽 자식과 오른쪽 자식이라는 개념이 있습니다.

### 이진 트리의 구성 요소
* 루트 노드 (Root Node): 최상위 노드로, 다른 모든 노드의 조상입니다.
* 내부 노드 (Internal Node): 적어도 하나의 자식을 가진 노드로, 루트 노드와 리프 노드 사이의 노드입니다.
* 리프 노드 (Leaf Node): 자식이 없는 노드로, 트리의 끝에 위치합니다.
* 부모 노드 (Parent Node): 어떤 노드의 바로 위에 있는 노드를 가리킵니다.
* 자식 노드 (Child Node): 어떤 노드의 바로 아래에 있는 노드를 가리킵니다.
* 깊이 (Depth): 루트 노드로부터 어떤 노드에 이르는 경로의 길이를 가리킵니다.
* 높이 (Height): 트리의 루트에서 가장 깊은 노드까지의 경로의 길이를 가리킵니다.

## 이진 트리의 특징
* 각 노드는 최대 두 개의 자식 노드를 가질 수 있습니다.
* 모든 하위 트리도 이진 트리입니다. 

 ### 장점
 * 데이터의 삽입, 삭제, 검색 등의 연산이 빠릅니다.
 * 이진 검색 트리의 경우 데이터를 정렬된 상태로 저장하여 효율적인 검색이 가능합니다.
 * 다양한 문제에 대한 효과적인 해결 방법을 제공합니다.

 ### 단점
 * 트리가 불균형적으로 성장할 경우 연산의 효율성이 떨어질 수 있습니다.
 * 최악의 경우 트리가 선형 구조가 될 수 있어 검색이나 삽입 시간이 O(n)이 될 수 있습니다.
 * 메모리 공간의 낭비가 발생할 수 있습니다.
 
## 이진 트리의 종류
* 정 이진 트리 (Full Binary Tree): 모든 노드가 0개 또는 2개의 자식을 가지는 이진 트리입니다.
* 완전 이진 트리 (Complete Binary Tree): 마지막 레벨을 제외한 모든 레벨이 완전히 채워진 이진 트리입니다.
* 포화 이진 트리 (Perfect Binary Tree): 모든 내부 노드가 두 개의 자식을 가지며, 모든 리프 노드가 같은 높이에 있는 이진 트리입니다.

## 이진 트리의 순회 (Traversal)
* 전위 순회 (Preorder Traversal): 루트 노드를 먼저 방문한 후, 왼쪽 서브트리를 전위 순회한 뒤에 오른쪽 서브트리를 전위 순회합니다.
* 중위 순회 (Inorder Traversal): 왼쪽 서브트리를 중위 순회한 후, 루트 노드를 방문한 뒤에 오른쪽 서브트리를 중위 순회합니다.
* 후위 순회 (Postorder Traversal): 왼쪽 서브트리를 후위 순회한 후, 오른쪽 서브트리를 후위 순회한 뒤에 루트 노드를 방문합니다.

## 이진 트리의 활용
 이진 트리는 데이터의 삽입, 삭제, 검색 등 다양한 작업에 사용됩니다. 이외에도 이진 트리는 이진 검색 트리(BST), AVL 트리, 레드-블랙 트리, 힙 등의 자료구조로 활용됩니다.

## 이진 트리의 구현
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

class BinaryTree 
{
private:
    Node* root;

public:
    BinaryTree() : root(nullptr) {}

    // 이진 트리에 노드 추가
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
        else 
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
};

int main() 
{
    BinaryTree tree;
    tree.insert(5);
    tree.insert(3);
    tree.insert(7);
    tree.insert(1);
    tree.insert(4);
    cout << "Inorder Traversal: ";
    tree.inorderTraversal();
    return 0;
}
```
