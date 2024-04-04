# 연결리스트 ( LinkedList )

## 연결리스트란?
연결 리스트는 데이터 요소를 순서대로 저장하는 자료구조입니다. 각 요소는 자신의 데이터와 다음 요소를 가리키는 포인터로 구성됩니다.

### 연결리스트 핵심 요소
- **노드(Node)**: 데이터와 포인터로 구성된 각각의 요소입니다.
- **헤드(Head)**: 연결 리스트의 시작점을 가리키는 포인터입니다.
- **테일(Tail)**: 연결 리스트의 끝을 가리키는 포인터입니다.

## 연결리스트 장단점
 
### 장점
- 삽입과 삭제가 간단하고 빠릅니다. 노드의 위치를 이동할 필요가 없기 때문입니다.
- 크기가 가변적이며, 동적으로 데이터를 관리할 수 있습니다.
- 메모리의 동적 할당을 사용하여 데이터를 저장하기 때문에, 메모리 공간을 효율적으로 사용할 수 있습니다.

### 단점
- 임의 접근이 불가능합니다. 특정 위치에 있는 데이터에 접근하기 위해서는 처음부터 순차적으로 탐색해야 합니다.
- 포인터를 사용하기 때문에 추가적인 메모리 공간이 필요합니다.

## 연결리스트를 사용하면 좋은 경우
- 데이터의 삽입 또는 삭제가 빈번한 경우
- 데이터의 크기가 변동적이거나, 사전에 크기를 알 수 없는 경우
- 데이터의 순서가 중요한 경우

## 연결리스트 구현 및 예시 코드

### 단일 연결 리스트 (Singly Linked List)

```cpp
class Node 
{
public:
    int data; // 데이터 저장
    Node* next; // 다음 노드를 가리키는 포인터

    // 생성자
    Node(int value) : data(value), next(nullptr) {}
};

// 단일 연결 리스트 클래스
class SinglyLinkedList 
{
private:
    Node* head; // 리스트의 첫 번째 노드를 가리키는 포인터

public:
    // 생성자
    SinglyLinkedList() : head(nullptr) {}

    // 리스트에 새로운 값을 뒤에 추가하는 함수
    void append(int value) 
    {
        Node* newNode = new Node(value);
        if (head == nullptr) 
        {
            head = newNode;
        } 
        else 
        {
            Node* current = head;
            while (current->next != nullptr) 
            {
                current = current->next;
            }
            current->next = newNode;
        }
    }

    // 리스트에서 특정 값을 찾는 함수
    Node* find(int value) 
    {
        Node* current = head;
        while (current != nullptr) 
        {
            if (current->data == value) 
            {
                return current;
            }
            current = current->next;
        }
        return nullptr; // 찾지 못한 경우 nullptr 반환
    }

    // 리스트를 출력하는 함수
    void display() 
    {
        Node* current = head;
        while (current != nullptr) 
        {
            std::cout << current->data << " ";
            current = current->next;
        }
        std::cout << std::endl;
    }

    // 리스트에서 특정 위치에 값을 삽입하는 함수
    void insert(int value, int position) 
    {
        Node* newNode = new Node(value);
        if (position == 0) 
        {
            newNode->next = head;
            head = newNode;
        } 
        else 
        {
            Node* current = head;
            for (int i = 0; i < position - 1 && current != nullptr; ++i)
            {
                current = current->next;
            }
            if (current != nullptr) 
            {
                newNode->next = current->next;
                current->next = newNode;
            }
        }
    }

    // 리스트에서 특정 값을 삭제하는 함수
    void remove(int value) 
    {
        if (head == nullptr) 
        {
            return;
        }
        if (head->data == value)
        {
            Node* temp = head;
            head = head->next;
            delete temp;
            return;
        }
        Node* current = head;
        while (current->next != nullptr) 
        {
            if (current->next->data == value) 
            {
                Node* temp = current->next;
                current->next = current->next->next;
                delete temp;
                return;
            }
            current = current->next;
        }
    }

    // 리스트에서 모든 값을 삭제하는 함수
    void removeAll() 
    {
        while (head != nullptr) 
        {
            Node* temp = head;
            head = head->next;
            delete temp;
        }
    }

    // 소멸자: 동적 할당된 메모리 해제
    ~SinglyLinkedList() 
    {
        removeAll();
    }
};
```

### 더블 연결 리스트 ( Double Linked List )
 더블 연결 리스트는 각각의 노드가 이전 노드와 다음 노드를 가리키는 포인터를 가지고 있는 연결 리스트입니다. 이로써 양방향으로 탐색이 가능하며, 단일 연결 리스트에 비해 더 많은 유연성을 제공합니다.

#### 더블 연결 리스트의 장단점

