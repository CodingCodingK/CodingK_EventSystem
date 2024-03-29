title：基于堆的优先级队列

# 用途

想象以下使用场景：

> 假如我有活动需要在2天后开启，我想用一个定时器去实现，那么怎么减少tick的次数呢？

那么给定时器的轮询队列，加上一个优先级的概念，就可以解决。到优先级靠后的就可以直接不tick。

# 实现

**完全二叉树（无中空节点）的存储方式：**

* 链式（`LeftChild、Data、RightChild`）
* 数组（`leftChildIndex = 2 * parentIndex + 1`），更优。

数组的结构去存储，下标就对应着完全二叉树的节点编号，从上到下从左到右，非常契合。所以选择**数组**去实现堆。



**那么堆是什么？**

* 完全二叉树
* 树中所有节点的值都大于/小于等于子节点中的值。

决定用**小顶堆**来实践。



**那么堆是怎么组成的？**

想象一个新节点插入一个小顶堆，直接插到最后一个位置，把它和它的父节点进行大小比较，它更小就互换位置，然后继续和新的父节点比较、交换，直至大于它的新父节点。至此，一个节点的位置就确定了。

**那么堆怎么实现Remove堆顶呢？**

Add在上面实现了，而Remove堆顶节点并没有那么简单，直接`Remove+重排序`会导致产生空节点的问题。

解决方案是把顶点和尾节点交换位置，然后删除尾节点（原先的顶点），最后再单独对顶点进行一次位置排序确定。

排序具体就是，不断地把自己、自己的2个子节点，3者之间进行排序，最小的放到顶部。如果没有进行交换，则说明自己已经比2个子节点小了，排序算结束；否则继续排序。
