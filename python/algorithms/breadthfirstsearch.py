# Author: Bhabesh Chandra Acharya
# Date: 03-May-2020, Sunday
# Breadth First Search & Depth First Search
import queue

_myQ = queue.Queue(maxsize=20)

class Node:
    def __init__(self, key):
        self.left = None
        self.right = None
        self.val = key

    def breadthfirstsearch(self):
        if self is not None:
            _myQ.put(self)
        temp = Node(-1) #dummy init

        while temp is not None:
            temp = _myQ.get()
            print(temp.val)
            leftnode = temp.left
            rightnode = temp.right
            _myQ.put(leftnode)
            _myQ.put(rightnode)

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
    root.breadthfirstsearch()

if __name__ == "__main__":
    main()
