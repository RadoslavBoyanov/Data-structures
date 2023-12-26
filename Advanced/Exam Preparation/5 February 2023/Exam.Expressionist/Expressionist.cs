using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Exam.Expressionist
{
    public class Expressionist : IExpressionist
    {
        private Dictionary<string, Expression> expressionsById;
        private Expression root;
        
        public Expressionist()
        {
            this.expressionsById = new Dictionary<string, Expression>();
        }

        public void AddExpression(Expression expression)
        {
            if (root != null)
            {
                throw new ArgumentException();
            }

            this.root = expression;

            this.BfsAddToExpressionsCollection(expression);
        }

        public void AddExpression(Expression expression, string parentId)
        {
            if (!this.expressionsById.ContainsKey(parentId))
            {
                throw new ArgumentException();
            }

            var parent = this.expressionsById[parentId];

            if (parent.LeftChild != null && parent.RightChild != null)
            {
                throw new ArgumentException();
            }

            if (parent.LeftChild is null)
            {
                parent.LeftChild = expression;
            }
            else if (parent.LeftChild != null && parent.RightChild is null)
            {
                parent.RightChild = expression;
            }

            expression.Parent = parent;

            this.BfsAddToExpressionsCollection(expression);
        }

        public bool Contains(Expression expression) => this.expressionsById.ContainsKey(expression.Id);

        public int Count() => this.expressionsById.Count;

        public Expression GetExpression(string expressionId)
        {
            if (!this.expressionsById.ContainsKey(expressionId))
            {
                throw new ArgumentException();
            }

            return this.expressionsById[expressionId];
        }

        public void RemoveExpression(string expressionId)
        {
            if (!this.expressionsById.ContainsKey(expressionId))
            {
                throw new ArgumentException();
            }

            var expression = this.expressionsById[expressionId];

            if (expression.Parent is null)
            {
                root = null;
                this.expressionsById.Remove(expressionId);
            }

            var parent = this.expressionsById[expression.Parent.Id];

            if (parent.LeftChild == expression)
            {
                parent.LeftChild = parent.RightChild;
            }

            parent.RightChild = null;

            this.Bfs(expression, e => this.expressionsById.Remove(e.Id));
        }

        public string Evaluate() => this.Evaluate(this.root, "");

        private string Evaluate(Expression expression, string result)
        {
            if (expression is null)
            {
                return result;
            }

            if (expression.Type == ExpressionType.Value)
            {
                result += expression.Value;
            }
            else
            {
                result +=
                    $"({Evaluate(expression.LeftChild, "")} {expression.Value} {Evaluate(expression.RightChild, "")})";
            }

            return result;
        }

        //helper methods
        private void BfsAddToExpressionsCollection(Expression expression)
        {
            var queue = new Queue<Expression>();
            queue.Enqueue(expression);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                this.expressionsById.Add(node.Id, node);

                if (node.LeftChild != null)
                {
                    queue.Enqueue(node.LeftChild);
                    node.LeftChild.Parent = node;
                }

                if (node.RightChild != null)
                {
                    queue.Enqueue(node.RightChild);
                    node.RightChild.Parent = node;
                }
            }
        }

        private void Bfs(Expression expression, Action<Expression> action)
        {
            var queue = new Queue<Expression>();
            queue.Enqueue(expression);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                action(node);

                if (node.LeftChild != null)
                {
                    queue.Enqueue(node.LeftChild);
                }

                if (node.RightChild != null)
                {
                    queue.Enqueue(node.RightChild);
                }
            }
        }
    }
}
