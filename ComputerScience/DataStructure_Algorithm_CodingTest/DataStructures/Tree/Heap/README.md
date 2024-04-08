# 힙(Heap)

## 힙이란?
 힙(Heap)은 완전 이진 트리(Complete Binary Tree)의 한 종류로, 각 노드의 값이 특정한 조건을 만족하는 자료구조입니다. 힙은 주로 우선순위 큐(Priority Queue)를 구현하는 데 사용됩니다.

### 힙의 종류

#### 최대 힙(Max Heap)
 * 각 노드의 값이 그 자식 노드들의 값보다 크거나 같은 완전 이진 트리입니다.
 * 루트 노드에는 최대값이 위치하며, 모든 부모 노드의 값은 자식 노드들의 값보다 크거나 같습니다.

#### 최소 힙(Min Heap)
 * 각 노드의 값이 그 자식 노드들의 값보다 작거나 같은 완전 이진 트리입니다.
 * 루트 노드에는 최소값이 위치하며, 모든 부모 노드의 값은 자식 노드들의 값보다 작거나 같습니다.

## 힙의 특징
 
 ### 장점
 * 삽입, 삭제, 최대/최소값 검색 등의 연산이 O(log n)의 시간 복잡도를 가짐
 * 우선순위 큐를 구현하는 데 효과적으로 사용됩니다.
 * 익스트라 알고리즘과 같은 알고리즘에서 효율적으로 활용됩니다.

 ### 단점
 * 배열 기반의 구현에서 공간의 낭비가 발생할 수 있음
 
## 이진 탐색 트리와 차이점
* 이진 탐색 트리는 각 노드의 값이 왼쪽 자식보다 작고 오른쪽 자식보다 큰 값을 가지는 반면, 힙은 최대 힙일 경우 루트 노드가 최대값을 가지고 있고 최소 힙일 경우 루트 노드가 최소값을 가집니다.
* 이진 탐색 트리는 탐색 연산에 특화되어 있지만, 힙은 주로 우선순위 큐를 구현하는 데 사용됩니다.

## 힙의 활용
* 주로 우선순위 큐를 구현하는 데 사용됩니다.
* 최대값 또는 최소값을 빠르게 찾아야 하는 상황에서 활용됩니다.
* 다익스트라 알고리즘, 힙 정렬 알고리즘 등 다양한 알고리즘에서 효율적인 데이터 관리를 위해 사용됩니다.
 
## 힙의 구현코드
```cpp
#include <iostream>

using namespace std;

// 최대 힙 구현
class MaxHeap 
{
private:
    int* heap;
    int capacity;
    int size;

    // 부모 노드의 인덱스를 반환하는 함수
    int parent(int i) 
    {
        return (i - 1) / 2;
    }

    // 왼쪽 자식 노드의 인덱스를 반환하는 함수
    int leftChild(int i) 
    {
        return 2 * i + 1;
    }

    // 오른쪽 자식 노드의 인덱스를 반환하는 함수
    int rightChild(int i) 
    {
        return 2 * i + 2;
    }

    // 최대 힙을 유지하는 함수
    void heapifyUp(int i) 
    {
        while (i > 0 && heap[parent(i)] < heap[i]) 
        {
            swap(heap[parent(i)], heap[i]);
            i = parent(i);
        }
    }

    // 최대 힙을 유지하는 함수
    void heapifyDown(int i) 
    {
        int maxIndex = i;
        int l = leftChild(i);
        int r = rightChild(i);

        if (l < size && heap[l] > heap[maxIndex]) 
        {
            maxIndex = l;
        }

        if (r < size && heap[r] > heap[maxIndex]) 
        {
            maxIndex = r;
        }

        if (i != maxIndex) 
        {
            swap(heap[i], heap[maxIndex]);
            heapifyDown(maxIndex);
        }
    }

public:
    // 생성자
    MaxHeap(int cap) : capacity(cap), size(0) 
    {
        heap = new int[capacity];
    }

    // 소멸자
    ~MaxHeap() 
    {
        delete[] heap;
    }

    // 삽입 연산
    void insert(int val) 
    {
        if (size == capacity) 
        {
            cout << "Heap is full" << endl;
            return;
        }
        size++;
        int i = size - 1;
        heap[i] = val;
        heapifyUp(i);
    }

    // 최대값 반환
    int getMax() 
    {
        return heap[0];
    }

    // 최대값 삭제
    int extractMax() 
    {
        if (size <= 0) 
        {
            cout << "Heap is empty" << endl;
            return -1;
        }
        if (size == 1)
        {
            size--;
            return heap[0];
        }

        int root = heap[0];
        heap[0] = heap[size - 1];
        size--;
        heapifyDown(0);

        return root;
    }

    // 힙의 크기 반환
    int getSize() 
    {
        return size;
    }

    // 힙이 비어있는지 확인하는 함수
    bool isEmpty() 
    {
        return size == 0;
    }
};

int main() {
    MaxHeap maxHeap(10);

    maxHeap.insert(5);
    maxHeap.insert(3);
    maxHeap.insert(8);
    maxHeap.insert(2);
    maxHeap.insert(7);

    cout << "insert 후 최대 힙: ";
    while (!maxHeap.isEmpty()) 
    {
        cout << maxHeap.extractMax() << " ";
    }
    cout << endl;

    return 0;
}
```