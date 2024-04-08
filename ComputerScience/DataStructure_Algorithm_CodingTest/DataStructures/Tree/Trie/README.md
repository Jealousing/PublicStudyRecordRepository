# 트라이(Trie)

## 트라이란?
 트라이(Trie)는 검색 트리의 일종으로, 문자열 집합을 저장하고 효율적으로 탐색하기 위한 트리 자료구조입니다. 다른 검색 트리와 달리, 트라이는 트리의 각 노드에 문자를 저장하는 방식으로 구성됩니다.

### 트라이의 구조
트라이는 노드들의 계층 구조로 이루어져 있습니다. 각 노드는 문자(character)나 노드의 종료를 나타내는 플래그(end_of_word)를 저장합니다. 트라이의 루트 노드는 빈 문자열을 나타냅니다.

### 트라이의 작동 원리
* 문자열 삽입: 문자열을 삽입할 때는 각 문자를 순차적으로 탐색하면서 해당 문자에 해당하는 자식 노드를 찾습니다. 만약 자식 노드가 존재하지 않는다면, 새로운 노드를 생성하여 연결합니다. 이 과정을 문자열의 마지막 문자에 도달할 때까지 반복한 후, 마지막 노드에 종료 플래그를 설정하여 해당 문자열이 종료되었음을 표시합니다.
* 문자열 검색: 문자열을 검색할 때는 각 문자를 순차적으로 탐색하면서 해당 문자에 해당하는 자식 노드를 찾습니다. 마지막 문자에 해당하는 노드에 종료 플래그가 설정되어 있다면, 검색이 성공한 것으로 간주합니다.
* 문자열 삭제: 문자열을 삭제할 때는 해당 문자열을 검색한 후, 검색된 마지막 노드에서부터 거꾸로 탐색하면서 필요한 노드를 삭제합니다. 삭제된 노드의 부모 노드에 다른 자식이 없다면 해당 부모 노드도 삭제합니다.

## 트라이의 특징
 
 ### 장점
* 문자열 검색에 높은 성능을 제공합니다.
* 공통 접두사를 공유하는 문자열들을 효율적으로 저장할 수 있습니다.
* 자동 완성, 철자 검사, 사전 검색 등에 유용하게 활용될 수 있습니다.

 ### 단점
* 메모리 사용량이 많을 수 있습니다.
* 입력 문자열의 길이에 비례하여 트라이의 깊이가 증가할 수 있습니다.
 
## 트라이의 활용
* 문자열 검색 및 검색 자동완성 시스템에서 사용됩니다.
* 사전 구현에 활용됩니다.
* 문자열 처리와 관련된 알고리즘에서 효율적인 데이터 관리를 위해 사용됩니다.
 
## 트라이의 구현 (소문자만 다루는 경우)
```cpp
#include <iostream>

using namespace std;

// 트라이 노드 정의
struct TrieNode 
{
    TrieNode* children[26]; // 알파벳 소문자만 다루는 경우
    bool end_of_word;

    TrieNode() 
    {
        end_of_word = false;
        for (int i = 0; i < 26; ++i) 
        {
            children[i] = nullptr;
        }
    }
};

// 알파벳 소문자를 인덱스로 변환
int charToIndex(char c) 
{
    return c - 'a';
}

// 트라이 구현
class Trie 
{
private:
    TrieNode* root;

public:
    Trie() 
    {
        root = new TrieNode();
    }

    // 문자열 삽입
    void insert(string word) 
    {
        TrieNode* current = root;
        for (char c : word) 
        {
            int index = charToIndex(c);
            if (current->children[index] == nullptr) 
            {
                current->children[index] = new TrieNode();
            }
            current = current->children[index];
        }
        current->end_of_word = true;
    }

    // 문자열 검색
    bool search(string word)
    {
        TrieNode* current = root;
        for (char c : word) 
        {
            int index = charToIndex(c);
            if (current->children[index] == nullptr) 
            {
                return false;
            }
            current = current->children[index];
        }
        return current->end_of_word;
    }

    // 문자열 삭제
    void remove(string word)
    {
        removeHelper(root, word, 0);
    }

private:
    //주어진 단어를 트라이에서 삭제하는 재귀 함수
    bool removeHelper(TrieNode* current, string& word, int index) 
    {
        if (index == word.length()) 
        {
            // 단어가 존재하지 않음
            if (!current->end_of_word) 
            {
                return false; 
            }
            current->end_of_word = false;
            return isNodeEmpty(current);
        }

        char ch = word[index];
        int chIndex = charToIndex(ch);

        // 단어가 존재하지 않음
        if (current->children[chIndex] == nullptr) 
        {
            return false; 
        }

        bool shouldDeleteCurrentNode = removeHelper(current->children[chIndex], word, index + 1);

        if (shouldDeleteCurrentNode) 
        {
            delete current->children[chIndex];
            current->children[chIndex] = nullptr;
            return isNodeEmpty(current);
        }

        return false;
    }

    // 노드가 비어 있는지 확인
    bool isNodeEmpty(TrieNode* node) 
    {
        for (int i = 0; i < 26; ++i) 
        {
            if (node->children[i] != nullptr) 
            {
                return false;
            }
        }
        return true;
    }
};

int main() 
{
    Trie trie;
    
    // 문자열 삽입
    trie.insert("apple");
    trie.insert("banana");
    trie.insert("orange");
    
    // 문자열 검색
    cout << trie.search("apple") << endl;   // true
    cout << trie.search("banana") << endl;  // true
    cout << trie.search("orange") << endl;  // true
    cout << trie.search("grape") << endl;   // false
    
    // 문자열 삭제
    trie.remove("banana");
    cout << trie.search("banana") << endl;  // false
    
    return 0;
}
```