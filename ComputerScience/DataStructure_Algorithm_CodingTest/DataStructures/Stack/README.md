# 스택 (Stack)

## 스택이란?
스택은 데이터를 저장하고 접근하는 데 사용되는 선형 자료구조입니다.    
데이터를 저장할 때에는 가장 최근에 추가된 데이터가 가장 먼저 제거되는 후입선출(LIFO, Last-In-First-Out) 원칙을 따릅니다.

## 스택 특징
 
 ### 장점
* 간단하고 직관적인 자료구조로, 구현이 쉽습니다.
* 데이터의 삽입과 삭제가 매우 빠릅니다.
* 함수 호출, 재귀 알고리즘, 역추적 등에 유용하게 사용됩니다.

 ### 단점
* 스택의 크기가 고정되어 있거나 메모리가 제한적인 경우 오버플로우가 발생할 수 있습니다.
* 데이터 접근이 제한적이며, 임의의 위치에 있는 데이터에 직접적으로 접근할 수 없습니다.
 
## 스택을 사용하면 좋은 경우
* 함수 호출의 순서를 추적하거나 되돌리는 작업이 필요한 경우
* 재귀 알고리즘을 구현하는 경우
* 역추적(Backtracking) 작업이 필요한 경우
* 깊이 우선 탐색(DFS) 등의 그래프 탐색 알고리즘을 구현하는 경우
 
## vector를 사용한 스택 구현 예시코드

```cpp
#include <iostream>
#include <vector>

using namespace std;

template <typename T>
class Stack 
{
private:
    vector<T> data;

public:
    // 스택이 비어있는지 확인하는 함수
    bool isEmpty() const
    {
        return data.empty();
    }

    // 스택의 크기를 반환하는 함수
    size_t size() const 
    {
        return data.size();
    }

    // 스택의 맨 위에 있는 요소를 반환하는 함수
    T top() const 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Stack is empty");
        }
        return data.back();
    }

    // 스택에 요소를 추가하는 함수
    void push(const T& value) 
    {
        data.push_back(value);
    }

    // 스택의 맨 위에 있는 요소를 제거하는 함수
    void pop() 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Stack is empty");
        }
        data.pop_back();
    }
};

int main() 
{
    Stack<int> stack;
    
    // 스택에 값 추가
    stack.push(10);
    stack.push(20);
    stack.push(30);

    // 스택의 크기 확인
    cout << "Stack size: " << stack.size() << endl;

    // 스택의 맨 위 요소 확인 및 제거
    cout << "Top element: " << stack.top() << endl;
    stack.pop();

    // 스택이 비어있는지 확인
    if (stack.isEmpty()) 
    {
        cout << "Stack is empty" << endl;
    } 
    else 
    {
        cout << "Stack is not empty" << endl;
    }

    return 0;
}
```
### vector를 사용해서 구현한 이유
* vector는 동적 배열로 구현되어 있어 크기가 동적으로 조절될 수 있습니다. 이는 스택에 데이터를 추가하거나 삭제할 때 효율적입니다.
* vector는 STL에서 제공하는 표준 라이브러리로, 범용성과 안정성이 높습니다.
* 벡터의 push_back() 및 pop_back() 함수를 사용하여 스택의 push 및 pop 작업을 간편하게 구현할 수 있습니다.

## vector 없이 구현한 코드

```cpp
#include <iostream>

using namespace std;

template <typename T, int MAX_SIZE>
class Stack 
{
private:
    T data[MAX_SIZE]; // 스택을 저장하는 배열
    int topIndex; // 스택의 맨 위 요소를 가리키는 인덱스

public:
    // 생성자
    Stack() : topIndex(-1) {}

    // 스택이 비어있는지 확인하는 함수
    bool isEmpty() const
    {
        return topIndex == -1;
    }

    // 스택이 가득 차있는지 확인하는 함수
    bool isFull() const
    {
        return topIndex == MAX_SIZE - 1;
    }

    // 스택의 크기를 반환하는 함수
    size_t size() const 
    {
        return topIndex + 1;
    }

    // 스택의 맨 위에 있는 요소를 반환하는 함수
    T top() const 
    {
        if (isEmpty()) 
        {
            cerr << "Error: Stack is empty" << endl;
            exit(EXIT_FAILURE);
        }
        return data[topIndex];
    }

    // 스택에 요소를 추가하는 함수
    void push(const T& value) 
    {
        if (isFull()) 
        {
            cerr << "Error: Stack is full" << endl;
            exit(EXIT_FAILURE);
        }
        data[++topIndex] = value;
    }

    // 스택의 맨 위에 있는 요소를 제거하는 함수
    void pop() 
    {
        if (isEmpty()) 
        {
            cerr << "Error: Stack is empty" << endl;
            exit(EXIT_FAILURE);
        }
        --topIndex;
    }
};

int main() 
{
    Stack<int, 100> stack; // 최대 크기가 100인 스택 생성
    
    // 스택에 값 추가
    stack.push(10);
    stack.push(20);
    stack.push(30);

    // 스택의 크기 확인
    cout << "Stack size: " << stack.size() << endl;

    // 스택의 맨 위 요소 확인 및 제거
    cout << "Top element: " << stack.top() << endl;
    stack.pop();

    // 스택이 비어있는지 확인
    if (stack.isEmpty()) 
    {
        cout << "Stack is empty" << endl;
    } 
    else 
    {
        cout << "Stack is not empty" << endl;
    }

    return 0;
}
```