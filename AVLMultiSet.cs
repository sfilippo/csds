using System;

namespace csds
{
    class AVLMultiset <T> where T : IComparable
    {
        class AVLNode
        {
            public T Value {get; private set;}
            public UInt32 Size {get; private set;}
            public SByte Height {get; private set;}
            public AVLNode Left {get; private set; }
            public AVLNode Right {get; private set; }
            //Balance is difference between left and right childs height. Should stay in [-1, 1]
            public SByte Balance
            {
                get
                {
                    return (SByte) ((Left == null ? (SByte)0 : Left.Height) - (Right == null ? (SByte)0 : Right.Height));
                }
            }
            public AVLNode(T val)
            {
                this.Value = val;
                this.Size = 1;
                this.Height = 1;
                this.Left = this.Right = null;
            }
            void Update()
            {
                this.Size = 1 + (Left != null ? Left.Size : 0) + (Right != null ? Right.Size : 0);
                this.Height = (SByte)(1 + Math.Max(Left != null ? Left.Height : (SByte)0, Right != null ? Right.Height : (SByte)0));
            }
        }
    }
}