##### 장점:
* 양방향으로 탐색이 가능하므로 특정 노드의 앞 또는 뒤에 있는 요소에 쉽게 접근할 수 있습니다. 이는 리스트를 반대 방향으로 탐색하는 경우나, 노드를 삭제할 때 이전 노드에 대한 접근이 필요한 경우에 유용합니다.
* 단일 연결 리스트와는 달리 각 노드가 이전 노드를 가리키므로, 노드를 역방향으로 탐색하거나 삭제하는 작업이 단순합니다.
* 리스트의 양 끝에 대한 삽입 및 삭제 작업도 단일 연결 리스트보다 효율적으로 수행할 수 있습니다.

##### 단점:
* 각 노드가 이전 노드와 다음 노드를 가리키는 포인터를 유지하기 때문에, 메모리 공간을 더 많이 사용합니다. 따라서 메모리 사용량이 더 크고, 이로 인해 캐시 효율이 떨어질 수 있습니다.
* 단일 연결 리스트에 비해 더블 연결 리스트의 구현은 조금 더 복잡합니다. 각 노드가 이전 노드를 가리키는 포인터를 유지해야 하므로, 삽입 및 삭제 작업 시에 이전 노드의 포인터를 업데이트해주어야 합니다.

##### 양방향(더블) 연결리스트를 사용하면 좋은 경우
* 양방향으로 탐색이 필요한 경우
* 리스트의 양 끝에서의 삽입 및 삭제 작업이 빈번한 경우
* 노드를 역방향으로 탐색하거나 삭제해야 하는 경우

```cpp
class Node 
{
public:
    int data; // 데이터 저장
    Node * next; // 다음 노드를 가리키는 포인터
    Node * prev; // 이전 노드를 가리키는 포인터

    // 생성자
    Node(int value) : data(value), next(nullptr), prev(nullptr) {}
};

// 더블 연결 리스트 클래스 (Double Linked List)
class DoublyLinkedList 
{
private:
    Node* head; // 리스트의 첫 번째 노드를 가리키는 포인터

public:
    // 생성자
    DoublyLinkedList() : head(nullptr) {}

    // 리스트에 새로운 값을 뒤에 추가하는 함수
    void append(int value) 
    {
        Node* newNode = new Node(value);
        if (head == nullptr) 
        {
            head = newNode;
        } 
        else 
        {
            Node* current = head;
            while (current->next != nullptr) 
            {
                current = current->next;
            }
            current->next = newNode;
            newNode->prev = current;
        }
    }

    // 리스트에서 특정 값을 찾는 함수
    Node* find(int value) 
    {
        Node* current = head;
        while (current != nullptr) 
        {
            if (current->data == value) 
            {
                return current;
            }
            current = current->next;
        }
        return nullptr; // 찾지 못한 경우 nullptr 반환
    }

    // 리스트를 앞에서부터 출력하는 함수
    void displayForward() 
    {
        Node* current = head;
        while (current != nullptr) 
        {
            std::cout << current->data << " ";
            current = current->next;
        }
        std::cout << std::endl;
    }

    // 리스트를 뒤에서부터 출력하는 함수
    void displayBackward() 
    {
        Node* current = head;
        if (current == nullptr) 
        {
            return;
        }
        while (current->next != nullptr) 
        {
            current = current->next;
        }
        while (current != nullptr)  
        {
            std::cout << current->data << " ";
            current = current->prev;
        }
        std::cout << std::endl;
    }

    // 리스트에서 특정 위치에 값을 삽입하는 함수
    void insert(int value, int position) 
    {
        Node* newNode = new Node(value);
        if (position == 0) 
        {
            newNode->next = head;
            if (head != nullptr) 
            {
                head->prev = newNode;
            }
            head = newNode;
        } 
        else 
        {
            Node* current = head;
            for (int i = 0; i < position - 1 && current != nullptr; ++i) 
            {
                current = current->next;
            }
            if (current != nullptr) 
            {
                newNode->next = current->next;
                if (current->next != nullptr) 
                {
                    current->next->prev = newNode;
                }
                current->next = newNode;
                newNode->prev = current;
            }
        }
    }

    // 리스트에서 특정 값을 삭제하는 함수
    void remove(int value) 
    {
        if (head == nullptr) 
        {
            return;
        }
        if (head->data == value) 
        {
            Node* temp = head;
            head = head->next;
            if (head != nullptr) 
            {
                head->prev = nullptr;
            }
            delete temp;
            return;
        }
        Node* current = head;
        while (current->next != nullptr) 
        {
            if (current->next->data == value) 
            {
                Node* temp = current->next;
                current->next = current->next->next;
                if (current->next != nullptr) 
                {
                    current->next->prev = current;
                }
                delete temp;
                return;
            }
            current = current->next;
        }
    }

    // 리스트에서 모든 값을 삭제하는 함수
    void removeAll() 
    {
        while (head != nullptr) 
        {
            Node* temp = head;
            head = head->next;
            delete temp;
        }
    }

    // 소멸자: 동적 할당된 메모리 해제
    ~DoublyLinkedList() 
    {
        removeAll();
    }
};

```