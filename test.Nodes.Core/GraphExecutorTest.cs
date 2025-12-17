using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Nodes;
using psdPH.Nodes.Core.Nodes;
using psdPH.Photoshop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Nodes.Core
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class GraphExecutorTest
    {
        class ExeNode : SingleBoolLetsNode
        {
            Action action = () => { };

            public ExeNode()
            {
            }

            public ExeNode(Action action)
            {
                this.action = action;
            }

            public override void Execute(DocumentWr doc)
            {
                action();
            }
        }
        [TestMethod]
        public void RootExecuteTest()
        {
            //Arrange
            bool executed = false;
            var graph = new NodeGraph();
            var rootNode = new ExeNode(()=>executed=true);
            graph.Nodes.Add(rootNode);
            graph.RootNode = rootNode;
            //Act&Assert
            GraphExecutor.Execute(graph,null);
            Assert.IsTrue(executed);
        }
        [TestMethod]
        public void SequenceExecuteTest()
        {
            //Arrange
            bool executed = false;
            var graph = new NodeGraph();
            var rootNode = new SingleBoolLetsNode();
            var nextNode = new ExeNode(() => executed = true);

            graph.Nodes.Add(rootNode);
            ///Нужно ли вообще добавлять что-то кроме главной ноды?
            graph.Nodes.Add(nextNode);
            graph.Link(rootNode.Outlets[0], nextNode.Inlets[0]);

            graph.RootNode = rootNode;
            //Act&Assert
            GraphExecutor.Execute(graph, null);
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

            graph.RootNode = rootNode;
            //Act&Assert
            executed = false;
            rootNode.Inlets[0].Value = false;
            GraphExecutor.Execute(graph, null);
            Assert.IsFalse(executed);

            executed = false;
            rootNode.Inlets[0].Value = true;
            GraphExecutor.Execute(graph, null);
            Assert.IsTrue(executed);
        }
    }
}
