using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBSummationCore
{
    class FoldableTree
    {
        private TreeNode _rootNode;                                                                                                           // Участок                                             
        private List<List<string>> _Structure;
        private Dictionary<string, List<KeyValuePair<string, decimal>>> _Data;
        public FoldableTree(List<List<string>> Structure, Dictionary<string, List<KeyValuePair<string, decimal>>> Data)
        {
            _Structure = Structure;
            _Data = Data;
            _rootNode = toTree(new List<string>() { _Structure[0][0] } );                                                                     // Ожидается что в 0 0 сожержится название участка
        }


        
        private TreeNode toTree(List<String> predecessors)                                                                                    // Если будет тормозить, то сделать рекурсию восходящей. Вызов для корня СТРОГО С ОДНИМ ЭЛЕМЕНТОМ В СПИСКЕ
        {
            List<String> childs = new List<string>();
            foreach (var record in _Structure)
            {
                if (record.Count == predecessors.Count + 1)
                {
                    bool isChild = true;
                    for (int i = 0; i < predecessors.Count; i++)
                    {
                        if (predecessors[i] != record[i]) isChild = false;
                    }

                    if (isChild)
                    {
                        childs.Add(record[predecessors.Count]);
                    }
                }
            }

            
            String name = predecessors[predecessors.Count - 1];
            Dictionary<string, decimal> variables = new Dictionary<string, decimal>();
            if (_Data.ContainsKey(string.Join(".", predecessors.ToArray())))
            {
                foreach (var record in _Data[string.Join(".", predecessors.ToArray())])
                {
                    variables.Add(record.Key, record.Value);
                }
            }


            if (childs.Count == 0) {
                
                return new TreeNode(name, (Dictionary<String, decimal> args) => { decimal result = 0; foreach (var arg in args) result += arg.Value; return result; }, variables);
            }
            else {
                List<TreeNode> childNodes = new List<TreeNode>();

                foreach(var child_name in childs){
                    List<string> next = new List<string>(predecessors);
                    next.Add(child_name);
                    childNodes.Add(toTree(next));
                }

                return new TreeNode(name, childNodes, (Dictionary<String, decimal> args) => { decimal result = 0; foreach (var arg in args) result += arg.Value; return result; }, variables);
            }

        }
        

        public static List<List<string>> listToStructure(List<List<string>> list)
        {
            List<List<string>> structure = new List<List<string>>();
            foreach(var record in list)
            {
                structure.Add(record[0].Split('.').ToList());
            }
            return structure;
        }

        public static Dictionary<string, List<KeyValuePair<string, decimal>>> listToData(List<List<string>> list)
        {
            Dictionary<string, List<KeyValuePair<string, decimal>>> data = new Dictionary<string, List<KeyValuePair<string, decimal>>>();
            SortedSet<string> unique = new SortedSet<string>();

            foreach(var record in list)
            {
                if (!data.ContainsKey(record[0]))
                {
                    data[record[0]] = new List<KeyValuePair<string, decimal>>();
                }
                data[record[0]].Add(new KeyValuePair<string, decimal>(record[1], Convert.ToDecimal(record[2])));
            }

            return data;
        }

        public decimal getValue()
        {
            return _rootNode.getValue();
        }


        public Dictionary<String, decimal> printTree()
        {
            Dictionary<string, decimal> ans = new Dictionary<string, decimal>();
            printTree(ref ans, _rootNode, new List<string>());
            return ans;
        }

        private void printTree(ref Dictionary<String, decimal> data,TreeNode node, List<String> predecessors)
        {
            List<String> new_predecessors = new List<String>(predecessors);
                new_predecessors.Add(node._name);
            data.Add(string.Join(".", new_predecessors.ToArray()), node.getValue());
            if (!node._isLeave)
            {
                foreach (var child in node._childs)
                {
                    printTree(ref data, child, new_predecessors);
                }
            }
        }
    }
}
