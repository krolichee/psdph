using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;
using psdPH.Nodes.Core.Nodes;
using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace test.Nodes.Core
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class GraphExecutorTests
    {
        class ExeNode : SingleBoolLetsNode
        {
            protected Action action = () => { };

            public ExeNode()
            {
            }

            public ExeNode(Action action)
            {
                this.action = action;
            }

            public override void Execute()
            {
                action();
            }
        }
        [TestMethod]
        public void RootNodeSearch()
        {

        }
        [TestMethod]
        public void RootExecuteTest()
        {
            //Arrange
            bool executed = false;
            var graph = new NodeGraph();
            var rootNode = new ExeNode(()=>executed=true);
            graph.Nodes.Add(rootNode);
            //Act&Assert
            GraphExecutor.ExecuteGraph(graph);
            Assert.IsTrue(executed);
        }
        [TestMethod]
        public void LetLinkExecuteTest()
        {
            //Arrange
            bool executed = false;
            var graph = new NodeGraph();
            var rootNode = new SingleBoolLetsNode();
            var nextNode = new ExeNode(() => executed = true);

            graph.Nodes.Add(rootNode);
            ///Нужно ли вообще добавлять что-то кроме главной ноды?
            graph.Nodes.Add(nextNode);
            graph.LetLink(rootNode.Outlets[0], nextNode.Inlets[0]);
            //Act&Assert
            GraphExecutor.ExecuteGraph(graph);
            Assert.IsTrue(executed);
        }
        [TestMethod]
        public void ChainExecuteTest()
        {
            //Arrange
            bool executed = false;
            var graph = new NodeGraph();
            var rootNode = new SingleBoolLetsNode();
            var nextNode = new ExeNode(() => executed = true);

            graph.Nodes.Add(rootNode);
            ///Нужно ли вообще добавлять что-то кроме главной ноды?
            graph.Nodes.Add(nextNode);
            graph.Chain(rootNode.Outlets[0], nextNode);
            //Act&Assert
            executed = false;
            rootNode.Inlets[0].Value = false;
            GraphExecutor.ExecuteGraph(graph);
            Assert.IsFalse(executed);

            executed = false;
            rootNode.Inlets[0].Value = true;
            GraphExecutor.ExecuteGraph(graph);
            Assert.IsTrue(executed);
        }
        [TestMethod]
        public void NodeLinkExecuteTest()
        {
            //Arrange
            bool executed = false;
            var graph = new NodeGraph();
            var rootNode = new SingleBoolLetsNode();
            var nextNode = new ExeNode(() => executed = true);
            graph.Nodes.Add(rootNode);
            graph.Nodes.Add(nextNode);
            graph.NodeLink(rootNode, nextNode);

            //Act&Assert
            GraphExecutor.ExecuteGraph(graph);
            Assert.IsTrue(executed);
        }
        class IntListAddNumberNode : ExeNode
        {
            int number;
            List<int> List;

            public IntListAddNumberNode(int number, List<int> list)
            {
                this.number = number;
                List = list;
                action = () => list.Add(number);
            }
        }
        [TestMethod]
        public void TwoLevelTreeExecutionTest()
        {
            //Arrange
            List<int> numbers = new List<int>();
            var graph = new NodeGraph();
            var rootNode = new IntListAddNumberNode(0, numbers);
            var node1 = new IntListAddNumberNode(1, numbers);
            var node2 = new IntListAddNumberNode(2, numbers);
            var node3 = new IntListAddNumberNode(3, numbers);
            var node4 = new IntListAddNumberNode(4, numbers);
            var nodes = new Node[] { rootNode, node1, node2, node3, node4 };
            graph.Nodes.AddRange(nodes);
            graph.NodeLink(rootNode, node1);
            graph.NodeLink(rootNode, node2);
            graph.NodeLink(node2, node3);
            graph.NodeLink(node2, node4);
            //Act&Assert
            GraphExecutor.ExecuteGraph(graph);
            Assert.IsTrue(numbers.Count == nodes.Count());
        }
        [TestMethod]
        public void CycleExceptionTest()
        {
            //Arrange
            List<int> numbers = new List<int>();
            var graph = new NodeGraph();
            var node1 = new EmptyNode();
            var node2 = new EmptyNode();
            var node3 = new EmptyNode();
            var node4 = new EmptyNode();

            graph.NodeLink(node1, node2);
            graph.NodeLink(node2, node3);
            graph.NodeLink(node3, node1);
            graph.NodeLink(node3, node1);

            graph.Nodes.Add(node1);
            graph.Nodes.Add(node2);
            graph.Nodes.Add(node3);
            graph.Nodes.Add(node4);

            //Act&Assert
            Assert.ThrowsException<ArgumentException>(()=>GraphExecutor.ExecuteGraph(graph));

        }
    }
}
