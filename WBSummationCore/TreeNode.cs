using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBSummationCore
{
    class TreeNode
    {
        public string _name;                                                                // Код WBS
        public List<TreeNode> _childs;                                                      // Подуровни WBS
        private Dictionary<String, decimal> _variables;                                     // Все виды работ для конкретного WBS
        public bool _isLeave;                                                              // Является ли данный уровень WBS последним
        private decimal _foldingResult = -1;                                                // Результат суммирования подуровней. !!! Возможны ошибки, в случае возникновения заменить на min(decimal) !!!
        private _FoldExpr _foldFunc;                                                        // Формула рассчета

        public delegate decimal _FoldExpr(Dictionary<String, decimal> args);


        /*
         *  Конструктор для подуровня, если он не является последним, т.е. сам имеет подуровни
         */
        public TreeNode(string name, List<TreeNode> childs, _FoldExpr foldFunc, Dictionary<String, decimal> variables)
        {
            _name = name;
            _childs = childs;
            _foldFunc = foldFunc;
            _variables = variables;
            _isLeave = false;
        }

        /*
         *  Конструктор для подуровня, если он является последним.
         */
        public TreeNode(string name, _FoldExpr foldFunc, Dictionary<String, decimal> variables)
        {
            _name = name;
            _variables = variables;
            _foldFunc = foldFunc;
            _isLeave = true;
        }

        /*
         *  Получить стоимость работ из всех подработ.
         */
        public decimal getValue()
        {
            if (_foldingResult == -1)                                                          // Если до этого рассчет не проводился
            {
                if (_isLeave)
                    return (_foldingResult = _foldFunc(_variables));
                else
                    return (_foldingResult = _foldFunc(getValues(_childs)));
            }
            else return _foldingResult;                                                        // Не выполняем лишней работы и возвр
        }


        /*
         *  Стилизация результатов вычисления подуровней под работы в последнем подуровне.
         */
        private Dictionary<String, decimal> getValues(List<TreeNode> nodes)
        {
            foreach(var node in nodes)
            {
                _variables.Add(node._name, node.getValue());
            }
            return _variables;
        }
    }
}
