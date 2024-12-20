//*****************************************************************************
//** 2415. Reverse Odd Levels of Binary Tree                        leetcode **
//*****************************************************************************

/**
 * Definition for a binary tree node.
 * struct TreeNode {
 *     int val;
 *     struct TreeNode *left;
 *     struct TreeNode *right;
 * };
 */

// Queue node definition
typedef struct QueueNode {
    struct TreeNode* treeNode;
    struct QueueNode* next;
} QueueNode;

// Queue structure
typedef struct Queue {
    QueueNode* front;
    QueueNode* rear;
} Queue;

// Function to create a new queue
Queue* createQueue() {
    Queue* queue = (Queue*)malloc(sizeof(Queue));
    queue->front = queue->rear = NULL;
    return queue;
}

// Function to enqueue a tree node
void enqueue(Queue* queue, struct TreeNode* node) {
    QueueNode* newNode = (QueueNode*)malloc(sizeof(QueueNode));
    newNode->treeNode = node;
    newNode->next = NULL;
    if (queue->rear == NULL) {
        queue->front = queue->rear = newNode;
        return;
    }
    queue->rear->next = newNode;
    queue->rear = newNode;
}

// Function to dequeue a tree node
struct TreeNode* dequeue(Queue* queue) {
    if (queue->front == NULL) {
        return NULL;
    }
    QueueNode* temp = queue->front;
    struct TreeNode* node = temp->treeNode;
    queue->front = queue->front->next;
    if (queue->front == NULL) {
        queue->rear = NULL;
    }
    free(temp);
    return node;
}

// Function to check if the queue is empty
bool isQueueEmpty(Queue* queue) {
    return queue->front == NULL;
}

// Function to reverse the values of nodes in an array
void reverseValues(struct TreeNode** nodes, int size) {
    for (int start = 0, end = size - 1; start < end; start++, end--) {
        int temp = nodes[start]->val;
        nodes[start]->val = nodes[end]->val;
        nodes[end]->val = temp;
    }
}

// Optimized function to reverse values at odd levels of a binary tree
struct TreeNode* reverseOddLevels(struct TreeNode* root) {
    if (root == NULL) {
        return NULL;
    }

    Queue* queue = createQueue();
    enqueue(queue, root);

    int level = 0;

    while (!isQueueEmpty(queue)) {
        int levelSize = 0;
        QueueNode* tempNode = queue->front;

        // Precompute the size of the current level
        while (tempNode != NULL) {
            levelSize++;
            tempNode = tempNode->next;
        }

        // Use a static array for improved performance
        struct TreeNode* currentLevelNodes[levelSize];
        int index = 0;

        for (int i = 0; i < levelSize; i++) {
            struct TreeNode* node = dequeue(queue);

            if (level % 2 == 1) {
                currentLevelNodes[index++] = node;
            }

            if (node->left) {
                enqueue(queue, node->left);
            }
            if (node->right) {
                enqueue(queue, node->right);
            }
        }

        if (level % 2 == 1) {
            reverseValues(currentLevelNodes, index);
        }

        level++;
    }

    free(queue);
    return root;
}
