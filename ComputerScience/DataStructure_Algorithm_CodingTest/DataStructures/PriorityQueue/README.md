# 우선순위 큐 (Priority Queue)

## 우선순위 큐란?
 우선순위 큐는 데이터를 저장하고 우선순위에 따라 처리하는 자료구조입니다.    
일반적으로 요소들은 우선순위에 따라 정렬되어 있으며, 가장 높은 우선순위를 가진 요소가 가장 먼저 처리됩니다.

### 핵심 기능
* push: 우선순위 큐에 요소를 추가합니다.
* pop: 우선순위가 가장 높은 요소를 제거합니다.
* top: 우선순위가 가장 높은 요소의 값을 반환합니다.
* size: 우선순위 큐에 저장된 요소의 개수를 반환합니다.
* empty: 우선순위 큐가 비어있는지 여부를 반환합니다.

### 우선순위 큐의 동작 방식
* 기본적으로는 최대 힙(Max Heap) 구조를 사용하여 가장 큰 요소가 가장 높은 우선순위를 가집니다.
* 최소 힙(Min Heap) 구조를 사용하여 가장 작은 요소가 가장 높은 우선순위를 갖도록 만들 수 있습니다.

#### 최대 힙(Max Heap)이란?
부모 노드가 자식 노드보다 항상 크거나 같은 완전 이진 트리입니다. 따라서 가장 큰 요소가 루트에 위치하게 됩니다.

#### 최소 힙(Min Heap)이란?
최소 힙(Min Heap): 부모 노드가 자식 노드보다 항상 작거나 같은 완전 이진 트리입니다. 따라서 가장 작은 요소가 루트에 위치하게 됩니다.

## 우선순위 큐 특징
 
 ### 장점
* 데이터를 삽입하는 동안에도 자동으로 정렬되어 있어 우선순위가 높은 요소에 빠르게 접근할 수 있습니다.
* 우선순위에 따라 요소가 자동으로 정렬되므로 추가적인 정렬 작업이 필요하지 않습니다.

 ### 단점
* 일반적으로 우선순위 큐의 구현은 힙(Heap)을 사용하는데, 힙에 대한 삽입 및 삭제 연산은 O(log n)의 시간복잡도를 가집니다.
* 우선순위 큐의 구현이 복잡할 수 있으며, 사용 시 우선순위 개념을 이해해야 합니다.
 
## 우선순위 큐를 사용하면 좋은 경우
* 작업 스케줄링(Scheduling)이나 이벤트 처리(Event Handling)와 같이 우선순위가 있는 작업을 처리할 때 사용합니다.
* 다익스트라 알고리즘(Dijkstra's Algorithm)과 같이 우선순위에 따라 노드를 탐색하거나 처리해야 할 때 유용합니다.
 
## 예시코드 c++

### 최대 힙(Max Heap) 우선순위 큐 
```cpp
#include <iostream>
#include <queue>

using namespace std;

int main() 
{
    // 최대 힙 구조로 우선순위 큐를 선언
    priority_queue<int> max_heap;

    // 요소 추가
    max_heap.push(30);
    max_heap.push(10);
    max_heap.push(50);

    // 우선순위가 가장 높은 요소 출력
    cout << "Top element (Max Heap): " << max_heap.top() << endl;

    // 요소 제거
    max_heap.pop();

    // 큐의 크기 출력
    cout << "Size of priority queue (Max Heap): " << max_heap.size() << endl;

    return 0;
}
```

### 최소 힙 (Min Heap) 우선순위 큐 
```cpp
#include <iostream>
#include <queue>

using namespace std;

int main() 
{
    // 최소 힙 구조로 우선순위 큐를 선언
    priority_queue<int, vector<int>, greater<int>> min_heap;

    // 요소 추가
    min_heap.push(30);
    min_heap.push(10);
    min_heap.push(50);

    // 우선순위가 가장 높은 요소 출력
    cout << "Top element (Min Heap): " << min_heap.top() << endl;

    // 요소 제거
    min_heap.pop();

    // 큐의 크기 출력
    cout << "Size of priority queue (Min Heap): " << min_heap.size() << endl;

    return 0;
}
```

### STL을 사용하지않고 구현한 우선순위 큐
```cpp
#include <iostream>

using namespace std;

const int MAX_SIZE = 100;

class PriorityQueue 
{
private:
    int heap[MAX_SIZE];
    int heapSize;

    void swap(int &a, int &b) 
    {
        int temp = a;
        a = b;
        b = temp;
    }

    void heapifyUp(int index) 
    {
        if (index == 0) return; // 루트 노드인 경우 종료
        int parent = (index - 1) / 2;
        if (heap[parent] < heap[index]) 
        {
            swap(heap[parent], heap[index]);
            heapifyUp(parent);
        }
    }

    void heapifyDown(int index) 
    {
        int leftChild = 2 * index + 1;
        int rightChild = 2 * index + 2;
        int largest = index;

        if (leftChild < heapSize && heap[leftChild] > heap[largest]) 
        {
            largest = leftChild;
        }

        if (rightChild < heapSize && heap[rightChild] > heap[largest]) 
        {
            largest = rightChild;
        }

        if (largest != index) 
        {
            swap(heap[index], heap[largest]);
            heapifyDown(largest);
        }
    }

public:
    PriorityQueue() : heapSize(0) {}

    void push(int value) 
    {
        if (heapSize == MAX_SIZE) 
        {
            cout << "Priority Queue is full!" << endl;
            return;
        }
        heap[heapSize] = value;
        heapSize++;
        heapifyUp(heapSize - 1);
    }

    void pop() 
    {
        if (empty()) 
        {
            cout << "Priority Queue is empty!" << endl;
            return;
        }
        swap(heap[0], heap[heapSize - 1]);
        heapSize--;
        heapifyDown(0);
    }

    int top() 
    {
        if (empty()) 
        {
            cout << "Priority Queue is empty!" << endl;
            return -1;
        }
        return heap[0];
    }

    bool empty() 
    {
        return heapSize == 0;
    }

    int size() 
    {
        return heapSize;
    }
};

int main() 
{
    PriorityQueue pq;

    pq.push(30);
    pq.push(10);
    pq.push(50);

    cout << "Top element: " << pq.top() << endl;

    pq.pop();

    cout << "Size after pop: " << pq.size() << endl;

    return 0;
}
```