# Author: Bhabesh Chandra Acharya
# Date: 03-May-2020, Sunday
# Breadth First Search & Depth First Search
import queue

_mybfsq = queue.Queue(maxsize=100)
_mydfsstack = queue.LifoQueue(maxsize=100)

class Node:
    def __init__(self, key):
        self.left = None
        self.right = None
        self.val = key

    def breadthfirstsearch(self):
        if self is not None:
            _mybfsq.put(self)
        temp = Node(-1) #dummy init

        while temp is not None:
            temp = _mybfsq.get()
            print(temp.val)
            leftnode = temp.left
            rightnode = temp.right
            _mybfsq.put(leftnode)
            _mybfsq.put(rightnode)

    def depthfirstsearch(self):
        if self is not None:
            _mydfsstack.put(self)
        temp = Node(-1)
        while temp is not None:
            temp = _mydfsstack.get()
            print(temp.val)
            leftnode = temp.left
            rightnode = temp.right
            if leftnode is not None:
                _mydfsstack.put(leftnode)
            if rightnode is not None:
                _mydfsstack.put(rightnode)

def main():
    print("breadth first search of graph")
    root = Node(10)
    root.left = Node(20)
    root.right = Node(30)
    # for 20
    root.left.left = Node(40)
    root.left.right = Node(50)
    # for 30
    root.right.left = Node(60)
    root.right.right = Node(70)
    print("BREADTH FIRST TRAVERSAL")
    #root.breadthfirstsearch()
    print("DEPTH FIRST TRAVERSAL")
    root.depthfirstsearch()

if __name__ == "__main__":
    main()
