# 큐 (Queue)

## 큐란?
 큐는 데이터를 저장하고 접근하는 데 사용되는 선형 자료구조입니다.
데이터를 저장할 때에는 가장 먼저 추가된 데이터가 가장 먼저 제거되는 선입선출(FIFO, First-In-First-Out) 원칙을 따릅니다.

## 핵심 기능
* push(): 큐에 요소를 추가합니다.
* pop(): 큐의 맨 앞에 있는 요소를 제거합니다.
* front(): 큐의 맨 앞에 있는 요소의 값을 반환합니다.
* back(): 큐의 맨 뒤에 있는 요소의 값을 반환합니다.
* size(): 큐에 저장된 요소의 개수를 반환합니다.
* empty(): 큐가 비어있는지 여부를 반환합니다.
* full(): 큐가 꽉 찼는지 여부를 반환합니다.

## 큐 특징

 ### 장점
* 간단하고 직관적인 자료구조로, 구현이 쉽습니다.
* 데이터의 삽입과 삭제가 매우 빠릅니다.
* 너비 우선 탐색(BFS) 등의 그래프 탐색 알고리즘을 구현하는 데 유용합니다.

 ### 단점
* 큐의 크기가 고정되어 있거나 메모리가 제한적인 경우 오버플로우가 발생할 수 있습니다.
* 데이터 접근이 제한적이며, 임의의 위치에 있는 데이터에 직접적으로 접근할 수 없습니다.
 
## 큐를 사용하면 좋은 경우
* 너비 우선 탐색(BFS) 등의 그래프 탐색 알고리즘을 구현하는 경우
* 자원을 공유하는 프로세스를 관리하는 시스템에서 사용자의 요청을 처리하는 경우
* 버퍼링(Buffering) 작업이 필요한 경우
* 프린터의 출력 작업을 관리하는 경우

## 예시코드

### 배열로 구현한 큐 코드

```cpp
#include <iostream>
#include <stdexcept>

using namespace std;
template <typename T, size_t MAX_SIZE>
class Queue 
{
private:
    T data[MAX_SIZE];
    size_t frontIndex;
    size_t rearIndex;
    size_t currentSize;

public:
    Queue() : frontIndex(0), rearIndex(0), currentSize(0) {}

    bool isEmpty() const 
    {
        return currentSize == 0;
    }

    bool isFull() const 
    {
        return currentSize == MAX_SIZE;
    }

    size_t size() const 
    {
        return currentSize;
    }

    T front() const 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        return data[frontIndex];
    }

    T back() const 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        return data[rearIndex - 1];
    }

    void push(const T& value) 
    {
        if (isFull()) 
        {
            throw out_of_range("Queue is full");
        }
        data[rearIndex] = value;
        rearIndex = (rearIndex + 1) % MAX_SIZE;
        ++currentSize;
    }

    void pop() 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        frontIndex = (frontIndex + 1) % MAX_SIZE;
        --currentSize;
    }
};

int main() 
{
    Queue<int, 5> queue;

    queue.push(10);
    queue.push(20);
    queue.push(30);

    cout << "Front element: " << queue.front() << endl;
    cout << "Back element: " << queue.back() << endl;

    queue.pop();

    cout << "Size after pop: " << queue.size() << endl;

    return 0;
}
```

#### 장점
* 구현이 간단하고 직관적입니다.
* 데이터의 삽입과 삭제가 빠릅니다.

#### 단점
* 큐의 크기가 고정되어 있거나 메모리가 제한적인 경우 오버플로우가 발생할 수 있습니다.
* 데이터를 삽입할 때마다 요소들을 이동해야 하는 경우가 생길 수 있습니다.

### 연결 리스트로 구현한 큐 코드

```cpp
#include <iostream>
#include <stdexcept>

using namespace std;

template <typename T>
class Node 
{
public:
    T data;
    Node* next;

    Node(const T& value) : data(value), next(nullptr) {}
};

template <typename T>
class Queue 
{
private:
    Node<T>* frontNode;
    Node<T>* rearNode;
    size_t currentSize;

public:
    Queue() : frontNode(nullptr), rearNode(nullptr), currentSize(0) {}

    bool isEmpty() const 
    {
        return currentSize == 0;
    }

    size_t size() const 
    {
        return currentSize;
    }

    T front() const 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        return frontNode->data;
    }

    T back() const 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        return rearNode->data;
    }

    void push(const T& value) 
    {
        Node<T>* newNode = new Node<T>(value);
        if (isEmpty()) 
        {
            frontNode = rearNode = newNode;
        } 
        else 
        {
            rearNode->next = newNode;
            rearNode = newNode;
        }
        ++currentSize;
    }

    void pop() 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        Node<T>* temp = frontNode;
        frontNode = frontNode->next;
        delete temp;
        --currentSize;
    }

    ~QueueUsingLinkedList() 
    {
        while (frontNode != nullptr) 
        {
            Node<T>* temp = frontNode;
            frontNode = frontNode->next;
            delete temp;
        }
        rearNode = nullptr;
        currentSize = 0;
    }
};

int main() 
{
    Queue<int> queue;

    queue.push(10);
    queue.push(20);
    queue.push(30);

    cout << "Front element: " << queue.front() << endl;
    cout << "Back element: " << queue.back() << endl;

    queue.pop();

    cout << "Size after pop: " << queue.size() << endl;

    return 0;
}
```

#### 장점
* 크기가 동적으로 조절될 수 있어 메모리를 효율적으로 사용할 수 있습니다.
* 데이터 삽입 및 삭제가 상수 시간에 이루어집니다.

#### 단점
* 각 요소마다 포인터를 저장해야 하므로 메모리 사용량이 더 많을 수 있습니다.
*포인터의 추가 및 삭제 작업에 따른 오버헤드가 발생할 수 있습니다.

### vector를 사용한 큐 코드

```cpp
#include <iostream>
#include <vector>
#include <stdexcept>

using namespace std;

template <typename T>
class Queue
{
private:
    vector<T> data;

public:
    bool isEmpty() const 
    {
        return data.empty();
    }

    size_t size() const 
    {
        return data.size();
    }

    T front() const 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        return data.front();
    }

    T back() const 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        return data.back();
    }

    void push(const T& value) 
    {
        data.push_back(value);
    }

    void pop() 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        data.erase(data.begin());
    }
};

int main() 
{
    Queue<int> queue;

    queue.push(10);
    queue.push(20);
    queue.push(30);

    cout << "Front element: " << queue.front() << endl;
    cout << "Back element: " << queue.back() << endl;

    queue.pop();

    cout << "Size after pop: " << queue.size() << endl;

    return 0;
}
```

#### 장점
* 동적 배열로 구현되어 크기가 동적으로 조절될 수 있습니다.
* STL에서 제공하는 표준 라이브러리로 안정성이 높고 범용적으로 사용됩니다.

#### 단점
* 크기가 동적으로 변하기 때문에 메모리 재할당에 따른 오버헤드가 발생할 수 있습니다.
* 데이터 삽입 및 삭제 시에 요소들을 이동해야 하는 경우가 생길 수 있습니다.

### 원형버퍼를 이용한 큐 코드

```cpp
#include <iostream>
#include <array>
#include <stdexcept>

using namespace std;

template <typename T, size_t MAX_SIZE>
class Queue
{
private:
    array<T, MAX_SIZE> data;
    size_t frontIndex;
    size_t rearIndex;
    size_t currentSize;

public:
    Queue() : frontIndex(0), rearIndex(0), currentSize(0) {}

    bool isEmpty() const 
    {
        return currentSize == 0;
    }

    bool isFull() const 
    {
        return currentSize == MAX_SIZE;
    }

    size_t size() const 
    {
        return currentSize;
    }

    T front() const 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        return data[frontIndex];
    }

    T back() const 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        return data[(rearIndex - 1 + MAX_SIZE) % MAX_SIZE];
    }

    void push(const T& value)  
    {
        if (isFull()) 
        {
            throw out_of_range("Queue is full");
        }
        data[rearIndex] = value;
        rearIndex = (rearIndex + 1) % MAX_SIZE;
        ++currentSize;
    }

    void pop() 
    {
        if (isEmpty()) 
        {
            throw out_of_range("Queue is empty");
        }
        frontIndex = (frontIndex + 1) % MAX_SIZE;
        --currentSize;
    }
};
int main() 
{
     Queue<int, 5> queue;

    queue.push(10);
    queue.push(20);
    queue.push(30);

    cout << "Front element: " << queue.front() << endl;
    cout << "Back element: " << queue.back() << endl;

    queue.pop();

    cout << "Size after pop: " << queue.size() << endl;

    return 0;
}
```

#### 장점
* 배열 기반이지만 원형 버퍼로 구현되어 메모리 재사용이 용이합니다.
* 요소의 삽입 및 삭제가 상수 시간에 이루어집니다.

#### 단점
* 최대 크기가 정해져 있기 때문에 큐의 크기가 동적으로 조절되지 않습니다.
*원형 버퍼에서 빈 공간과 꽉 찬 공간을 구분하기 위한 추가적인 조건이 필요합니다.


		#include <stdexcept>은 예외 처리를 쉽게 하기 위한 표준 라이브러리 중 하나입니다.
		'out_of_range'을 통해 특정 범위를 벗어나는 인덱스 또는 값에 접근할 때 발생하는 예외를 처리하기 위해 사용됩니다.